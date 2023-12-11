using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Installer
{
    /// <summary>
    /// Инициализирует appsettings всех серверов
    /// </summary>
    static class ServersAppsettings
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
    }
}
