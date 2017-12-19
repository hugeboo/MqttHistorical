using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttHistoricalUtils.Data
{
    /// <summary>
    /// Ответ сервера
    /// </summary>
    public class JsonServerResponse
    {
        public JsonResultCode ResultCode { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// Код результата выполнения запроса
    /// </summary>
    public enum JsonResultCode
    {
        /// <summary>
        /// Нет ошибок
        /// </summary>
        OK,

        /// <summary>
        /// Внутренняя ошибка сервера
        /// </summary>
        InternalError,

        /// <summary>
        /// Ошибка в запросе
        /// </summary>
        InvalidRequest,

        /// <summary>
        /// Указанного подключения к Mqtt-брокеру не найдено
        /// </summary>
        MqttConnectionNotFound
    }
}
