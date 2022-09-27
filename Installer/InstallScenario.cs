using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Linq;

namespace Installer
{
    public static class InstallScenario
    {
        public static List<string> MustInstalled = new List<string>()
        {
            "erlang",
            "rabbitmq",
          //"redis",
          //"java",
          //"neo4j"
        };
        public static void AddInstall(string install) 
        {
            MustInstalled.Add(install);
        }
        public static void RemoveInstall(string install) 
        {
            MustInstalled.Remove(install);
        }
        public static void DefaultInstall()
        {
            InstallSelected(MustInstalled);
            

            if(Check().Count != MustInstalled.Count) Reinstall();
        }
        public static List<string> Check()
        {
            List<string> Installs = new List<string>();
            List<string> DetectedInstalls = new List<string>();
            
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

            foreach (string selectInstall in MustInstalled)
            {
                foreach (string install in Installs)
                {
                    if (install.ToLower().IndexOf(selectInstall) != -1)
                    {
                        DetectedInstalls.Add(selectInstall);
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
                                { }
                            }
                        }
                    }
                }
            }
        }
        public static void InstallSelected(List<string> mustSelectInstalled)
        {
            foreach(string select in mustSelectInstalled)
            {
                switch (select)
                {
                    case "erlang":
                        InstallUnit Erlang = new InstallUnit("start otp_win64_24.3.4.exe /S", "Установка Erlang");//установка Erlang через класс InstallUnit
                        Erlang.CmdRun();
                        Environment.SetEnvironmentVariable("ERLANG_HOME", @"C:\Program Files\erl-24.3.4");//установка переменной среды ERLANG_HOME //сделать, чтобы автоматически бралось имя erl
                        break;
                    case "rabbitmq":
                        InstallUnit Rabbit = new InstallUnit("start rabbitmq-server-3.10.1.exe /S", "Установка RabbitMQ");//установка Rabbit через класс InstallUnit
                        Rabbit.CmdRun();

                        InstallUnit RabbitRemove = new InstallUnit(@"pushd C:\Program Files\RabbitMQ Server\rabbitmq_server-3.10.1\sbin" + "\new" + "rabbitmq-service remove", "Переустановка сервиса RabbitMQ");
                        RabbitRemove.CmdRun();
                        
                        InstallUnit RabbitInstall = new InstallUnit(@"pushd C:\Program Files\RabbitMQ Server\rabbitmq_server-3.10.1\sbin" + "\new" + "rabbitmq-service install", "Переустановка сервиса RabbitMQ");
                        RabbitInstall.CmdRun();
                        
                        InstallUnit StopRabbitMQ = new InstallUnit("", "net", "stop RabbitMQ", false, "Перезапуск RabbitMQ");
                        StopRabbitMQ.CmdRun();
                        
                        InstallUnit StartRabbitMQ = new InstallUnit("", "net", "start RabbitMQ", false, "Перезапуск RabbitMQ");
                        StartRabbitMQ.CmdRun();

                        InstallUnit RabbitPlugins = new InstallUnit(@"pushd C:\Program Files\RabbitMQ Server\rabbitmq_server-3.10.1\sbin" + "\new" + "rabbitmq-plugins enable rabbitmq_management", "Запуска плагина RabbitMQ");
                        RabbitPlugins.CmdRun();
                        RabbitRemove.CmdRun();
                        RabbitInstall.CmdRun();
                        StopRabbitMQ.CmdRun();
                        StartRabbitMQ.CmdRun();
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
            List<string> detectedInstalls;
            detectedInstalls = Check();
            List<string> toReinstall;
            toReinstall = (System.Collections.Generic.List<string>)MustInstalled.Except(detectedInstalls);
            InstallSelected(toReinstall);
            if (Check().Count != MustInstalled.Count) Reinstall();
        }
    }
}
