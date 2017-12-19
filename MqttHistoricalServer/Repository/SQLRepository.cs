using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.IO;

namespace MqttHistoricalServer.Repository
{
    internal sealed class SQLRepository : IRepository
    {
        #region private fields

        private SQLRepositorySettings _Settings;
        private string _sConnectionString;

        #endregion

        #region Конструкторы

        public SQLRepository(string settingsXmlFileName)
        {
            if (string.IsNullOrEmpty(settingsXmlFileName)) throw new ArgumentNullException();
            var xml = new XmlSerializer(typeof(SQLRepositorySettings));
            using (var sr = new StreamReader(settingsXmlFileName))
            {
                _Settings = xml.Deserialize(sr) as SQLRepositorySettings;
            }
            _sConnectionString = MakeConnectionString(_Settings);
        }

        public SQLRepository(SQLRepositorySettings settings)
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
            Do(c => Connection.Insert(c, conn));
        }

        public void CreatePayload(Payload p)
        {
            Do(c => Payload.Insert(c, p));
        }

        public void CreateSubscription(Subscription sub)
        {
            Do(c => Subscription.Insert(c, sub));
        }

        public void CreateTopic(Topic topic)
        {
            Do(c => Topic.Insert(c, topic));
        }

        public void CreateUser(User user)
        {
            Do(c => User.Insert(c, user));
        }

        public void DeleteConnection(int connId)
        {
            Do(c => Connection.Delete(c, connId));
        }

        public void DeletePayloads(int topicId)
        {
            Do(c => { Payload.DeleteAll(c, null, topicId); });
        }

        public void DeleteSubscription(int subscriptionId)
        {
            Do(c => Subscription.Delete(c, subscriptionId));
        }

        public void DeleteTopic(int topicId)
        {
            Do(c => 
            {
                var t = c.BeginTransaction();
                Payload.DeleteAll(c, t, topicId);
                Topic.Delete(c, t, topicId);
                t.Commit();
            });
        }

        public void DeleteUser(int userId)
        {
            Do(c => User.Delete(c, userId));
        }

        public Connection GetConnection(int connId)
        {
            return Do(c => Connection.Select(c, connId));
        }

        public IEnumerable<Connection> GetConnections(string userName)
        {
            return Do(c => Connection.SelectAllByUser(c, userName));
        }

        public Payload GetPayload(string topicName, long? startTime, long? stopTime)
        {
            return Do(c => Payload.SelectAllByTopicAndTimestampLast(c, topicName, startTime, stopTime));
        }

        public IEnumerable<Payload> GetPayloads(string topicName, long? startTime, long? stopTime)
        {
            return Do(c => Payload.SelectAllByTopicAndTimestamp(c, topicName, startTime, stopTime));
        }

        public Payload GetRecord(int recId)
        {
            return Do(c => Payload.Select(c, recId));
        }

        public Subscription GetSubscription(int subscriptionId)
        {
            return Do(c => Subscription.Select(c, subscriptionId));
        }

        public IEnumerable<Subscription> GetSubscriptions(int connId)
        {
            return Do(c => Subscription.SelectAllByConnection(c, connId));
        }

        public Topic GetTopic(int topicId)
        {
            return Do(c => Topic.Select(c, topicId));
        }

        public Topic GetTopic(int connId, string topicName)
        {
            return Do(c => Topic.Select(c, connId, topicName));
        }

        public IEnumerable<Topic> GetTopics(int connId)
        {
            return Do(c => Topic.SelectAllByConnection(c, connId));
        }

        public User GetUser(int userId)
        {
            return Do(c => User.Select(c, userId));
        }

        public User GetUser(string name)
        {
            return Do(c => User.Select(c, name));
        }

        public IEnumerable<User> GetUsers()
        {
            return Do(c => User.SelectAll(c));
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

        private string MakeConnectionString(SQLRepositorySettings settings)
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
