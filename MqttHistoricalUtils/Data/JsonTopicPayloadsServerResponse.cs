using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttHistoricalUtils.Data
{
    /// <summary>
    /// Ответ сервера с выборкой записей
    /// </summary>
    public sealed class JsonTopicPayloadsServerResponse : JsonServerResponse
    {
        public JsonTopicPayloads[] Topics { get; set; }
    }
}
