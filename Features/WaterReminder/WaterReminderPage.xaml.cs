using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Runtime.Versioning;

namespace omni_multitool.Features.WaterReminder
{
    [SupportedOSPlatform("windows")]
    public partial class WaterReminderPage : Page
    {
        private readonly MediaPlayer _reminderSound;

        public WaterReminderPage()
        {
            InitializeComponent();

            string soundPath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Assets",
                "Audio",
                "mixkit-uplifting-flute.wav");

            _reminderSound = new MediaPlayer
            {
                Volume = 0.35
            };

            _reminderSound.Open(new Uri(soundPath));
        }

        private void BtnPlaySound_Click(object sender, RoutedEventArgs e)
        {
            _reminderSound.Position = TimeSpan.Zero;
            _reminderSound.Play();
        }
    }
}