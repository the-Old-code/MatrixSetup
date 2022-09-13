using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace Installer
{
    /// <summary>
    /// Interaction logic for RabbitMQStartPage.xaml
    /// </summary>
    public partial class RabbitMQStartPage : Window
    {
        public RabbitMQStartPage()
        {
            InitializeComponent();
        }
        public void UpdateProgressBar(string taskName, int addToBar) 
        {
            prgbar_taskProgress.Value += addToBar;
            lbl_taskProgress.Content = "Выполняется: " + taskName;
        }
        private void btn_RabbitInstall_Click(object sender, RoutedEventArgs e)
        {
            
            
            InstallUnit Erlang = new InstallUnit("start otp_win64_24.3.4.exe /S", "Установка Erlang");//установка Erlang через класс InstallUnit
            Erlang.CmdRun();

            Environment.SetEnvironmentVariable("ERLANG_HOME", @"C:\Program Files\erl-24.3.4");//установка переменной среды ERLANG_HOME //сделать, чтобы автоматически бралось имя erl 
            UpdateProgressBar("Установка RabbitMQ",5); 
            InstallUnit Rabbit = new InstallUnit("start rabbitmq-server-3.10.1.exe /S", "Установка RabbitMQ");//установка Rabbit через класс InstallUnit
            Rabbit.CmdRun();
                
            UpdateProgressBar("Переустановка сервиса RabbitMQ", 5);
            InstallUnit RabbitRemove = new InstallUnit(@"pushd C:\Program Files\RabbitMQ Server\rabbitmq_server-3.10.1\sbin" + "\new" + "rabbitmq-service remove", "Переустановка сервиса RabbitMQ");
            RabbitRemove.CmdRun();
            UpdateProgressBar("Переустановка сервиса RabbitMQ", 5);
            InstallUnit RabbitInstall = new InstallUnit(@"pushd C:\Program Files\RabbitMQ Server\rabbitmq_server-3.10.1\sbin" + "\new" + "rabbitmq-service install", "Переустановка сервиса RabbitMQ");
            RabbitInstall.CmdRun();
            UpdateProgressBar("Перезапуск RabbitMQ", 5);
            InstallUnit StopRabbitMQ = new InstallUnit("", "net", "stop RabbitMQ", false, "Перезапуск RabbitMQ");
            StopRabbitMQ.CmdRun();
            UpdateProgressBar("Перезапуск RabbitMQ", 5);
            InstallUnit StartRabbitMQ = new InstallUnit("", "net", "start RabbitMQ", false, "Перезапуск RabbitMQ");
            StartRabbitMQ.CmdRun();
                
            UpdateProgressBar("Запуска плагина RabbitMQ", 5);
            InstallUnit RabbitPlugins = new InstallUnit(@"pushd C:\Program Files\RabbitMQ Server\rabbitmq_server-3.10.1\sbin" + "\new" + "rabbitmq-plugins enable rabbitmq_management", "Запуска плагина RabbitMQ");
            RabbitPlugins.CmdRun();
            RabbitRemove.CmdRun();
            RabbitInstall.CmdRun();

            StopRabbitMQ.CmdRun();
            StartRabbitMQ.CmdRun();
            
            Task wait = new Task(() =>
            {
                Thread.Sleep(5000);
            });
            wait.Start();
            wait.Wait();

            prgbar_taskProgress.Value = 100;
            lbl_taskProgress.Content = "Установка завершена"; 
            MessageBox.Show("Установка RabbitMQ завершена.");
            Process.Start("explorer.exe", @"http://localhost:15672");
        }
    }
}
