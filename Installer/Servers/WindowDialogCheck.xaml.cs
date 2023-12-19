﻿using System;
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

namespace Installer.Servers
{
    /// <summary>
    /// Логика взаимодействия для WindowDialogCheck.xaml
    /// </summary>
    public partial class WindowDialogCheck : Window
    {
        public WindowDialogCheck()
        {
            InitializeComponent();
            this.DataContext = App.ViewModel;
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.ViewModel.CheckServerInstallPath = txtbx_check_installpath.Text;
                Close();
            }
            catch (DirectoryNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (txtbx_check_installpath.Text == "") Close();
        }

        private void btn_browsepath_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = false;
            folderDialog.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;
            System.Windows.Forms.DialogResult result = folderDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                txtbx_check_installpath.Text = folderDialog.SelectedPath;
            }
        }
    }
}
