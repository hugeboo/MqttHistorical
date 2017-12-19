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
    internal static class TopicHelper
    {
        public static Topic Read(SqlDataReader dr)
        {
            var t = new Topic();
            t.Id = SqlHelper.GetInt(dr, "Id") ?? 0;
            t.ConnectionId = SqlHelper.GetInt(dr, "ConnectionId") ?? 0;
            t.Name = SqlHelper.GetString(dr, "Name");
            return t;
        }

        public static void Insert(SqlConnection c, Topic t)
        {
            var cmd = new SqlCommand("INSERT INTO [Topics] ([ConnectionId], [Name]) " +
                "VALUES (@CONID,@NAME)", c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@CONID", t.ConnectionId));
            cmd.Parameters.Add(SqlHelper.CreateParam("@NAME", t.Name));
            cmd.ExecuteNonQuery();
        }

        public static IEnumerable<Topic> SelectAllByConnection(SqlConnection c, int connectionId)
        {
            var lst = new List<Topic>();
            var cmd = new SqlCommand("SELECT * FROM [Topics] WHERE [ConnectionId]=@CONID", c);
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

        public static Topic Select(SqlConnection c, int topicId)
        {
            var cmd = new SqlCommand("SELECT * FROM [Topics] WHERE [Id]=@ID", c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@ID", topicId));
            using (var dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    return Read(dr);
                }
            }
            return null;
        }


        public static Topic Select(SqlConnection c, int connectionId, string topicName)
        {
            var cmd = new SqlCommand("SELECT * FROM [Topics] WHERE [Name]=@NAME AND [ConnectionId]=@CONID", c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@NAME", topicName));
            cmd.Parameters.Add(SqlHelper.CreateParam("@CONID", connectionId));
            using (var dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    return Read(dr);
                }
            }
            return null;
        }

        public static void Delete(SqlConnection c, SqlTransaction t, int topicId)
        {
            var cmd = new SqlCommand("DELETE FROM [Topics] WHERE [Id]=@ID", c);
            if (t != null) cmd.Transaction = t;
            cmd.Parameters.Add(SqlHelper.CreateParam("@ID", topicId));
            cmd.ExecuteNonQuery();
        }

        public static void DeleteAll(SqlConnection c, SqlTransaction t, int connId)
        {
            var cmd = new SqlCommand("DELETE FROM [Topics] WHERE [ConnectionId]=@CONID", c);
            if (t != null) cmd.Transaction = t;
            cmd.Parameters.Add(SqlHelper.CreateParam("@CONID", connId));
            cmd.ExecuteNonQuery();
        }
    }
}
