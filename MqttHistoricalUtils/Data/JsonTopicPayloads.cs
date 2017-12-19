using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttHistoricalUtils.Data
{
    /// <summary>
    /// Выборка записей из одного топика
    /// </summary>
    public sealed class JsonTopicPayloads
    {
        public string TopicName { get; set; }
        public JsonPayload[] Payloads { get; set; }
    }
}
