using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Sample.Views;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample
{
    public sealed partial class AppShell : UserControl
    {
        public static AlternativeFrame Frame { get; private set; }

        public AppShell()
        {
            this.InitializeComponent();
            Frame = this.RootFrame;
            this.RootFrame.Navigate(typeof (MainPage));
        }
    }
}
