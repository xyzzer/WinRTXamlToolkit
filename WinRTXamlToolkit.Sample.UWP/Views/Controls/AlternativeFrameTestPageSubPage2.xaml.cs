using System.Threading.Tasks;
using WinRTXamlToolkit.Controls;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class AlternativeFrameTestPageSubPage2 : AlternativePage
    {
        public AlternativeFrameTestPageSubPage2()
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

        private async void OnNavigateButtonClick(object sender, RoutedEventArgs e)
        {
            await this.Frame.NavigateAsync(
                typeof(AlternativeFrameTestPageSubPage1), _parameter + 1);
        }

        private async void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            await this.Frame.GoBackAsync();
        }

        private async void OnForwardButtonClick(object sender, RoutedEventArgs e)
        {
            await this.Frame.GoForwardAsync();
        }
    }
}
