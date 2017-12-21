using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using StriderMqtt;
using DotKit.RESTclient;
using MqttHistoricalUtils.Data;
using MqttHistoricalUtils.Requests;

namespace MqttHistoricalSubscriber
{
    internal sealed class Subscriber : IDisposable
    {
        private Thread _worker;
        private readonly object _syncObj = new object();
        private ConnectionInfo _connectionInfo;
        private RequestSettings _RESTRequestSettings;

        public Subscriber(ConnectionInfo ci, RequestSettings restTRequestSettings)
        {
            _connectionInfo = ci;
            _RESTRequestSettings = restTRequestSettings;
            StartWorker();
        }

        public void Dispose()
        {
            StopWorker();
        }

        public void Update(ConnectionInfo ci)
        {
            //...
        }

        private void StartWorker()
        {
            _worker = new Thread(WorkerThreadProc);
            _worker.Start();
        }

        private void StopWorker()
        {
            try { _worker.Abort(); } catch { }
        }

        private void WorkerThreadProc() // !!!! нужен реконнект
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
                    conn.Subscribe(new string[] { "my/subscription/topic" },
                        new MqttQos[] { MqttQos.AtMostOnce });

                    while (conn.Loop())
                    {
                        // etc...
                    }
                }
            }
            catch (Exception ex)
            {
                //...
            }
        }

        private void HandlePublishReceived(object sender, PublishReceivedEventArgs e)
        {
            try
            {
                // !!!! убрать retain ???

                var data = Encoding.UTF8.GetString(e.Message ?? new byte[0]);

                var p = new JsonTopicPayloads()
                {
                    TopicName = e.Topic,
                    Payloads = new[] { new JsonPayload() { Timestamp = DateTime.Now.ToBinary(), Data = data } }
                };

                var response = new PostPayloadsServerRequest(_RESTRequestSettings).
                    Execute(_connectionInfo.Connection.Server, _connectionInfo.Connection.ConnectionUser, p);

                if (response.ResultCode != JsonResultCode.OK)
                {
                    throw new ApplicationException($"responseResultCode={response.ResultCode.ToString()}, '{response.Message}'");
                }

            }
            catch (Exception ex)
            {
                //...
            }
        }
    }
}
