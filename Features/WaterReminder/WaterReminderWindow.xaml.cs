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

namespace omni_multitool.Features.WaterReminder
{
    /// <summary>
    /// Interaction logic for WaterReminderWindow.xaml
    /// </summary>
    public partial class WaterReminderWindow : Window
    {
        public WaterReminderWindow()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
