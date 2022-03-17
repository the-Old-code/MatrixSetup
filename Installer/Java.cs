using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Installer
{
    class Java
    {
        string path;
        public string Path
        {
            get
            {
                return Path; // возвращаем значение свойства
            }
            set
            {
                Path = value;   // устанавливаем новое значение свойства
            }
        }
        public Java()
        {
            path = "C:\\Program Files";
        }
        public void InstallJava()
        {
            if (path != string.Empty)
            {
                System.Diagnostics.Process JavaProcess = new System.Diagnostics.Process();
                JavaProcess.StartInfo.FileName = path;
                JavaProcess.StartInfo.Verb = "runas";
                JavaProcess.Start();
            }
            
            else MessageBox.Show("Нет установочного файла Java!");
        }
    }
}
