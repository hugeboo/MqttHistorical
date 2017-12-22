using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;

using DotKit.RESTserver;
using MqttHistoricalServer;
using MqttHistoricalServer.Repository;

namespace MqttHistoricalServerConsole
{
    class Program
    {
        private static DBWebServer _WebServer;
        private static DBAdminWebServer _AdminWebServer;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                var sqls = new SQLDBRepositorySettings()
                {
                    SQLServerAddr = Settings.Default.SQLServerAddr,
                    SQLServerPort = Settings.Default.SQLServerPort,
                    Database = Settings.Default.Database,
                    User = Settings.Default.DBUser,
                    Password = Settings.Default.DBPassword
                };

                _WebServer = new DBWebServer(new DBWebServerSettings()
                {
                    SQLRepositorySettings = sqls,
                    WebServerSettings = new WebServerSettings()
                    {
                        UseBasicAuthorization = true,
                        UseSSL = false,
                        Port = Settings.Default.WebServerPort
                    }
                });
                _WebServer.StatusChanged += WebServer_StatusChanged;
                _WebServer.Start();

                _AdminWebServer = new DBAdminWebServer(new DBWebServerSettings()
                {
                    SQLRepositorySettings = sqls,
                    WebServerSettings = new WebServerSettings()
                    {
                        UseBasicAuthorization = true,
                        UseSSL = false,
                        Port = Settings.Default.AdminWebServerPort
                    }
                });
                _AdminWebServer.StatusChanged += AdminWebServer_StatusChanged;
                _AdminWebServer.Start();

                Console.Title = "Mqtt Historical Servers (press any key for stop)";
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                logger.Error("Error in main", ex);
            }
            finally
            {
                try { _WebServer.Dispose(); } catch { }
                try { _AdminWebServer.Dispose(); } catch { }
            }
        }

        private static void WebServer_StatusChanged(object sender, WebServer.StatusChangedEventArgs e)
        {
            Console.WriteLine($"   WebServer {_WebServer.Settings.Port} -> {e.Status} {e.Message}");
        }

        private static void AdminWebServer_StatusChanged(object sender, WebServer.StatusChangedEventArgs e)
        {
            Console.WriteLine($"   AdminWebServer {_AdminWebServer.Settings.Port} -> {e.Status} {e.Message}");
        }
    }
}
