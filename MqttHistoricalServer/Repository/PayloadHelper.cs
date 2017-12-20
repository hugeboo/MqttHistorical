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
    internal static class PayloadHelper
    {
        public static Payload Read(SqlDataReader dr)
        {
            var p = new Payload();
            p.Id = SqlHelper.GetInt(dr, "Id") ?? 0;
            p.TopicId = SqlHelper.GetInt(dr, "TopicId") ?? 0;
            p.Timestamp = SqlHelper.GetLong(dr, "Timestamp") ?? 0L;
            p.Data = SqlHelper.GetString(dr, "Data");
            return p;
        }

        public static void Insert(SqlConnection c, Payload p)
        {
            var cmd = new SqlCommand("INSERT INTO [Payloads] ([TopicId], [Timestamp], [Data]) " +
                "VALUES (@TID,@TST,@DATA)", c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@TID", p.TopicId));
            cmd.Parameters.Add(SqlHelper.CreateParam("@TST", p.Timestamp));
            cmd.Parameters.Add(SqlHelper.CreateParam("@DATA", p.Data));
            cmd.ExecuteNonQuery();
        }

        public static IEnumerable<Payload> SelectAllByTopicAndTimestamp(SqlConnection c, string topicName, long? startTs, long? stopTs)
        {
            var lst = new List<Payload>();
            var cmds = "SELECT [Payloads].* FROM [Payloads], [Topics] " +
                "WHERE [Topics].[Name]=@TNAME AND [Topics].[Id]=[Payloads].[TopicId]";

            if (startTs.HasValue)
            {
                cmds += " AND [Payload].[Timestamp]>=@START";
            }
            if (startTs.HasValue)
            {
                cmds += " AND [Payload].[Timestamp]<@STOP";
            }

            var cmd = new SqlCommand(cmds, c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@TNAME", topicName));
            if (startTs.HasValue) cmd.Parameters.Add(SqlHelper.CreateParam("@START", startTs));
            if (stopTs.HasValue) cmd.Parameters.Add(SqlHelper.CreateParam("@STOP", stopTs));
            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    lst.Add(Read(dr));
                }
            }
            return lst;
        }

        public static Payload SelectAllByTopicAndTimestampLast(SqlConnection c, string topicName, long? startTs, long? stopTs)
        {
            var cmds = "SELECT [Payloads].* FROM [Payloads], [Topics] " +
                "WHERE [Topics].[Name]=@TNAME AND [Topics].[Id]=[Payloads].[TopicId] " +
                "AND [Payloads].[Timestamp] IN (SELECT MAX([Timestamp]) FROM [Payloads]";

            if (startTs.HasValue || stopTs.HasValue)
            {
                cmds += " WHERE";
                if (startTs.HasValue)
                {
                    cmds += " AND [Timestamp]>=@START";
                }
                if (startTs.HasValue)
                {
                    cmds += " AND [Timestamp]<@STOP";
                }
            }

            cmds += ")";

            var cmd = new SqlCommand(cmds, c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@TNAME", topicName));
            if (startTs.HasValue) cmd.Parameters.Add(SqlHelper.CreateParam("@START", startTs));
            if (stopTs.HasValue) cmd.Parameters.Add(SqlHelper.CreateParam("@STOP", stopTs));
            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    return Read(dr);
                }
            }
            return null;
        }

        public static Payload Select(SqlConnection c, int payloadId)
        {
            var cmd = new SqlCommand("SELECT * FROM [Payloads] WHERE [Id]=@ID", c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@ID", payloadId));
            using (var dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    return Read(dr);
                }
            }
            return null;
        }

        public static void Delete(SqlConnection c, int payloadId)
        {
            var cmd = new SqlCommand("DELETE FROM [Payloads] WHERE [Id]=@ID", c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@ID", payloadId));
            cmd.ExecuteNonQuery();
        }

        public static void DeleteAll(SqlConnection c, SqlTransaction t, int topicId)
        {
            var cmd = new SqlCommand("DELETE FROM [Payloads] WHERE [TopicId]=@TID", c);
            if (t!=null) cmd.Transaction = t;
            cmd.Parameters.Add(SqlHelper.CreateParam("@TID", topicId));
            cmd.ExecuteNonQuery();
        }
    }
}
