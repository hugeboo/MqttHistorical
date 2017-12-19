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
    internal sealed class GetUsersRequestProcessor : AGETRequestProcessor
    {
        public GetUsersRequestProcessor(WebServer webServer)
            : base(webServer)
        {
        }

        public override string GetPathForListener()
        {
            return StaticValues.RestPrefix + "/users";
        }

        protected override bool CheckPath(string path)
        {
            return GetPathForListener().Equals(path);
        }

        protected override object Execute(IDictionary<string, string> parameters, string path, string userName)
        {
            var response = new JsonUsersServerResponse();

            var rep = (WebServer as DBWebServer).Repository;

            var conn = rep.GetUsers();
            response.Users = conn.ToArray();

            return response;
        }
    }
}
