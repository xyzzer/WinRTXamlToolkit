using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// Layout panel that arranges its children in a NxN sized grid.
    /// Note: The elements are arranged rows first.
    /// Note: RTL is not supported.
    /// </summary>
    public class SquareGrid : Panel
    {
        /// <summary>
        /// Provides the behavior for the Measure pass of the layout cycle. Classes can override this method to define their own Measure pass behavior.
        /// </summary>
        /// <param name="availableSize">The available size that this object can give to child objects. Infinity can be specified as a value to indicate that the object will size to whatever content is available.</param>
        /// <returns>
        /// The size that this object determines it needs during layout, based on its calculations of the allocated sizes for child objects or based on other considerations such as a fixed container size.
        /// </returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            var s = Math.Min(availableSize.Width, availableSize.Height);

            var division = this.Children.Count == 0 ? 1 : Math.Ceiling(Math.Sqrt(this.Children.Count));
            var childS = s / division;
            var childSize = new Size(childS, childS);
            var left = 0d;
            var top = 0d;
            var right = 0d;
            var bottom = Math.Round(childS);
            var i = 0;

            foreach (var child in this.Children)
            {
                if (this.UseLayoutRounding)
                {
                    right = Math.Round((i % division + 1) * childS);

                    var size = new Size(right - left, bottom - top);
                    child.Measure(size);

                    i++;

                    if (i % division != 0)
                    {
                        left = right;
                    }
                    else
                    {
                        top = bottom;
                        bottom = Math.Round(Math.Floor(1 + (i / division)) * childS);
                        left = 0;
                    }
                }
                else
                {
                    child.Measure(childSize);
                }
            }

            var stretchedSize = new Size(s, s);
            return stretchedSize;
        }

        /// <summary>
        /// Provides the behavior for the Arrange pass of layout. Classes can override this method to define their own Arrange pass behavior.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this object should use to arrange itself and its children.</param>
        /// <returns>
        /// The actual size that is used after the element is arranged in layout.
        /// </returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var s = Math.Min(finalSize.Width, finalSize.Height);
            var stretchedSize = new Size(s, s);

            var division = this.Children.Count == 0 ? 1 : Math.Ceiling(Math.Sqrt(this.Children.Count));
            var childS = s / division;
            var i = 0;
            var left = 0d;
            var top = 0d;
            var right = 0d;
            var bottom = Math.Round(childS);

            foreach (var child in this.Children)
            {
                if (this.UseLayoutRounding)
                {
                    right = Math.Round((i % division + 1) * childS);

                    var rect = new Rect(left, top, right - left, bottom - top);
                    child.Arrange(rect);

                    i++;

                    if (i % division != 0)
                    {
                        left = right;
                    }
                    else
                    {
                        top = bottom;
                        bottom = Math.Round(Math.Floor(1 + (i / division)) * childS);
                        left = 0;
                    }
                }
                else
                {
                    left = (i % division) * childS;
                    top = Math.Floor(i / division) * childS;

                    var rect = new Rect(left, top, childS, childS);
                    child.Arrange(rect);
                    i++;
                }
            }

            return stretchedSize;
        }
    }
}
