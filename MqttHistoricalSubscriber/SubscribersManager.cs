using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using NLog;

using DotKit.RESTclient;
using MqttHistoricalUtils;
using MqttHistoricalUtils.Data;
using MqttHistoricalUtils.Requests;

namespace MqttHistoricalSubscriber
{
    public sealed class SubscribersManager : IDisposable
    {
        private readonly Dictionary<string, Subscriber> _dictSubscribers = new Dictionary<string, Subscriber>();
        private readonly object _syncObj = new object();
        private readonly Timer _Timer;
        private bool _bDisposed;
        private RequestSettings _RESTRequestSettings;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public SubscribersManager(RequestSettings restRequestSettings)
        {
            logger.Info("Starting");
            _RESTRequestSettings = restRequestSettings;
            _Timer = new Timer(TimerProc, null, 1, Timeout.Infinite);
        }

        public void Dispose()
        {
            if (!_bDisposed)
            {
                _bDisposed = true;
                logger.Info("Disposing");
                lock (_syncObj)
                {
                    try { _Timer.Dispose(); } catch { }
                    foreach (var s in _dictSubscribers.Values)
                    {
                        try { s.Dispose(); } catch { }
                    }
                    _dictSubscribers.Clear();
                }
            }
        }

        private void TimerProc(object obj)
        {
            if (_bDisposed) return;
            try
            {
                _Timer.Change(Timeout.Infinite, Timeout.Infinite);
                var dictConns = GetAllConnectionsFromServer();
                DeleteSubscribers(dictConns);
                AddAndUpdateSubscribers(dictConns);
            }
            catch (Exception ex)
            {
                logger.Error("TimerProc exception", ex);
            }
            finally
            {
                try { _Timer.Change(20000, Timeout.Infinite); } catch { }
            }
        }

        private void DeleteSubscribers(Dictionary<string, ConnectionInfo> dictConns)
        {
            lock (_syncObj)
            {
                foreach (var kvp in _dictSubscribers.ToArray())
                {
                    if (!dictConns.ContainsKey(kvp.Key))
                    {
                        logger.Info("Remove subscriber: " + kvp.Key);
                        _dictSubscribers.Remove(kvp.Key);
                        kvp.Value.Dispose();
                    }
                }
            }
        }

        private void AddAndUpdateSubscribers(Dictionary<string, ConnectionInfo> dictConns)
        {
            lock (_syncObj)
            {
                foreach (var kvp in dictConns)
                {
                    if (!_dictSubscribers.TryGetValue(kvp.Key, out Subscriber s))
                    {
                        logger.Info("Add subscriber: " + kvp.Key);
                        s = new Subscriber(kvp.Value, _RESTRequestSettings);
                        _dictSubscribers[kvp.Key] = s;
                    }
                    else
                    {
                        s.Update(kvp.Value);
                    }
                }
            }
        }

        private Dictionary<string, ConnectionInfo> GetAllConnectionsFromServer()
        {
            var usreq = new GetUsersServerRequest(_RESTRequestSettings);
            var users = usreq.Execute();
            if (users == null)
            {
                throw new ApplicationException(usreq.LastError);
            }
            else if (users.ResultCode != JsonResultCode.OK)
            {
                throw new ApplicationException($"usersResultCode={users.ResultCode.ToString()}, '{users.Message}'");
            }

            var dictConns = new Dictionary<string, ConnectionInfo>();
            foreach (var user in users.Users)
            {
                if (user.Enabled)
                {
                    var ureq = new GetConnectionsServerRequest(_RESTRequestSettings);
                    var uconns = ureq.Execute(user.Id);
                    if (uconns == null)
                    {
                        throw new ApplicationException(ureq.LastError);
                    }
                    else if (uconns.ResultCode != JsonResultCode.OK)
                    {
                        throw new ApplicationException($"uconnsResultCode={uconns.ResultCode.ToString()}, '{uconns.Message}'");
                    }
                    foreach (var conn in uconns.Connections)
                    {
                        if (conn.Enabled)
                        {
                            var ci = new ConnectionInfo() { UserName = user.Name, Connection = conn };
                            dictConns[ci.Key] = ci;
                            var creq = new GetSubscriptionsServerRequest(_RESTRequestSettings);
                            var csubs = creq.Execute(conn.Id);
                            if (csubs == null)
                            {
                                throw new ApplicationException(creq.LastError);
                            }
                            else if (csubs.ResultCode != JsonResultCode.OK)
                            {
                                throw new ApplicationException($"csubsResultCode={csubs.ResultCode.ToString()}, '{csubs.Message}'");
                            }
                            var a = csubs.Subscriptions.Where(s => s.Enabled).OrderBy(s => s.Id).ToArray();
                            ci.Subscriptions = csubs.Subscriptions.Where(s => s.Enabled).ToArray();
                        }
                    }
                }
            }
            return dictConns;
        }
    }
}
