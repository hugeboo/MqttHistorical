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

        public override bool Equals(object obj)
        {
            var other = obj as Subscription;
            if (other == null) return false;
            return Id == other.Id &&
                   ConnectionId == other.ConnectionId &&
                   TopicFilter == other.TopicFilter &&
                   QoS == other.QoS &&
                   Enabled == other.Enabled;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
