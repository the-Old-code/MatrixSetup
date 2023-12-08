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
            string jsonString = File.ReadAllText(Directory.GetCurrentDirectory() + "\\resfiles\\web4.4.6\\appsettings.json");
            // Convert the JSON string to a JObject:
            dynamic jsonObj = JsonConvert.DeserializeObject(jsonString);
            jsonObj["ConnectionStrings"]["connectionString"] = "Server=" + Server + " ;Port=" + Port + " ; User Id=" + User_ID + " ; Password=" + Password + " ; Database=" + Database + ";";
            jsonObj["ConnectionStrings"]["providerName"] = "Npgsql";
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(Directory.GetCurrentDirectory() + "\\resfiles\\web4.4.6\\appsettings.json", output);
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
