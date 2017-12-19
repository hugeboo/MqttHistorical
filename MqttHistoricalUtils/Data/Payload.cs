using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttHistoricalUtils.Data
{
    public sealed class Payload
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public long Timestamp { get; set; }
        public string Data { get; set; }
    }
}
