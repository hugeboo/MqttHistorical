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
    internal sealed class PostPayloadsRequestProcessor :APOSTRequestProcessor<JsonTopicPayloads[]>
    {
        public PostPayloadsRequestProcessor(WebServer webServer)
            : base(webServer)
        {
        }

        public override string GetPathForListener()
        {
            return StaticValues.RestPrefix + "/payloads";
        }

        protected override bool CheckPath(string path)
        {
            var split = path.Split('/');
            return split.Length > 3 &&
                split[split.Length - 3] == "payloads" &&
                !string.IsNullOrEmpty(split[split.Length - 2]) &&
                !string.IsNullOrEmpty(split[split.Length - 1]);
        }

        protected override object Execute(JsonTopicPayloads[] req, IDictionary<string, string> parameters, string path, string userName)
        {
            var response = new JsonServerResponse();

            var psplits = path.Split('/');
            var mqttServer = psplits[psplits.Length - 2];
            var mqttUser = psplits[psplits.Length - 1];

            var rep = (WebServer as DBWebServer).Repository;
            var conn = rep.GetConnections(userName).Where(c => c.Server == mqttServer && c.ConnectionUser == mqttUser).FirstOrDefault();
            if (conn == null)
            {
                response.ResultCode = JsonResultCode.MqttConnectionNotFound;
            }
            else
            {
                foreach(var t in req)
                {
                    var topic = rep.GetTopic(conn.Id, t.TopicName);
                    if (topic == null)
                    {
                        rep.CreateTopic(new Topic() { ConnectionId = conn.Id, Name = t.TopicName });
                        topic = rep.GetTopic(conn.Id, t.TopicName);
                    }
                    foreach (var p in t.Payloads)
                    {
                        rep.CreatePayload(new Payload() { TopicId = topic.Id, Timestamp = p.Timestamp, Data = p.Data });
                    }
                }
            }

            return response;
        }
    }
}
