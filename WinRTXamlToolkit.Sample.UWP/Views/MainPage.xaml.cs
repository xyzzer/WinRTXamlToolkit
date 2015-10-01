using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Sample.ViewModels;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class MainPage : AlternativePage
    {
        public MainPage()
        {
            this.InitializeComponent();
            //DC.ShowVisualTree(this);
            ViewModelLocator.Instance.MainPage.SampleGroups.CollectionChanged += SampleGroups_CollectionChanged;
        }

        private async void SampleGroups_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                await Task.Delay(25);

                if (e.NewItems.Count > 0 &&
                    ViewModelLocator.Instance.MainPage.SampleGroups.Contains(e.NewItems[0]))
                {
                    this.ButtonsGridView.ScrollIntoView(e.NewItems[0]);
                }

                //var vm = e.ClickedItem as ButtonViewModel;
                //vm.Command.Execute(null);

                //// Trying to work around the platform bug that-- prevents GridView from rendering items after inserting some items causes the scrollable content to grow.
                //await Task.Delay(1000);
                //var sv = this.ButtonsGridView.GetScrollViewer();
                ////this.ButtonsGridView.ScrollIntoView(ViewModelLocator.Instance.MainPage.SampleGroups.Last());
                ////await Task.Delay(250);
                ////this.ButtonsGridView.ScrollIntoView(e.ClickedItem);
                ////sv.InvalidateMeasure();
                ////await Task.Delay(250);
                ////var offset = sv.VerticalOffset;
                //sv.ChangeView(null, sv.ScrollableHeight, null);
                ////await Task.Delay(250);
                ////sv.CancelDirectManipulations();
                ////sv.ChangeView(null, offset, null);
            }
        }

        #region OnNavigatedToAsync()
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected override async Task OnNavigatedToAsync(AlternativeNavigationEventArgs e)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            GC.Collect();
#pragma warning disable 4014
            base.OnNavigatedToAsync(e);
#pragma warning restore 4014

            WindowTitleBar.SetText("WinRT XAML Toolkit Samples", true);
        } 
        #endregion

        protected override async Task OnNavigatedFromAsync(AlternativeNavigationEventArgs e)
        {
            await base.OnNavigatedFromAsync(e);
        }

        private void ButtonsGridView_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            var gvi = e.OriginalSource as GridViewItem;
            if (gvi != null &&
                e.Key == VirtualKey.Space)
            {
                var vm = gvi.Content as ButtonViewModel;
                vm.Command.Execute(null);
            }
        }
    }
}
