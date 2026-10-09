using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using omni_multitool.Data;
using omni_multitool.Data.Models;

namespace omni_multitool.Features.WaterReminder
{
    public class WaterReminderService
    {
        private static WaterReminderService? _instance;
        public static WaterReminderService Instance => _instance ??= new WaterReminderService();

        private readonly DispatcherTimer _timer;
        private MediaPlayer? _mediaPlayer;
        private WaterReminderSettings? _currentSettings;

        private WaterReminderService()
        {
            _timer = new DispatcherTimer();
            _timer.Tick += Timer_Tick;
            InitMediaPlayer();
        }

        private void InitMediaPlayer()
        {
            try
            {
                string soundPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Assets",
                    "Audio",
                    "mixkit-uplifting-flute.wav");

                _mediaPlayer = new MediaPlayer
                {
                    Volume = 0.35
                };

                if (File.Exists(soundPath))
                {
                    _mediaPlayer.Open(new Uri(soundPath));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing MediaPlayer: {ex.Message}");
            }
        }

        public async Task InitializeAsync()
        {
            var service = new SettingsService();
            _currentSettings = await service.GetWaterReminderSettingsAsync();
            ApplySettings(_currentSettings);
        }

        public async Task UpdateSettingsAsync(WaterReminderSettings settings)
        {
            _currentSettings = settings;
            var service = new SettingsService();
            await service.SaveWaterReminderSettingsAsync(settings);
            ApplySettings(settings);
        }

        public WaterReminderSettings? CurrentSettings => _currentSettings;

        public void ApplySettings(WaterReminderSettings settings)
        {
            _timer.Stop();

            if (settings.IsEnabled && settings.IntervalInMinutes > 0)
            {
                _timer.Interval = TimeSpan.FromMinutes(settings.IntervalInMinutes);
                _timer.Start();
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TriggerNotification();
        }

        public void TriggerNotification()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var waterReminderWindow = new WaterReminderWindow();
                waterReminderWindow.Show();
                waterReminderWindow.Activate();

                if (_currentSettings != null && _currentSettings.PlaySound)
                {
                    PlaySound();
                }
            });
        }

        public void PlaySound()
        {
            try
            {
                if (_mediaPlayer != null)
                {
                    _mediaPlayer.Position = TimeSpan.Zero;
                    _mediaPlayer.Play();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error playing sound: {ex.Message}");
            }
        }
    }
}
