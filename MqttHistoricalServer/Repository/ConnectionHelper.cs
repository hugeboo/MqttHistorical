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
    internal static class ConnectionHelper
    {
        public static Connection Read(SqlDataReader dr)
        {
            var con = new Connection();
            con.Id = SqlHelper.GetInt(dr, "Id") ?? 0;
            con.UserId = SqlHelper.GetInt(dr, "UserId") ?? 0;
            con.Server = SqlHelper.GetString(dr, "Server");
            con.ConnectionUser = SqlHelper.GetString(dr, "ConnectionUser");
            con.Password = SqlHelper.GetString(dr, "Password");
            con.Port = SqlHelper.GetInt(dr, "Port") ?? 0;
            con.SSLPort = SqlHelper.GetInt(dr, "SSLPort") ?? 0;
            con.UseSSL = SqlHelper.GetBool(dr, "UseSSL") ?? false;
            con.Enabled = SqlHelper.GetBool(dr, "Enabled") ?? false;
            return con;
        }

        public static void Insert(SqlConnection c, Connection con)
        {
            var cmd = new SqlCommand("INSERT INTO [Connections] ([UserId], [Server], [ConnectionUser], [Password], [Port], [SSLPort], [UseSSL], [Enabled]) " +
                "VALUES (@USERID,@SERVER,@CUSER,@PASS,@PORT,@SSLPORT,@USESSL,@ENAB)", c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@USERID", con.UserId));
            cmd.Parameters.Add(SqlHelper.CreateParam("@SERVER", con.Server));
            cmd.Parameters.Add(SqlHelper.CreateParam("@CUSER", con.ConnectionUser));
            cmd.Parameters.Add(SqlHelper.CreateParam("@PASS", con.Password));
            cmd.Parameters.Add(SqlHelper.CreateParam("@PORT", con.Port));
            cmd.Parameters.Add(SqlHelper.CreateParam("@SSLPORT", con.SSLPort));
            cmd.Parameters.Add(SqlHelper.CreateParam("@USESSL", con.UseSSL));
            cmd.Parameters.Add(SqlHelper.CreateParam("@ENAB", con.Enabled));
            cmd.ExecuteNonQuery();
        }

        public static IEnumerable<Connection> SelectAllByUser(SqlConnection c, string userName)
        {
            var lst = new List<Connection>();
            var cmd = new SqlCommand("SELECT [Connections].* FROM [Connections], [Users] WHERE [Users].[Name]=@USN AND [Users].[Id]=[Connections].[UserId]", c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@USN", userName));
            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    lst.Add(Read(dr));
                }
            }
            return lst;
        }

        public static IEnumerable<Connection> SelectAllByUser(SqlConnection c, int userId)
        {
            var lst = new List<Connection>();
            var cmd = new SqlCommand("SELECT * FROM [Connections] WHERE [UserId]=@USID", c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@USID", userId));
            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    lst.Add(Read(dr));
                }
            }
            return lst;
        }

        public static Connection Select(SqlConnection c, int connectionId)
        {
            var cmd = new SqlCommand("SELECT * FROM [Connections] WHERE [Id]=@ID", c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@ID", connectionId));
            using (var dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    return Read(dr);
                }
            }
            return null;
        }

        public static Connection Select(SqlConnection c, string server, string connectionUser)
        {
            var cmd = new SqlCommand("SELECT * FROM [Connections] WHERE [Server]=@SERVER AND [ConnectionUser]=@CUSER", c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@SERVER", server));
            cmd.Parameters.Add(SqlHelper.CreateParam("@CUSER", connectionUser));
            using (var dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    return Read(dr);
                }
            }
            return null;
        }

        public static void Delete(SqlConnection c, int connectionId)
        {
            var cmd = new SqlCommand("DELETE FROM [Connections] WHERE [Id]=@ID", c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@ID", connectionId));
            cmd.ExecuteNonQuery();
        }
    }
}
