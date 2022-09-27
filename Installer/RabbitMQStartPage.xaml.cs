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
using Microsoft.Win32;
using System.Linq;

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
        
        private void btn_RabbitInstall_Click(object sender, RoutedEventArgs e)
        {
            InstallScenario.DefaultInstall();
            MessageBox.Show("Установка RabbitMQ завершена.");
            Process.Start("explorer.exe", @"http://localhost:15672");
            
            //Installer.App.Current.Shutdown();
        }
    }
}
