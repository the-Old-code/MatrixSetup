﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Installer
{
    public class InstallPropertiesViewModel : INotifyPropertyChanged 
    {
        public string WebServerConnectionString 
        {
            get 
            {
                return InstallScenario.WebServerConnectionString;
            }
        }
        public string Neo4jInstallPath 
        {
            get 
            {
                return InstallScenario.Neo4jInstallPath;
            }
            set 
            {
                InstallScenario.Neo4jInstallPath = value;
                OnPropertyChanged(nameof(Neo4jInstallPath));
            }
        }
        public string WebServerInstallPath
        {
            get 
            {
                return InstallScenario.WebServerInstallPath;
            }
            set 
            {
                InstallScenario.WebServerInstallPath = value;
                OnPropertyChanged(nameof(WebServerInstallPath));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
