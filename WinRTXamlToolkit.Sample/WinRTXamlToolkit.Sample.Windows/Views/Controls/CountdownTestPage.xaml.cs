using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class CountdownTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        private bool _isLoaded;

        public CountdownTestPage()
        {
            this.InitializeComponent();
            this.Loaded += CountdownTestPage_Loaded;
            this.Unloaded += (s, e) => _isLoaded = false;
        }

        private async void CountdownTestPage_Loaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;

            while (_isLoaded)
            {
                myCountdownControl.Visibility = Visibility.Visible;
                await myCountdownControl.StartCountdownAsync(3);
                myCountdownControl.Visibility = Visibility.Collapsed;
                await Task.Delay(1000);
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
