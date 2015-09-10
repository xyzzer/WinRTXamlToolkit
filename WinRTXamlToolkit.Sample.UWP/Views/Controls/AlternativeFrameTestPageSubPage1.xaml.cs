using System.Threading.Tasks;
using WinRTXamlToolkit.Controls;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class AlternativeFrameTestPageSubPage1 : AlternativePage
    {
        public AlternativeFrameTestPageSubPage1()
        {
            this.InitializeComponent();
        }

        private int _parameter;

        protected override async Task OnNavigatingToAsync(AlternativeNavigationEventArgs e)
        {
            _parameter = (int)e.Parameter;
            ParameterTextBlock.Text = "Parameter: " + _parameter;
            await base.OnNavigatingToAsync(e);
        }

        protected override async Task OnNavigatedToAsync(AlternativeNavigationEventArgs e)
        {
            await this.Frame.PreloadAsync(typeof(AlternativeFrameTestPageSubPage3), _parameter + 1);
        }

        private async void OnNavigateButtonClick(object sender, RoutedEventArgs e)
        {
            await this.Frame.NavigateAsync(
                typeof (AlternativeFrameTestPageSubPage2), _parameter + 1);
        }

        private async void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            await this.Frame.GoBackAsync();
        }

        private async void OnForwardButtonClick(object sender, RoutedEventArgs e)
        {
            await this.Frame.GoForwardAsync();
        }

        private async void OnNavigatePreloadedButtonClick(object sender, RoutedEventArgs e)
        {
            await this.Frame.NavigateAsync(typeof(AlternativeFrameTestPageSubPage3), _parameter + 1);
        }
    }
}
