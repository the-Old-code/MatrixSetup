using System.IO;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Linq;
using System.DirectoryServices.ActiveDirectory;
using System.Threading;
using System;

namespace Installer
{
    /// <summary>
    /// Инициализирует appsettings всех серверов
    /// </summary>
    static class ServersConfig
    {
        /// <summary>
        /// Инициализация ConnectionStrings в appsettings.json для Web Server
        /// </summary>
        public static void InitConnectionStrings(string connectionString, string providerName) 
        {
            string jsonString = File.ReadAllText(InstallScenario.WebServerDistroPath + @"\appsettings.json");
            dynamic jsonObj = JsonConvert.DeserializeObject(jsonString);
            jsonObj["ConnectionStrings"]["connectionString"] = connectionString;
            jsonObj["ConnectionStrings"]["providerName"] = providerName;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(InstallScenario.WebServerDistroPath + @"\appsettings.json", output);
        }
        /// <summary>
        /// Инициализация домена WebServer в его конфигурационном файле и иинциализация домена WebServer в конфигурационных файлах других серверов
        /// </summary>
        public static void InitAllServersUrlConfig(string webServerUrl) 
        {
            SetWebServerApsettingsUrl(webServerUrl);
            SetOtherServersURLConfig(webServerUrl);
        }
        /// <summary>
        /// Меняет поле http-binding в appsettings.json web server
        /// </summary>
        private static void SetWebServerApsettingsUrl(string webServerUrl) 
        {
            string jsonString = File.ReadAllText(InstallScenario.WebServerDistroPath + @"\appsettings.json");
            dynamic jsonObj = JsonConvert.DeserializeObject(jsonString);
            jsonObj["AppSettings"]["http-binding"] = webServerUrl;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(InstallScenario.WebServerDistroPath + @"\appsettings.json", output);
        }
        /// <summary>
        /// Сервера, в config файлах которых нужно поменять ServerUrl
        /// </summary>
        enum ServersForURLConfig { poll, check };
        /// <summary>
        /// Меняет XML config в poll sheck sheduler
        /// Иинциализация домена WebServer в конфигурационных файлах других серверов
        /// </summary>
        private static void SetOtherServersURLConfig(string webServerUrl) 
        {
            Type serversForURLConfig = typeof(ServersForURLConfig);
            XDocument ServerConfig;
            string configPath = default;
            foreach (var server in Enum.GetNames(serversForURLConfig)) 
            {
                switch (server) 
                {
                    case nameof(ServersForURLConfig.poll):
                        configPath = InstallScenario.PollServerConfigPath;
                        break;
                    case nameof(ServersForURLConfig.check):
                        configPath = InstallScenario.CheckServerConfigPath;
                        break;
                    default: throw new NotImplementedException();
                }
            }
            ServerConfig = XDocument.Load(configPath);
            var ServerUrlAttribute = ServerConfig?
                .Element("configuration")?
                .Element("appSettings")?
                .Elements("add")
                .FirstOrDefault(p => p.Attribute("key")?.Value == "serverUrl");
            ServerUrlAttribute.Attribute("value").Value = webServerUrl;
            ServerConfig.Save(configPath);
        }
    }
}
