using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class InputDialogTestView : UserControl
    {
        public InputDialogTestView()
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
