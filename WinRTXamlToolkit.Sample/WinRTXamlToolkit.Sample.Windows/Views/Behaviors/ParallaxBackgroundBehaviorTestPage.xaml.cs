using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ParallaxBackgroundBehaviorTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public ParallaxBackgroundBehaviorTestPage()
        {
            this.InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
