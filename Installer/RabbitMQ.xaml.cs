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
            InstallUnit Erlang = new InstallUnit("start otp_win64_24.3.4.exe /S");//установка Erlang через класс InstallUnit
            Erlang.CmdInstall();
            InstallUnit Rabbit = new InstallUnit("start rabbitmq-server-3.10.1.exe");//установка Rabbit через класс InstallUnit
            Rabbit.CmdInstall();

            //BatchReplace("xcopy", "xcopy " + Directory.GetCurrentDirectory() + @"\resfiles\RabbitPrompt");
            //BatchReplace("ProgramDirectory" , Directory.GetCurrentDirectory());

            InstallUnit BatchFile = new InstallUnit("start run.bat");
            BatchFile.CmdInstall();
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



            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd";//изменяемое// обычно это "cmd.exe"
            cmd.StartInfo.Verb = "runas";//права администратора
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = false;//выполнение без открытия окна // обычно true
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();


            cmd.StandardInput.WriteLine("pushd " + Directory.GetCurrentDirectory() + "\nstart Installer.exe rbtpr");//переход в директорию resfiles и ввод комманд в командную строку или другую программу
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();

            //Process.Start("net", "stop RabbitMQ")?.WaitForExit();
            //Process.Start("net", "start RabbitMQ")?.WaitForExit();

        }

        private void btn_Kill_Click(object sender, RoutedEventArgs e)
        {
            //BatchReplace("xcopy", "xcopy " + Directory.GetCurrentDirectory() + @"\resfiles\RabbitPrompt");
            //BatchReplace("ProgramDirectory", Directory.GetCurrentDirectory());

            InstallUnit BatchFile = new InstallUnit("start run.bat");
            BatchFile.CmdInstall();

        }
    }
}
        

        
    