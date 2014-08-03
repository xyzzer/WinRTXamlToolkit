using System.Threading.Tasks;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.AwaitableUI
{
    /// <summary>
    /// Extension methods for scrolling a ScrollViewer with a way to await the scoll to complete and with scrolling animation support.
    /// </summary>
    public static class ScrollViewerExtensions
    {
        #region ScrollToVerticalOffsetAsync()
        /// <summary>
        /// Scrolls to vertical offset asynchronously.
        /// </summary>
        /// <param name="scrollViewer">The scroll viewer.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>The task that completes when scrolling is complete.</returns>
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

            if (!scrollViewer.ChangeView(null, offset, null))
            {
                return;
            }

            if (scrollViewer.VerticalOffset == offset)
                return;

            if (scrollViewer.VerticalOffset != currentOffset)
                return;
            // ReSharper restore CompareOfFloatsByEqualityOperator

            await EventAsync.FromEvent<ScrollViewerViewChangedEventArgs>(
                eh => scrollViewer.ViewChanged += eh,
                eh => scrollViewer.ViewChanged -= eh);
        } 
        #endregion

        #region ScrollToHorizontalOffsetAsync()
        /// <summary>
        /// Scrolls to horizontal offset asynchronously.
        /// </summary>
        /// <param name="scrollViewer">The scroll viewer.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>The task that completes when scrolling is complete.</returns>
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

            if (!scrollViewer.ChangeView(offset, null, null, true))
            {
                return;
            }

            if (scrollViewer.HorizontalOffset == offset)
                return;

            if (scrollViewer.HorizontalOffset != currentOffset)
                return;
            // ReSharper restore CompareOfFloatsByEqualityOperator

            await EventAsync.FromEvent<ScrollViewerViewChangedEventArgs>(
                eh => scrollViewer.ViewChanged += eh,
                eh => scrollViewer.ViewChanged -= eh);
        } 
        #endregion

        #region ScrollToVerticalOffsetWithAnimationAsync()
        /// <summary>
        /// Scrolls to vertical offset with animation asynchronously.
        /// </summary>
        /// <param name="scrollViewer">The scroll viewer.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>The task that completes when scrolling is complete.</returns>
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
        #endregion

        #region ScrollToHorizontalOffsetWithAnimationAsync()
        /// <summary>
        /// Scrolls to horizontal offset with animation asynchronously.
        /// </summary>
        /// <param name="scrollViewer">The scroll viewer.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>The task that completes when scrolling is complete.</returns>
        public static async Task ScrollToHorizontalOffsetWithAnimationAsync(
            this ScrollViewer scrollViewer,
            double offset)
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
        #endregion
    }
}
