using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.AwaitableUI
{
    public static class WriteableBitmapExtensions
    {
        public async static Task WaitForLoaded(this WriteableBitmap wb, int timeoutInMs = 0)
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
