using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttHistoricalSubscriber
{
    internal sealed class Subscriber : IDisposable
    {

        public Subscriber(ConnectionInfo ci)
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Update(ConnectionInfo ci)
        {

        }
    }
}
