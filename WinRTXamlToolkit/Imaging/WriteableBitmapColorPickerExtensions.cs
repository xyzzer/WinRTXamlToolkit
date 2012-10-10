using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
    public static class WriteableBitmapColorPickerExtensions
    {
        public static void RenderColorPickerHueLightness(this WriteableBitmap target, double saturation)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            for (int y = 0; y < ph; y++)
            {
                double lightness = 1.0 * (ph - 1 - y) / ph;

                for (int x = 0; x < pw; x++)
                {
                    double hue = 360.0 * x / pw;
                    var c = ColorExtensions.FromHsl(hue, saturation, lightness);
                    pixels[pw * y + x] = c.AsInt();
                }
            }

            pixels.UpdateFromBytes();
            target.Invalidate();
        }
    }
}
