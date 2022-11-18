using System.Windows;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading.Tasks;

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
        delegate void updateTxtBlc(string state);
        private void btn_RabbitInstall_Click(object sender, RoutedEventArgs e)
        {
            /*Task task = new Task(() => { InstallScenario.DefaultInstall(); MessageBox.Show("Установка RabbitMQ завершена."); });
            Task task2 = new Task(()=>{
                prgbar_taskProgress.Visibility = Visibility.Visible;
                prgbar_taskProgress.IsIndeterminate = true;
                
            });
            
            task.Start();
            task2.Start();

            task.Wait();
            task2.Wait();
            prgbar_taskProgress.IsIndeterminate = false;
            
            Process.Start("explorer.exe", @"http://localhost:15672");
            //Installer.App.Current.Shutdown();*/
            StartTask();
        }

        private async void StartTask()
        {
            btn_RabbitInstall.IsEnabled = false;
            txtblck_InstallStage.Text = "Начало установки";
            prgbar_taskProgress.Visibility = Visibility.Visible;
            prgbar_taskProgress.IsIndeterminate = true;
            await Task.Run(()=> LongRunningTask());
            txtblck_InstallStage.Text = "Установка завершена";
            prgbar_taskProgress.IsIndeterminate = false;
            prgbar_taskProgress.Visibility = Visibility.Hidden;
            btn_RabbitInstall.IsEnabled = true;
            Process.Start("explorer.exe", @"http://localhost:15672");
        }
        private void LongRunningTask() 
        {
            InstallScenario.Notify = UpdateState;
            InstallScenario.DefaultInstall();
        }
        private void Update(string taskName) 
        {
            txtblck_InstallStage.Text = taskName;
        }
        private void UpdateState(string task)
        {
            Dispatcher.BeginInvoke(new updateTxtBlc(Update), new object[] {task});
        }

        private void chkbox_Web_Checked(object sender, RoutedEventArgs e)
        {
            
            WindowDialogWeb win = new WindowDialogWeb();
            win.Show();
            InstallScenario.AddInstall("web");
        }

        private void chkbox_Web_Unchecked(object sender, RoutedEventArgs e)
        {
            InstallScenario.RemoveInstall("web");
        }


        private void chkbox_rabbit_Checked(object sender, RoutedEventArgs e)
        {
            InstallScenario.AddInstall("rabbitmq");
        }

        private void chkbox_rabbit_Unchecked(object sender, RoutedEventArgs e)
        {
            InstallScenario.RemoveInstall("rabbitmq");
        }

        private void chkbox_java_Checked(object sender, RoutedEventArgs e)
        {
            InstallScenario.AddInstall("java");
        }

        private void chkbox_java_Unchecked(object sender, RoutedEventArgs e)
        {
            InstallScenario.RemoveInstall("neo4j");
        }
    }
}
