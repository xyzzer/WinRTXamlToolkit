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

        protected override async Task OnNavigatedFromAsync(AlternativeNavigationEventArgs e)
        {
            await base.OnNavigatedFromAsync(e);
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
