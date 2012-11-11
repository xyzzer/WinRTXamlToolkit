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

        protected override async Task OnNavigatingTo(AlternativeNavigationEventArgs e)
        {
            _parameter = (int)e.Parameter;
            ParameterTextBlock.Text = "Parameter: " + _parameter;
            await base.OnNavigatingTo(e);
        }

        protected override async Task OnNavigatedTo(AlternativeNavigationEventArgs e)
        {
            await this.Frame.Preload(typeof(AlternativeFrameTestPageSubPage3), _parameter + 1);
        }

        private void OnNavigateButtonClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(
                typeof (AlternativeFrameTestPageSubPage2), _parameter + 1);
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void OnForwardButtonClick(object sender, RoutedEventArgs e)
        {
            this.Frame.GoForward();
        }

        private void OnNavigatePreloadedButtonClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AlternativeFrameTestPageSubPage3), _parameter + 1);
        }
    }
}
