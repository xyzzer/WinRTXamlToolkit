using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class CountdownTestView : UserControl
    {
        private bool _isLoaded;

        public CountdownTestView()
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
    }
}
