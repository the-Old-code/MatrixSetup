using System.Windows;
using System.Windows.Controls;
using System.IO;
using Newtonsoft.Json;

namespace Installer
{
    /// <summary>
    /// Interaction logic for WindowDialogMsSQL.xaml
    /// </summary>
    public partial class WindowDialogMsSQL : Window
    {
        public WindowDialogMsSQL()
        {
            InitializeComponent();
        }

        private void cmbbox_integratedSecurity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbbox_integratedSecurity.SelectedItem is TextBlock txt)
            {
                if (txt.Text == "Проверка подлинности Windows")
                {
                    txtbox_password.IsEnabled = false;
                    txtbox_userID.IsEnabled = false;
                }
                else 
                {
                    txtbox_password.IsEnabled = true;
                    txtbox_userID.IsEnabled = true;
                }
            }
        }

        private void SetJsonConnectionStrings(string dataSource, string initialCatalog,string integrSecurity,string userID, string password)
        {
            string connectionString = "data source=" + dataSource + ";initial catalog=" + initialCatalog + "; Integrated Security=" + integrSecurity + "; User ID=" + userID + "; Password=" + password + ";";
            //InstallScenario.InitializeWebServerConnectionStrings(InstallScenario.ConnectionStringsType.MsSQL, connectionString);
            App.ViewModel.WebServerConnectionConfig = new InstallPropertiesViewModel.ConnectionString { type = InstallScenario.ConnectionStringsType.MsSQL, connectionString = connectionString };
        }

        private void SetJsonConnectionStrings(string dataSource, string initialCatalog, string integrSecurity)
        {
            string connectionString = "data source=" + dataSource + ";initial catalog=" + initialCatalog + "; Integrated Security=" + integrSecurity + ";";
            //InstallScenario.InitializeWebServerConnectionStrings(InstallScenario.ConnectionStringsType.MsSQL, connectionString);
            App.ViewModel.WebServerConnectionConfig = new InstallPropertiesViewModel.ConnectionString { type = InstallScenario.ConnectionStringsType.MsSQL, connectionString = connectionString };
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            
            bool isSuccesfullEnter = true;
            foreach (object a in grd_msSQL.Children)
            {
                if (a is TextBox txt)
                {
                    if (txt.Text == "" && txt.IsEnabled)
                    {
                        MessageBox.Show("Есть невведенные поля");
                        isSuccesfullEnter = false;
                        break;
                    }
                }
            }
            if (isSuccesfullEnter && txtbox_password.IsEnabled)
            {
                SetJsonConnectionStrings(txtbox_dataSource.Text, txtbox_initialCatalog.Text, (!txtbox_password.IsEnabled).ToString(), txtbox_userID.Text, txtbox_password.Text);
                Close();
            }
            else if (isSuccesfullEnter && !txtbox_password.IsEnabled) 
            {
                SetJsonConnectionStrings(txtbox_dataSource.Text, txtbox_initialCatalog.Text, (!txtbox_password.IsEnabled).ToString());
                Close();
            }
        }
    }
}
