using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DotKit.RESTclient;
using MqttHistoricalUtils.Data;

namespace MqttHistoricalUtils.Requests
{
    public sealed class GetMyTopicsServerRequest : AGETRequest<JsonTopicsServerResponse>
    {
        public string Path { get { return StaticValues.RestPrefix + "/mytopics"; } }

        public GetMyTopicsServerRequest(RequestSettings requestSettings)
            : base(requestSettings)
        {
        }

        public JsonTopicsServerResponse Execute(int connectionId)
        {
            return this.Execute(Path + "?conn=" + connectionId, null);
        }
    }
}
