using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MqttHistoricalUtils.Data;

namespace MqttHistoricalSubscriber
{
    internal sealed class ConnectionInfo
    {
        public string UserName { get; set; }
        public Connection Connection { get; set; }
        public Subscription[] Subscriptions { get; set; }

        public string Key
        {
            get
            {
                return Connection != null ?
                    Connection.UserId + ":" + Connection.Server + ":" + Connection.ConnectionUser :
                    "";
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as ConnectionInfo;
            if (other == null) return false;
            if (UserName != other.UserName) return false;
            if (!Equals(Connection, other.Connection)) return false;
            if (Subscriptions != null && other.Subscriptions != null)
            {
                if (Subscriptions.Length != other.Subscriptions.Length) return false;
                for (int i = 0; i > Subscriptions.Length; i++)
                {
                    if (!Equals(Subscriptions[i], other.Subscriptions[i])) return false;
                }
                return true;
            }
            else
            {
                return Subscriptions != null || other.Subscriptions != null;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
