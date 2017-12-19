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
    internal sealed class GetPayloadsRequestProcessor : AGETRequestProcessor
    {
        public GetPayloadsRequestProcessor(WebServer webServer)
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
            if (split.Length > 4)
            {
                for (int i = 0; i <= split.Length - 4; i++)
                {
                    if (split[i] == "payloads")
                    {
                        if (!string.IsNullOrEmpty(split[i + 1]) &&
                            !string.IsNullOrEmpty(split[i + 2]) &&
                            !string.IsNullOrEmpty(split[i + 3]))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        protected override object Execute(IDictionary<string, string> parameters, string path, string userName)
        {
            var response = new JsonTopicPayloadsServerResponse();

            var psplits = path.Split('/');
            string mqttServer = null;
            string mqttUser = null;
            string topicName = null;

            for (int i = 0; i <= psplits.Length - 4; i++)
            {
                if (psplits[i] == "payloads")
                {
                    mqttServer = psplits[i+1];
                    mqttUser = psplits[i+2];
                    topicName = string.Join("/", psplits, i + 3, psplits.Length - i - 3);
                    break;
                }
            }

            string s;
            var last = parameters.TryGetValue("last", out s) && s == "1";

            long? startTs = null;
            long? stopTs = null;
            long d;
            if (parameters.TryGetValue("start", out s))
            {
                if (!long.TryParse(s, out d) || d < 0)
                {
                    response.ResultCode = JsonResultCode.InvalidRequest;
                    response.Message = "bad start";
                }
                startTs = d;
            }
            if (parameters.TryGetValue("stop", out s))
            {
                if (!long.TryParse(s, out d) || d < 0)
                {
                    response.ResultCode = JsonResultCode.InvalidRequest;
                    response.Message = "bad stop";
                }
                stopTs = d;
            }
            if (startTs > stopTs)
            {
                response.ResultCode = JsonResultCode.InvalidRequest;
                response.Message = "start>stop";
            }

            if (response.ResultCode != JsonResultCode.OK) return response;

            var rep = (WebServer as DBWebServer).Repository;

            var conn = rep.GetConnections(userName).Where(c => c.Server == mqttServer && c.ConnectionUser == mqttUser).FirstOrDefault();
            if (conn == null)
            {
                response.ResultCode = JsonResultCode.MqttConnectionNotFound;
            }
            else
            {
                if (last)
                {
                    var p = rep.GetPayload(topicName, startTs, stopTs);
                    response.Topics = new[]
                    {
                        new JsonTopicPayloads() {
                            TopicName = topicName,
                            Payloads = new[] { new JsonPayload() { Timestamp = p.Timestamp, Data = p.Data } }
                        }
                    };
                }
                else
                {
                    var pp = rep.GetPayloads(topicName, startTs, stopTs);
                    var arr = pp.Select(p => new JsonPayload() { Timestamp = p.Timestamp, Data = p.Data }).ToArray();
                    response.Topics = new[]
                    {
                        new JsonTopicPayloads() {
                            TopicName = topicName,
                            Payloads = arr
                        }
                    };
                }
            }

            return response;
        }
    }
}
