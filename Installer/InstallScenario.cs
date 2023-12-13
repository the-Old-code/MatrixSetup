using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Installer
{

    //Нужно создать статические свойства для скрипта каждого установочного компонента

    public static class InstallScenario
    {
        private static InstallUnit erlangInstall;//установка Erlang
        private static InstallUnit rabbitInstall;//установка Rabbit
        private static InstallUnit rabbitServiceRemove;//Перезапуск сервиса Rabbit
        private static InstallUnit rabbitServiceInstall;// Перезапуск сервиса Rabbit
        private static InstallUnit startRabbitService;
        private static InstallUnit rabbitPluginsEnable;//Включение плагина rabbit
        private static InstallUnit java;//установка Java
        private static InstallUnit neo4j;//установка Neo4j через класс InstallUnit
        private static InstallUnit webServer;
        private static InstallUnit pollServer;
        private static InstallUnit checkServer;
        private static InstallUnit shedulerServer;

        private static string webServerInstallPath;
        private static string neo4jInstallPath;
        private static string pollServerInstallPath;
        private static string checkServerInstallPath;
        private static string shedulerServerInstallPath;
        private static string webServerConnectionStringConfig;
        private static string webServerDomain;
        private static string defaultConnctionString = "data source=matrix.db;";
        private static ConnectionStringsType connectionStringsType;
        public static ConnectionStringsType WebServerConnectionStringsType
        {
            get
            {
                if (connectionStringsType == default) return ConnectionStringsType.SQLite;
                else return connectionStringsType;
            }
        }

        public static ProgressBarHandler Notify;
        /// <summary>
        /// Инициализация заранее определенных неизменяемых установочных компонентов
        /// </summary>
        static InstallScenario() 
        {
            erlangInstall = new InstallUnit(new List<string> { "start " + ErlangDistroFileName + " /S" }, "Установка Erlang");//установка Erlang
            rabbitInstall = new InstallUnit(new List<string> { "start " + RabbitmqDistroFileName + " /S" }, "Установка RabbitMQ");//установка Rabbit
            rabbitServiceRemove = new InstallUnit(new List<string> { @"pushd " + RabbitMQSbinPath, "rabbitmq-service remove" }, "Переустановка сервиса RabbitMQ");//Удаление сервиса Rabbit
            rabbitServiceInstall = new InstallUnit(new List<string> { @"pushd " + RabbitMQSbinPath, "rabbitmq-service install" }, "Переустановка сервиса RabbitMQ");// Устновка сервиса Rabbit
            startRabbitService = new InstallUnit(null, "net", "start RabbitMQ", false, "Перезапуск RabbitMQ");
            rabbitPluginsEnable = new InstallUnit(new List<string> { @"pushd " + RabbitMQSbinPath, "rabbitmq-plugins enable rabbitmq_management" }, "Включение плагина RabbitMQ");//Включение плагина rabbit
            java = new InstallUnit(new List<string> { @"jre-8u321-windows-x64.exe INSTALLCFG=%cd%\config.cfg" }, "Установка Java");//установка Java
        }
        /// <summary>
        /// Все установочные компоненты
        /// </summary>
        public enum InstallComponent 
        {
            erlang,
            rabbitmq,
            redis,
            java,
            neo4j,
            web,
            poll,
            check,
            sheduler
        }
        /// <summary>
        /// Виды ConnectionStrings для разных СУБД
        /// </summary>
        public enum ConnectionStringsType
        {
            MsSQL,
            PostgreSQL,
            SQLite
        }
        private enum InstallPathType 
        {
            neo4j,
            webserver,
            pollserver,
            checkserver,
            shedulerserver,
        }
        #region DistroFile&DirectoriesNames
        /// <summary>
        /// Возвращает имя файла дистрибутива rabbitmq в resfiles
        /// Используется только, чтобы обратиться к дистрибутиву в resfiles
        /// </summary>
        private static string RabbitmqDistroFileName
        {
            get
            {
                List<string> files = new List<string>(Directory.EnumerateFiles(ResfilesPath, "rabbitmq-server*", SearchOption.TopDirectoryOnly));
                return Path.GetFileName(files[0]);
            }
        }
        /// <summary>
        /// Возвращает имя файла дистрибутива erlang в resfiles
        /// Используется только, чтобы обратиться к дистрибутиву в resfiles
        /// </summary>
        private static string ErlangDistroFileName
        {
            get
            {
                List<string> files = new List<string>(Directory.EnumerateFiles(ResfilesPath, "otp_win*", SearchOption.TopDirectoryOnly));
                return Path.GetFileName(files[0]);
            }
        }
        /// <summary>
        /// Возвращает имя директории дистрибутива web server
        /// Используется, чтобы обратиться к дистрибутиву в resfiles и чтобы скопировать его в конечную директорию
        /// </summary>
        private static string WebServerDirectoryName 
        {
            get 
            {
                return "web4.4.6";
            }
        }
        /// <summary>
        /// Возвращает имя директории дистрибутива neo4j
        /// Используется, чтобы обратиться к дистрибутиву в resfiles и чтобы скопировать его в конечную директорию
        /// </summary>
        private static string Neo4jDirectoryName 
        {
            get 
            {
                return "neo4j-community-3.2.1";
            }
        }
        /// <summary>
        /// Возвращает имя директории дистрибутива PollServer
        /// Используется, чтобы обратиться к дистрибутиву в resfiles и чтобы скопировать его в конечную директорию
        /// </summary>
        private static string PollServerDirectoryName 
        {
            get 
            {
                return "poll";
            }
        }
        /// <summary>
        /// Возвращает имя директории дистрибутива CheckServer
        /// Используется, чтобы обратиться к дистрибутиву в resfiles и чтобы скопировать его в конечную директорию
        /// </summary>
        private static string CheckServerDirectoryName 
        {
            get 
            {
                return "check";
            }
        }
        /// <summary>
        /// Возвращает имя директории дистрибутива ShedulerServer
        /// Используется, чтобы обратиться к дистрибутиву в resfiles и чтобы скопировать его в конечную директорию
        /// </summary>
        private static string ShedulerServerDirectoryName 
        {
            get 
            {
                return "sheduler";
            }
        }
        #endregion
        /// <summary>
        /// Возвращает путь к Program Files системного диска
        /// </summary>
        private static string SystemProgramFilesFolder 
        {
            get 
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            }
        }
        /// <summary>
        /// Возвращает путь к директории resfiles, в которой находятся все дистрибутивы установочных компонентов
        /// </summary>
        public static string ResfilesPath 
        {
            get 
            {
                return Directory.GetCurrentDirectory() + @"\resfiles";
            } 
        }
        /// <summary>
        /// Возвращает локацию директории c уже установленной RabbitMQ, RabbitMQ устанаваливается в [Системный диск]:\ProgramFiles\RabbitMQ Server\rabbitmq_server-3.10.1\sbin
        /// Т.е. содержит путь, в который нужно установить RabbitMQ 
        /// </summary>
        private static string RabbitMQSbinPath 
        {
            get 
            {
                return SystemProgramFilesFolder + @"\RabbitMQ Server\rabbitmq_server-3.10.1\sbin";
            }
        }
        #region ChangablePaths
        /// <summary>
        /// Возвращает и устанавливает путь к дистрибутиву neo4j в resfiles
        /// </summary>
        public static string Neo4jInstallPath
        {
            set
            {
                SetInstallPath(InstallPathType.neo4j, value);
            }
            get
            {
                if (neo4jInstallPath == default) //по умолчанию корневая папка несистемного или Program Files, если только один диск(системный) на машине
                {
                    List<DriveInfo> avaible = new List<DriveInfo>();
                    DriveInfo[] d = DriveInfo.GetDrives();
                    string sysdrive = Path.GetPathRoot(Environment.SystemDirectory);
                    foreach (DriveInfo drive in d)
                    {
                        if ((drive.Name != sysdrive) && (drive.DriveType == DriveType.Fixed)) avaible.Add(drive);//получение всех дисков, кроме системных
                    }
                    if (avaible.Count == 0) neo4jInstallPath = SystemProgramFilesFolder + @"\Matrix\" + Neo4jDirectoryName;
                    else neo4jInstallPath = avaible[0].ToString() + @"\Matrix\" + Neo4jDirectoryName;

                    return neo4jInstallPath;
                }
                else return neo4jInstallPath;

            }
        }
        /// <summary>
        /// возвращает и устанавливает путь, в который нужно установить webserver
        /// </summary>
        public static string WebServerInstallPath
        {
            set
            {
                SetInstallPath(InstallPathType.webserver, value);
            }
            get
            {
                return GetInstallPath(InstallPathType.webserver);
            }
        }
        /// <summary>
        /// Возвращает и устанавливает путь, в который нужно установить Pollserver
        /// </summary>
        public static string PollServerInstallPath 
        {
            set 
            {
                SetInstallPath(InstallPathType.pollserver, value);
            }
            get 
            {
                return GetInstallPath(InstallPathType.pollserver);
            }
        }
        /// <summary>
        /// Возвращает и устанавливает путь, в который нужно установить Checkserver
        /// </summary>
        public static string CheckServerInstallPath
        {
            set
            {
                SetInstallPath(InstallPathType.checkserver, value);
            }
            get
            {
                return GetInstallPath(InstallPathType.checkserver);
            }
        }
        /// <summary>
        /// Возвращает и устанавливает путь, в который нужно установить Shedulerserver
        /// </summary>
        public static string ShedulerServerInstallPath 
        {
            set
            {
                SetInstallPath(InstallPathType.shedulerserver, value);
            }
            get
            {
                return GetInstallPath(InstallPathType.shedulerserver);
            }
        }
        #endregion
        #region PathsOfDistributives
        /// <summary>
        /// Путь к установочному дистрибутиву neo4j в resfiles
        /// </summary>
        private static string Neo4jDistroPath
        {
            get
            {
                return ResfilesPath + @"\" + Neo4jDirectoryName;
            }
        }
        /// <summary>
        /// Путь к установочнуому дистрибутиву Web Server в resfiles
        /// </summary>
        public static string WebServerDistroPath 
        {
            get 
            {
                return ResfilesPath + @"\" + WebServerDirectoryName;   
            }
        }
        /// <summary>
        /// Путь к установочнуому дистрибутиву Poll Server в resfiles
        /// </summary>
        public static string PollServerDistroPath 
        {
            get
            {
                return ResfilesPath + @"\" + PollServerDirectoryName;
            }
        }
        /// <summary>
        /// Путь к установочнуому дистрибутиву Check Server в resfiles
        /// </summary>
        public static string CheckServerDistroPath 
        {
            get
            {
                return ResfilesPath + @"\" + CheckServerDirectoryName;
            }
        }
        /// <summary>
        /// Путь к установочнуому дистрибутиву Sheduler Server в resfiles
        /// </summary>
        public static string ShedulerServerDistroPath 
        {
            get
            {
                return ResfilesPath + @"\" + ShedulerServerDirectoryName;
            }
        }
        #endregion
        #region ServerConfigPaths
        public static string WebServerConfigPath 
        {
            get 
            {
                return WebServerDistroPath + @"\appsettings.json";
            }
        }
        public static string PollServerConfigPath
        {
            get
            {
                return PollServerDistroPath + @"\Matrix.PollServer.exe.config";
            }
        }
        public static string CheckServerConfigPath
        {
            get
            {
                return CheckServerDistroPath + @"\Matrix.CheckServer.exe.config";
            }
        }
        #endregion
        #region WebServerConfiguration
        public static string WebServerConnectionString 
        {
            get 
            {
                if (webServerConnectionStringConfig == default) return defaultConnctionString;
                return webServerConnectionStringConfig;
            }
        }
        public static void InitializeWebServerConnectionStrings(ConnectionStringsType type, string connectionString)
        {
            connectionStringsType = type;
            webServerConnectionStringConfig = connectionString;
        }
        #endregion

        public static List<InstallComponent> MustInstalled = new List<InstallComponent>();
        public static void AddInstall(InstallComponent installComponent)
        {
            if (!MustInstalled.Contains(installComponent)) 
            {
                if(installComponent == InstallComponent.java || installComponent == InstallComponent.erlang) MustInstalled.Insert(0, installComponent);
                else MustInstalled.Add(installComponent);
            }
            
        }
        public static void RemoveInstall(InstallComponent installComponent)
        {
            MustInstalled.Remove(installComponent);
        }
        public static void DefaultInstall()
        {
            //InstallSelected(MustInstalled.Except(Check()).ToList());
            InstallSelected(MustInstalled);
            //if (Check().Count != MustInstalled.Count) Reinstall();
        }
        public static List<InstallComponent> Check()
        {
            List<string> Installs = new List<string>();
            List<InstallComponent> DetectedInstalls = new List<InstallComponent>();

            List<string> keys = new List<string>()
            {
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
            };

            // The RegistryView.Registry64 forces the application to open the registry as x64 even if the application is compiled as x86 
            FindInstalls(RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64), keys, Installs);
            FindInstalls(RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64), keys, Installs);

            Installs = Installs.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            Installs.Sort(); // The list of ALL installed applications

            foreach (var selectInstall in MustInstalled)
            {
                foreach (var install in Installs)
                {
                    if (install.ToLower().IndexOf(selectInstall.ToString()) != -1)
                    {
                        DetectedInstalls.Add(selectInstall);//добавление распознанных программ в список программ
                    }
                }
            }

            return DetectedInstalls;

            void FindInstalls(RegistryKey regKey, List<string> keys, List<string> installed)
            {
                foreach (string key in keys)
                {
                    using (RegistryKey rk = regKey.OpenSubKey(key))
                    {
                        if (rk == null)
                        {
                            continue;
                        }
                        foreach (string skName in rk.GetSubKeyNames())
                        {
                            using (RegistryKey sk = rk.OpenSubKey(skName))
                            {
                                try
                                {
                                    installed.Add(Convert.ToString(sk.GetValue("DisplayName")));
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex.ToString());
                                    MessageBox.Show(ex.ToString());
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void InstallSelected(List<InstallComponent> selected)
        {
            InstallUnit.Notify = Notify;
            foreach (var select in selected)
            {
                switch (select)
                {
                    case InstallComponent.erlang:
                        erlangInstall.ExecuteInstallationScriptForJavaErlang();//TODO: Разобраться почему при RedirectStandartOutput = false сразу запускается установка Rabbit, не дожидаясь установки Erlang
                        Environment.SetEnvironmentVariable("ERLANG_HOME", @"C:\Program Files\erl-24.3.4");//установка переменной среды ERLANG_HOME //сделать, чтобы автоматически бралось имя erl
                        break;
                    case InstallComponent.rabbitmq:

                        rabbitInstall.ExecuteInstallationScript();
                        rabbitServiceRemove.ExecuteInstallationScript();
                        rabbitServiceInstall.ExecuteInstallationScript();

                        bool IsInstalled = false;
                        while (!IsInstalled)
                        {
                            rabbitPluginsEnable.ExecuteInstallationScript();
                            startRabbitService.ExecuteInstallationScript();
                            try
                            {
                                string url = "http://localhost:15672";//порт раббита
                                // создание web запроса на указанный url. 
                                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                                // создается запрос и дожидается получения ответа.
                                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();//получение того ответа
                                if (myHttpWebResponse.StatusCode == HttpStatusCode.OK) IsInstalled = true;

                                myHttpWebResponse.Close();
                            }
                            catch (WebException)//если сервер не отвечает
                            {
                                IsInstalled = false;
                            }
                            catch (Exception)//если url некорректный или подобное
                            {
                                IsInstalled = false;
                            }
                        }
                        Process.Start("explorer.exe", @"http://localhost:15672");
                        break;
                    case InstallComponent.java:
                        java.ExecuteInstallationScriptForJavaErlang();
                        /*
                        var name = "Path";
                        var scope = EnvironmentVariableTarget.Machine;
                        var odlvalue = Environment.GetEnvironmentVariable(name,scope);
                        var newvalue = odlvalue + @";C:\Program Files\Java\";
                        Environment.SetEnvironmentVariable(name, newvalue, scope);*/
                        break;
                    case InstallComponent.neo4j:
                        neo4j = new InstallUnit(new List<string> { @"cd " + Neo4jDistroPath, @"xcopy %cd%\ " + @"""" + Neo4jInstallPath + @"""" + " /s /q", "pushd " + Neo4jInstallPath + @"\bin", "neo4j install-service", "neo4j start" }, "Установка Neo4j");
                        neo4j.ExecuteInstallationScript();
                        break;
                    case InstallComponent.web:
                        InitConnectionString();
                        webServer = new InstallUnit(new List<string> { @"cd " + WebServerDistroPath, @"xcopy %cd%\ " + @"""" + WebServerInstallPath + @"""" + " /s /q" }, "Установка Web Server");
                        webServer.ExecuteInstallationScript();
                        break;
                    case InstallComponent.poll:
                        pollServer = new InstallUnit(new List<string> { @"cd " + PollServerDistroPath, @"xcopy %cd%\ " + @"""" + PollServerInstallPath + @"""" + " /s /q" }, "Установка Poll Server");
                        pollServer.ExecuteInstallationScript();
                        break;
                    case InstallComponent.check:
                        checkServer = new InstallUnit(new List<string> { @"cd " + CheckServerDistroPath, @"xcopy %cd%\ " + @"""" + CheckServerInstallPath + @"""" + " /s /q" }, "Установка Check Server");
                        checkServer.ExecuteInstallationScript();
                        break;
                    case InstallComponent.sheduler:
                        shedulerServer = new InstallUnit(new List<string> { @"cd " + ShedulerServerDistroPath, @"xcopy %cd%\ " + @"""" + ShedulerServerInstallPath + @"""" + " /s /q" }, "Установка Sheduler Server");
                        shedulerServer.ExecuteInstallationScript();
                        break;
                }
            }
            Task wait = new Task(() =>
            {
                Thread.Sleep(5000);
            });
            wait.Start();
            wait.Wait();
        }
        public static void Reinstall()
        {
            List<InstallComponent> detectedInstalls;
            detectedInstalls = Check();
            List<InstallComponent> toReinstall;
            toReinstall = (List<InstallComponent>)MustInstalled.Except(detectedInstalls);//не работает
            InstallSelected(toReinstall);
            if (Check().Count != MustInstalled.Count) Reinstall();
        }
        private static void InitConnectionString() 
        {
            switch (connectionStringsType) 
            {
                case ConnectionStringsType.PostgreSQL:
                    ServersConfig.InitConnectionStrings(WebServerConnectionString, "Npgsql");
                    break;
                case ConnectionStringsType.SQLite:
                    ServersConfig.InitConnectionStrings(WebServerConnectionString, "sqlite");
                    break;
                case ConnectionStringsType.MsSQL:
                    ServersConfig.InitConnectionStrings(WebServerConnectionString, "System.Data.SqlClient");
                    break;
            }
        }
        private static void SetInstallPath(InstallPathType type, string path)
        {
            if (ValidatePath(path)) 
            {
                switch(type) 
                {
                    case InstallPathType.webserver:
                        webServerInstallPath = path + @"\" + WebServerDirectoryName;
                        break;
                    case InstallPathType.neo4j:
                        neo4jInstallPath = path + @"\" + Neo4jDirectoryName;
                        break;
                    case InstallPathType.pollserver:
                        pollServerInstallPath = path + @"\" + PollServerDirectoryName;
                        break;
                    case InstallPathType.checkserver:
                        checkServerInstallPath = path + @"\" + CheckServerDirectoryName;
                        break;
                    case InstallPathType.shedulerserver:
                        shedulerServerInstallPath = path + @"\" + ShedulerServerDirectoryName;
                        break;
                    default: throw new NotImplementedException();
                }
            }

        }
        private static string GetInstallPath(InstallPathType type) 
        {
            string path = type switch 
            {
                InstallPathType.webserver => webServerInstallPath,
                InstallPathType.pollserver => pollServerInstallPath,
                InstallPathType.checkserver => checkServerInstallPath,
                InstallPathType.shedulerserver => shedulerServerInstallPath,
                _=> throw new NotImplementedException()
            };
            if (path == default) 
            {
                switch (type) 
                {
                    case InstallPathType.webserver: return SystemProgramFilesFolder + @"\Matrix\" + WebServerDirectoryName;//по умлочанию web server устанваливается в Program Files
                    case InstallPathType.pollserver: return SystemProgramFilesFolder + @"\Matrix\" + PollServerDirectoryName;//по умлочанию poll server устанваливается в Program Files
                    case InstallPathType.checkserver: return SystemProgramFilesFolder + @"\Matrix\" + CheckServerDirectoryName;//по умлочанию check server устанваливается в Program Files
                    case InstallPathType.shedulerserver: return SystemProgramFilesFolder + @"\Matrix\" + ShedulerServerDirectoryName; //по умлочанию sheduler server устанваливается в Program Files
                    default: throw new NotImplementedException();
                }
            }
            else return path;
        }
        private static bool ValidatePath(string path) 
        {
            if (path == "") throw new DirectoryNotFoundException("Пустая строка");
            else if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException("Не существующая папка");
            }
            return true;
        }
    }
}
