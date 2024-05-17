using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace WinRTXamlToolkit.StylesBrowser
{
    public sealed partial class AppBarButtonStylesPage : Page
    {
        public AppBarButtonStylesPage()
        {
            this.InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
