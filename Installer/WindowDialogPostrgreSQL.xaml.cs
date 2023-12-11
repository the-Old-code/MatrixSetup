using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Installer
{
    /// <summary>
    /// Interaction logic for WindowDialogPostrgreSQL.xaml
    /// </summary>
    public partial class WindowDialogPostrgreSQL : Window
    {
        public WindowDialogPostrgreSQL()
        {
            InitializeComponent();
        }

        private void SetJsonConnectionStrings(string Server, string Port, string User_ID, string Password, string Database)
        {
            string connectionString = "Server=" + Server + " ;Port=" + Port + " ; User Id=" + User_ID + " ; Password=" + Password + " ; Database=" + Database + ";";
            InstallScenario.InitializeWebServerConnectionStrings(InstallScenario.ConnectionStringsType.PostgreSQL, connectionString);
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            bool IsSuccessfullEnter = true;
            foreach (object a in grd_Postgre.Children) 
            {
                if (a is TextBox txt) 
                {
                    if (txt.Text == "")
                    {
                        IsSuccessfullEnter = false;
                        MessageBox.Show("Есть невведенные поля");
                        break;
                    }
                }
            }
            if (IsSuccessfullEnter)
            {
                SetJsonConnectionStrings(txtbox_Server.Text, txtbox_Port.Text, txtbox_User_ID.Text, txtbox_Password.Text, txtbox_Database.Text);
                Close();
            }
        }
    }
}
