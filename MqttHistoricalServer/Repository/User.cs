using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace MqttHistoricalServer.Repository
{
    internal sealed class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public static User Read(SqlDataReader dr)
        {
            var u = new User();
            u.Id = SqlHelper.GetInt(dr, "Id") ?? 0;
            u.Name = SqlHelper.GetString(dr, "Name");
            u.Password = SqlHelper.GetString(dr, "Password");
            return u;
        }

        public static void Insert(SqlConnection c, User u)
        {
            var cmd = new SqlCommand("INSERT INTO [Users] ([Name],[Password]) VALUES (@NAME,@PASS)", c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@NAME", u.Name));
            cmd.Parameters.Add(SqlHelper.CreateParam("@PASS", u.Password));
            cmd.ExecuteNonQuery();
        }

        public static IEnumerable<User> SelectAll(SqlConnection c)
        {
            var lst = new List<User>();
            var cmd = new SqlCommand("SELECT * FROM [Users]", c);
            using (var dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    lst.Add(Read(dr));
                }
            }
            return lst;
        }

        public static User Select(SqlConnection c, int userId)
        {
            var cmd = new SqlCommand("SELECT * FROM [Users] WHERE [Id]=@ID", c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@ID", userId));
            using (var dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    return Read(dr);
                }
            }
            return null;
        }

        public static User Select(SqlConnection c, string name)
        {
            var cmd = new SqlCommand("SELECT * FROM [Users] WHERE [Name]=@NAME", c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@NAME", name));
            using (var dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    return Read(dr);
                }
            }
            return null;
        }

        public static void Delete(SqlConnection c, int userId)
        {
            var cmd = new SqlCommand("DELETE FROM [Users] WHERE [Id]=@ID", c);
            cmd.Parameters.Add(SqlHelper.CreateParam("@ID", userId));
            cmd.ExecuteNonQuery();
        }
    }
}
