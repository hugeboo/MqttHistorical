using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using MqttHistoricalUtils.Data;

namespace MqttHistoricalServer.Repository
{
    internal static class SubscriptionHelper
    {
        public static Subscription Read(SqlDataReader dr)
        {
            var s = new Subscription();
            s.Id = SqlHelper.GetInt(dr, "Id") ?? 0;
            s.ConnectionId = SqlHelper.GetInt(dr, "ConnectionId") ?? 0;
            s.TopicFilter = SqlHelper.GetString(dr, "TopicFilter");
            s.QoS = SqlHelper.GetInt(dr, "QoS") ?? 0;
            s.Enabled = SqlHelper.GetBool(dr, "Enabled") ?? false;
            return s;
        }

        public static void Insert(SqlConnection c, Subscription s)
        {
            var cmd = new SqlCommand("INSERT INTO [Subscriptions] ([ConnectionId], [TopicFilter], [QoS], [Enabled]) " +
                "VALUES (@CONID,@TFIL,@QOS,@ENAB)", c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@CONID", s.ConnectionId));
            cmd.Parameters.Add(SqlHelper.CreateParam("@TFIL", s.TopicFilter));
            cmd.Parameters.Add(SqlHelper.CreateParam("@QOS", s.QoS));
            cmd.Parameters.Add(SqlHelper.CreateParam("@ENAB", s.Enabled));
            cmd.ExecuteNonQuery();
        }

        public static IEnumerable<Subscription> SelectAllByConnection(SqlConnection c, int connectionId)
        {
            var lst = new List<Subscription>();
            var cmd = new SqlCommand("SELECT * FROM [Subscriptions] WHERE [ConnectionId]=@CONID", c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@CONID", connectionId));
            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    lst.Add(Read(dr));
                }
            }
            return lst;
        }

        public static Subscription Select(SqlConnection c, int subscriptionId)
        {
            var cmd = new SqlCommand("SELECT * FROM [Subscriptions] WHERE [Id]=@ID", c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@ID", subscriptionId));
            using (var dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    return Read(dr);
                }
            }
            return null;
        }

        public static void Delete(SqlConnection c, int subscriptionId)
        {
            var cmd = new SqlCommand("DELETE FROM [Subscriptions] WHERE [Id]=@ID", c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@ID", subscriptionId));
            cmd.ExecuteNonQuery();
        }
    }
}
