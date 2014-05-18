using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ListViewExtensionsItemToBringIntoViewTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public ListViewExtensionsItemToBringIntoViewTestPage()
        {
            this.InitializeComponent();
            this.DataContext = new ListViewExtensionsTestViewModel();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
