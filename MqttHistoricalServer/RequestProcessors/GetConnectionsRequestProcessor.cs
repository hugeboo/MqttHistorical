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
    internal sealed class GetConnectionsRequestProcessor : AGETRequestProcessor
    {
        public GetConnectionsRequestProcessor(WebServer webServer)
            : base(webServer)
        {
        }

        public override string GetPathForListener()
        {
            return StaticValues.RestPrefix + "/connections";
        }

        protected override bool CheckPath(string path)
        {
            return GetPathForListener().Equals(path);
        }

        protected override object Execute(IDictionary<string, string> parameters, string path, string userName)
        {
            var response = new JsonConnectionsServerResponse();

            int userId;
            if (!parameters.TryGetValue("user", out string s) || !int.TryParse(s,out userId))
            {
                response.ResultCode = JsonResultCode.InvalidRequest;
                response.Message = "Missing user";
                return response;
            }

            var rep = (WebServer as DBWebServer).Repository;

            var conn = rep.GetConnections(userId);
            response.Connections = conn.ToArray();

            return response;
        }
    }
}
