using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using omni_multitool.Data;
using omni_multitool.Data.Models;

namespace omni_multitool.Pages
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Window
    {
        private AppSettings? _appSettings;
        private bool _isLoading = true;

        public WelcomePage()
        {
            InitializeComponent();
            Loaded += WelcomePage_Loaded;
        }

        private async void WelcomePage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadSettingsAsync();
        }

        private async Task LoadSettingsAsync()
        {
            _isLoading = true;
            try
            {
                var settingsService = new SettingsService();
                _appSettings = await settingsService.GetAppSettingsAsync();
                ChkDontShowAgain.IsChecked = !_appSettings.ShowWelcomePage;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading settings in WelcomePage: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async void ChkDontShowAgain_Click(object sender, RoutedEventArgs e)
        {
            await SaveSettingsAsync();
        }

        private async Task SaveSettingsAsync()
        {
            if (_isLoading) return;

            try
            {
                var settingsService = new SettingsService();
                if (_appSettings == null)
                {
                    _appSettings = await settingsService.GetAppSettingsAsync();
                }

                _appSettings.ShowWelcomePage = !(ChkDontShowAgain.IsChecked ?? false);
                await settingsService.SaveAppSettingsAsync(_appSettings);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving settings in WelcomePage: {ex.Message}");
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ContinueGlow_Loaded(object sender, RoutedEventArgs e)
        {
            var startAnimation = new PointAnimation
            {
                From = new Point(-1, 0),
                To = new Point(1, 0),
                Duration = TimeSpan.FromSeconds(5),
                RepeatBehavior = RepeatBehavior.Forever
            };

            var endAnimation = new PointAnimation
            {
                From = new Point(0, 0),
                To = new Point(2, 0),
                Duration = TimeSpan.FromSeconds(5),
                RepeatBehavior = RepeatBehavior.Forever
            };

            ContinueGradient.BeginAnimation(
                LinearGradientBrush.StartPointProperty,
                startAnimation);

            ContinueGradient.BeginAnimation(
                LinearGradientBrush.EndPointProperty,
                endAnimation);
        }

        private async void BtnContinue_Click(object sender, RoutedEventArgs e)
        {
            await SaveSettingsAsync();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            Close();
        }
    }
}
