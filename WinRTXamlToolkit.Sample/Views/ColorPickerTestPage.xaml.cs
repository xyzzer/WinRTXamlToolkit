using WinRTXamlToolkit.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ColorPickerTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public ColorPickerTestPage()
        {
            this.InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void trianglePicker_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            var wb = new WriteableBitmap((int)trianglePicker.ActualWidth, (int)trianglePicker.ActualHeight);
            wb.RenderColorPickerSaturationValueTriangle(hueRing.Value);
            triangleBrush.ImageSource = wb;
        }

        private void hueRing_ValueChanged_1(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            var wb = new WriteableBitmap((int)trianglePicker.ActualWidth, (int)trianglePicker.ActualHeight);
            wb.RenderColorPickerSaturationValueTriangle(hueRing.Value);
            triangleBrush.ImageSource = wb;
        }
    }
}
