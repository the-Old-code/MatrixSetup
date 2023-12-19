using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using Installer.Servers;

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
            this.DataContext = App.ViewModel;
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
                        InstallScenario.AddInstall(InstallScenario.InstallComponent.web);
                        WindowDialogWeb win = new WindowDialogWeb();
                        win.ShowDialog();
                        break;
                    case "chkbox_SQL":
                        WindowDialogSQL SQL = new WindowDialogSQL();
                        SQL.ShowDialog();
                        break;
                    case "chkbox_Poll":
                        InstallScenario.AddInstall(InstallScenario.InstallComponent.poll);
                        WindowDialogPoll poll = new WindowDialogPoll();
                        poll.ShowDialog();
                        break;
                    case "chkbox_Check":
                        InstallScenario.AddInstall(InstallScenario.InstallComponent.check);
                        WindowDialogCheck check = new WindowDialogCheck();
                        check.ShowDialog();
                        break;
                    case "chkbox_Sheduler":
                        InstallScenario.AddInstall(InstallScenario.InstallComponent.sheduler);
                        WindowDialogSheduler sheduler = new WindowDialogSheduler();
                        sheduler.ShowDialog();
                        break;
                    case "chkbox_Neo4j":
                        InstallScenario.AddInstall(InstallScenario.InstallComponent.neo4j);
                        WindowDialogNeo4j neo4j = new WindowDialogNeo4j();
                        neo4j.ShowDialog();
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
                        InstallScenario.RemoveInstall(InstallScenario.InstallComponent.web);
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
                    case "chkbox_Neo4j":
                        InstallScenario.RemoveInstall(InstallScenario.InstallComponent.neo4j);
                        break;
                }
            }
        }

        private void btn_Set_To_Default_Click(object sender, RoutedEventArgs e)
        {
            InstallScenario.SetDefaultInstallParameters();
            App.ViewModel.UpdateProperties();
        }
    }
}
