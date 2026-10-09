using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Runtime.Versioning;
using omni_multitool.Data;
using omni_multitool.Data.Models;

namespace omni_multitool.Features.WaterReminder
{
    [SupportedOSPlatform("windows")]
    public partial class WaterReminderPage : Page
    {
        private WaterReminderSettings? _settings;
        private bool _isLoading = true;

        public WaterReminderPage()
        {
            InitializeComponent();
            Loaded += WaterReminderPage_Loaded;
        }

        private async void WaterReminderPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadSettingsAsync();
        }

        private async Task LoadSettingsAsync()
        {
            _isLoading = true;
            try
            {
                var settingsService = new SettingsService();
                _settings = await settingsService.GetWaterReminderSettingsAsync();

                WaterReminderToggle.IsChecked = _settings.IsEnabled;
                TxtInterval.Text = _settings.IntervalInMinutes.ToString();
                TogglePlaySound.IsChecked = _settings.PlaySound;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading WaterReminderSettings: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async void WaterReminderSetting_Changed(object sender, RoutedEventArgs e)
        {
            if (_isLoading || _settings == null)
                return;

            _settings.IsEnabled = WaterReminderToggle.IsChecked ?? false;
            _settings.PlaySound = TogglePlaySound.IsChecked ?? false;

            if (int.TryParse(TxtInterval.Text, out int interval) && interval > 0)
            {
                _settings.IntervalInMinutes = interval;
            }

            try
            {
                await WaterReminderService.Instance.UpdateSettingsAsync(_settings);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating WaterReminderSettings: {ex.Message}");
            }
        }

        private void BtnPlaySound_Click(object sender, RoutedEventArgs e)
        {
            WaterReminderService.Instance.PlaySound();
        }

        private void BtnTestNotif_Click(object sender, RoutedEventArgs e)
        {
            WaterReminderService.Instance.TriggerNotification();
        }
    }
}