using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttHistoricalUtils.Data
{
    public sealed class Connection
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Server { get; set; }
        public string ConnectionUser { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public int SSLPort { get; set; }
        public bool UseSSL { get; set; }
        public bool Enabled { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Connection;
            if (other == null) return false;
            return Id == other.Id &&
                   UserId == other.UserId &&
                   Server == other.Server &&
                   ConnectionUser == other.ConnectionUser &&
                   Password == other.Password &&
                   Port == other.Port &&
                   SSLPort == other.SSLPort &&
                   UseSSL == other.UseSSL &&
                   Enabled == other.Enabled;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
