using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace Installer
{
    /// <summary>
    /// Interaction logic for RabbitMQ.xaml
    /// </summary>
    public partial class RabbitMQ : Page
    {
        public RabbitMQ()
        {
            InitializeComponent();
        }

        private void btn_RabbitInstall_Click(object sender, RoutedEventArgs e)
        {
            InstallUnit Erlang = new InstallUnit("start otp_win64_24.3.4.exe /S");//установка Erlang через класс InstallUnit
            Erlang.CmdInstall();

            InstallUnit Rabbit = new InstallUnit("start rabbitmq-server-3.10.1.exe", System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\resfiles\\rabbitmq-server-3.10.1.exe","");//установка Rabbit через класс InstallUnit
            Rabbit.CmdInstall();

            //InstallUnit RabbitPrompt = new InstallUnit("","cmd.exe", "/k cd /d C:\\Program Files\\RabbitMQ Server\\rabbitmq_server-3.10.1\\sbin",true, "rabbitmq-plugins enable rabbitmq_management");
            //RabbitPrompt.CmdInstall();
        }

        

        private void next4_btn_Click_1(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("----------------------");
            Process[] processes = Process.GetProcesses();
            foreach (Process p in processes)
            {
                if (!String.IsNullOrEmpty(p.MainWindowTitle))
                {
                    Debug.WriteLine(p.MainWindowTitle);
                }
            }
        }

        
    }
}