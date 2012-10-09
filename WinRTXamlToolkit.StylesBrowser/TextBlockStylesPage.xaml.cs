using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.StylesBrowser
{
    public sealed partial class TextBlockStylesPage : Page
    {
        public TextBlockStylesPage()
        {
            this.InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
