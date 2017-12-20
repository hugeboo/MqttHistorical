using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DotKit.RESTclient;
using MqttHistoricalUtils.Data;

namespace MqttHistoricalUtils.Requests
{
    public sealed class GetConnectionsServerRequest : AGETRequest<JsonConnectionsServerResponse>
    {
        public string Path { get { return StaticValues.RestPrefix + "/connections"; } }

        public GetConnectionsServerRequest(RequestSettings requestSettings)
            : base(requestSettings)
        {
        }

        public JsonConnectionsServerResponse Execute(int userId)
        {
            return this.Execute(Path + "?user=" + userId, null);
        }
    }
}
