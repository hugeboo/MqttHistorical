using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DotKit.RESTclient;
using MqttHistoricalUtils.Data;

namespace MqttHistoricalUtils.Requests
{
    public sealed class GetUsersServerRequest : AGETRequest<JsonUsersServerResponse>
    {
        public string Path { get { return StaticValues.RestPrefix + "/users"; } }

        public GetUsersServerRequest(RequestSettings requestSettings)
            : base(requestSettings)
        {
        }

        public JsonUsersServerResponse Execute()
        {
            return this.Execute(Path, null);
        }
    }
}
