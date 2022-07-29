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
    public class InstallUnit
    {
        private string path;//путь из коготорого запускаются установочные файлы
        private string arg;//часть того, что вводится в командную строку после ее открытия
        private string filename;//имя запускаемого файла, может быть cmd или установочный exe
        private string startarg;//аргументы при запуске файла установки или cmd
        private bool write;//true - вводит нужные тектовые данные, false - не вводит
        private string writearg;//все текстовые данные, которые вводятся 
        
        public InstallUnit(string arg1)//конструктор с аргументом для командной строки, путь где находятся файлы это одна директория с установщиком
        {
            Path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Arg = arg1;
            filename = "cmd.exe";
            startarg = "";          
            write = true;
            writearg = "pushd " + Path + "\ncd resfiles\n" + Arg;//was writearg = "pushd " + Path + "\n cd ..\n cd ..\n cd ..\ncd resfiles\n" + Arg;
        }
        public InstallUnit(string arg1,string filename1, string startarg1) // startarg это аргументы запуска
        {
            Path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Arg = arg1;
            filename = filename1;
            startarg = startarg1;            
            write = true;
            writearg = "pushd " + Path + "\ncd resfiles\n" + Arg;//was writearg = "pushd " + Path + "\n cd ..\n cd ..\n cd ..\ncd resfiles\n" + Arg;
        }
        
        public InstallUnit(string arg1, string filename1, string startarg1, bool write1)
        {
            Path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Arg = arg1;
            filename = filename1;
            startarg = startarg1;
            write = write1;
            writearg = "pushd " + Path + "\ncd resfiles\n" + Arg;//was writearg = "pushd " + Path + "\n cd ..\n cd ..\n cd ..\ncd resfiles\n" + Arg;
        }
        public InstallUnit(string arg1, string filename1, string startarg1, bool write1,string writearg1)
        {
            Path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Arg = arg1;
            filename = filename1;
            startarg = startarg1;
            write = write1;
            writearg = writearg1;
        }
        public string Path//путь к директории, в которой хранятся установочные файлы
        {
            get 
            {
                return path;
            }
            set
            {
                path = value;   // устанавливаем новое значение свойства
            }
        }
        public string Arg//строка с аргументами для командной строки
        {
            get 
            {
                return arg;
            }
            set
            {
                arg = value;   // устанавливаем новое значение свойства
            }

        }
        

        public virtual void CmdInstall()//установка через командую строку
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = filename;//изменяемое// обычно это "cmd.exe"
            cmd.StartInfo.Verb = "runas";//права администратора
            cmd.StartInfo.Arguments = startarg;//изменяемое// обычно ""
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;//выполнение без открытия окна // обычно true
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            
            if (write == true) 
            {
                cmd.StandardInput.WriteLine(writearg);//переход в директорию resfiles и выполенение аргумента для командной строки
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                Console.WriteLine(cmd.StandardOutput.ReadToEnd());
            }
            //Process Currentprocess = Process.GetCurrentProcess();//отладочный вывод
            //Debug.WriteLine(Currentprocess.ToString() + "нужный процесс");//отладочный вывод
            
            cmd.Close();

            

        }
    }
    class JavaInstallUnit : InstallUnit //версия класса InstallUnit для установки Java
    {
        private string javapath;
        public string JavaPath// дополнительное свойство, если пользователь задаст свой путь установки Java
        {
            set
            {
                javapath = value;   // устанавливаем новое значение свойства
            }
            get 
            {
                if (javapath == null) return "0";//в случае если пользователь не задал свой путь для установки, то возвращает 0 и устанавливает в папку по умолчанию
                else return javapath;
            }

        }

        public JavaInstallUnit(string arg1) :base(arg1) { }
        public JavaInstallUnit(string arg1,string javapath1) : base(arg1) 
        {
            JavaPath = javapath1;
        }
        public override void CmdInstall() 
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.Verb = "runas";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine("pushd " + Path + "\n cd ..\n cd ..\n cd ..\ncd resfiles\n" + Arg);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
            if (JavaPath != "0") JavaConfigUpdate();//если не 0, то значит пользователь установил свой путь для установки Java
        }
        private void JavaConfigUpdate() 
        {
            string newpath = (System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Remove(24)+ "\\resfiles");
            File.WriteAllText(newpath, String.Empty);
            StreamWriter javaconfig = new StreamWriter(newpath);
            javaconfig.WriteLine("INSTALLDIR="+JavaPath+"\nINSTALL_SILENT = Enable\nREBOOT = Disable");//задание нового установочного пути
            javaconfig.Close();
        }
    }
    public class RabbitInstallUnit : InstallUnit
    {
        private string Filename;
        public RabbitInstallUnit(string arg1, string filename) : base(arg1) { Filename = filename; }

        public override void CmdInstall()//установка через командую строку
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = Filename;
            cmd.StartInfo.Verb = "runas";//права администратора
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = false;//выполнение без открытия окна
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.Close();
        }
        public void RabbitPrompt() 
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.Arguments = "/k cd /d C:\\Program Files\\RabbitMQ Server\\rabbitmq_server-3.10.1\\sbin";
            cmd.StartInfo.Verb = "runas";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = false;//was true
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine("rabbitmq-plugins enable rabbitmq_management");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
        }
    }

}
