using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace omni_multitool.Helpers
{
    public static class DraggableWindow 
    {
        public static bool GetIsDraggable(DependencyObject element) =>
            (bool)element.GetValue(IsDraggableProperty);

        public static void SetIsDraggable(DependencyObject element, bool value) =>
            element.SetValue(IsDraggableProperty, value);

        public static readonly DependencyProperty IsDraggableProperty =
            DependencyProperty.RegisterAttached(
                "IsDraggable",
                typeof(bool),
                typeof(DraggableWindow),
                new PropertyMetadata(false, OnIsDraggableChanged)
            );
        
        private static void OnIsDraggableChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is not FrameworkElement element) return;

            if ((bool)e.NewValue)
            {
                element.PreviewMouseLeftButtonDown += TitleBar_MouseLeftButtonDown;
            }
            else
            {
                element.PreviewMouseLeftButtonDown -= TitleBar_MouseLeftButtonDown;
            }
        }

        private static void TitleBar_MouseLeftButtonDown(
            object sender,
            MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DependencyObject source &&
                FindParent<Button>(source) is not null)
            {
                return;
            }

            if (sender is FrameworkElement element &&
                Window.GetWindow(element) is Window window &&
                e.ButtonState == MouseButtonState.Pressed)
            {
                window.DragMove();
            }
        }
        //Helper for titlebar drag handler
        private static T? FindParent<T>(DependencyObject child)
        where T : DependencyObject
        {
            DependencyObject? parent = child;

            while (parent is not null)
            {
                if (parent is T target)
                    return target;

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }
    }
}
