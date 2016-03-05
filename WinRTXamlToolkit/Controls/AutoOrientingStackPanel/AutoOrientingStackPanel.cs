using System;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// A panel similar to StackPanel, but with it's orientation flipping from horizontal to vertical if all elements don't fit horizontally.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Panel" />
    public class AutoOrientingStackPanel
        : Panel
    {
        private Orientation orientation = Orientation.Horizontal;
        private double maxWidth;
        private double maxHeight;

        protected override Size MeasureOverride(Size availableSize)
        {
            var width = 0d;
            var height = 0d;
            this.maxWidth = 0d;
            this.maxHeight = 0d;

            foreach(var child in this.Children)
            {
                child.Measure(availableSize);
                var desired = child.DesiredSize;
                this.maxWidth = Math.Max(desired.Width, this.maxWidth);
                this.maxHeight = Math.Max(desired.Height, this.maxHeight);
                width += desired.Width;
                height += desired.Height;
            }

            if (width > availableSize.Width)
            {
                width = this.maxWidth;
                orientation = Orientation.Vertical;
            }
            else
            {
                height = this.maxHeight;
                orientation = Orientation.Horizontal;
            }

            return new Size(width, height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double offseth = 0;
            double offsetv = 0;

            if (this.orientation == Orientation.Horizontal)
            {
                foreach (var child in this.Children)
                {
                    child.Arrange(new Rect(offseth, offsetv, child.DesiredSize.Width, this.maxHeight));
                    offseth += child.DesiredSize.Width;
                }

                return new Size(offseth, this.maxHeight);
            }
            else
            {
                foreach (var child in this.Children)
                {
                    child.Arrange(new Rect(offseth, offsetv, this.maxWidth, child.DesiredSize.Height));
                    offsetv += child.DesiredSize.Height;
                }

                return new Size(this.maxWidth, offsetv);
            }
        }
    }
}
