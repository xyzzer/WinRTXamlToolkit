using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ImageExtensionsTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public ImageExtensionsTestPage()
        {
            this.InitializeComponent();
            this.ShouldWaitForImagesToLoad = false;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
