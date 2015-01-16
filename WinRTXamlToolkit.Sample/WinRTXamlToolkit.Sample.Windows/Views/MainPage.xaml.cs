using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Search;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Debugging;
using WinRTXamlToolkit.Sample.ViewModels;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class MainPage : AlternativePage
    {
        public MainPage()
        {
            this.InitializeComponent();
            //DC.ShowVisualTree(this);
        }

#pragma warning disable 1998
        protected override async Task OnNavigatedTo(AlternativeNavigationEventArgs e)
        {
            GC.Collect();
#pragma warning disable 4014
            base.OnNavigatedTo(e);
#pragma warning restore 4014

            //SearchPane.GetForCurrentView().ShowOnKeyboardInput = true;
        }

        protected override async Task OnNavigatedFrom(AlternativeNavigationEventArgs e)
        {
            await base.OnNavigatedFrom(e);

            if (DisableSearchPaneOnFocusHandler.IsSearchEnabled)
            {
                SearchPane.GetForCurrentView().ShowOnKeyboardInput = false;
            }
        }
#pragma warning restore 1998

        private void OnExitButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void ButtonsGridView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var vm = e.ClickedItem as ButtonViewModel;
            vm.Command.Execute(null);
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
