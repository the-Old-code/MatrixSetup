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
    /// Interaction logic for WindowDialogNeo4j.xaml
    /// </summary>
    public partial class WindowDialogNeo4j : Window
    {
        public WindowDialogNeo4j()
        {
            InitializeComponent();
            this.DataContext = App.ViewModel;
        }

        private void btn_browsepath_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;
            folderDialog.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;
            System.Windows.Forms.DialogResult result = folderDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtbox_path_Neo4j.Text = folderDialog.SelectedPath;
            }
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                App.ViewModel.Neo4jInstallPath = txtbox_path_Neo4j.Text;
                Close();
            }
            catch(DirectoryNotFoundException ex) 
            {
                MessageBox.Show(ex.Message);
            }
            if (txtbox_path_Neo4j.Text == "") Close();
        }
    }
}
