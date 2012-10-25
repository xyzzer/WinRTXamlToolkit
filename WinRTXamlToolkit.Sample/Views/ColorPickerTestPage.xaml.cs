using System.Threading;
using System.Threading.Tasks;
using WinRTXamlToolkit.Async;
using WinRTXamlToolkit.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ColorPickerTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        private bool _isLoaded;
        private AutoResetEventAsync _triangleUpdateRequired = new AutoResetEventAsync();

        public ColorPickerTestPage()
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

                    await wb.RenderColorPickerSaturationValueTriangleAsync(hueRing.Value);

                    if (_isLoaded)
                    {
                        triangleBrush.ImageSource = wb;
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
