using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DotKit.RESTserver;
using MqttHistoricalServer.Repository;
using MqttHistoricalUtils;
using MqttHistoricalUtils.Data;

namespace MqttHistoricalServer.RequestProcessors
{
    internal sealed class GetMyConnectionsRequestProcessor : AGETRequestProcessor
    {
        public GetMyConnectionsRequestProcessor(WebServer webServer)
            : base(webServer)
        {
        }

        public override string GetPathForListener()
        {
            return StaticValues.RestPrefix + "/myconnections";
        }

        protected override bool CheckPath(string path)
        {
            return GetPathForListener().Equals(path);
        }

        protected override object Execute(IDictionary<string, string> parameters, string path, string userName)
        {
            var response = new JsonConnectionsServerResponse();

            var rep = (WebServer as DBWebServer).Repository;

            var conn = rep.GetConnections(userName);
            response.Connections = conn.ToArray();

            return response;
        }
    }
}
