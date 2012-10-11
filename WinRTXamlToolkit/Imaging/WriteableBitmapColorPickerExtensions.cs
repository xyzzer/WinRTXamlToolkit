using System;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
    public static class WriteableBitmapColorPickerExtensions
    {
        #region RenderColorPickerHueLightness()
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

            target.Invalidate();
        } 
        #endregion

        #region RenderColorPickerSaturationLightnessRect()
        public static void RenderColorPickerSaturationLightnessRect(this WriteableBitmap target, double hue = 0)
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

            target.Invalidate();
        } 
        #endregion

        #region RenderColorPickerSaturationLightnessTriangle()
        public static void RenderColorPickerSaturationLightnessTriangle(this WriteableBitmap target, double hue = 0)
        {
            var pw = target.PixelWidth;
            var hw = pw / 2;
            var ph = target.PixelHeight;
            var invPh = 1.0 / ph;
            var pixels = target.PixelBuffer.GetPixels();

            for (int y = 0; y < ph; y++)
            {
                double lightness = 1.0 * (ph - 1 - y) / ph;

                var xmin = (int)(hw * (1 - invPh * y));
                var xmax = pw - xmin;

                for (int x = xmin; x < xmax; x++)
                {
                    //var saturation = (double)x / pw;
                    var saturation = (double)(x - xmin) / (xmax - xmin);
                    var c = ColorExtensions.FromHsl(hue, saturation, lightness);
                    pixels[pw * y + x] = c.AsInt();
                }
            }

            target.Invalidate();
        }
        #endregion

        #region RenderColorPickerHueValue()
        public static void RenderColorPickerHueValue(this WriteableBitmap target, double saturation)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            for (int y = 0; y < ph; y++)
            {
                double value = 1.0 * (ph - 1 - y) / ph;

                for (int x = 0; x < pw; x++)
                {
                    double hue = 360.0 * x / pw;
                    var c = ColorExtensions.FromHsv(hue, saturation, value);
                    pixels[pw * y + x] = c.AsInt();
                }
            }

            target.Invalidate();
        }
        #endregion

        #region RenderColorPickerSaturationValueRect()
        public static void RenderColorPickerSaturationValueRect(this WriteableBitmap target, double hue = 0)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            for (int y = 0; y < ph; y++)
            {
                double value = 1.0 * (ph - 1 - y) / ph;

                for (int x = 0; x < pw; x++)
                {
                    var saturation = (double)x / pw;
                    var c = ColorExtensions.FromHsv(hue, saturation, value);
                    pixels[pw * y + x] = c.AsInt();
                }
            }

            target.Invalidate();
        }
        #endregion

        #region RenderColorPickerSaturationValueTriangle()
        public static void RenderColorPickerSaturationValueTriangle(this WriteableBitmap target, double hue = 0)
        {
            var pw = target.PixelWidth;
            var hw = pw / 2;
            var ph = target.PixelHeight;
            var invPh = 1.0 / ph;
            var pixels = target.PixelBuffer.GetPixels();

            for (int y = 0; y < ph; y++)
            {
                double value = 1.0 * (ph - 1 - y) / ph;

                var xmin = (int)(hw * (1 - invPh * y));
                var xmax = pw - xmin;

                for (int x = xmin; x < xmax; x++)
                {
                    //var saturation = (double)x / pw;
                    var saturation = (double)(x - xmin) / (xmax - xmin);
                    var c = ColorExtensions.FromHsv(hue, saturation, value);
                    pixels[pw * y + x] = c.AsInt();
                }
            }

            target.Invalidate();
        }
        #endregion

        #region RenderColorPickerHueRing()
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
            const double piInv = 1.0 / Math.PI;

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

            target.Invalidate();
        } 
        #endregion
    }
}
