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
using System.Diagnostics;
using System.Reflection;

namespace Installer
{
    public delegate void ProgressBarHandler(string taskName);
    
    public class InstallUnit
    {
        //private string arg;//часть того, что вводится в командную строку после ее открытия // для запуска из cmd
        private string filename;//имя запускаемого файла, может быть cmd или установочный exe
        private string startarg;//аргумент командной строки
        private bool write;//true - вводит нужные текстовые данные(обычно команды) в запущенной программе(обычно в cmd), false - не вводит
        private List <string> installScript;//текст установочного скрипта, каждый элмент List это отдельная строка
        public string taskName;//имя процесса для отображения пользователю
        public static ProgressBarHandler Notify;
        public InstallUnit()
        {
            
        }

        public InstallUnit(List <string> script, string taskName) : this()//конструктор с аргументом для командной строки, путь где находятся файлы это одна директория с установщиком
        {
            //this.Notify = Notify;
            this.taskName = taskName;
            filename = "cmd.exe";
            startarg = "";
            write = true;
            installScript = new List<string>() { "chcp 1251", "pushd " + InstallScenario.ResfilesPath};
            if (script != null) installScript.AddRange(script);
        }
        public InstallUnit(List <string> script, string filename, string startarg, string taskName) : this(script, taskName) // startarg это аргументы запуска
        {
            this.filename = filename;
            this.startarg = startarg;
            write = true;
            if (script != null) installScript.AddRange(script);//was writearg = "pushd " + Path + "\n cd ..\n cd ..\n cd ..\ncd resfiles\n" + Arg;
        }
        
        public InstallUnit(List<string> script, string filename, string startarg, bool write, string taskName) : this(script, filename, startarg, taskName)
        {
            this.write = write;//при write == false теряют смысл в существовании поля writearg, Path, Arg 
            if(script != null) installScript.AddRange(script);//was writearg = "pushd " + Path + "\n cd ..\n cd ..\n cd ..\ncd resfiles\n" + Arg;
        }
        public InstallUnit(List<string> script, string filename, string startarg, bool write, List <string> writearg, string taskName) : this(script, filename, startarg, write, taskName)
        {
            this.installScript = writearg;
        }
        
        public virtual void ExecuteInstallationScriptForJavaErlang() 
        {
            Notify.Invoke(taskName);

            //вызов события // в событие передается имя задачи и то, насколько заполняется прогрессбар
            Process cmd = new Process();
            cmd.StartInfo.FileName = filename;//изменяемое// обычно это "cmd.exe"
            cmd.StartInfo.Verb = "runas";//права администратора
            cmd.StartInfo.Arguments = startarg;//изменяемое// обычно "" или /K
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;//выполнение без открытия окна // обычно true
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            if (write == true)
            {
                foreach (string arg in installScript)
                {
                    cmd.StandardInput.WriteLine(arg);//построчное выполнение скрипта  
                }

                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                Console.WriteLine(cmd.StandardOutput.ReadToEnd());
            }
            cmd.Dispose();
        }
        /// <summary>
        /// Выполнение скрипта. По умолчанию InstallUnit выполняет через cmd.exe
        /// Также в программе есть части, которые выполняют скрипт через net
        /// </summary>
        public virtual void ExecuteInstallationScript()
        {
            Notify.Invoke(taskName);//вызов события // в событие передается имя задачи и то, насколько заполняется прогрессбар

            Process cmd = new Process();
            cmd.StartInfo.FileName = filename;//изменяемое// обычно это "cmd.exe"
            cmd.StartInfo.Verb = "runas";//права администратора
            cmd.StartInfo.Arguments = startarg;//изменяемое// обычно "" еще /K
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = false;
            cmd.StartInfo.CreateNoWindow = true;//выполнение без открытия окна // обычно true
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            
            if (write == true)
            {
                foreach(string arg in installScript) 
                {
                    cmd.StandardInput.WriteLine(arg);//ввод комманд в командную строку или другую программу  
                }
                
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                cmd.Dispose();
            }
            
            
        }
    }
}