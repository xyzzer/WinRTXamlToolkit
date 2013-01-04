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

        protected override async Task OnNavigatingTo(AlternativeNavigationEventArgs e)
        {
            _parameter = (int)e.Parameter;
            ParameterTextBlock.Text = "Parameter: " + _parameter;
            await base.OnNavigatingTo(e);
        }

        private void OnNavigateButtonClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(
                typeof(AlternativeFrameTestPageSubPage1), _parameter + 1);
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void OnForwardButtonClick(object sender, RoutedEventArgs e)
        {
            this.Frame.GoForward();
        }
    }
}
