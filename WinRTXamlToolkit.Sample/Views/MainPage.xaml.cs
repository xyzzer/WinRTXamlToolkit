using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Search;
using Windows.UI.Xaml;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Controls.Extensions;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
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
    }
}
