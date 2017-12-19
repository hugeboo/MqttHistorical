using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttHistoricalServer.Repository
{
    internal interface IRepository
    {
        #region Работа с пользователями

        /// <summary>
        /// Создать нового пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        void CreateUser(User user);

        /// <summary>
        /// Получить пользователей
        /// </summary>
        /// <returns>Список пользователей</returns>
        IEnumerable<User> GetUsers();

        /// <summary>
        /// Получить пользователя по идентификатору
        /// </summary>
        /// <param name="userId">Идентификатор</param>
        /// <returns>Найденный пользователь</returns>
        User GetUser(int userId);

        /// <summary>
        /// Получить пользователя по имени
        /// </summary>
        /// <param name="name">Имя</param>
        /// <returns>Найденный пользователь</returns>
        User GetUser(string name);

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="userId">Идентификатор</param>
        void DeleteUser(int userId);

        #endregion

        #region Работа с Mqtt-подключениями

        /// <summary>
        /// Создать новое подключение
        /// </summary>
        /// <param name="conn">Подключение</param>
        void CreateConnection(Connection conn);

        /// <summary>
        /// Получить все подключения пользователя
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <returns>Список подключений</returns>
        IEnumerable<Connection> GetConnections(string userName);

        /// <summary>
        /// Получить подключение по идентификатору
        /// </summary>
        /// <param name="connId">Идентификатор подключения</param>
        /// <returns>Найденное подключение</returns>
        Connection GetConnection(int connId);

        ///// <summary>
        ///// Получить подключение по mqtt-серверу и mqtt-юзеру
        ///// </summary>
        ///// <param name="server">mqtt-сервер</param>
        ///// <param name="connectionUser">mqtt-пользователь</param>
        ///// <returns>Найденное подключение</returns>
        //Connection GetConnection(string server, string connectionUser);

        /// <summary>
        /// Удалить подключение
        /// </summary>
        /// <param name="connId">Идентификатор подключения</param>
        void DeleteConnection(int connId);

        #endregion

        #region Работа с подписками

        /// <summary>
        /// Создать новую подписку
        /// </summary>
        /// <param name="sub">Подписка</param>
        void CreateSubscription(Subscription sub);

        /// <summary>
        /// Получить все подписки подключения
        /// </summary>
        /// <param name="connId">Идентификатор подключения</param>
        /// <returns>Список подписок</returns>
        IEnumerable<Subscription> GetSubscriptions(int connId);

        /// <summary>
        /// Получить подписку по идентификатору
        /// </summary>
        /// <param name="subscriptionId">Идентификатор подписки</param>
        /// <returns>Найденная подписка</returns>
        Subscription GetSubscription(int subscriptionId);

        /// <summary>
        /// Удалить подписку
        /// </summary>
        /// <param name="subscriptionId">Идентификатор подписки</param>
        void DeleteSubscription(int subscriptionId);

        #endregion

        #region Работа с топиками

        /// <summary>
        /// Создать новый топик
        /// </summary>
        /// <param name="topic">Топик</param>
        void CreateTopic(Topic topic);

        /// <summary>
        /// Получить все топики подключения
        /// </summary>
        /// <param name="connId">Идентификатор подключения</param>
        /// <returns>Список топиков</returns>
        IEnumerable<Topic> GetTopics(int connId);

        /// <summary>
        /// Получить топик по идентификатору
        /// </summary>
        /// <param name="topicId">Идентификатор топика</param>
        /// <returns>Найденный топик</returns>
        Topic GetTopic(int topicId);

        /// <summary>
        /// Получить топик по имени
        /// </summary>
        /// <param name="connId">Идентификатор подключения</param>
        /// <param name="topicName">Уникальное имя</param>
        /// <returns>Найденный топик</returns>
        Topic GetTopic(int connId, string topicName);

        /// <summary>
        /// Удалить топик со всеми записями
        /// </summary>
        /// <param name="topicId">Идентификатор топика</param>
        void DeleteTopic(int topicId);

        #endregion

        #region Работа с записями

        ///// <summary>
        ///// Создать новую запись
        ///// </summary>
        ///// <param name="connId">Идентификатор подключения</param>
        ///// <param name="topicName">Имя топика</param>
        ///// <param name="timestamp">Время</param>
        ///// <param name="data">Данные</param>
        //void AddPayload(int connId, string topicName, long timestamp, string data);

        /// <summary>
        /// Создать новую запись
        /// </summary>
        /// <param name="p">Запись</param>
        void CreatePayload(Payload p);

        /// <summary>
        /// Получить запись по идентификатору
        /// </summary>
        /// <param name="recId">Идентификатор</param>
        /// <returns>Найденная запись</returns>
        Payload GetRecord(int recId);

        /// <summary>
        /// Получить записи из топика за диапазон времени
        /// </summary>
        /// <param name="topicName">Имя топика</param>
        /// <param name="startTime">Начальное время (вкючительно, не обязательно)</param>
        /// <param name="stopTime">Конечное время (не обязательно)</param>
        /// <returns>Список записей</returns>
        IEnumerable<Payload> GetPayloads(string topicName, long? startTime, long? stopTime);

        /// <summary>
        /// Получить последнюю (по времени) запись из топика за диапазон времени
        /// </summary>
        /// <param name="topicName">Имя топика</param>
        /// <param name="startTime">Начальное время (вкючительно, не обязательно)</param>
        /// <param name="stopTime">Конечное время (не обязательно)</param>
        /// <returns>Найденная запись</returns>
        Payload GetPayload(string topicName, long? startTime, long? stopTime);

        /// <summary>
        /// Удалить все записи топика
        /// </summary>
        /// <param name="topicId">Идентификатор топика</param>
        void DeletePayloads(int topicId);

        #endregion
    }
}
