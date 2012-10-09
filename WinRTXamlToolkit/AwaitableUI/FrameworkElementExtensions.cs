#if SILVERLIGHT
using System.Windows;
using System.Threading.Tasks;
#elif NETFX_CORE
using System.Diagnostics;
using System.Threading.Tasks;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

#elif WPF
using System.Threading.Tasks;
using System.Windows;
#endif

namespace WinRTXamlToolkit.AwaitableUI
{
    public static class FrameworkElementExtensions
    {
        /// <summary>
        /// Waits for the element to load (construct and add to the main object tree).
        /// </summary>
        public static async Task WaitForLoadedAsync(this FrameworkElement frameworkElement)
        {
            if (frameworkElement.IsInVisualTree())
                return;

            await EventAsync.FromRoutedEvent(
                eh => frameworkElement.Loaded += eh,
                eh => frameworkElement.Loaded -= eh);
        }

        /// <summary>
        /// Waits for the element to unload (disconnect from the main object tree).
        /// </summary>
        public static async Task WaitForUnloadedAsync(this FrameworkElement frameworkElement)
        {
            await EventAsync.FromRoutedEvent(
                eh => frameworkElement.Unloaded += eh,
                eh => frameworkElement.Unloaded -= eh);
        }

        public static async Task WaitForLayoutUpdateAsync(this FrameworkElement frameworkElement)
        {
            await EventAsync.FromEvent<object>(
                eh => frameworkElement.LayoutUpdated += eh,
                eh => frameworkElement.LayoutUpdated -= eh);
        }

        public static async Task WaitForNonZeroSizeAsync(this FrameworkElement frameworkElement)
        {
            while (frameworkElement.ActualWidth == 0 && frameworkElement.ActualHeight == 0)
            {
                var tcs = new TaskCompletionSource<object>();

                SizeChangedEventHandler sceh = null;

                sceh = (s, e) =>
                {
                    frameworkElement.SizeChanged -= sceh;
                    tcs.SetResult(e);
                };

                frameworkElement.SizeChanged += sceh;

                await tcs.Task;
                
                //await EventAsync.FromEvent<object>(
                //    eh => frameworkElement.LayoutUpdated += eh,
                //    eh => frameworkElement.LayoutUpdated -= eh);
            }
        }

        /// <summary>
        /// Waits for all the image sources in the visual tree to complete loading (useful to call before a page transition).
        /// </summary>
        /// <param name="frameworkElement"></param>
        /// <returns></returns>
        public static async Task WaitForImagesToLoad(this FrameworkElement frameworkElement, int millisecondsTimeout = 0)
        {
            foreach (var image in frameworkElement.GetDescendantsOfType<Image>())
            {
                if (image.Source != null)
                {
                    var bi = image.Source as BitmapImage;

                    if (bi != null)
                    {
                        await bi.WaitForLoadedAsync(millisecondsTimeout);
                    }
                    else
                    {
                        var wb = image.Source as WriteableBitmap;

                        if (wb != null)
                        {
                            await wb.WaitForLoaded(millisecondsTimeout);
                        }
                    }
                }
            }
        }
    }
}
