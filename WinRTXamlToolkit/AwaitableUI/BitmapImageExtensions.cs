using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.AwaitableUI
{
    /// <summary>
    /// Extension methods for awaiting BitmapImage state changes.
    /// </summary>
    public static class BitmapImageExtensions
    {
        /// <summary>
        /// Waits for the BitmapImage to load.
        /// </summary>
        /// <param name="bitmapImage">The bitmap image.</param>
        /// <param name="timeoutInMs">The timeout in ms.</param>
        /// <returns></returns>
        public async static Task<ExceptionRoutedEventArgs> WaitForLoadedAsync(this BitmapImage bitmapImage, int timeoutInMs = 0)
        {
            var tcs = new TaskCompletionSource<ExceptionRoutedEventArgs>();

            // TODO: NOTE: This returns immediately if the image is already loaded,
            // but if the image already failed to load - the task will never complete and the app might hang.
            if (bitmapImage.PixelWidth > 0 ||
                bitmapImage.PixelHeight > 0)
            {
                tcs.SetResult(null);
                return await tcs.Task;
            }

            //var tc = new TimeoutCheck(bitmapImage);

            // Need to set it to null so that the compiler does not
            // complain about use of unassigned local variable.
            RoutedEventHandler reh = null;
            ExceptionRoutedEventHandler ereh = null;
            EventHandler<object> progressCheckTimerTickHandler = null;
            var progressCheckTimer = new DispatcherTimer();
            Action dismissWatchmen = () =>
                                     {
                                         bitmapImage.ImageOpened -= reh;
                                         bitmapImage.ImageFailed -= ereh;
                                         progressCheckTimer.Tick -= progressCheckTimerTickHandler;
                                         progressCheckTimer.Stop();
                                         //tc.Stop();
                                     };

            int totalWait = 0;
            progressCheckTimerTickHandler = (sender, o) =>
                                            {
                                                totalWait += 10;

                                                if (bitmapImage.PixelWidth > 0)
                                                {
                                                    dismissWatchmen.Invoke();
                                                    tcs.SetResult(null);
                                                }
                                                else if (timeoutInMs > 0 && totalWait >= timeoutInMs)
                                                {
                                                    dismissWatchmen.Invoke();
                                                    tcs.SetResult(null);
                                                    //ErrorMessage = string.Format("BitmapImage loading timed out after {0}ms for {1}.", totalWait, bitmapImage.UriSource)
                                                }
                                            };

            progressCheckTimer.Interval = TimeSpan.FromMilliseconds(10);
            progressCheckTimer.Tick += progressCheckTimerTickHandler;
            progressCheckTimer.Start();
                
            reh = (s, e) =>
            {
                dismissWatchmen.Invoke();
                tcs.SetResult(null);
            };

            ereh = (s, e) =>
            {
                dismissWatchmen.Invoke();
                tcs.SetResult(e);
            };

            bitmapImage.ImageOpened += reh;
            bitmapImage.ImageFailed += ereh;

            return await tcs.Task; 
        }
    }
}
