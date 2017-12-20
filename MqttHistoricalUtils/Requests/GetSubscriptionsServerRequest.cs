using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DotKit.RESTclient;
using MqttHistoricalUtils.Data;

namespace MqttHistoricalUtils.Requests
{
    public sealed class GetSubscriptionsServerRequest : AGETRequest<JsonSubscriptionsServerResponse>
    {
        public string Path { get { return StaticValues.RestPrefix + "/subscriptions"; } }

        public GetSubscriptionsServerRequest(RequestSettings requestSettings)
            : base(requestSettings)
        {
        }

        public JsonSubscriptionsServerResponse Execute(int connId)
        {
            return this.Execute(Path + "?conn=" + connId, null);
        }
    }
}
