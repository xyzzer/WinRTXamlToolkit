using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.AwaitableUI
{
    /// <summary>
    /// Extension methods for awaiting WriteableBitmap events.
    /// </summary>
    public static class WriteableBitmapExtensions
    {
        /// <summary>
        /// Waits for the given WriteableBitmap to be loaded (non-zero size).
        /// </summary>
        /// <param name="wb">The WriteableBitmap to wait for.</param>
        /// <param name="timeoutInMs">The timeout in ms after which the wait will be cancelled. Use 0 to wait without a timeout.</param>
        /// <returns></returns>
        public async static Task WaitForLoadedAsync(this WriteableBitmap wb, int timeoutInMs = 0)
        {
            int totalWait = 0;

            while (
                wb.PixelWidth <= 1 &&
                wb.PixelHeight <= 1)
            {
                await Task.Delay(10);
                totalWait += 10;

                if (timeoutInMs > 0 &&
                    totalWait > timeoutInMs)
                    return;
            }
        }
    }
}
