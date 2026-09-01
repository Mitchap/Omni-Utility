using omni_multitool.Helpers;
using omni_multitool.Pages;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using omni_multitool.Features.WaterReminder;
namespace omni_multitool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            NavigateToPage(new FavoritesPage());
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void BtnFavorites_Click(object sender, RoutedEventArgs e)
        {
            SetActiveSidebarButton(BtnFavorites);
            NavigateToPage(new FavoritesPage());
        }

        private void BtnUtilities_Click(object sender, RoutedEventArgs e)
        {
            SetActiveSidebarButton(BtnUtilities);
            NavigateToPage(new UtilitiesPage());
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            SetActiveSidebarButton(BtnSettings);
            NavigateToPage(new SettingsPage());
        }

        private void BtnWater_Click(object sender, RoutedEventArgs e)
        {
            SetActiveSidebarButton(BtnUtilities);
            NavigateToPage(new WaterReminderPage());
        }

        private void SetActiveSidebarButton(Button activeButton)
        {
            SidebarNavigation.SetIsActive(BtnFavorites, false);
            SidebarNavigation.SetIsActive(BtnUtilities, false);
            SidebarNavigation.SetIsActive(BtnSettings, false);

            SidebarNavigation.SetIsActive(activeButton, true);
        }


        //Navigation Animation from App.xaml
        public void NavigateToPage(Page page)
        {
            MainFrame.Navigate(page);

            MainFrame.Opacity = 0;

            DoubleAnimation fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = (Duration)Application.Current.Resources["MotionPage"]
            };

            MainFrame.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}