using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class CountdownTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public CountdownTestPage()
        {
            this.InitializeComponent();
            this.Loaded += CountdownTestPage_Loaded;
        }

        private async void CountdownTestPage_Loaded(object sender, RoutedEventArgs e)
        {
            while (true)
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
