using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DotKit.RESTclient;
using MqttHistoricalUtils.Data;

namespace MqttHistoricalUtils.Requests
{
    public sealed class PostPayloadsServerRequest : APOSTRequest<JsonServerResponse>
    {
        public string Path { get { return StaticValues.RestPrefix + "/payloads"; } }

        public PostPayloadsServerRequest(RequestSettings requestSettings)
            : base(requestSettings)
        {
        }

        public JsonServerResponse Execute(string server, string user, JsonTopicPayloads req)
        {
            return this.Execute(Path + "/" + server + "/" + user, req);
        }
    }
}
