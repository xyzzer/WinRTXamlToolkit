using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Extension methods for the Viewbox control.
    /// </summary>
    public static class ViewboxExtensions
    {
        /// <summary>
        /// Gets the child horizontal scale factor.
        /// </summary>
        /// <param name="viewbox">The viewbox.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Can't tell effective scale of a Viewbox child for a Viewbox with no child.</exception>
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

        /// <summary>
        /// Gets the child vertical scale factor.
        /// </summary>
        /// <param name="viewbox">The viewbox.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Can't tell effective scale of a Viewbox child for a Viewbox with no child.</exception>
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
