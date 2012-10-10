using System;
using WinRTXamlToolkit.Imaging;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ImagingTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public ImagingTestPage()
        {
            this.InitializeComponent();
            this.Loaded += ImagingTestPage_Loaded;
        }

        private async void ImagingTestPage_Loaded(object sender, RoutedEventArgs e)
        {
            var hueLightnessBitmap = new WriteableBitmap(1024, 256);
            await Dispatcher.RunAsync(
                CoreDispatcherPriority.High,
                () => hueLightnessBitmap.RenderColorPickerHueLightness(1.0));
            hueLightnessImage.Source = hueLightnessBitmap;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
