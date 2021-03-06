﻿using System;
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
    internal sealed class GetSubscriptionsRequestProcessor : AGETRequestProcessor
    {
        public GetSubscriptionsRequestProcessor(WebServer webServer)
            : base(webServer)
        {
        }

        public override string GetPathForListener()
        {
            return StaticValues.RestPrefix + "/subscriptions";
        }

        protected override bool CheckPath(string path)
        {
            return GetPathForListener().Equals(path);
        }

        protected override object Execute(IDictionary<string, string> parameters, string path, string userName)
        {
            var response = new JsonSubscriptionsServerResponse();

            int connId;
            if (!parameters.TryGetValue("conn", out string s) || !int.TryParse(s, out connId))
            {
                response.ResultCode = JsonResultCode.InvalidRequest;
                response.Message = "Missing conn arg";
                return response;
            }

            var rep = (WebServer as DBWebServer).Repository;

            var conn = rep.GetSubscriptions(connId);
            response.Subscriptions = conn.ToArray();

            return response;
        }
    }
}
