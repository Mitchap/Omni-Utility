using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace omni_multitool.Pages
{
    /// <summary>
    /// Interaction logic for CloseConfirmWindow.xaml
    /// </summary>
    public partial class CloseConfirmWindow : Window
    {
        public CloseConfirmWindow()
        {
            InitializeComponent();
        
        }   

        //Cancel button / X button on navbar

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Shutdown_App_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); 

        }


        private void MinimizeToTray_Click(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();
            if (mainWindow != null)
            {
                mainWindow.Hide();
            }
            Close();
        }
    }
}
