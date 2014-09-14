using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Imaging;

namespace WinRTXamlToolkit.Controls.Fx
{
    public class AlphaExpandShaderEffect : CpuShaderEffect
    {
        public Color Color { get; set; }

        public AlphaExpandShaderEffect()
        {
            this.Color = Colors.Red;
        }

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
            int expansion = 1;

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