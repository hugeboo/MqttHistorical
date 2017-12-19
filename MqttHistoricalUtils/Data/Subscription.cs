using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttHistoricalUtils.Data
{
    public sealed class Subscription
    {
        public int Id { get; set; }
        public int ConnectionId { get; set; }
        public string TopicFilter { get; set; }
        public int QoS { get; set; }
        public bool Enabled { get; set; }
    }
}
