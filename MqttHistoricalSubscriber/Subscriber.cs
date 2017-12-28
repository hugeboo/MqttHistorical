using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using NLog;

using StriderMqtt;
using DotKit.RESTclient;
using MqttHistoricalUtils.Data;
using MqttHistoricalUtils.Requests;

namespace MqttHistoricalSubscriber
{
    internal sealed class Subscriber : IDisposable
    {
        private Thread _worker;
        private ConnectionInfo _connectionInfo;
        private RequestSettings _RESTRequestSettings;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public bool IsAlive
        {
            get
            {
                var w = _worker;
                return w != null && w.IsAlive;
            }
        }

        public string Key
        {
            get
            {
                var c = _connectionInfo;
                return c != null ? c.Key : "";
            }
        }

        public Subscriber(ConnectionInfo ci, RequestSettings restTRequestSettings)
        {
            _connectionInfo = ci;
            _RESTRequestSettings = restTRequestSettings.Clone() as RequestSettings;
            _RESTRequestSettings.UserId = _connectionInfo?.UserName;
            Update(_connectionInfo);
        }

        public void Dispose()
        {
            StopWorker();
        }

        public void Update(ConnectionInfo ci)
        {
            if (!Equals(_connectionInfo, ci))
            {
                _connectionInfo = ci;
                _RESTRequestSettings.UserId = _connectionInfo?.UserName;
                StopWorker();
                if (_connectionInfo != null)
                {
                    StartWorker();
                }
            }
            else if (!IsAlive)
            {
                if (_connectionInfo != null) StartWorker();
            }
        }

        private void StartWorker()
        {
            logger.Info("Start: " + Key);
            _worker = new Thread(WorkerThreadProc);
            _worker.Start();
        }

        private void StopWorker()
        {
            logger.Info("Stop: " + Key);
            try { _worker.Abort(); } catch { }
        }

        private void WorkerThreadProc()
        {
            try
            {
                var connArgs = new MqttConnectionArgs()
                {
                    Hostname = _connectionInfo.Connection.Server,
                    Port = _connectionInfo.Connection.Port,
                    Secure = false,
                    ProtocolVersion = MqttProtocolVersion.V3_1_1,
                    ClientId = _connectionInfo.Connection.ConnectionUser,
                    Username = _connectionInfo.Connection.ConnectionUser,
                    Password = _connectionInfo.Connection.Password,
                    CleanSession = true,
                    Keepalive = TimeSpan.FromSeconds(60),
                    WillMessage = null,
                    ReadTimeout = TimeSpan.FromSeconds(10),
                    WriteTimeout = TimeSpan.FromSeconds(10)
                };
                using (var conn = new MqttConnection(connArgs))
                {
                    conn.Connect();

                    conn.PublishReceived += HandlePublishReceived;

                    if (_connectionInfo.Subscriptions != null)
                    {
                        foreach (var s in _connectionInfo.Subscriptions)
                        {
                            logger.Info($"Subscribe: {Key} -> {s.TopicFilter}, {s.QoS}");
                            conn.Subscribe(s.TopicFilter, (MqttQos)s.QoS);
                        }
                    }

                    while (conn.Loop())
                    {
                        // etc...
                    }

                    logger.Warn("Disconnected: " + Key);
                }
            }
            catch (ThreadAbortException)
            {
                logger.Info("Aborted: " + Key);
            }
            catch (Exception ex)
            {
                logger.Error("Error in WorkerProc", ex);
            }
        }

        private void HandlePublishReceived(object sender, PublishReceivedEventArgs e)
        {
            try
            {
                if (e.Retain) return;

                var data = Encoding.UTF8.GetString(e.Message ?? new byte[0]);

                logger.Debug($"PublishReceived: {e.Topic} '{data}'");

                var p = new JsonTopicPayloads()
                {
                    TopicName = e.Topic,
                    Payloads = new[] { new JsonPayload() { Timestamp = DateTime.UtcNow.Ticks, Data = data } }
                };

                var req = new PostPayloadsServerRequest(_RESTRequestSettings);
                var response = req.Execute(_connectionInfo.Connection.Server, _connectionInfo.Connection.ConnectionUser, new[] { p });

                if (response == null)
                {
                    throw new ApplicationException(req.LastError);
                }
                else if (response.ResultCode != JsonResultCode.OK)
                {
                    throw new ApplicationException($"responseResultCode={response.ResultCode.ToString()}, '{response.Message}'");
                }

            }
            catch (Exception ex)
            {
                logger.Error("Error in HandlePublishReceived", ex);
            }
        }
    }
}
