using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class InputDialogTestPage : Page
    {
        public InputDialogTestPage()
        {
            this.InitializeComponent();
        }

        private void GridHostedTest(object sender, RoutedEventArgs e)
        {
            GridHostedDialog.ShowAsync(
                "Grid-hosted InputDialog",
                "This dialog is defined as a child of a Grid",
                "OK");
        }
    }
}
