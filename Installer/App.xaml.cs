using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace Installer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            /*if (e.Args.Length !=0) 
            {
                if (e.Args[0] == "rbtpr")
                {
                    InstallUnit RabbitPrompt = new InstallUnit("", "cmd.exe", "", true, "pushd C:\\Program Files\\RabbitMQ Server\\rabbitmq_server-3.10.1\\sbin\nrabbitmq-plugins enable rabbitmq_management");//выполнение нужных комманд в cmd
                    RabbitPrompt.CmdRun();
                    Process.Start("net", "stop RabbitMQ")?.WaitForExit();
                    Process.Start("net", "start RabbitMQ")?.WaitForExit();
                    
                }
            }*/
            MainWindow wnd = new MainWindow();
            wnd.Show();
            
        }

    }
}
