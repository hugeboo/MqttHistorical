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
        public Connection Connection { get; set; }
        public Subscription[] Subscriptions { get; set; }

        public string Key
        {
            get
            {
                return Connection != null ?
                    Connection.UserId + "\n" + Connection.Server + "\n" + Connection.ConnectionUser :
                    "";
            }
        }
    }
}
