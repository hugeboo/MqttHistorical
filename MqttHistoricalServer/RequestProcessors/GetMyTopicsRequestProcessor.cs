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
    internal sealed class GetMyTopicsRequestProcessor : AGETRequestProcessor
    {
        public GetMyTopicsRequestProcessor(WebServer webServer)
            : base(webServer)
        {
        }

        public override string GetPathForListener()
        {
            return StaticValues.RestPrefix + "/mytopics";
        }

        protected override bool CheckPath(string path)
        {
            return GetPathForListener().Equals(path);
        }

        protected override object Execute(IDictionary<string, string> parameters, string path, string userName)
        {
            var response = new JsonTopicsServerResponse();

            int connId;
            if (!parameters.TryGetValue("conn", out string s) || !int.TryParse(s, out connId))
            {
                response.ResultCode = JsonResultCode.InvalidRequest;
                response.Message = "Missing conn arg";
                return response;
            }

            var rep = (WebServer as DBWebServer).Repository;

            var conn = rep.GetConnection(connId);
            var user = rep.GetUser(conn.UserId);

            if (user.Name != userName)
            {
                response.ResultCode = JsonResultCode.InvalidRequest;
                response.Message = "Invalid connection id";
                return response;
            }

            var topics = rep.GetTopics(connId);
            response.Topics = topics.ToArray();

            return response;
        }
    }
}
