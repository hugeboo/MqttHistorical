using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttHistoricalServer.Repository
{
    public sealed class SQLDBRepositorySettings : ICloneable
    {
        public string SQLServerAddr { get; set; }
        public int SQLServerPort { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as SQLDBRepositorySettings;
            if (other == null) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public SQLDBRepositorySettings Clone2()
        {
            return Clone() as SQLDBRepositorySettings;
        }
    }
}
