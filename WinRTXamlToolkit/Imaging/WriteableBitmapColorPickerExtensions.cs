using System;
using System.Diagnostics;
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

        public static void RenderColorPickerSaturationLightness(this WriteableBitmap target, double hue = 0)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            for (int y = 0; y < ph; y++)
            {
                double lightness = 1.0 * (ph - 1 - y) / ph;

                for (int x = 0; x < pw; x++)
                {
                    var saturation = (double)x / pw;
                    var c = ColorExtensions.FromHsl(hue, saturation, lightness);
                    pixels[pw * y + x] = c.AsInt();
                }
            }

            pixels.UpdateFromBytes();
            target.Invalidate();
        }

        public static void RenderColorPickerHueRing(this WriteableBitmap target, int innerRingRadius = 0, int outerRingRadius = 0)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pch = pw / 2;
            var pcv = ph / 2;
            var pixels = target.PixelBuffer.GetPixels();
            
            if (outerRingRadius == 0)
            {
                outerRingRadius = Math.Min(pw, ph) / 2;
            }
            if (innerRingRadius == 0)
            {
                innerRingRadius = outerRingRadius * 2 / 3;
            }

            // Outer ring radius square
            var orr2 = outerRingRadius * outerRingRadius;

            // Inner ring radius square
            var irr2 = innerRingRadius * innerRingRadius;
            var piInv = 1.0 / Math.PI;

            for (int y = 0; y < ph; y++)
            {
                for (int x = 0; x < pw; x++)
                {
                    // Radius square
                    var r2 = (x - pch) * (x - pch) + (y - pcv) * (y - pcv);
                    
                    if (r2 >= irr2 && r2 <= orr2)
                    {
                        var angleRadians = Math.Atan2(y - pcv, x - pch);
                        var angleDegrees = angleRadians * 180 * piInv + 180;
                        var c = ColorExtensions.FromHsl(angleDegrees, 1.0, 0.5);
                        pixels[pw * y + x] = c.AsInt();
                    }
                }
            }

            pixels.UpdateFromBytes();
            target.Invalidate();
        }
    }
}
