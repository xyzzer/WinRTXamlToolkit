using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class FrameworkElementExtensionsCursorTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public FrameworkElementExtensionsCursorTestPage()
        {
            this.InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
