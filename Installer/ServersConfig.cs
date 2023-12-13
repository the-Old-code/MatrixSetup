using System.IO;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Linq;
using System.DirectoryServices.ActiveDirectory;

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
        public static void InitServersDomain(string domain) 
        {
            SetWebServerApsettings(domain);
            SetServersConfig(domain);
        }
        /// <summary>
        /// Меняет поле http-binding в appsettings.json web server
        /// </summary>
        /// <param name="domain"></param>
        private static void SetWebServerApsettings(string domain) 
        {
            string jsonString = File.ReadAllText(InstallScenario.WebServerDistroPath + @"\appsettings.json");
            dynamic jsonObj = JsonConvert.DeserializeObject(jsonString);
            jsonObj["AppSettings"]["http-binding"] = domain;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(InstallScenario.WebServerDistroPath + @"\appsettings.json", output);
        }
        /// <summary>
        /// Меняет XML config в poll sheck sheduler
        /// </summary>
        private static void SetServersConfig(string domain) 
        {
            XDocument pollServerConfig = XDocument.Load(InstallScenario.PollServerConfigPath);
            XDocument checkServerConfig = XDocument.Load(InstallScenario.CheckServerConfigPath);
            var ServerUrlPoll = pollServerConfig?
                .Element("configuration")?
                .Element("appSettings")?
                .Elements("add")
                .FirstOrDefault(p => p.Attribute("key")?.Value == "serverUrl");
            ServerUrlPoll.Attribute("value").Value = domain;

            var ServerUrlCheck = pollServerConfig?
                .Element("configuration")?
                .Element("appSettings")?
                .Elements("add")
                .FirstOrDefault(p => p.Attribute("key")?.Value == "serverUrl");
            ServerUrlCheck.Attribute("value").Value = domain;

            pollServerConfig.Save(InstallScenario.PollServerConfigPath);
            checkServerConfig.Save(InstallScenario.CheckServerConfigPath);
        }
    }
}
