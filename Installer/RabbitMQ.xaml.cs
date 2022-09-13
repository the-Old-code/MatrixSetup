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
using System.Threading;
using RabbitMQ.Client;

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
            //InstallUnit Erlang = new InstallUnit("start otp_win64_24.3.4.exe /S");//установка Erlang через класс InstallUnit
            //Erlang.CmdRun();
            //InstallUnit RabbitSet = new InstallUnit(@"pushd C:\Program Files\RabbitMQ Server\rabbitmq_server-3.10.1\sbin" + "\new" + @"set ERLANG_HOME = C:\Program Files\erl-24.3.5");//was erl-24.3.4
            //RabbitSet.CmdRun();
            //Environment.SetEnvironmentVariable("ERLANG_HOME", @"C:\Program Files\erl-24.3.4");
            //InstallUnit Rabbit = new InstallUnit("start rabbitmq-server-3.10.1.exe");//установка Rabbit через класс InstallUnit
            //Rabbit.CmdRun();

            
            //InstallUnit RabbitRemove = new InstallUnit(@"pushd C:\Program Files\RabbitMQ Server\rabbitmq_server-3.10.1\sbin"+"\new"+"rabbitmq-service remove");
            //RabbitRemove.CmdRun();
            //InstallUnit RabbitInstall = new InstallUnit(@"pushd C:\Program Files\RabbitMQ Server\rabbitmq_server-3.10.1\sbin"+"\new"+"rabbitmq-service install");
            //RabbitInstall.CmdRun();
            //Process.Start("net", "stop RabbitMQ")?.WaitForExit();
            //Process.Start("net", "start RabbitMQ")?.WaitForExit();
            
            //InstallUnit RabbitPlugins = new InstallUnit(@"pushd C:\Program Files\RabbitMQ Server\rabbitmq_server-3.10.1\sbin" + "\new" + "rabbitmq-plugins enable rabbitmq_management");
            //RabbitPlugins.CmdRun();
            //RabbitRemove.CmdRun();
            //RabbitInstall.CmdRun();

            Process.Start("net", "stop RabbitMQ")?.WaitForExit();
             Process.Start("net", "start RabbitMQ")?.WaitForExit();
            
        }

        private void BatchReplace(string sourceStr, string destStr) 
        {
            string txtBatFile = Directory.GetCurrentDirectory() + @"\resfiles\run.bat";
            if (File.Exists(txtBatFile))
            {
                StreamReader SR = new StreamReader(txtBatFile);
                string strFileText = SR.ReadToEnd();
                strFileText = strFileText.Replace(sourceStr , destStr);
                SR.Close();
                SR.Dispose();

                StreamWriter Batch = new StreamWriter(txtBatFile);
                Batch.WriteLine(strFileText);
                Batch.Close();
                Batch.Dispose();
            }
        }

        private void next4_btn_Click_1(object sender, RoutedEventArgs e)
        {


            
            


            //Process.Start("net", "stop RabbitMQ")?.WaitForExit();
            //Process.Start("net", "start RabbitMQ")?.WaitForExit();

        }

        private void btn_Kill_Click(object sender, RoutedEventArgs e)
        {
            //BatchReplace("xcopy", "xcopy " + Directory.GetCurrentDirectory() + @"\resfiles\RabbitPrompt");
            //BatchReplace("ProgramDirectory", Directory.GetCurrentDirectory());

            //InstallUnit BatchFile = new InstallUnit("start run.bat");
            //BatchFile.CmdRun();

        }
    }
}
        

        
    