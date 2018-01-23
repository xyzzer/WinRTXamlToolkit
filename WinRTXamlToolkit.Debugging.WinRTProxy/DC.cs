using System;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Debugging.WinRTProxy
{
    public static class DC
    {
        public static void Show()
        {
            Debugging.DC.Show();
        }

        public static void ShowLog()
        {
            Debugging.DC.ShowLog();
        }

        public static void Trace(object value)
        {
            Debugging.DC.Trace(value);
        }

        public static IAsyncAction ShowVisualTreeAsync()
        {
            return Debugging.DC.ShowVisualTreeAsync().AsAsyncAction();
        }

        public static IAsyncAction ShowVisualTreeAsync(UIElement element)
        {
            return Debugging.DC.ShowVisualTreeAsync(element).AsAsyncAction();
        }
    }
}
