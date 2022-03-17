using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Installer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void selectfile_btn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;//for java install use https://docs.oracle.com/javase/8/docs/technotes/guides/install/windows_jdk_install.html#CHDHHBDD 
            //https://stackoverflow.com/questions/28043588/installing%20-jdk-8-and-jre-8-silently-on-a-windows-machine-through-command-line
            //https://qastack.ru/programming/133379/elevating-process-privilege-programmatically

            folderDialog.SelectedPath = System.AppDomain.CurrentDomain.BaseDirectory;
            System.Windows.Forms.DialogResult result = folderDialog.ShowDialog();
            
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtbox_javapath.Text = folderDialog.SelectedPath;
            }
        }

        private void btn_test_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process TestProcess = new System.Diagnostics.Process();
            
            TestProcess.StartInfo.Verb = "runas";
            TestProcess = System.Diagnostics.Process.Start("", "");
        }
    }
}