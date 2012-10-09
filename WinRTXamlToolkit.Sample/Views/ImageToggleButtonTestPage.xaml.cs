using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ImageToggleButtonTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public ImageToggleButtonTestPage()
        {
            this.InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
