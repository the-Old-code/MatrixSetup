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
            if (txtbox_dataSource.Text != "") SetJsonConnectionStrings(txtbox_dataSource.Text);
            Close();
        }

        private void SetJsonConnectionStrings(string dataSource)
        {
            string connectinString = "data source=" + dataSource +  ";";
            InstallScenario.InitializeWebServerConnectionStrings(InstallScenario.ConnectionStringsType.SQLite, connectinString);
        }
    }
}
