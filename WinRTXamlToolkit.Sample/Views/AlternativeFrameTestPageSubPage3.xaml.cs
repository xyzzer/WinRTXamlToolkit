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
        protected override async Task Preload(object parameter)
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
        protected override async Task UnloadPreloaded()
        {
            _isPreloaded = false;
        }
#pragma warning restore 1998

        protected override async Task OnNavigatingTo(AlternativeNavigationEventArgs e)
        {
            _isPreloaded = false;
            _parameter = (int)e.Parameter;
            ParameterTextBlock.Text = "Parameter: " + _parameter;
            await base.OnNavigatingTo(e);
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
    }
}
