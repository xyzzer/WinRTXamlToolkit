using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Imaging;

namespace WinRTXamlToolkit.Sample.Views
{
    public class BlurFxContentControl : FxContentControl
    {
        public BlurFxContentControl()
        {
            this.DefaultStyleKey = typeof (FxContentControl);
        }
        protected override async Task ProcessBackgroundImage(RenderTargetBitmap rtb, WriteableBitmap wb, int pw, int ph)
        {
        }

        protected override async Task ProcessForegroundImage(RenderTargetBitmap rtb, WriteableBitmap wb, int pw, int ph)
        {
            var sw = new Stopwatch();
            sw.Start();
            var rtbBuffer = await rtb.GetPixelsAsync();
            var rtbPixels = rtbBuffer.GetPixels();
            var wbBuffer = wb.PixelBuffer;
            var wbPixels = wbBuffer.GetPixels();

            // Expand
            int expansion = 1;

            for (int x = 0; x < pw; x++)
                for (int y = 0; y < ph; y++)
                {
                    int x1min = Math.Max(0, x - expansion);
                    int x1max = Math.Min(x + expansion, pw - 1);
                    int y1min = Math.Max(0, y - expansion);
                    int y1max = Math.Min(y + expansion, ph - 1);

                    int count = (x1max - x1min + 1) * (y1max - y1min + 1) + 7;
                    var sum = new int[4];
                    

                    for (int x1 = x1min; x1 <= x1max; x1++)
                        for (int y1 = y1min; y1 <= y1max; y1++)
                            for (int i = 0; i < 4; i++)
                                sum[i] += 
                                    (x == x1 && y == y1) ?
                                    rtbPixels.Bytes[4 * (y1 * pw + x1) + i] * 8 :
                                    rtbPixels.Bytes[4 * (y1 * pw + x1) + i];

                    for (int i = 0; i < 4; i++)
                        wbPixels.Bytes[4 * (y * pw + x) + i] = (byte)(sum[i] / count);
                }

            wbPixels.UpdateFromBytes();
            sw.Stop();
            new MessageDialog(sw.ElapsedMilliseconds.ToString()).ShowAsync();
        }
    }
}
