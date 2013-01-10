using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using WinRTXamlToolkit.Controls;

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
            base.OnNavigatedTo(e);
        }
#pragma warning restore 1998

        private void OnExitButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
