using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DotKit.RESTclient;
using MqttHistoricalUtils.Data;

namespace MqttHistoricalUtils.Requests
{
    public sealed class GetMyConnectionsServerRequest : AGETRequest<JsonConnectionsServerResponse>
    {
        public string Path { get { return StaticValues.RestPrefix + "/myconnections"; } }

        public GetMyConnectionsServerRequest(RequestSettings requestSettings)
            : base(requestSettings)
        {
        }

        public JsonConnectionsServerResponse Execute()
        {
            return this.Execute(Path, null);
        }
    }
}
