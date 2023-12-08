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
    /// Interaction logic for WindowDialogSQLite.xaml
    /// </summary>
    public partial class WindowDialogSQLite : Window
    {
        public WindowDialogSQLite()
        {
            InitializeComponent();
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            if (txtbox_dataSource.Text == "") MessageBox.Show("Поле пустое");
            else
            {
                SetJsonConnectionStrings(txtbox_dataSource.Text);
                Close();
            }
        }

        private void SetJsonConnectionStrings(string dataSource)
        {
            string jsonString = File.ReadAllText(Directory.GetCurrentDirectory() + "\\resfiles\\web4.4.6\\appsettings.json");
            // Convert the JSON string to a JObject:
            dynamic jsonObj = JsonConvert.DeserializeObject(jsonString);
            jsonObj["ConnectionStrings"]["connectionString"] = "data source=" + dataSource +  ";";
            jsonObj["ConnectionStrings"]["providerName"] = "sqlite";
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(Directory.GetCurrentDirectory() + "\\resfiles\\web4.4.6\\appsettings.json", output);
        }
    }
}
