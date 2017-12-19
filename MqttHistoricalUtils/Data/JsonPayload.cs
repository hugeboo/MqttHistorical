using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttHistoricalUtils.Data
{
    /// <summary>
    /// Одна запись из топика
    /// </summary>
    public sealed class JsonPayload
    {
        public long Timestamp { get; set; }
        public string Data { get; set; }
    }
}
