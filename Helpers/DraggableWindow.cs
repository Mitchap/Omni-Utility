using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace omni_multitool.Helpers
{
    public static class DraggableWindow
    {
        public static bool GetIsDraggable(Window window) =>
            (bool)window.GetValue(IsDraggableProperty);

        public static void SetIsDraggable(Window window, bool value) =>
            window.SetValue(IsDraggableProperty, value);

        public static readonly DependencyProperty IsDraggableProperty =
            DependencyProperty.RegisterAttached(
                "IsDraggable",
                typeof(bool),
                typeof(DraggableWindow),
                new PropertyMetadata(false, OnIsDraggableChanged)
            );

        private static void OnIsDraggableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Window window) return;

            if ((bool)e.NewValue)
            {
                window.MouseLeftButtonDown += Window_MouseLeftButtonDown;
            }
            else
            {
                window.MouseLeftButtonDown -= Window_MouseLeftButtonDown;
            }
        }

        private static void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Window window && e.ButtonState == MouseButtonState.Pressed)
            {
                window.DragMove();
            }
        }
    }
}
