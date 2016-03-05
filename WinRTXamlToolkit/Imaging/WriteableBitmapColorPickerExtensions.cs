using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
    /// <summary>
    /// Extension methods for WriteableBitmap used for generating color picker bitmaps.
    /// </summary>
    public static class WriteableBitmapColorPickerExtensions
    {
        #region RenderColorPickerHueLightness()
        /// <summary>
        /// Renders the color picker rectangle based on hue and lightness.
        /// </summary>
        /// <param name="target">The target bitmap.</param>
        /// <param name="saturation">The saturation.</param>
        public static void RenderColorPickerHueLightness(this WriteableBitmap target, double saturation)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            RenderColorPickerHueLightnessCore(saturation, pw, ph, pixels);

            target.Invalidate();
        }

        /// <summary>
        /// Renders the color picker rectangle based on hue and lightness asynchronously.
        /// </summary>
        /// <param name="target">The target bitmap.</param>
        /// <param name="saturation">The saturation.</param>
        /// <returns></returns>
        public static async Task RenderColorPickerHueLightnessAsync(this WriteableBitmap target, double saturation)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            await Task.Run(() => RenderColorPickerHueLightnessCore(saturation, pw, ph, pixels));

            target.Invalidate();
        }

        private static void RenderColorPickerHueLightnessCore(
            double saturation, int pw, int ph, IBufferExtensions.PixelBufferInfo pixels)
        {
            var xmax = pw - 1;
            var ymax = ph - 1;

            for (int y = 0; y < ph; y++)
            {
                double lightness = 1.0 * (ph - 1 - y) / ymax;

                for (int x = 0; x < pw; x++)
                {
                    double hue = 360.0 * x / xmax;
                    var c = ColorExtensions.FromHsl(hue, saturation, lightness);
                    pixels[pw * y + x] = c.AsInt();
                }
            }
        }
        #endregion

        #region RenderColorPickerSaturationLightnessRect()
        /// <summary>
        /// Renders the color picker rectangle based on saturation and lightness.
        /// </summary>
        /// <param name="target">The target bitmap.</param>
        /// <param name="hue">The hue.</param>
        public static void RenderColorPickerSaturationLightnessRect(this WriteableBitmap target, double hue = 0)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            RenderColorPickerSaturationLightnessRectCore(hue, pw, ph, pixels);

            target.Invalidate();
        }

        /// <summary>
        /// Renders the color picker rectangle based on saturation and lightness asynchronously.
        /// </summary>
        /// <param name="target">The target bitmap.</param>
        /// <param name="hue">The hue.</param>
        public static async Task RenderColorPickerSaturationLightnessRectAsync(this WriteableBitmap target, double hue = 0)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            await Task.Run(() => RenderColorPickerSaturationLightnessRectCore(hue, pw, ph, pixels));

            target.Invalidate();
        }

        private static void RenderColorPickerSaturationLightnessRectCore(
            double hue, int pw, int ph, IBufferExtensions.PixelBufferInfo pixels)
        {
            var xmax = pw - 1;
            var ymax = ph - 1;

            for (int y = 0; y < ph; y++)
            {
                double lightness = 1.0 * (ph - 1 - y) / ymax;

                for (int x = 0; x < pw; x++)
                {
                    var saturation = (double)x / xmax;
                    var c = ColorExtensions.FromHsl(hue, saturation, lightness);
                    pixels[pw * y + x] = c.AsInt();
                }
            }
        }
        #endregion

        #region RenderColorPickerSaturationLightnessTriangle()
        /// <summary>
        /// Renders the color picker triangle based on saturation and lightness.
        /// </summary>
        /// <param name="target">The target bitmap.</param>
        /// <param name="hue">The hue.</param>
        public static void RenderColorPickerSaturationLightnessTriangle(this WriteableBitmap target, double hue = 0)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            RenderColorPickerSaturationLightnessTriangleCore(hue, pw, ph, pixels);

            target.Invalidate();
        }

        /// <summary>
        /// Renders the color picker triangle based on saturation and lightness asynchronously.
        /// </summary>
        /// <param name="target">The target bitmap.</param>
        /// <param name="hue">The hue.</param>
        public static async Task RenderColorPickerSaturationLightnessTriangleAsync(this WriteableBitmap target, double hue = 0)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            await Task.Run(() => RenderColorPickerSaturationLightnessTriangleCore(hue, pw, ph, pixels));

            target.Invalidate();
        }

        private static void RenderColorPickerSaturationLightnessTriangleCore(
            double hue, int pw, int ph, IBufferExtensions.PixelBufferInfo pixels)
        {
            var hw = pw / 2;
            var invPh = 1.0 / ph;

            for (int y = 0; y < ph; y++)
            {
                double lightness = 1 - 1.0 * (ph - 1 - y) / ph;

                var xmin = (int)(hw * (1 - invPh * y));
                var xmax = pw - xmin;

                for (int x = xmin; x < xmax; x++)
                {
                    //var saturation = (double)x / xmax;
                    var saturation = 1 - (double)(x + xmax - pw) / pw;
                    var c = ColorExtensions.FromHsl(hue, saturation, lightness);
                    pixels[pw * y + x] = c.AsInt();
                }
            }
        }
        #endregion

        #region RenderColorPickerHueValue()
        /// <summary>
        /// Renders the color picker rectangle based on hue and value.
        /// </summary>
        /// <param name="target">The target bitmap.</param>
        /// <param name="saturation">The saturation.</param>
        public static void RenderColorPickerHueValue(this WriteableBitmap target, double saturation)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            RenderColorPickerHueValueCore(saturation, pw, ph, pixels);

            target.Invalidate();
        }

        /// <summary>
        /// Renders the color picker rectangle based on hue and value asynchronously.
        /// </summary>
        /// <param name="target">The target bitmap.</param>
        /// <param name="saturation">The saturation.</param>
        public static async Task RenderColorPickerHueValueAsync(this WriteableBitmap target, double saturation)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            await Task.Run(() => RenderColorPickerHueValueCore(saturation, pw, ph, pixels));

            target.Invalidate();
        }

        private static void RenderColorPickerHueValueCore(
            double saturation, int pw, int ph, IBufferExtensions.PixelBufferInfo pixels)
        {
            var xmax = pw - 1;
            var ymax = ph - 1;

            for (int y = 0; y < ph; y++)
            {
                double value = 1.0 * (ph - 1 - y) / ymax;

                for (int x = 0; x < pw; x++)
                {
                    double hue = 360.0 * x / xmax;
                    var c = ColorExtensions.FromHsv(hue, saturation, value);
                    pixels[pw * y + x] = c.AsInt();
                }
            }
        }
        #endregion

        #region RenderColorPickerSaturationValueRect()
        /// <summary>
        /// Renders the color picker rectangle based on saturation and value.
        /// </summary>
        /// <param name="target">The target bitmap.</param>
        /// <param name="hue">The hue.</param>
        public static void RenderColorPickerSaturationValueRect(this WriteableBitmap target, double hue = 0)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            RenderColorPickerSaturationValueRectCore(hue, pw, ph, pixels);

            target.Invalidate();
        }

        /// <summary>
        /// Renders the color picker rectangle based on saturation and value asynchronously.
        /// </summary>
        /// <param name="target">The target bitmap.</param>
        /// <param name="hue">The hue.</param>
        public static async Task RenderColorPickerSaturationValueRectAsync(this WriteableBitmap target, double hue = 0)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            await Task.Run(() => RenderColorPickerSaturationValueRectCore(hue, pw, ph, pixels));

            target.Invalidate();
        }

        private static void RenderColorPickerSaturationValueRectCore(
            double hue, int pw, int ph, IBufferExtensions.PixelBufferInfo pixels)
        {
            var xmax = pw - 1;
            var ymax = ph - 1;

            for (int y = 0; y < ph; y++)
            {
                double value = 1.0 * (ph - 1 - y) / ymax;

                for (int x = 0; x < pw; x++)
                {
                    var saturation = (double)x / xmax;
                    var c = ColorExtensions.FromHsv(hue, saturation, value);
                    pixels[pw * y + x] = c.AsInt();
                }
            }
        }
        #endregion

        #region RenderColorPickerSaturationValueTriangleAsync()
        /// <summary>
        /// Renders the color picker triangle based on saturation and value.
        /// </summary>
        /// <param name="target">The target bitmap.</param>
        /// <param name="hue">The hue.</param>
        public static void RenderColorPickerSaturationValueTriangle(this WriteableBitmap target, double hue = 0)
        {
            var pw = target.PixelWidth;
            var hw = pw / 2;
            var ph = target.PixelHeight;
            var invPh = 1.0 / ph;
            var pixels = target.PixelBuffer.GetPixels();

            RenderColorPickerSaturationValueTriangleCore(hue, ph, hw, invPh, pw, pixels);

            target.Invalidate();
        }

        /// <summary>
        /// Renders the color picker triangle based on saturation and value asynchronously.
        /// </summary>
        /// <param name="target">The target bitmap.</param>
        /// <param name="hue">The hue.</param>
        public static async Task RenderColorPickerSaturationValueTriangleAsync(this WriteableBitmap target, double hue = 0)
        {
            var pw = target.PixelWidth;
            var hw = pw / 2;
            var ph = target.PixelHeight;
            var invPh = 1.0 / ph;
            var pixels = target.PixelBuffer.GetPixels();

            await Task.Run(() => RenderColorPickerSaturationValueTriangleCore(hue, ph, hw, invPh, pw, pixels));

            target.Invalidate();
        }

        private static void RenderColorPickerSaturationValueTriangleCore(
            double hue, int ph, int hw, double invPh, int pw, IBufferExtensions.PixelBufferInfo pixels)
        {
            for (int y = 0; y < ph; y++)
            {
                double value = 1 - 1.0 * (ph - 1 - y) / ph;

                var xmin = (int)(hw * (1 - invPh * y));
                var xmax = pw - xmin;

                for (int x = xmin; x < xmax; x++)
                {
                    //var saturation = (double)x / pw;
                    var saturation = 1 - (double)(x + xmax - pw) / pw;
                    var c = ColorExtensions.FromHsv(hue, saturation, value);
                    pixels[pw * y + x] = c.AsInt();
                }
            }
        }
        #endregion

        #region RenderColorPickerHueRing()
        /// <summary>
        /// Renders the color picker hue ring.
        /// </summary>
        /// <param name="target">The target bitmap.</param>
        /// <param name="innerRingRadius">The inner ring radius.</param>
        /// <param name="outerRingRadius">The outer ring radius.</param>
        public static void RenderColorPickerHueRing(this WriteableBitmap target, int innerRingRadius = 0, int outerRingRadius = 0)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            RenderColorPickerHueRingCore(innerRingRadius, outerRingRadius, pw, ph, pixels);

            target.Invalidate();
        }

        /// <summary>
        /// Renders the color picker hue ring asynchronously.
        /// </summary>
        /// <param name="target">The target bitmap.</param>
        /// <param name="innerRingRadius">The inner ring radius.</param>
        /// <param name="outerRingRadius">The outer ring radius.</param>
        public static async Task RenderColorPickerHueRingAsync(this WriteableBitmap target, int innerRingRadius = 0, int outerRingRadius = 0)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            await Task.Run(() => RenderColorPickerHueRingCore(innerRingRadius, outerRingRadius, pw, ph, pixels));

            target.Invalidate();
        }

        private static void RenderColorPickerHueRingCore(
            int innerRingRadius, int outerRingRadius, int pw, int ph, IBufferExtensions.PixelBufferInfo pixels)
        {
            var pch = pw / 2;
            var pcv = ph / 2;

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

            //var orr22 = (outerRingRadius - 5) * (outerRingRadius - 5);
            //var irr22 = (innerRingRadius + 5) * (innerRingRadius + 5);
            const double piInv = 1.0 / Math.PI;

            for (int y = 0; y < ph; y++)
            {
                for (int x = 0; x < pw; x++)
                {
                    // Radius square
                    var r2 = (x - pch) * (x - pch) + (y - pcv) * (y - pcv);

                    if (r2 >= irr2 &&
                        r2 <= orr2)
                    {
                        var angleRadians = Math.Atan2(y - pcv, x - pch);
                        var angleDegrees = (angleRadians * 180 * piInv + 90 + 360) % 360;
                        //var alpha = (r2 - irr22 < 5) || (orr22 - r2 < 5) ? 0.5 : 1;
                        //var c = ColorExtensions.FromHsl(angleDegrees, 1.0 * alpha, 0.5 * alpha, alpha);
                        var c = ColorExtensions.FromHsl(angleDegrees, 1.0, 0.5);
                        pixels[pw * y + x] = c.AsInt();
                    }
                    //else
                    //{
                    //    pixels[pw * y + x] = int.MaxValue;
                    //}
                }
            }
        }
        #endregion 

        #region RenderColorPickerHSVHueBar()
        /// <summary>
        /// Renders the color picker HSV hue bar.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="saturation">The saturation.</param>
        /// <param name="value">The value.</param>
        public static void RenderColorPickerHSVHueBar(this WriteableBitmap target, double saturation, double value)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            RenderColorPickerHSVHueBarCore(saturation, value, pw, ph, pixels);

            target.Invalidate();
        }

        /// <summary>
        /// Renders the color picker HSV hue bar async.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="saturation">The saturation.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static async Task RenderColorPickerHSVHueBarAsync(this WriteableBitmap target, double saturation, double value)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            await Task.Run(() => RenderColorPickerHSVHueBarCore(saturation, value, pw, ph, pixels));

            target.Invalidate();
        }

        private static void RenderColorPickerHSVHueBarCore(
            double saturation, double value, int pw, int ph, IBufferExtensions.PixelBufferInfo pixels)
        {
            var xmax = pw - 1;

            for (int x = 0; x < pw; x++)
            {
                double hue = 360.0 * x / xmax;
                var c = ColorExtensions.FromHsv(hue, saturation, value);

                for (int y = 0; y < ph; y++)
                {
                    pixels[pw * y + x] = c.AsInt();
                }
            }
        }
        #endregion

        #region RenderColorPickerHSVSaturationBar()
        /// <summary>
        /// Renders the color picker HSV saturation bar.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="hue">The hue.</param>
        /// <param name="value">The value.</param>
        public static void RenderColorPickerHSVSaturationBar(this WriteableBitmap target, double hue, double value)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();
            
            RenderColorPickerHSVSaturationBarCore(hue, value, pw, ph, pixels);

            target.Invalidate();
        }

        /// <summary>
        /// Renders the color picker HSV saturation bar async.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="hue">The hue.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static async Task RenderColorPickerHSVSaturationBarAsync(this WriteableBitmap target, double hue, double value)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            await Task.Run(() => RenderColorPickerHSVSaturationBarCore(hue, value, pw, ph, pixels));

            target.Invalidate();
        }

        private static void RenderColorPickerHSVSaturationBarCore(
            double hue, double value, int pw, int ph, IBufferExtensions.PixelBufferInfo pixels)
        {
            var xmax = pw - 1;

            for (int x = 0; x < pw; x++)
            {
                double saturation = (double)x / xmax;
                var c = ColorExtensions.FromHsv(hue, saturation, value);

                for (int y = 0; y < ph; y++)
                {
                    pixels[pw * y + x] = c.AsInt();
                }
            }
        }
        #endregion

        #region RenderColorPickerHSVValueBar()
        /// <summary>
        /// Renders the color picker HSV value bar.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="hue">The hue.</param>
        /// <param name="saturation">The saturation.</param>
        public static void RenderColorPickerHSVValueBar(this WriteableBitmap target, double hue, double saturation)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            RenderColorPickerHSVValueBarCore(hue, saturation, pw, ph, pixels);

            target.Invalidate();
        }

        /// <summary>
        /// Renders the color picker HSV value bar async.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="hue">The hue.</param>
        /// <param name="saturation">The saturation.</param>
        /// <returns></returns>
        public static async Task RenderColorPickerHSVValueBarAsync(this WriteableBitmap target, double hue, double saturation)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            await Task.Run(() => RenderColorPickerHSVValueBarCore(hue, saturation, pw, ph, pixels));

            target.Invalidate();
        }

        private static void RenderColorPickerHSVValueBarCore(
            double hue, double saturation, int pw, int ph, IBufferExtensions.PixelBufferInfo pixels)
        {
            var xmax = pw - 1;

            for (int x = 0; x < pw; x++)
            {
                double value = (double)x / xmax;
                var c = ColorExtensions.FromHsv(hue, saturation, value);

                for (int y = 0; y < ph; y++)
                {
                    pixels[pw * y + x] = c.AsInt();
                }
            }
        }
        #endregion

        #region RenderColorPickerHSLHueBar()
        /// <summary>
        /// Renders the color picker HSL hue bar.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="saturation">The saturation.</param>
        /// <param name="lightness">The lightness.</param>
        public static void RenderColorPickerHSLHueBar(this WriteableBitmap target, double saturation, double lightness)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();
            RenderColorPickerHSLHueBarCore(saturation, lightness, pw, ph, pixels);

            target.Invalidate();
        }

        /// <summary>
        /// Renders the color picker HSL hue bar async.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="saturation">The saturation.</param>
        /// <param name="lightness">The lightness.</param>
        /// <returns></returns>
        public static async Task RenderColorPickerHSLHueBarAsync(this WriteableBitmap target, double saturation, double lightness)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            await Task.Run(() => RenderColorPickerHSLHueBarCore(saturation, lightness, pw, ph, pixels));

            target.Invalidate();
        }

        private static void RenderColorPickerHSLHueBarCore(
            double saturation, double lightness, int pw, int ph, IBufferExtensions.PixelBufferInfo pixels)
        {
            var xmax = pw - 1;

            for (int x = 0; x < pw; x++)
            {
                double hue = 360.0 * x / xmax;
                var c = ColorExtensions.FromHsl(hue, saturation, lightness);

                for (int y = 0; y < ph; y++)
                {
                    pixels[pw * y + x] = c.AsInt();
                }
            }
        }
        #endregion

        #region RenderColorPickerHSLSaturationBar()
        /// <summary>
        /// Renders the color picker HSL saturation bar.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="hue">The hue.</param>
        /// <param name="lightness">The lightness.</param>
        public static void RenderColorPickerHSLSaturationBar(this WriteableBitmap target, double hue, double lightness)
        {
            var pixels = target.PixelBuffer.GetPixels();
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;

            RenderColorPickerHSLSaturationBarCore(hue, lightness, pw, ph, pixels);

            target.Invalidate();
        }

        /// <summary>
        /// Renders the color picker HSL saturation bar async.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="hue">The hue.</param>
        /// <param name="lightness">The lightness.</param>
        /// <returns></returns>
        public static async Task RenderColorPickerHSLSaturationBarAsync(this WriteableBitmap target, double hue, double lightness)
        {
            var pixels = target.PixelBuffer.GetPixels();
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;

            await Task.Run(() => RenderColorPickerHSLSaturationBarCore(hue, lightness, pw, ph, pixels));

            target.Invalidate();
        }

        private static void RenderColorPickerHSLSaturationBarCore(
            double hue, double lightness, int pw, int ph, IBufferExtensions.PixelBufferInfo pixels)
        {
            var xmax = pw - 1;

            for (int x = 0; x < pw; x++)
            {
                double saturation = (double)x / xmax;
                var c = ColorExtensions.FromHsl(hue, saturation, lightness);

                for (int y = 0; y < ph; y++)
                {
                    pixels[pw * y + x] = c.AsInt();
                }
            }
        }
        #endregion

        #region RenderColorPickerHSLLightnessBar()
        /// <summary>
        /// Renders the color picker HSL lightness bar.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="hue">The hue.</param>
        /// <param name="saturation">The saturation.</param>
        public static void RenderColorPickerHSLLightnessBar(this WriteableBitmap target, double hue, double saturation)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();
            
            RenderColorPickerHSLLightnessBarCore(hue, saturation, pw, ph, pixels);

            target.Invalidate();
        }

        /// <summary>
        /// Renders the color picker HSL lightness bar async.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="hue">The hue.</param>
        /// <param name="saturation">The saturation.</param>
        /// <returns></returns>
        public static async Task RenderColorPickerHSLLightnessBarAsync(this WriteableBitmap target, double hue, double saturation)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            await Task.Run(() => RenderColorPickerHSLLightnessBarCore(hue, saturation, pw, ph, pixels));

            target.Invalidate();
        }

        private static void RenderColorPickerHSLLightnessBarCore(
            double hue, double saturation, int pw, int ph, IBufferExtensions.PixelBufferInfo pixels)
        {
            var xmax = pw - 1;

            for (int x = 0; x < pw; x++)
            {
                double lightness = (double)x / xmax;
                var c = ColorExtensions.FromHsl(hue, saturation, lightness);

                for (int y = 0; y < ph; y++)
                {
                    pixels[pw * y + x] = c.AsInt();
                }
            }
        }
        #endregion

        #region RenderColorPickerRGBRedBar()
        /// <summary>
        /// Renders the color picker RGB red bar.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="green">The green.</param>
        /// <param name="blue">The blue.</param>
        public static void RenderColorPickerRGBRedBar(this WriteableBitmap target, double green, double blue)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            RenderColorPickerRGBRedBarCore(green, blue, pw, ph, pixels);

            target.Invalidate();
        }

        /// <summary>
        /// Renders the color picker RGB red bar async.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="green">The green.</param>
        /// <param name="blue">The blue.</param>
        /// <returns></returns>
        public static async Task RenderColorPickerRGBRedBarAsync(this WriteableBitmap target, double green, double blue)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            await Task.Run(() => RenderColorPickerRGBRedBarCore(green, blue, pw, ph, pixels));

            target.Invalidate();
        }

        private static void RenderColorPickerRGBRedBarCore(
            double green, double blue, int pw, int ph, IBufferExtensions.PixelBufferInfo pixels)
        {
            var xmax = pw - 1;

            for (int x = 0; x < pw; x++)
            {
                double red = (double)x / xmax;
                var c = ColorExtensions.FromRgb(red, green, blue);

                for (int y = 0; y < ph; y++)
                {
                    pixels[pw * y + x] = c.AsInt();
                }
            }
        }
        #endregion

        #region RenderColorPickerRGBGreenBar()
        /// <summary>
        /// Renders the color picker RGB green bar.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="red">The red.</param>
        /// <param name="blue">The blue.</param>
        public static void RenderColorPickerRGBGreenBar(this WriteableBitmap target, double red, double blue)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            RenderColorPickerRGBGreenBarCore(red, blue, pw, ph, pixels);

            target.Invalidate();
        }

        /// <summary>
        /// Renders the color picker RGB green bar async.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="red">The red.</param>
        /// <param name="blue">The blue.</param>
        /// <returns></returns>
        public static async Task RenderColorPickerRGBGreenBarAsync(this WriteableBitmap target, double red, double blue)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            await Task.Run(() => RenderColorPickerRGBGreenBarCore(red, blue, pw, ph, pixels));

            target.Invalidate();
        }

        private static void RenderColorPickerRGBGreenBarCore(
            double red, double blue, int pw, int ph, IBufferExtensions.PixelBufferInfo pixels)
        {
            var xmax = pw - 1;

            for (int x = 0; x < pw; x++)
            {
                double green = (double)x / xmax;
                var c = ColorExtensions.FromRgb(red, green, blue);

                for (int y = 0; y < ph; y++)
                {
                    pixels[pw * y + x] = c.AsInt();
                }
            }
        }
        #endregion

        #region RenderColorPickerRGBBlueBar()
        /// <summary>
        /// Renders the color picker RGB blue bar.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="red">The red.</param>
        /// <param name="green">The green.</param>
        public static void RenderColorPickerRGBBlueBar(this WriteableBitmap target, double red, double green)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            RenderColorPickerRGBBlueBarCore(red, green, pw, ph, pixels);

            target.Invalidate();
        }

        /// <summary>
        /// Renders the color picker RGB blue bar async.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="red">The red.</param>
        /// <param name="green">The green.</param>
        /// <returns></returns>
        public static async Task RenderColorPickerRGBBlueBarAsync(this WriteableBitmap target, double red, double green)
        {
            var pw = target.PixelWidth;
            var ph = target.PixelHeight;
            var pixels = target.PixelBuffer.GetPixels();

            await Task.Run(() => RenderColorPickerRGBBlueBarCore(red, green, pw, ph, pixels));

            target.Invalidate();
        }

        private static void RenderColorPickerRGBBlueBarCore(
            double red, double green, int pw, int ph, IBufferExtensions.PixelBufferInfo pixels)
        {
            var xmax = pw - 1;

            for (int x = 0; x < pw; x++)
            {
                double blue = (double)x / xmax;
                var c = ColorExtensions.FromRgb(red, green, blue);

                for (int y = 0; y < ph; y++)
                {
                    pixels[pw * y + x] = c.AsInt();
                }
            }
        }
        #endregion
    }

    //public enum ColorChannel
    //{
    //    Red,
    //    Green,
    //    Blue,
    //    Hue,
    //    Saturation,
    //    Value,
    //    Lightness
    //}
}
