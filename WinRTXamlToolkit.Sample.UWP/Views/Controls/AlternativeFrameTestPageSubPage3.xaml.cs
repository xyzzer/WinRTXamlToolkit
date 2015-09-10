using System.Threading.Tasks;
using WinRTXamlToolkit.Controls;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class AlternativeFrameTestPageSubPage3 : AlternativePage
    {
        public AlternativeFrameTestPageSubPage3()
        {
            this.InitializeComponent();
            this.ShouldWaitForImagesToLoad = false;
        }

        private int _parameter;

        private int _secondsOfLife;
        private bool _isPreloaded;

#pragma warning disable 1998
        protected override async Task PreloadAsync(object parameter)
        {
            _secondsOfLife = 0;
            _isPreloaded = true;
            DoWhilePreloaded();
        }
#pragma warning restore 1998

        private async void DoWhilePreloaded()
        {
            while (_isPreloaded)
            {
                await Task.Delay(1000);
                _secondsOfLife++;
                TimeTextBlock.Text = "Preload started " + _secondsOfLife + " seconds ago.";
            }
        }

#pragma warning disable 1998
        protected override async Task UnloadPreloadedAsync()
        {
            _isPreloaded = false;
        }
#pragma warning restore 1998

        protected override async Task OnNavigatingToAsync(AlternativeNavigationEventArgs e)
        {
            _isPreloaded = false;
            _parameter = (int)e.Parameter;
            ParameterTextBlock.Text = "Parameter: " + _parameter;
            await base.OnNavigatingToAsync(e);
        }

        private async void OnNavigateButtonClick(object sender, RoutedEventArgs e)
        {
            await this.Frame.NavigateAsync(
                typeof(AlternativeFrameTestPageSubPage2), _parameter + 1);
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
