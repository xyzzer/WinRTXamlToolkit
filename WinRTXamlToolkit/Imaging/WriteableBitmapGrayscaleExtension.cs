using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
    public static class WriteableBitmapGrayscaleExtension
    {
        public static WriteableBitmap Grayscale(this WriteableBitmap target)
        {
            var pixels = target.PixelBuffer.GetPixels();

            for (int i = 0; i < pixels.Bytes.Length; i += 4)
            {
                byte a = pixels.Bytes[i + 3];

                if (a > 0)
                {
                    double ad = (double)a / 255.0; // 0..1 range alpha
                    double rd = (double)pixels.Bytes[i + 2] / ad; // 0..255 range red, non-alpha-premultiplied
                    double gd = (double)pixels.Bytes[i + 1] / ad; // 0..255 range green, non-alpha-premultiplied
                    double bd = (double)pixels.Bytes[i + 0] / ad; // 0..255 range blue, non-alpha-premultiplied

                    // gain is the difference between current value and maximum (255), multiplied by the amount and alpha-premultiplied
                    double luminance = 0.2126 * rd + 0.7152 * gd + 0.0722 * bd;
                    double newR = luminance * ad;
                    double newG = newR;
                    double newB = newR;

                    pixels.Bytes[i + 0] = (byte)newB;
                    pixels.Bytes[i + 1] = (byte)newG;
                    pixels.Bytes[i + 2] = (byte)newR;
                }
            }

            pixels.UpdateFromBytes();
            target.Invalidate();
            return target;
        }

        public static WriteableBitmap Grayscale(this WriteableBitmap target, double amount)
        {
            var pixels = target.PixelBuffer.GetPixels();

            for (int i = 0; i < pixels.Bytes.Length; i += 4)
            {
                byte a = pixels.Bytes[i + 3];

                if (a > 0)
                {
                    double ad = (double)a / 255.0; // 0..1 range alpha
                    double rd = (double)pixels.Bytes[i + 2] / ad; // 0..255 range red, non-alpha-premultiplied
                    double gd = (double)pixels.Bytes[i + 1] / ad; // 0..255 range green, non-alpha-premultiplied
                    double bd = (double)pixels.Bytes[i + 0] / ad; // 0..255 range blue, non-alpha-premultiplied

                    double luminance = 0.2126 * rd + 0.7152 * gd + 0.0722 * bd;
                    double newR = ((1.0 - amount) * rd + amount * luminance) * ad;
                    double newG = ((1.0 - amount) * gd + amount * luminance) * ad;
                    double newB = ((1.0 - amount) * bd + amount * luminance) * ad;

                    pixels.Bytes[i + 0] = (byte)newB;
                    pixels.Bytes[i + 1] = (byte)newG;
                    pixels.Bytes[i + 2] = (byte)newR;
                }
            }

            pixels.UpdateFromBytes();
            target.Invalidate();
            return target;
        }
    }
}
