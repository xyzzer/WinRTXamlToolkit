using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
    public static class WriteableBitmapDarkenExtension
    {
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
