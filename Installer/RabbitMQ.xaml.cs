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

            InstallUnit Rabbit = new InstallUnit("start rabbitmq-server-3.10.1.exe");//установка Rabbit через класс InstallUnit
            Rabbit.CmdInstall();

            InstallUnit RabbitPrompt = new InstallUnit("","cmd.exe", "",true, "cd /d C:\\Program Files\\RabbitMQ Server\\rabbitmq_server-3.10.1\\sbin\nrabbitmq-plugins enable rabbitmq_management");
            RabbitPrompt.CmdInstall();
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

        private void testrabbitprompt_btn_Click(object sender, RoutedEventArgs e)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";//изменяемое// обычно это "cmd.exe"
            cmd.StartInfo.Verb = "runas";//права администратора
            cmd.StartInfo.Arguments = "";//изменяемое// обычно ""
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = false;//выполнение без открытия окна // обычно true
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
        }
    }
}