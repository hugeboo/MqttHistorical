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
    }
}
