using System;
using Windows.UI;

namespace WinRTXamlToolkit.Imaging
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Returns the color value as an Int32 - 4 byte ARGB structure.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static int AsInt(this Color color)
        {
            var a = color.A + 1;
            var col = (color.A << 24)
                      | ((byte)((color.R * a) >> 8) << 16)
                      | ((byte)((color.G * a) >> 8) << 8)
                      | ((byte)((color.B * a) >> 8));
            return col;
        }

        /// <summary>
        /// Returns a Color struct based on HSL model.
        /// </summary>
        /// <param name="hue">0..360 range hue</param>
        /// <param name="saturation">0..1 range saturation</param>
        /// <param name="lightness">0..1 range lightness</param>
        /// <returns></returns>
        public static Color FromHsl(double hue, double saturation, double lightness, double alpha = 1.0)
        {
            double chroma = (1 - Math.Abs(2 * lightness - 1)) * saturation;
            double h1 = hue / 60;
            double x = chroma * (1 - Math.Abs(h1 % 2 - 1));
            double m = lightness - 0.5 * chroma;
            double r1, g1, b1;
            //double M = Math.Max(Math.Max())

            if (h1 < 1)
            {
                r1 = chroma;
                g1 = x;
                b1 = 0;
            }
            else if (h1 < 2)
            {
                r1 = x;
                g1 = chroma;
                b1 = 0;
            }
            else if (h1 < 3)
            {
                r1 = 0;
                g1 = chroma;
                b1 = x;
            }
            else if (h1 < 4)
            {
                r1 = 0;
                g1 = x;
                b1 = chroma;
            }
            else if (h1 < 5)
            {
                r1 = x;
                g1 = 0;
                b1 = chroma;
            }
            else //if (h1 < 6)
            {
                r1 = chroma;
                g1 = 0;
                b1 = x;
            }

            byte r = (byte)(255 * (r1 + m));
            byte g = (byte)(255 * (g1 + m));
            byte b = (byte)(255 * (b1 + m));
            byte a = (byte)(255 * alpha);

            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Returns a Color struct based on HSV model.
        /// </summary>
        /// <param name="hue">0..360 range hue</param>
        /// <param name="saturation">0..1 range saturation</param>
        /// <param name="lightness">0..1 range value</param>
        /// <returns></returns>
        public static Color FromHsv(double hue, double saturation, double lightness, double alpha = 1.0)
        {
            double chroma = (1 - Math.Abs(2 * lightness - 1)) * saturation;
            double h1 = hue / 60;
            double x = chroma * (1 - Math.Abs(h1 % 2 - 1));
            double m = lightness - 0.5 * chroma;
            double r1, g1, b1;
            //double M = Math.Max(Math.Max())

            if (h1 < 1)
            {
                r1 = chroma;
                g1 = x;
                b1 = 0;
            }
            else if (h1 < 2)
            {
                r1 = x;
                g1 = chroma;
                b1 = 0;
            }
            else if (h1 < 3)
            {
                r1 = 0;
                g1 = chroma;
                b1 = x;
            }
            else if (h1 < 4)
            {
                r1 = 0;
                g1 = x;
                b1 = chroma;
            }
            else if (h1 < 5)
            {
                r1 = x;
                g1 = 0;
                b1 = chroma;
            }
            else //if (h1 < 6)
            {
                r1 = chroma;
                g1 = 0;
                b1 = x;
            }

            byte r = (byte)(255 * (r1 + m));
            byte g = (byte)(255 * (g1 + m));
            byte b = (byte)(255 * (b1 + m));
            byte a = (byte)(255 * alpha);

            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Converts four bytes to an Int32 - 4 byte ARGB structure.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int IntColorFromBytes(byte a, byte r, byte g, byte b)
        {
            var col =
                a << 24
                | r << 16
                | g << 8
                | b;
            return col;
        }

        /// <summary>
        /// Converts a byte array/pixel buffer into an int array.
        /// </summary>
        /// <remarks>
        /// It might be worth it to avoid the conversion altogether by working directly with bytes,
        /// but the conversion improves cross-platform portability of the code.
        /// </remarks>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int[] ToPixels(this byte[] bytes)
        {
            var pixels = new int[bytes.Length >> 2];

            var j = 0;
            for (int i = 0; i < bytes.Length; i += 4)
            {
                pixels[j] =
                    IntColorFromBytes(
                        bytes[i + 3],
                        bytes[i + 2],
                        bytes[i + 1],
                        bytes[i + 0]);
                j++;
            }

            return pixels;
        }
        
        /// <summary>
        /// Converts an int array/pixel buffer into a new byte array.
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this int[] pixels)
        {
            var bytes = new byte[pixels.Length << 2];
            return pixels.ToBytes(bytes);
        }

        /// <summary>
        /// Copies an int array/pixel buffer into an existing byte array.
        /// </summary>
        /// <param name="pixels"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this int[] pixels, byte[] bytes)
        {
            var j = 0;
            for (int i = 0; i < bytes.Length; i += 4)
            {
                bytes[i+3] = (byte)((pixels[j] >> 24) & 0xff);
                bytes[i+2] = (byte)((pixels[j] >> 16) & 0xff);
                bytes[i+1] = (byte)((pixels[j] >> 8) & 0xff);
                bytes[i+0] = (byte)((pixels[j]) & 0xff);
                j++;
            }

            return bytes;
        }

        public static byte MaxDiff(this int pixel, int color)
        {
            byte maxDiff = (byte)Math.Abs(
                ((pixel >> 24) & 0xff) -
                ((color >> 24) & 0xff));
            maxDiff = Math.Max(maxDiff, (byte)Math.Abs(
                ((pixel >> 16) & 0xff) -
                ((color >> 16) & 0xff)));
            maxDiff = Math.Max(maxDiff, (byte)Math.Abs(
                ((pixel >> 8) & 0xff) -
                ((color >> 8) & 0xff)));
            return Math.Max(maxDiff, (byte)Math.Abs(
                (pixel & 0xff) -
                (color & 0xff)));
        }
    }
}
