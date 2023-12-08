using System.Windows;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;

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
        private void btn_RabbitInstall_Click(object sender, RoutedEventArgs e)//was async
        {
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
            Dispatcher.BeginInvoke(new updateTxtBlc(Update), new string[] {task});
        }

        private void chkbox_Install_Checked(object sender, RoutedEventArgs e) 
        {
            if (sender is CheckBox checkBox)
            {
                switch (checkBox.Name)
                {
                    case "chkbox_java":
                        InstallScenario.AddInstall(InstallScenario.InstallComponent.java);
                        break;
                    case "chkbox_rabbit":
                        InstallScenario.AddInstall(InstallScenario.InstallComponent.erlang);
                        InstallScenario.AddInstall(InstallScenario.InstallComponent.rabbitmq);
                        break;
                    case "chkbox_Web":
                        WindowDialogWeb win = new WindowDialogWeb();
                        win.ShowDialog();
                        break;
                    case "chkbox_SQL":
                        WindowDialogSQL SQL = new WindowDialogSQL();
                        SQL.ShowDialog();
                        break;
                    case "chkbox_Poll":
                        InstallScenario.AddInstall(InstallScenario.InstallComponent.poll);
                        break;
                    case "chkbox_Check":
                        InstallScenario.AddInstall(InstallScenario.InstallComponent.check);
                        break;
                    case "chkbox_Sheduler":
                        InstallScenario.AddInstall(InstallScenario.InstallComponent.sheduler);
                        break;
                }

            }
        }
        private void chkbox_Install_Unсhecked(object sender, RoutedEventArgs e)
        {
            
            if (sender is CheckBox checkBox)
            {
                switch (checkBox.Name)
                {
                    case "chkbox_java":
                        InstallScenario.RemoveInstall(InstallScenario.InstallComponent.java);
                        break;
                    case "chkbox_rabbit":
                        InstallScenario.RemoveInstall(InstallScenario.InstallComponent.erlang);
                        InstallScenario.RemoveInstall(InstallScenario.InstallComponent.rabbitmq);
                        break;
                    case "chkbox_Web":
                        //InstallScenario.RemoveInstall("web");
                        //InstallScenario.RemoveInstall("neo4j");
                        break;
                    case "chkbox_SQL":
                        break;
                    case "chkbox_Poll":
                        InstallScenario.RemoveInstall(InstallScenario.InstallComponent.poll);
                        break;
                    case "chkbox_Check":
                        InstallScenario.RemoveInstall(InstallScenario.InstallComponent.check);
                        break;
                    case "chkbox_Sheduler":
                        InstallScenario.RemoveInstall(InstallScenario.InstallComponent.sheduler);
                        break;
                }
            }
        }
    }
}
