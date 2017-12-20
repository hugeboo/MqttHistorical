using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.IO;

using MqttHistoricalUtils.Data;

namespace MqttHistoricalServer.Repository
{
    public sealed class SQLDBRepository : IDBRepository
    {
        #region private fields

        private SQLDBRepositorySettings _Settings;
        private string _sConnectionString;

        #endregion

        #region Конструкторы

        public SQLDBRepository(string settingsXmlFileName)
        {
            if (string.IsNullOrEmpty(settingsXmlFileName)) throw new ArgumentNullException();
            var xml = new XmlSerializer(typeof(SQLDBRepositorySettings));
            using (var sr = new StreamReader(settingsXmlFileName))
            {
                _Settings = xml.Deserialize(sr) as SQLDBRepositorySettings;
            }
            _sConnectionString = MakeConnectionString(_Settings);
        }

        public SQLDBRepository(SQLDBRepositorySettings settings)
        {
            if (settings == null) throw new ArgumentNullException();
            _Settings = settings;
            _sConnectionString = MakeConnectionString(_Settings);
        }

        #endregion


        #region IRepository

        //public void AddPayload(int connId, string topicName, long timestamp, string data)
        //{
        //    Do(c => 
        //    {
        //        var t = c.BeginTransaction();

        //        var topic = Topic.Select(c,connectionId,)

        //        Payload.Insert(c, p);

        //        t.Commit();
        //    });
        //}

        public void CreateConnection(Connection conn)
        {
            Do(c => ConnectionHelper.Insert(c, conn));
        }

        public void CreatePayload(Payload p)
        {
            Do(c => PayloadHelper.Insert(c, p));
        }

        public void CreateSubscription(Subscription sub)
        {
            Do(c => SubscriptionHelper.Insert(c, sub));
        }

        public void CreateTopic(Topic topic)
        {
            Do(c => TopicHelper.Insert(c, topic));
        }

        public void CreateUser(User user)
        {
            Do(c => UserHelper.Insert(c, user));
        }

        public void DeleteConnection(int connId)
        {
            Do(c => ConnectionHelper.Delete(c, connId));
        }

        public void DeletePayloads(int topicId)
        {
            Do(c => { PayloadHelper.DeleteAll(c, null, topicId); });
        }

        public void DeleteSubscription(int subscriptionId)
        {
            Do(c => SubscriptionHelper.Delete(c, subscriptionId));
        }

        public void DeleteTopic(int topicId)
        {
            Do(c => 
            {
                var t = c.BeginTransaction();
                PayloadHelper.DeleteAll(c, t, topicId);
                TopicHelper.Delete(c, t, topicId);
                t.Commit();
            });
        }

        public void DeleteUser(int userId)
        {
            Do(c => UserHelper.Delete(c, userId));
        }

        public Connection GetConnection(int connId)
        {
            return Do(c => ConnectionHelper.Select(c, connId));
        }

        public IEnumerable<Connection> GetConnections(int userId)
        {
            return Do(c => ConnectionHelper.SelectAllByUser(c, userId));
        }

        public IEnumerable<Connection> GetConnections(string userName)
        {
            return Do(c => ConnectionHelper.SelectAllByUser(c, userName));
        }

        public Payload GetPayload(string topicName, long? startTime, long? stopTime)
        {
            return Do(c => PayloadHelper.SelectAllByTopicAndTimestampLast(c, topicName, startTime, stopTime));
        }

        public IEnumerable<Payload> GetPayloads(string topicName, long? startTime, long? stopTime)
        {
            return Do(c => PayloadHelper.SelectAllByTopicAndTimestamp(c, topicName, startTime, stopTime));
        }

        public Payload GetRecord(int recId)
        {
            return Do(c => PayloadHelper.Select(c, recId));
        }

        public Subscription GetSubscription(int subscriptionId)
        {
            return Do(c => SubscriptionHelper.Select(c, subscriptionId));
        }

        public IEnumerable<Subscription> GetSubscriptions(int connId)
        {
            return Do(c => SubscriptionHelper.SelectAllByConnection(c, connId));
        }

        public Topic GetTopic(int topicId)
        {
            return Do(c => TopicHelper.Select(c, topicId));
        }

        public Topic GetTopic(int connId, string topicName)
        {
            return Do(c => TopicHelper.Select(c, connId, topicName));
        }

        public IEnumerable<Topic> GetTopics(int connId)
        {
            return Do(c => TopicHelper.SelectAllByConnection(c, connId));
        }

        public User GetUser(int userId)
        {
            return Do(c => UserHelper.Select(c, userId));
        }

        public User GetUser(string name)
        {
            return Do(c => UserHelper.Select(c, name));
        }

        public IEnumerable<User> GetUsers()
        {
            return Do(c => UserHelper.SelectAll(c));
        }

        #endregion

        #region private methods

        private void Do(Action<SqlConnection> action)
        {
            using (var connection = new SqlConnection(_sConnectionString))
            {
                connection.Open();
                action(connection);
                connection.Close();
            }
        }

        private T Do<T>(Func<SqlConnection, T> action) //where T : new()
        {
            T res;
            using (var connection = new SqlConnection(_sConnectionString))
            {
                connection.Open();
                res = action(connection);
                connection.Close();
            }
            return res;
        }

        private string MakeConnectionString(SQLDBRepositorySettings settings)
        {
            if (String.IsNullOrEmpty(settings.User))
            {
                if (settings.SQLServerPort != 0)
                {
                    return String.Format(@"data source={0},{2};initial catalog={1};integrated security=True;",
                        settings.SQLServerAddr,
                        settings.Database,
                        settings.SQLServerPort);
                }
                else
                {
                    return String.Format(@"data source={0};initial catalog={1};integrated security=True;",
                        settings.SQLServerAddr,
                        settings.Database);
                }
            }
            else
            {
                if (settings.SQLServerPort != 0)
                {
                    return String.Format(@"data source={0},{4};initial catalog={1};UID={2};PWD={3};integrated security=false;",
                        settings.SQLServerAddr,
                        settings.Database,
                        settings.User,
                        settings.Password,
                        settings.SQLServerPort);
                }
                else
                {
                    return String.Format(@"data source={0};initial catalog={1};UID={2};PWD={3};integrated security=false;",
                        settings.SQLServerAddr,
                        settings.Database,
                        settings.User,
                        settings.Password);
                }
            }
        }

        #endregion
    }
}
