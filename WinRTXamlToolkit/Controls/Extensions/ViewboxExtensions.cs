using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
    public static class ViewboxExtensions
    {
        public static double GetChildScaleX(this Viewbox viewbox)
        {
            if (viewbox.Child == null)
                throw new InvalidOperationException("Can't tell effective scale of a Viewbox child for a Viewbox with no child.");

            var fe = viewbox.Child as FrameworkElement;

            if (fe == null)
                throw new InvalidOperationException("Can't tell effective scale of a Viewbox child for a Viewbox with a child that is not a FrameworkElement.");

            if (fe.ActualWidth == 0)
                throw new InvalidOperationException("Can't tell effective scale of a Viewbox child for a Viewbox with a child that is not laid out.");

            return viewbox.ActualWidth / fe.ActualWidth;
        }

        public static double GetChildScaleY(this Viewbox viewbox)
        {
            if (viewbox.Child == null)
                throw new InvalidOperationException("Can't tell effective scale of a Viewbox child for a Viewbox with no child.");

            var fe = viewbox.Child as FrameworkElement;

            if (fe == null)
                throw new InvalidOperationException("Can't tell effective scale of a Viewbox child for a Viewbox with a child that is not a FrameworkElement.");

            if (fe.ActualHeight == 0)
                throw new InvalidOperationException("Can't tell effective scale of a Viewbox child for a Viewbox with a child that is not laid out.");

            return viewbox.ActualHeight / fe.ActualHeight;
        }
    }
}
