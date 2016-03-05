using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
    /// <summary>
    /// WriteableBitmap extensions to make the image brighter.
    /// </summary>
    public static class WriteableBitmapLightenExtension
    {
        /// <summary>
        /// Lightens the specified bitmap.
        /// </summary>
        /// <param name="target">The target bitmap.</param>
        /// <param name="amount">The 0..1 range amount to lighten by where 0 does not affect the bitmap and 1 makes the bitmap completely white.</param>
        /// <returns></returns>
        public static WriteableBitmap Lighten(this WriteableBitmap target, double amount)
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
                    double gainR = (255.0 - rd) * amount * ad;
                    double gainG = (255.0 - gd) * amount * ad;
                    double gainB = (255.0 - bd) * amount * ad;

                    pixels.Bytes[i + 0] += (byte)gainB;
                    pixels.Bytes[i + 1] += (byte)gainG;
                    pixels.Bytes[i + 2] += (byte)gainR;
                }
            }

            pixels.UpdateFromBytes();
            target.Invalidate();
            return target;
        }
    }
}
