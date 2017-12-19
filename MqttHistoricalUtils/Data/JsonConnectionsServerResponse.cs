using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttHistoricalUtils.Data
{
    public sealed class JsonConnectionsServerResponse : JsonServerResponse
    {
        public Connection[] Connections { get; set; }
    }
}
