using System;
using System.Collections;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Extensions for ItemsControl and its subclasses (ListBox, ListView, GridView, etc.).
    /// </summary>
    public static class ItemsControlExtensions
    {
        /// <summary>
        /// Gets the ScrollViewer used for scrolling the items of the ItemsControl.
        /// </summary>
        /// <param name="itemsControl">The ItemsControl.</param>
        /// <returns>The first ScrollViewer descendant of the ItemsControl.</returns>
        public static ScrollViewer GetScrollViewer(this ItemsControl itemsControl)
        {
            return itemsControl.GetFirstDescendantOfType<ScrollViewer>();
        }

        /// <summary>
        /// Gets the first visible element.
        /// </summary>
        /// <param name="itemsControl">The ItemsControl.</param>
        /// <returns>The first visible item or null if not found.</returns>
        public static object GetFirstVisibleItem(this ItemsControl itemsControl)
        {
            var index = GetFirstVisibleIndex(itemsControl);

            if (index == -1)
            {
                return null;
            }

            var list = itemsControl.ItemsSource as IList;

            if (itemsControl.ItemsSource != null &&
                list != null &&
                list.Count > index)
            {
                return list[index];
            }

            if (itemsControl.Items != null &&
                itemsControl.Items.Count > index)
            {
                return itemsControl.Items[index];
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets the index of the first visible element.
        /// </summary>
        /// <param name="itemsControl">The ItemsControl.</param>
        /// <returns>The index of the first visible item or -1 if not found.</returns>
        public static int GetFirstVisibleIndex(this ItemsControl itemsControl)
        {
            // First checking if no items source or an empty one is used
            if (itemsControl.ItemsSource == null)
            {
                return -1;
            }

            var enumItemsSource = itemsControl.ItemsSource as IEnumerable;

            if (enumItemsSource != null && !enumItemsSource.GetEnumerator().MoveNext())
            {
                return -1;
            }

            // Check if a modern panel is used as an items panel
            var sourcePanel = itemsControl.ItemsPanelRoot;

            if (sourcePanel == null)
            {
                throw new InvalidOperationException("Can't get first visible index from an ItemsControl with no ItemsPanel.");
            }

            var isp = sourcePanel as ItemsStackPanel;

            if (isp != null)
            {
                return isp.FirstVisibleIndex;
            }

            var iwg = sourcePanel as ItemsWrapGrid;

            if (iwg != null)
            {
                return iwg.FirstVisibleIndex;
            }

            // Check containers for first one in view
            if (sourcePanel.Children.Count == 0)
            {
                return -1;
            }

            if (itemsControl.ActualWidth == 0)
            {
                throw new InvalidOperationException("Can't get first visible index from an ItemsControl that is not loaded or has zero size.");
            }

            for (int i = 0; i < sourcePanel.Children.Count; i++)
            {
                var container = (FrameworkElement)sourcePanel.Children[i];
                var bounds = container.TransformToVisual(itemsControl).TransformBounds(new Rect(0, 0, container.ActualWidth, container.ActualHeight));

                if (bounds.Left < itemsControl.ActualWidth &&
                    bounds.Top < itemsControl.ActualHeight &&
                    bounds.Right > 0 &&
                    bounds.Bottom > 0)
                {
                    return itemsControl.IndexFromContainer(container);
                }
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Synchronizes the scroll offset of the target ItemsControl to the offset of the source ItemsControl
        /// such that the first visible item in both controls is the same item.
        /// </summary>
        /// <param name="targetItemsControl">The ItemsControl whose scroll offset is to be updated.</param>
        /// <param name="sourceItemsControl">The ItemsControl whose scroll offset will be matched in the target.</param>
        /// <param name="throwOnFail">Specifies whether an exception should be thrown on failure.</param>
        public static void SynchronizeScrollOffset(this ItemsControl targetItemsControl, ItemsControl sourceItemsControl, bool throwOnFail = false)
        {
            var firstVisibleIndex = sourceItemsControl.GetFirstVisibleIndex();

            if (firstVisibleIndex == -1)
            {
                if (throwOnFail)
                {
                    throw new InvalidOperationException();
                }

                return;
            }

            var targetListBox = targetItemsControl as ListBox;

            if (targetListBox != null)
            {
                targetListBox.ScrollIntoView(sourceItemsControl.IndexFromContainer(sourceItemsControl.ContainerFromIndex(firstVisibleIndex)));
                return;
            }

            var targetListViewBase = targetItemsControl as ListViewBase;

            if (targetListViewBase != null)
            {
                targetListViewBase.ScrollIntoView(sourceItemsControl.IndexFromContainer(sourceItemsControl.ContainerFromIndex(firstVisibleIndex)), ScrollIntoViewAlignment.Leading);
                return;
            }

            var scrollViewer = targetItemsControl.GetScrollViewer();

            if (scrollViewer != null)
            {
                var container = (FrameworkElement)targetItemsControl.ContainerFromIndex(firstVisibleIndex);
                var position = container.TransformToVisual(scrollViewer).TransformPoint(new Point());
                scrollViewer.ChangeView(scrollViewer.HorizontalOffset + position.X, scrollViewer.VerticalOffset + position.Y, null);
            }
        }
    }
}
