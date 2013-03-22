//HSL and HSV conversions credits to Wikipedia contributors (http://en.wikipedia.org/wiki/HSL_and_HSV)
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Windows.UI;
using Windows.UI.Xaml.Markup;

namespace WinRTXamlToolkit.Imaging
{
    /// <summary>
    /// Extension and helper methods for converting color values
    /// between different RGB data types and different color spaces.
    /// </summary>
    public static class ColorExtensions
    {
        #region AsInt()
        /// <summary>
        /// Returns the color value as a premultiplied Int32 - 4 byte ARGB structure.
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
        #endregion

        #region FromString()
        /// <summary>
        /// Returns a Color based on XAML color string.
        /// </summary>
        /// <param name="c">The color string. Any format used in XAML should work.</param>
        /// <returns></returns>
        public static Color FromString(string c)
        {
            if (string.IsNullOrEmpty(c))
                throw new ArgumentException("Invalid color string.", "c");

            if (c[0] == '#')
            {
                switch (c.Length)
                {
                    case 9:
                    {
                        //var cuint = uint.Parse(c.Substring(1), NumberStyles.HexNumber);
                        var cuint = Convert.ToUInt32(c.Substring(1), 16);
                        var a = (byte)(cuint >> 24);
                        var r = (byte)((cuint >> 16) & 0xff);
                        var g = (byte)((cuint >> 8) & 0xff);
                        var b = (byte)(cuint & 0xff);

                        return Color.FromArgb(a, r, g, b);
                    }
                    case 7:
                    {
                        var cuint = Convert.ToUInt32(c.Substring(1), 16);
                        var r = (byte)((cuint >> 16) & 0xff);
                        var g = (byte)((cuint >> 8) & 0xff);
                        var b = (byte)(cuint & 0xff);

                        return Color.FromArgb(255, r, g, b);
                    }
                    case 5:
                    {
                        var cuint = Convert.ToUInt16(c.Substring(1), 16);
                        var a = (byte)(cuint >> 12);
                        var r = (byte)((cuint >> 8) & 0xf);
                        var g = (byte)((cuint >> 4) & 0xf);
                        var b = (byte)(cuint & 0xf);
                        a = (byte)(a << 4 | a);
                        r = (byte)(r << 4 | r);
                        g = (byte)(g << 4 | g);
                        b = (byte)(b << 4 | b);

                        return Color.FromArgb(a, r, g, b);
                    }
                    case 4:
                    {
                        var cuint = Convert.ToUInt16(c.Substring(1), 16);
                        var r = (byte)((cuint >> 8) & 0xf);
                        var g = (byte)((cuint >> 4) & 0xf);
                        var b = (byte)(cuint & 0xf);
                        r = (byte)(r << 4 | r);
                        g = (byte)(g << 4 | g);
                        b = (byte)(b << 4 | b);

                        return Color.FromArgb(255, r, g, b);
                    }
                    default:
                        throw new FormatException(string.Format("The {0} string passed in the c argument is not a recognized Color format.", c));
                }
            }
            else if (
                c.Length > 3 &&
                c[0] == 's' &&
                c[1] == 'c' &&
                c[2] == '#')
            {
                var values = c.Split(',');

                if (values.Length == 4)
                {
                    var scA = double.Parse(values[0].Substring(3));
                    var scR = double.Parse(values[1]);
                    var scG = double.Parse(values[2]);
                    var scB = double.Parse(values[3]);

                    return Color.FromArgb(
                        (byte)(scA * 255),
                        (byte)(scR * 255),
                        (byte)(scG * 255),
                        (byte)(scB * 255));
                }
                else if (values.Length == 3)
                {
                    var scR = double.Parse(values[0].Substring(3));
                    var scG = double.Parse(values[1]);
                    var scB = double.Parse(values[2]);

                    return Color.FromArgb(
                        255,
                        (byte)(scR * 255),
                        (byte)(scG * 255),
                        (byte)(scB * 255));
                }
                else
                {
                    throw new FormatException(string.Format("The {0} string passed in the c argument is not a recognized Color format (sc#[scA,]scR,scG,scB).", c));
                }
            }
            else
            {
                var prop = typeof(Colors).GetTypeInfo().GetDeclaredProperty(c);
                return (Color)prop.GetValue(null);
            }
        } 
        #endregion

        #region FromStringUsingXamlReader()
        /// <summary>
        /// Returns a Color based on XAML color string.
        /// </summary>
        /// <remarks>
        /// A reference implementation that parses the color using the XAML parser.
        /// About 40 times slower than FromString() and needs to be run on dispatcher thread.
        /// Use FromString() instead.
        /// </remarks>
        /// <param name="c">The color string. Any format used in XAML should work.</param>
        /// <returns></returns>
        public static Color FromStringUsingXamlReader(string c)
        {
            return
                (Color)
                XamlReader.Load(
                    string.Format(
                        "<Color xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">{0}</Color>",
                        c));
        }
        #endregion

        #region FromRgb()
        /// <summary>
        /// Returns a Color based on 0..1 double RGB values.
        /// </summary>
        /// <param name="red">The red.</param>
        /// <param name="green">The green.</param>
        /// <param name="blue">The blue.</param>
        /// <returns></returns>
        public static Color FromRgb(double red, double green, double blue)
        {
            return Color.FromArgb(
                255,
                (byte)(255.0 * red),
                (byte)(255.0 * green),
                (byte)(255.0 * blue));
        } 
        #endregion

        #region FromHsl()
        /// <summary>
        /// Returns a Color struct based on HSL model.
        /// </summary>
        /// <param name="hue">0..360 range hue</param>
        /// <param name="saturation">0..1 range saturation</param>
        /// <param name="lightness">0..1 range lightness</param>
        /// <param name="alpha">0..1 alpha</param>
        /// <returns></returns>
        public static Color FromHsl(double hue, double saturation, double lightness, double alpha = 1.0)
        {
            Debug.Assert(hue >= 0);
            Debug.Assert(hue <= 360);

            double chroma = (1 - Math.Abs(2 * lightness - 1)) * saturation;
            double h1 = hue / 60;
            double x = chroma * (1 - Math.Abs(h1 % 2 - 1));
            double m = lightness - 0.5 * chroma;
            double r1, g1, b1;

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
        #endregion

        #region FromHsv()
        /// <summary>
        /// Returns a Color struct based on HSV model.
        /// </summary>
        /// <param name="hue">0..360 range hue</param>
        /// <param name="saturation">0..1 range saturation</param>
        /// <param name="value">0..1 range value</param>
        /// <param name="alpha">0..1 alpha</param>
        /// <returns></returns>
        public static Color FromHsv(double hue, double saturation, double value, double alpha = 1.0)
        {
            Debug.Assert(hue >= 0);
            Debug.Assert(hue <= 360);

            double chroma = value * saturation;
            double h1 = hue / 60;
            double x = chroma * (1 - Math.Abs(h1 % 2 - 1));
            double m = value - chroma;
            double r1, g1, b1;

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
        #endregion

        #region ToHsl()
        /// <summary>
        /// Converts an RGBA Color the HSL representation.
        /// </summary>
        /// <param name="rgba">The rgba.</param>
        /// <returns></returns>
        public static HslColor ToHsl(this Color rgba)
        {
            const double toDouble = 1.0 / 255;
            var r = toDouble * rgba.R;
            var g = toDouble * rgba.G;
            var b = toDouble * rgba.B;
            var max = Math.Max(Math.Max(r, g), b);
            var min = Math.Min(Math.Min(r, g), b);
            var chroma = max - min;
            double h1;

// ReSharper disable CompareOfFloatsByEqualityOperator
            if (chroma == 0)
            {
                h1 = 0;
            }
            else if (max == r)
            {
                h1 = ((g - b) / chroma) % 6;
            }
            else if (max == g)
            {
                h1 = 2 + (b - r ) / chroma;
            }
            else //if (max == b)
            {
                h1 = 4 + (r - g ) / chroma;
            }

            double lightness = 0.5 * (max - min);
            double saturation = chroma == 0 ? 0 : chroma / (1 - Math.Abs(2 * lightness - 1));
            HslColor ret;
            ret.H = 60 * h1;
            ret.S = saturation;
            ret.L = lightness;
            ret.A = toDouble * rgba.A;
            return ret;
// ReSharper restore CompareOfFloatsByEqualityOperator
        } 
        #endregion

        #region ToHsv()
        /// <summary>
        /// Converts an RGBA Color the HSV representation.
        /// </summary>
        /// <param name="rgba">The rgba.</param>
        /// <returns></returns>
        public static HsvColor ToHsv(this Color rgba)
        {
            const double toDouble = 1.0 / 255;
            var r = toDouble * rgba.R;
            var g = toDouble * rgba.G;
            var b = toDouble * rgba.B;
            var max = Math.Max(Math.Max(r, g), b);
            var min = Math.Min(Math.Min(r, g), b);
            var chroma = max - min;
            double h1;

// ReSharper disable CompareOfFloatsByEqualityOperator
            if (chroma == 0)
            {
                h1 = 0;
            }
            else if (max == r)
            {
                h1 = ((g - b) / chroma) % 6;
            }
            else if (max == g)
            {
                h1 = 2 + (b - r) / chroma;
            }
            else //if (max == b)
            {
                h1 = 4 + (r - g) / chroma;
            }

            double lightness = 0.5 * (max - min);
            double saturation = chroma == 0 ? 0 : chroma / (1 - Math.Abs(2 * lightness - 1));
            HsvColor ret;
            ret.H = 60 * h1;
            ret.S = saturation;
            ret.V = max;
            ret.A = toDouble * rgba.A;
            return ret;
// ReSharper restore CompareOfFloatsByEqualityOperator
        }
        #endregion

        #region IntColorFromBytes()
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
        #endregion

        #region ToPixels()
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
        #endregion

        #region ToBytes()
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
        /// <param name="pixels">Source int pixel buffer</param>
        /// <param name="bytes">Target byte array</param>
        /// <returns></returns>
        public static byte[] ToBytes(this int[] pixels, byte[] bytes)
        {
            var j = 0;

            for (int i = 0; i < bytes.Length; i += 4)
            {
                bytes[i + 3] = (byte)((pixels[j] >> 24) & 0xff);
                bytes[i + 2] = (byte)((pixels[j] >> 16) & 0xff);
                bytes[i + 1] = (byte)((pixels[j] >> 8) & 0xff);
                bytes[i + 0] = (byte)((pixels[j]) & 0xff);
                j++;
            }

            return bytes;
        } 
        #endregion

        #region MaxDiff()
        /// <summary>
        /// Returns the maximum difference of any of the RGBA byte components of the two int-encoded color values.
        /// </summary>
        /// <remarks>
        /// This is useful in tolerance-enabled image processing algorithms.
        /// </remarks>
        /// <param name="pixel">Pixel color</param>
        /// <param name="color">Color to compare with</param>
        /// <returns>Maximum difference of any of the RGBA byte components of the two parameters.</returns>
        public static byte MaxDiff(this int pixel, int color)
        {
            //TODO: The bitwise & operators in the first statement don't seem to be necessary
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
        #endregion

        public static List<Color> GetNamedColors()
        {
            var colorsProperties = typeof(Colors).GetTypeInfo().DeclaredProperties;

            // Get defined colors
            return colorsProperties.Select(pi => (Color)pi.GetValue(null)).ToList();
        }

        public static List<string> GetColorNames()
        {
            var colorsProperties = typeof(Colors).GetTypeInfo().DeclaredProperties;

            // Get defined colors
            return colorsProperties.Select(pi => pi.Name).ToList();
        }
    }

    /// <summary>
    /// Defines a color in Hue/Saturation/Lightness (HSL) space.
    /// </summary>
    public struct HslColor
    {
        /// <summary>
        /// The Hue in 0..360 range.
        /// </summary>
        public double H;
        /// <summary>
        /// The Saturation in 0..1 range.
        /// </summary>
        public double S;
        /// <summary>
        /// The Lightness in 0..1 range.
        /// </summary>
        public double L;
        /// <summary>
        /// The Alpha/opacity in 0..1 range.
        /// </summary>
        public double A;
    }

    /// <summary>
    /// Defines a color in Hue/Saturation/Value (HSV) space.
    /// </summary>
    public struct HsvColor
    {
        /// <summary>
        /// The Hue in 0..360 range.
        /// </summary>
        public double H;
        /// <summary>
        /// The Saturation in 0..1 range.
        /// </summary>
        public double S;
        /// <summary>
        /// The Value in 0..1 range.
        /// </summary>
        public double V;
        /// <summary>
        /// The Alpha/opacity in 0..1 range.
        /// </summary>
        public double A;
    }
}
