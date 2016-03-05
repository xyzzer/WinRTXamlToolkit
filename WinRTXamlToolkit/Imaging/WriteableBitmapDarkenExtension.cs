using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
    /// <summary>
    /// WriteableBitmap extensions to make the image darker.
    /// </summary>
    public static class WriteableBitmapDarkenExtension
    {
        /// <summary>
        /// Darkens the specified bitmap.
        /// </summary>
        /// <param name="target">The target bitmap.</param>
        /// <param name="amount">The 0..1 range amount to darken by where 0 does not affect the bitmap and 1 makes the bitmap completely black.</param>
        /// <returns></returns>
        public static WriteableBitmap Darken(this WriteableBitmap target, double amount)
        {
            var pixels = target.PixelBuffer.GetPixels();

            for (int i = 0; i < pixels.Bytes.Length; i += 4)
            {
                byte a = pixels.Bytes[i + 3];

                if (a > 0)
                {
                    double rd = (double)pixels.Bytes[i + 2]; // 0..255 range red, alpha-premultiplied
                    double gd = (double)pixels.Bytes[i + 1]; // 0..255 range green, alpha-premultiplied
                    double bd = (double)pixels.Bytes[i + 0]; // 0..255 range blue, alpha-premultiplied

                    double newR = rd * (1 - amount);
                    double newG = gd * (1 - amount);
                    double newB = bd * (1 - amount);

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
