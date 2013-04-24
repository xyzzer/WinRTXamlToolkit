using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Debugging.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Debugging.Views
{
    public sealed partial class VisualTreeView : UserControl
    {
        public VisualTreeView()
        {
            this.InitializeComponent();
            this.DataContext = new VisualTreeViewModel();
        }

        private void OnRefreshButtonClick(object sender, RoutedEventArgs e)
        {
            var vm = (VisualTreeViewModel)this.DataContext;
#pragma warning disable 4014
            vm.Refresh();
#pragma warning restore 4014
        }

        private async void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = e.NewValue;

            if (selectedItem == null)
            {
                return;
            }

            try
            {
                bool needsScrolling = false;

                do
                {
                    needsScrolling = false;
                    await Task.Delay(100);

                    if (treeView.SelectedItem != selectedItem)
                        return;

                    var container = treeView.SelectedContainer as TreeViewItem;

                    if (container == null)
                    {
                        Debugger.Break();
                        return;
                    }

                    var scrollViewer = treeView.GetFirstDescendantOfType<ScrollViewer>();
                    var scrollContentPresenter =
                        scrollViewer.GetFirstDescendantOfType<ScrollContentPresenter>();
                    var containerBounds =
                        container.GetBoundingRect(scrollContentPresenter);

                    if (containerBounds.Top >
                        scrollViewer.ViewportHeight - 20)
                    {
                        var delta = scrollViewer.ViewportHeight - containerBounds.Top -
                                    20;
                        scrollViewer.ScrollToVerticalOffset(
                            scrollViewer.VerticalOffset + delta);
                        needsScrolling = true;
                    }
                    else if (containerBounds.Top < 0)
                    {
                        var delta = containerBounds.Top;
                        scrollViewer.ScrollToVerticalOffset(
                            scrollViewer.VerticalOffset + delta);
                        needsScrolling = true;
                    }

                    if (containerBounds.Left >
                        scrollViewer.ViewportWidth - 50)
                    {
                        var delta = scrollViewer.ViewportWidth - containerBounds.Left -
                                    50;
                        scrollViewer.ScrollToHorizontalOffset(
                            scrollViewer.HorizontalOffset + delta);
                        needsScrolling = true;
                    }
                    else if (containerBounds.Left < 0)
                    {
                        var delta = containerBounds.Left;
                        scrollViewer.ScrollToHorizontalOffset(
                            scrollViewer.HorizontalOffset + delta);
                        needsScrolling = true;
                    }
                } while (needsScrolling);
            }
#pragma warning disable 168
            catch (Exception ex)
#pragma warning restore 168
            {
                Debugger.Break();
            }
        }
    }
}
