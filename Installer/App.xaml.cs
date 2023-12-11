using System.Windows;

namespace Installer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Модель для создания связки между бизнес-объектом и интерфейсом
        /// </summary>
        public static InstallPropertiesViewModel ViewModel { get; } = new InstallPropertiesViewModel();
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            RabbitMQStartPage window = new RabbitMQStartPage();
            window.Show();
        }
    }
}
