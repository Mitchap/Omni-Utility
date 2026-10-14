using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace omni_multitool.Helpers
{
    public static class ToastService
    {
        public static void ShowToast(StackPanel container, string message = "Changes saved automatically")
        {
            if (container == null) return;

            if (!container.Dispatcher.CheckAccess())
            {
                container.Dispatcher.Invoke(() => ShowToast(container, message));
                return;
            }

            // Maintain maximum limit of 3 prompts
            while (container.Children.Count >= 3)
            {
                container.Children.RemoveAt(0);
            }

            var textBlock = new TextBlock
            {
                Text = message,
                Foreground = (Brush)new BrushConverter().ConvertFrom("#C94FF8")!,
                FontSize = 14,
                FontWeight = FontWeights.Medium,
                Margin = new Thickness(0, 4, 0, 4),
                Opacity = 1.0
            };

            container.Children.Add(textBlock);

            // Display for 1 second, then fade out over 400ms
            var fadeOutAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                BeginTime = TimeSpan.FromSeconds(1.0),
                Duration = TimeSpan.FromMilliseconds(400)
            };

            fadeOutAnimation.Completed += (s, e) =>
            {
                if (container.Children.Contains(textBlock))
                {
                    container.Children.Remove(textBlock);
                }
            };

            textBlock.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
        }
    }
}
