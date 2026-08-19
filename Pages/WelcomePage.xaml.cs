using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace omni_multitool.Pages
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Window
    {
        public WelcomePage()
        {
            InitializeComponent();
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

        private void BtnContinue_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            Close();
        }
    }
}
