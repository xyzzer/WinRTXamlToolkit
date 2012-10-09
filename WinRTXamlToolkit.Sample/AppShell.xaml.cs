using WinRTXamlToolkit.Sample.Views;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace WinRTXamlToolkit.Sample
{
    public sealed partial class AppShell : UserControl
    {
        public AppShell()
        {
            this.InitializeComponent();
            this.RootFrame.Navigate(typeof (MainPage));
        }
    }
}
