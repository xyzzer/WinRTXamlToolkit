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
            var hueLightnessBitmap = new WriteableBitmap(512, 128);
            await Dispatcher.RunAsync(
                CoreDispatcherPriority.High,
                () => hueLightnessBitmap.RenderColorPickerHueLightness(1.0));
            hueLightnessImage.Source = hueLightnessBitmap;
            var hueLightnessBitmap2 = new WriteableBitmap(512, 128);
            await Dispatcher.RunAsync(
                CoreDispatcherPriority.High,
                () => hueLightnessBitmap2.RenderColorPickerHueLightness(0.5));
            hueLightnessImage2.Source = hueLightnessBitmap2;
            var hueRingBitmap = new WriteableBitmap(512,512);
            await Dispatcher.RunAsync(
                CoreDispatcherPriority.High,
                () => hueRingBitmap.RenderColorPickerHueRing());
            hueRingImage.Source = hueRingBitmap;

            var saturationLightnessBitmap = new WriteableBitmap(512, 128);
            await Dispatcher.RunAsync(
                CoreDispatcherPriority.High,
                () => saturationLightnessBitmap.RenderColorPickerSaturationLightness());
            saturationLightnessImage.Source = saturationLightnessBitmap;
            
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
