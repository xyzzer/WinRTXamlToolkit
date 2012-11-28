using WinRTXamlToolkit.AwaitableUI;
using WinRTXamlToolkit.Composition;
using WinRTXamlToolkit.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class WriteableBitmapRenderTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public WriteableBitmapRenderTestPage()
        {
            this.InitializeComponent();
            RunTest();
        }

        private async void RunTest()
        {
            await this.WaitForLoadedAsync();
            var wb = new WriteableBitmap(1, 1);
            wb.Render(this.source);
            this.target.Source = wb;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void OverlaidPreviewButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var wb = (WriteableBitmap)this.target.Source;
            wb = wb.Copy();
            wb.Lighten(0.5);
            wb.Grayscale();
            overlaidPreview.Source = wb;
        }

        private void OverlaidPreviewButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            overlaidPreview.Source = null;
        }
    }
}
