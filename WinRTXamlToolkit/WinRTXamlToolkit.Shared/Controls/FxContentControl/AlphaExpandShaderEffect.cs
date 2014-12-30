using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Imaging;

namespace WinRTXamlToolkit.Controls.Fx
{
    /// <summary>
    /// Implements alpha-smoothed expansion effect using CPU.
    /// The effect expands opaque and semi-transparent pixels of a bitmap using the source alpha values
    /// for smoothing and the key color for hue.
    /// It makes sense for providing an outline for text overlaid on top of an unknown background (photo/video).
    /// </summary>
    public class AlphaExpandShaderEffect : CpuShaderEffect
    {
        /// <summary>
        /// Color that specifies the hue of the expanded pixels.
        /// The alpha value is ignored.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlphaExpandShaderEffect"/> class,
        /// </summary>
        public AlphaExpandShaderEffect()
        {
            this.Color = Colors.Red;
        }

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
            var rtbBuffer = await rtb.GetPixelsAsync();
            var rtbPixels = rtbBuffer.GetPixels();
            var wbBuffer = wb.PixelBuffer;
            var wbPixels = wbBuffer.GetPixels();
            var r = this.Color.R;
            var g = this.Color.G;
            var b = this.Color.B;

            // Expand
            const int expansion = 1;

            for (int x = 0; x < pw; x++)
                for (int y = 0; y < ph; y++)
                {
                    int x1min = Math.Max(0, x - expansion);
                    int x1max = Math.Min(x + expansion, pw - 1);
                    int y1min = Math.Max(0, y - expansion);
                    int y1max = Math.Min(y + expansion, ph - 1);
                    byte maxa = 0;

                    for (int x1 = x1min; x1 <= x1max; x1++)
                        for (int y1 = y1min; y1 <= y1max; y1++)
                        {
                            var a = rtbPixels.Bytes[4 * (y1 * pw + x1) + 3];
                            if (a > maxa)
                                maxa = a;
                        }
                    wbPixels.Bytes[4 * (y * pw + x)] = b;
                    wbPixels.Bytes[4 * (y * pw + x) + 1] = g;
                    wbPixels.Bytes[4 * (y * pw + x) + 2] = r;
                    wbPixels.Bytes[4 * (y * pw + x) + 3] = maxa;
                }

            wbPixels.UpdateFromBytes();
            wb.Invalidate();
        }
    }
}