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
    internal class DBWebServer : WebServer, IAuthorizationVerifier
    {
        public IDBRepository Repository { get; private set; }

        public DBWebServer(DBWebServerSettings settings)
        {
            Repository = new SQLDBRepository(settings.SQLRepositorySettings);
            this.AuthorizationVerifier = this;
            this.Settings = settings.WebServerSettings;
            this.RegisterRequestProcessor(typeof(GetPayloadsRequestProcessor));
        }

        bool IAuthorizationVerifier.Verify(string userId, string password)
        {
            var user = Repository.GetUser(userId);
            return user != null && user.Enabled && user.Password == password;
        }
    }
}
