using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Imaging;

namespace WinRTXamlToolkit.Controls.Fx
{
    /// <summary>
    /// Implements the blur pixel shader effect running on CPU.
    /// </summary>
    public class BlurShaderEffect : CpuShaderEffect
    {
        /// <summary>
        /// Processes the RenderTargetBitmap and outputs the result to the output WriteableBitmap.
        /// </summary>
        /// <param name="rtb">The RenderTargetBitmap that typically includes a screen grab of the portion of UI.</param>
        /// <param name="wb">The WriteableBitmap that the effect output is written to.</param>
        /// <param name="pw">The pixel width of both bitmaps.</param>
        /// <param name="ph">The pixel height of both bitmaps.</param>
        /// <returns>A task that completes when the processing is complete.</returns>
        public override async Task ProcessBitmap(RenderTargetBitmap rtb, WriteableBitmap wb, int pw, int ph)
        {
            //var sw = new Stopwatch();
            //sw.Start();

            var rtbBuffer = await rtb.GetPixelsAsync();
            var rtbPixels = rtbBuffer.GetPixels();
            var wbBuffer = wb.PixelBuffer;
            var wbPixels = wbBuffer.GetPixels();

            // Blur
            int radius = 1;

            for (int x = 0; x < pw; x++)
                for (int y = 0; y < ph; y++)
                {
                    int x1min = Math.Max(0, x - radius);
                    int x1max = Math.Min(x + radius, pw - 1);
                    int y1min = Math.Max(0, y - radius);
                    int y1max = Math.Min(y + radius, ph - 1);

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
            wb.Invalidate();

            //sw.Stop();
            //new MessageDialog(sw.ElapsedMilliseconds.ToString()).ShowAsync();
        }
    }
}