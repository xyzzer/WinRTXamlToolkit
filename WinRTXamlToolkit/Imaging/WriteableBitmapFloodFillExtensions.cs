#if SILVERLIGHT
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using SilverlightWriteableBitmapFun;
#elif NETFX_CORE
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;

#endif

namespace WinRTXamlToolkit.Imaging
{
    /// <summary>
    /// WriteableBitmap extensions for filling regions of the bitmap with flood fill and similar algorithms.
    /// </summary>
    public static class WriteableBitmapFloodFillExtensions
    {
        private struct Pnt
        {
            public int X;
            public int Y;
        }

        /// <summary>
        /// Fills a region of the bitmap within an outline
        /// of <paramref name="outlineColor"/>
        /// with a <paramref name="fillColor"/>.
        /// </summary>
        /// <remarks>
        /// This is the most simple flood fill algorithm implementation.
        /// Not the most efficient.
        /// </remarks>
        /// <param name="target">Bitmap to fill.</param>
        /// <param name="x">X coordinate of the fill seed point.</param>
        /// <param name="y">Y coordinate of the fill seed point.</param>
        /// <param name="outlineColor">Color of the outline.</param>
        /// <param name="fillColor">New color.</param>
        public static void FloodFill(this WriteableBitmap target, int x, int y, int outlineColor, int fillColor)
        {
            var width = target.PixelWidth;
            var height = target.PixelHeight;
            var queue = new List<Pnt>();

#if NETFX_CORE
            var pixels = target.PixelBuffer.GetPixels();
#else
            var pixels = target.Pixels;
#endif

            queue.Add(new Pnt { X = x, Y = y });

            while (queue.Count > 0)
            {
                var p = queue[queue.Count - 1];
                queue.RemoveAt(queue.Count - 1);
                if (p.X == -1) continue;
                if (p.X == width) continue;
                if (p.Y == -1) continue;
                if (p.Y == height) continue;
                if (pixels[width * p.Y + p.X] == outlineColor) continue;
                if (pixels[width * p.Y + p.X] == fillColor) continue;

                pixels[width * p.Y + p.X] = fillColor;
                queue.Add(new Pnt { X = p.X, Y = p.Y - 1 });
                queue.Add(new Pnt { X = p.X + 1, Y = p.Y });
                queue.Add(new Pnt { X = p.X, Y = p.Y + 1 });
                queue.Add(new Pnt { X = p.X - 1, Y = p.Y });
            }

            target.Invalidate();
        }

        /// <summary>
        /// Fills a region of the bitmap replacing
        /// the <paramref name="oldColor"/>
        /// with a <paramref name="fillColor"/>.
        /// </summary>
        /// <remarks>
        /// This is the most simple flood fill algorithm implementation.
        /// Not the most efficient.
        /// </remarks>
        /// <param name="target">Bitmap to fill.</param>
        /// <param name="x">X coordinate of the fill seed point.</param>
        /// <param name="y">Y coordinate of the fill seed point.</param>
        /// <param name="oldColor">Old color to replace.</param>
        /// <param name="fillColor">New color.</param>
        public static void FloodFillReplace(this WriteableBitmap target, int x, int y, int oldColor, int fillColor)
        {
#if NETFX_CORE
            var pixels = target.PixelBuffer.GetPixels();
#else
            var pixels = target.Pixels;
#endif

            var width = target.PixelWidth;
            var height = target.PixelHeight;
            var queue = new List<Pnt>();

            queue.Add(new Pnt { X = x, Y = y });

            while (queue.Count > 0)
            {
                var p = queue[queue.Count - 1];
                queue.RemoveAt(queue.Count - 1);
                if (p.X == -1) continue;
                if (p.X == width) continue;
                if (p.Y == -1) continue;
                if (p.Y == height) continue;
                if (pixels[width * p.Y + p.X] != oldColor) continue;

                pixels[width * p.Y + p.X] = fillColor;
                queue.Add(new Pnt { X = p.X, Y = p.Y - 1 });
                queue.Add(new Pnt { X = p.X + 1, Y = p.Y });
                queue.Add(new Pnt { X = p.X, Y = p.Y + 1 });
                queue.Add(new Pnt { X = p.X - 1, Y = p.Y });
            }

            target.Invalidate();
        }

        /// <summary>
        /// Fills a region of the bitmap within an outline
        /// of <paramref name="outlineColor"/>
        /// with a <paramref name="fillColor"/>.
        /// </summary>
        /// <remarks>
        /// This is the most simple flood fill algorithm implementation.
        /// Not the most efficient.
        /// </remarks>
        /// <param name="target">Bitmap to fill.</param>
        /// <param name="x">X coordinate of the fill seed point.</param>
        /// <param name="y">Y coordinate of the fill seed point.</param>
        /// <param name="outlineColor">Color of the outline.</param>
        /// <param name="fillColor">New color.</param>
        public static void FloodFillScanline(this WriteableBitmap target, int x, int y, int outlineColor, int fillColor)
        {
#if NETFX_CORE
            var pixels = target.PixelBuffer.GetPixels();
#else
            var pixels = target.Pixels;
#endif

            var width = target.PixelWidth;
            var height = target.PixelHeight;
            var stack = new Stack<Pnt>();

            stack.Push(new Pnt { X = x, Y = y });

            while (stack.Count > 0)
            {
                Pnt pnt = stack.Pop();
                x = pnt.X;
                y = pnt.Y;

                int y1 = y;

                // Find first unfilled point in line
                while (
                    y1 >= 0 &&
                    pixels[x + width * y1] != fillColor &&
                    pixels[x + width * y1] != outlineColor)
                {
                    y1--;
                }

                y1++;
                bool spanLeft = false;
                bool spanRight = false;

                while (
                    y1 < height &&
                    pixels[x + width * y1] != fillColor &&
                    pixels[x + width * y1] != outlineColor)
                {
                    pixels[x + width * y1] = fillColor;

                    if (!spanLeft && x > 0 &&
                        pixels[x - 1 + width * y1] != fillColor &&
                        pixels[x - 1 + width * y1] != outlineColor)
                    {
                        stack.Push(new Pnt { X = x - 1, Y = y1 });

                        spanLeft = true;
                    }
                    else if (
                        spanLeft && x > 0 &&
                        (pixels[x - 1 + width * y1] == fillColor ||
                        pixels[x - 1 + width * y1] == outlineColor))
                    {
                        spanLeft = false;
                    }

                    if (!spanRight &&
                        x < width - 1 &&
                        pixels[x + 1 + width * y1] != fillColor &&
                        pixels[x + 1 + width * y1] != outlineColor)
                    {
                        stack.Push(new Pnt { X = x + 1, Y = y1 });
                        spanRight = true;
                    }
                    else if (
                        spanRight &&
                        x < width - 1 &&
                        (pixels[x + 1 + width * y1] == fillColor ||
                        pixels[x + 1 + width * y1] == outlineColor))
                    {
                        spanRight = false;
                    }

                    y1++;
                }
            }
        }

        /// <summary>
        /// Fills a region of the bitmap replacing
        /// the <paramref name="oldColor"/>
        /// with a <paramref name="fillColor"/>.
        /// </summary>
        /// <remarks>
        /// Improves upon the basic algorithm being about 7x faster.
        /// </remarks>
        /// <param name="target">Bitmap to fill.</param>
        /// <param name="x">X coordinate of the fill seed point.</param>
        /// <param name="y">Y coordinate of the fill seed point.</param>
        /// <param name="oldColor">Old color to replace.</param>
        /// <param name="fillColor">New color.</param>
        public static void FloodFillScanlineReplace(this WriteableBitmap target, int x, int y, int oldColor, int fillColor)
        {
#if NETFX_CORE
            var pixels = target.PixelBuffer.GetPixels();
#else
            var pixels = target.Pixels;
#endif

            var width = target.PixelWidth;
            var height = target.PixelHeight;

            if (pixels[x + width * y] == fillColor)
            {
                return;
            }
            
            var stack = new Stack<Pnt>();

            stack.Push(new Pnt { X = x, Y = y });

            while (stack.Count > 0)
            {
                Pnt pnt = stack.Pop();
                x = pnt.X;
                y = pnt.Y;

                int y1 = y;

                // Find first unfilled point in line
                while (
                    y1 >= 0 &&
                    pixels[x + width * y1] == oldColor)
                {
                    y1--;
                }

                y1++;
                bool spanLeft = false;
                bool spanRight = false;

                while (
                    y1 < height &&
                    pixels[x + width * y1] == oldColor)
                {
                    pixels[x + width * y1] = fillColor;

                    if (!spanLeft && x > 0 &&
                        pixels[x - 1 + width * y1] == oldColor)
                    {
                        stack.Push(new Pnt { X = x - 1, Y = y1 });

                        spanLeft = true;
                    }
                    else if (
                        spanLeft && x > 0 &&
                        pixels[x - 1 + width * y1] != oldColor)
                    {
                        spanLeft = false;
                    }

                    if (!spanRight &&
                        x < width - 1 &&
                        pixels[x + 1 + width * y1] == oldColor)
                    {
                        stack.Push(new Pnt { X = x + 1, Y = y1 });
                        spanRight = true;
                    }
                    else if (
                        spanRight &&
                        x < width - 1 &&
                        pixels[x + 1 + width * y1] != oldColor)
                    {
                        spanRight = false;
                    }

                    y1++;
                }
            }
        }

        /// <summary>
        /// Fills a region of the bitmap replacing
        /// the <paramref name="oldColor"/>
        /// with a <paramref name="fillColor"/>.
        /// </summary>
        /// <remarks>
        /// Improves upon the basic algorithm being about 7x faster.
        /// </remarks>
        /// <param name="target">Bitmap to fill.</param>
        /// <param name="x">X coordinate of the fill seed point.</param>
        /// <param name="y">Y coordinate of the fill seed point.</param>
        /// <param name="oldColor">Old color to replace.</param>
        /// <param name="fillColor">New color.</param>
        /// <param name="maxDiff">Max differenve between old color and other colors that will be filled too.</param>
        public static void FloodFillScanlineReplace(
            this WriteableBitmap target, int x, int y, int oldColor, int fillColor, byte maxDiff)
        {
#if NETFX_CORE
            var pixels = target.PixelBuffer.GetPixels();
#else
            var pixels = target.Pixels;
#endif

            var width = target.PixelWidth;
            var height = target.PixelHeight;

            if (pixels[x + width * y] == fillColor)
            {
                return;
            }

            var stack = new Stack<Pnt>();

            stack.Push(new Pnt { X = x, Y = y });

            while (stack.Count > 0)
            {
                Pnt pnt = stack.Pop();
                x = pnt.X;
                y = pnt.Y;

                int y1 = y;

                // Find first unfilled point in line
                while (
                    y1 >= 0 &&
                    pixels[x + width * y1].MaxDiff(oldColor) < maxDiff)
                //pixels[x + width * y1] == oldColor)
                {
                    y1--;
                }

                y1++;
                bool spanLeft = false;
                bool spanRight = false;

                while (
                    y1 < height &&
                    pixels[x + width * y1].MaxDiff(oldColor) < maxDiff)
                {
                    pixels[x + width * y1] = fillColor;

                    if (!spanLeft &&
                        x > 0 &&
                        pixels[x - 1 + width * y1].MaxDiff(oldColor) < maxDiff)
                    {
                        stack.Push(new Pnt { X = x - 1, Y = y1 });

                        spanLeft = true;
                    }
                    else if (
                        spanLeft &&
                        x > 0 &&
                        pixels[x - 1 + width * y1].MaxDiff(oldColor) >= maxDiff)
                    {
                        spanLeft = false;
                    }

                    if (!spanRight &&
                        x < width - 1 &&
                        pixels[x + 1 + width * y1].MaxDiff(oldColor) < maxDiff)
                    {
                        stack.Push(new Pnt { X = x + 1, Y = y1 });
                        spanRight = true;
                    }
                    else if (
                        spanRight &&
                        x < width - 1 &&
                        pixels[x + 1 + width * y1].MaxDiff(oldColor) >= maxDiff)
                    {
                        spanRight = false;
                    }

                    y1++;
                }
            }
        }
    }
}
