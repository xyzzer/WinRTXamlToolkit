using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.AwaitableUI;
using WinRTXamlToolkit.Controls.Extensions;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ItemsControlExtensionsTestView : UserControl
    {
        private IEnumerable<int> range;

        public ItemsControlExtensionsTestView()
        {
            this.InitializeComponent();
            this.range = Enumerable.Range(0, 100);
            this.Loaded += this.OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ItemsControlSelector.GetFirstDescendantOfType<Border>().Margin = new Thickness();
        }

        private void OnItemsControlLoaded(object sender, RoutedEventArgs e)
        {
            var itemsControl = sender as ItemsControl;

            itemsControl.ItemsSource = range;
            var sv = itemsControl.GetDescendantsOfType<ScrollViewer>().FirstOrDefault();

            if (sv != null)
            {
                sv.ViewChanged += this.OnItemsControlScrollViewerViewChanged;
            }
        }

        private void OnItemsControlScrollViewerViewChanged(object sender, ScrollViewerViewChangedEventArgs scrollViewerViewChangedEventArgs)
        {
            var sv = sender as ScrollViewer;
            var ic = sv.GetFirstAncestorOfType<ItemsControl>();
            this.FirstVisibleItemControl.Content = ic.GetFirstVisibleItem();
        }

        private async void OnFlipViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var fv = sender as FlipView;
            var fe = fv.SelectedItem as FrameworkElement;
            var ic = fe.GetFirstDescendantOfType<ItemsControl>();
            await ic.WaitForNonZeroSizeAsync();

            if (ic != null &&
                this.FirstVisibleItemControl != null)
            {
                this.FirstVisibleItemControl.Content = ic.GetFirstVisibleItem();
            }
        }
    }
}
