using System.Windows;
using System.Windows.Controls;
using Hardcodet.Wpf.TaskbarNotification;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using omni_multitool.Data;
using omni_multitool.Pages;
using omni_multitool.Features.WaterReminder;
using omni_multitool.Helpers;

namespace omni_multitool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        private TaskbarIcon? _trayIcon;

        public App()
        {
            InitializeComponent();

            //Dependency Injection container setup
            var services = new ServiceCollection();

            services.AddDbContext<OmniDbContext>();
            services.AddScoped<SettingsService>();

            _serviceProvider = services.BuildServiceProvider();

            _trayIcon = (TaskbarIcon)Resources["OmniTrayIcon"];
        }

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            // Check single instance guard
            if (!SingleInstanceGuard.CheckAndRegister())
            {
                Shutdown();
                return;
            }

            using var scope = _serviceProvider.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<OmniDbContext>();
            db.Database.EnsureCreated();

            // Initialize background water reminder timer
            await WaterReminderService.Instance.InitializeAsync();

            // Check AppSettings to determine startup window
            var settingsService = new SettingsService();
            var settings = await settingsService.GetAppSettingsAsync();

            if (settings.ShowWelcomePage)
            {
                var welcomePage = new WelcomePage();
                welcomePage.Show();
            }
            else
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            SingleInstanceGuard.Cleanup();
            base.OnExit(e);
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
            {
                mainWindow = new MainWindow();
            }

            mainWindow.Show();
            mainWindow.WindowState = WindowState.Normal;
            mainWindow.Activate();
        }

        public void RestoreMainWindowAndNavigate(Page page)
        {
            MainWindow? mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            if (mainWindow is null)
            {
                mainWindow = new MainWindow();
            }

            mainWindow.Show();
            mainWindow.WindowState = WindowState.Normal;
            mainWindow.Activate();
            mainWindow.NavigateToPage(page);
        }

        private void OpenOmni_Click(object sender, RoutedEventArgs e)
        {
            RestoreMainWindow();
        }
    }

}


