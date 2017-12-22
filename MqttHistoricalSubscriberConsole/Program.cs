using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;

using DotKit.RESTclient;
using MqttHistoricalSubscriber;

namespace MqttHistoricalSubscriberConsole
{
    class Program
    {
        private static SubscribersManager _SubscriberManager;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                var restSettings = new RequestSettings()
                {
                    ServerAddress = Settings.Default.MqttHistoricalAdminServer,
                    UserId = "admin",
                    Password = "12345678",
                    UseBasicAuthorization = true,
                    UseSSL = false,
                    TimeoutMSec = 5000
                };
                _SubscriberManager = new SubscribersManager(restSettings);

                Console.Title = "Mqtt Historical Subscriber (press any key for stop)";
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                logger.Error("Error in main", ex);
            }
            finally
            {
                try { _SubscriberManager.Dispose(); } catch { }
            }
        }
    }
}
