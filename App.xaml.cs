using System.Configuration;
using System.Data;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using System.Linq;
namespace omni_multitool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon? _trayIcon;

        public App()
        {
            InitializeComponent();

            _trayIcon = (TaskbarIcon)Resources["OmniTrayIcon"];
        }
        private void TrayIcon_LeftMouseDown(object sender, RoutedEventArgs e)
        {
            RestoreMainWindow();
        }


        public void RestoreMainWindow()
        {
            MainWindow? mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            if (mainWindow is null)
                return;

            mainWindow.Show();
            mainWindow.WindowState = WindowState.Normal;
            mainWindow.Activate();
        }

        private void OpenOmni_Click(object sender, RoutedEventArgs e)
        {
            RestoreMainWindow();
        }
    }

}
