using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls
{
    public class SquareGrid : Panel
    {
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
