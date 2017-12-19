using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttHistoricalUtils.Data
{
    public sealed class Topic
    {
        public int Id { get; set; }
        public int ConnectionId { get; set; }
        public string Name { get; set; }
    }
}
