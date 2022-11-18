using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.IO;
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
    /// Interaction logic for WindowDialogWeb.xaml
    /// </summary>
    public partial class WindowDialogWeb : Window
    {
        public WindowDialogWeb()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(txtbx_webinstallpath.Text))
            {
                InstallScenario.WebPath = txtbx_webinstallpath.Text;
                this.Close();
            }
            else MessageBox.Show("Не существующая папка");
        }

        private void btn_browsepath_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;
            folderDialog.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
            System.Windows.Forms.DialogResult result = folderDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtbx_webinstallpath.Text = folderDialog.SelectedPath;
            }
        }

        private void chkbox_Neo4j_Checked(object sender, RoutedEventArgs e)
        {
            InstallScenario.AddInstall("neo4j");
        }
        private void chkbox_Neo4j_Unchecked(object sender, RoutedEventArgs e)
        {
            InstallScenario.RemoveInstall("neo4j");
        }

        private void chkbox_WebServer_Checked(object sender, RoutedEventArgs e)
        {
            InstallScenario.AddInstall("web");
        }

        private void chkbox_WebServer_Unchecked(object sender, RoutedEventArgs e)
        {
            InstallScenario.RemoveInstall("web");
        }
    }
}
