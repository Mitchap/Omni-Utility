using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using omni_multitool.Data;
using omni_multitool.Data.Models;
using omni_multitool.Helpers;

namespace omni_multitool.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private AppSettings? _appSettings;
        private bool _isLoading = true;

        public SettingsPage()
        {
            InitializeComponent();
            Loaded += SettingsPage_Loaded;
        }

        private async void SettingsPage_Loaded(object sender, RoutedEventArgs e)
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

                ToggleStartWithWindows.IsChecked = _appSettings.StartWithWindows;
                ToggleMinimizeToTray.IsChecked = _appSettings.MinimizeToTray;
                ToggleShowWelcomePage.IsChecked = _appSettings.ShowWelcomePage;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading AppSettings: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async void SettingToggle_Click(object sender, RoutedEventArgs e)
        {
            if (_isLoading || _appSettings == null)
                return;

            _appSettings.StartWithWindows = ToggleStartWithWindows.IsChecked ?? false;
            _appSettings.MinimizeToTray = ToggleMinimizeToTray.IsChecked ?? false;
            _appSettings.ShowWelcomePage = ToggleShowWelcomePage.IsChecked ?? true;

            try
            {
                var settingsService = new SettingsService();
                await settingsService.SaveAppSettingsAsync(_appSettings);

                // Apply StartWithWindows behavior
                AutoStartHelper.SetAutoStart(_appSettings.StartWithWindows);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving AppSettings: {ex.Message}");
            }
        }
    }
}
