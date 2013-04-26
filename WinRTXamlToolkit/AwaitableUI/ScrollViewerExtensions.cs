using System.Threading.Tasks;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.AwaitableUI
{
    public static class ScrollViewerExtensions
    {
        public static async Task ScrollToVerticalOffsetAsync(this ScrollViewer scrollViewer, double offset)
        {
            if (offset < 0)
                offset = 0;

            if (offset > scrollViewer.ScrollableHeight)
                offset = scrollViewer.ScrollableHeight;

            var currentOffset = scrollViewer.VerticalOffset;
// ReSharper disable CompareOfFloatsByEqualityOperator
            if (offset == currentOffset)
                return;

            scrollViewer.ScrollToVerticalOffset(offset);

            if (scrollViewer.VerticalOffset == offset)
                return;

            if (scrollViewer.VerticalOffset != currentOffset)
                return;
// ReSharper restore CompareOfFloatsByEqualityOperator

            await EventAsync.FromEvent<ScrollViewerViewChangedEventArgs>(
                eh => scrollViewer.ViewChanged += eh,
                eh => scrollViewer.ViewChanged -= eh);
        }

        public static async Task ScrollToHorizontalOffsetAsync(this ScrollViewer scrollViewer, double offset)
        {
            if (offset < 0)
                offset = 0;

            if (offset > scrollViewer.ScrollableWidth)
                offset = scrollViewer.ScrollableWidth;

            var currentOffset = scrollViewer.HorizontalOffset;
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (offset == currentOffset)
                return;

            scrollViewer.ScrollToHorizontalOffset(offset);

            if (scrollViewer.HorizontalOffset == offset)
                return;

            if (scrollViewer.HorizontalOffset != currentOffset)
                return;
            // ReSharper restore CompareOfFloatsByEqualityOperator

            await EventAsync.FromEvent<ScrollViewerViewChangedEventArgs>(
                eh => scrollViewer.ViewChanged += eh,
                eh => scrollViewer.ViewChanged -= eh);
        }

        public static async Task ScrollToVerticalOffsetWithAnimationAsync(this ScrollViewer scrollViewer, double offset)
        {
            if (offset < 0)
                offset = 0;

            if (offset > scrollViewer.ScrollableHeight)
                offset = scrollViewer.ScrollableHeight;

            var currentOffset = scrollViewer.VerticalOffset;
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (offset == currentOffset)
                return;

            await scrollViewer.ScrollToVerticalOffsetWithAnimation(offset);

            if (scrollViewer.VerticalOffset == offset)
                return;

            if (scrollViewer.VerticalOffset != currentOffset)
                return;
            // ReSharper restore CompareOfFloatsByEqualityOperator

            await EventAsync.FromEvent<ScrollViewerViewChangedEventArgs>(
                eh => scrollViewer.ViewChanged += eh,
                eh => scrollViewer.ViewChanged -= eh);
        }

        public static async Task ScrollToHorizontalOffsetWithAnimationAsync(this ScrollViewer scrollViewer, double offset)
        {
            if (offset < 0)
                offset = 0;

            if (offset > scrollViewer.ScrollableWidth)
                offset = scrollViewer.ScrollableWidth;

            var currentOffset = scrollViewer.HorizontalOffset;
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (offset == currentOffset)
                return;

            await scrollViewer.ScrollToHorizontalOffsetWithAnimation(offset);

            if (scrollViewer.HorizontalOffset == offset)
                return;

            if (scrollViewer.HorizontalOffset != currentOffset)
                return;
            // ReSharper restore CompareOfFloatsByEqualityOperator

            await EventAsync.FromEvent<ScrollViewerViewChangedEventArgs>(
                eh => scrollViewer.ViewChanged += eh,
                eh => scrollViewer.ViewChanged -= eh);
        }
    }
}
