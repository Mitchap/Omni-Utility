using omni_multitool.Pages;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            MainFrame.Navigate(new FavoritesPage());
        }

        private void BtnFavorites_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new FavoritesPage());
        }

        private void BtnUtilities_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UtilitiesPage());
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SettingsPage());
        }
    }
}