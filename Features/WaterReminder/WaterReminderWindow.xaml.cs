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
using System.Linq;
using omni_multitool.Features.WaterReminder;


namespace omni_multitool.Features.WaterReminder
{
    /// <summary>
    /// Interaction logic for WaterReminderWindow.xaml
    /// </summary>
    public partial class WaterReminderWindow : Window
    {
        //Makes the water reminder single instance. No justification for multi instances as of now
        //Can be changed if unique instances are needed in the future
        public WaterReminderWindow()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void OpenReminderBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow? mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            if (mainWindow == null)
            {
                mainWindow = new MainWindow();
                mainWindow.Show();
            }

            mainWindow.NavigateToPage(new WaterReminderPage());
            mainWindow.Activate();

            Close();
        }

    }
}
