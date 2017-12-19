using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DotKit.RESTserver;
using MqttHistoricalServer.Repository;

namespace MqttHistoricalServer
{
    public sealed class DBWebServerSettings
    {
        public WebServerSettings WebServerSettings { get; set; }
        public SQLRepositorySettings SQLRepositorySettings { get; set; }
    }
}
