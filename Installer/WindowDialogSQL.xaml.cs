using System;
using System.Collections.Generic;
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
    /// Interaction logic for WindowDialogSQL.xaml
    /// </summary>
    public partial class WindowDialogSQL : Window
    {
        public WindowDialogSQL()
        {
            InitializeComponent();
        }

        private void btn_PostgreSql_Click(object sender, RoutedEventArgs e)
        {
            WindowDialogPostrgreSQL Postrge = new WindowDialogPostrgreSQL();
            Postrge.ShowDialog();
        }

        private void btn_MsSql_Click(object sender, RoutedEventArgs e)
        {
            WindowDialogMsSQL msSQL= new WindowDialogMsSQL();
            msSQL.ShowDialog();
        }

        private void btn_SqLite_Click(object sender, RoutedEventArgs e)
        {
            WindowDialogSQLite SQLite = new WindowDialogSQLite();
            SQLite.ShowDialog();
        }
    }
}
