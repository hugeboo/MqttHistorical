using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DotKit.RESTserver;
using DotKit.RESTutils;
using MqttHistoricalServer.Repository;
using MqttHistoricalServer.RequestProcessors;

namespace MqttHistoricalServer
{
    internal sealed class DBAdminWebServer : DBWebServer, IAuthorizationVerifier
    {
        public DBAdminWebServer(DBWebServerSettings settings)
            :base(settings)
        {
            this.RegisterRequestProcessor(typeof(PostPayloadsRequestProcessor));
            this.RegisterRequestProcessor(typeof(GetUsersRequestProcessor));
            this.RegisterRequestProcessor(typeof(GetConnectionsRequestProcessor));
            this.RegisterRequestProcessor(typeof(GetSubscriptionsRequestProcessor));
        }

        bool IAuthorizationVerifier.Verify(string userId, string password)
        {
            return true;
        }
    }
}
