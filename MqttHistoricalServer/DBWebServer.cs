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
    internal sealed class DBWebServer : WebServer, IAuthorizationVerifier
    {
        public IRepository Repository { get; private set; }

        public DBWebServer(DBWebServerSettings settings)
        {
            Repository = new SQLRepository(settings.SQLRepositorySettings);
            this.AuthorizationVerifier = this;
            this.Settings = settings.WebServerSettings;
            this.RegisterRequestProcessor(typeof(GetPayloadsRequestProcessor));
            this.RegisterRequestProcessor(typeof(PostPayloadsRequestProcessor));
        }

        bool IAuthorizationVerifier.Verify(string userId, string password)
        {
            var user = Repository.GetUser(userId);
            return user != null && user.Password == password;
        }
    }
}
