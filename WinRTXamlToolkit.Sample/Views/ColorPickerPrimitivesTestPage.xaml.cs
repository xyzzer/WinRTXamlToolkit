using System.Threading.Tasks;
using WinRTXamlToolkit.Async;
using WinRTXamlToolkit.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ColorPickerPrimitivesTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        private bool _isLoaded;
        private AsyncAutoResetEvent _triangleUpdateRequired = new AsyncAutoResetEvent();

        public ColorPickerPrimitivesTestPage()
        {
            this.InitializeComponent();

            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;
            _triangleUpdateRequired.Set();
            RunTriangleUpdaterAsync();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = false;
            _triangleUpdateRequired.Set();
        }

        private async void RunTriangleUpdaterAsync()
        {
            do
            {
                await _triangleUpdateRequired.WaitAsync();

                if (_isLoaded)
                {
                    var wb = new WriteableBitmap(
                        (int)trianglePicker.ActualWidth,
                        (int)trianglePicker.ActualHeight);

                    var wbHue = new WriteableBitmap(
                        (int)slidersPanel.ActualWidth - 80,
                        1);

                    var wbSaturation = new WriteableBitmap(
                        (int)slidersPanel.ActualWidth - 80,
                        1);

                    var wbLightness = new WriteableBitmap(
                        (int)slidersPanel.ActualWidth - 80,
                        1);

                    var wbRed = new WriteableBitmap(
                        (int)slidersPanel.ActualWidth - 80,
                        1);

                    var wbGreen = new WriteableBitmap(
                        (int)slidersPanel.ActualWidth - 80,
                        1);

                    var wbBlue = new WriteableBitmap(
                        (int)slidersPanel.ActualWidth - 80,
                        1);

                    var color = ColorExtensions.FromHsl(hueRing.Value, 1, 0.5);

                    await Task.WhenAll(
                        wb.RenderColorPickerSaturationValueTriangleAsync(hueRing.Value),
                        wbHue.RenderColorPickerHSLHueBarAsync(1.0, 0.5),
                        wbSaturation.RenderColorPickerHSLSaturationBarAsync(hueRing.Value, 0.5),
                        wbLightness.RenderColorPickerHSLLightnessBarAsync(hueRing.Value, 0.5),
                        wbRed.RenderColorPickerRGBRedBarAsync(color.G / 255.0, color.B / 255.0),
                        wbGreen.RenderColorPickerRGBGreenBarAsync(color.R / 255.0, color.B / 255.0),
                        wbBlue.RenderColorPickerRGBBlueBarAsync(color.R / 255.0, color.G / 255.0));

                    if (_isLoaded)
                    {
                        triangleBrush.ImageSource = wb;
                        hueBackground.ImageSource = wbHue;
                        saturationBackground.ImageSource = wbSaturation;
                        lightnessBackground.ImageSource = wbLightness;
                        redBackground.ImageSource = wbRed;
                        greenBackground.ImageSource = wbGreen;
                        blueBackground.ImageSource = wbBlue;
                    }
                }
            } while (_isLoaded);
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void trianglePicker_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            _triangleUpdateRequired.Set();
        }

        private void hueRing_ValueChanged_1(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            _triangleUpdateRequired.Set();
        }
    }
}
