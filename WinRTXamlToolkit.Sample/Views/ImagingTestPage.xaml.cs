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
            hueRingImage2.Source = hueRingBitmap;

            var saturationLightnessBitmap = new WriteableBitmap(512, 128);
            await Dispatcher.RunAsync(
                CoreDispatcherPriority.High,
                () => saturationLightnessBitmap.RenderColorPickerSaturationLightnessRect());
            saturationLightnessImage.Source = saturationLightnessBitmap;

            var hueValueBitmap = new WriteableBitmap(512, 128);
            await Dispatcher.RunAsync(
                CoreDispatcherPriority.High,
                () => hueValueBitmap.RenderColorPickerHueValue(1.0));
            hueValueImage.Source = hueValueBitmap;
            var hueValueBitmap2 = new WriteableBitmap(512, 128);
            await Dispatcher.RunAsync(
                CoreDispatcherPriority.High,
                () => hueValueBitmap2.RenderColorPickerHueValue(0.5));
            hueValueImage2.Source = hueValueBitmap2;

            var saturationValueBitmap = new WriteableBitmap(512, 128);
            await Dispatcher.RunAsync(
                CoreDispatcherPriority.High,
                () => saturationValueBitmap.RenderColorPickerSaturationValueRect());
            saturationValueImage.Source = saturationValueBitmap;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void OnHueRingImageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (hueRingImage.ActualHeight == 0 ||
                hueRingImage.ActualWidth == 0)
            {
                return;
            }

            var minSize = Math.Min(hueRingImage.ActualHeight, hueRingImage.ActualWidth);
            var outerRingRadius = minSize / 2;
            var innerRingRadius = outerRingRadius * 2 / 3;
            var triangleWidth = innerRingRadius * Math.Sqrt(3);
            var triangleHeight = innerRingRadius * 3 / 2;
            var wb = new WriteableBitmap((int)triangleWidth, (int)triangleHeight);
            wb.RenderColorPickerSaturationLightnessTriangle();
            saturationLightnessTriangleImage.Source = wb;
            saturationLightnessTriangleImage.Margin = new Thickness(0, outerRingRadius - innerRingRadius, 0, outerRingRadius - innerRingRadius * 0.5);
        }

        private void OnHueRingImage2SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (hueRingImage2.ActualHeight == 0 ||
                hueRingImage2.ActualWidth == 0)
            {
                return;
            }

            var minSize = Math.Min(hueRingImage2.ActualHeight, hueRingImage2.ActualWidth);
            var outerRingRadius = minSize / 2;
            var innerRingRadius = outerRingRadius * 2 / 3;
            var triangleWidth = innerRingRadius * Math.Sqrt(3);
            var triangleHeight = innerRingRadius * 3 / 2;
            var wb = new WriteableBitmap((int)triangleWidth, (int)triangleHeight);
            wb.RenderColorPickerSaturationValueTriangleAsync();
            saturationValueTriangleImage.Source = wb;
            saturationValueTriangleImage.Margin = new Thickness(0, outerRingRadius - innerRingRadius, 0, outerRingRadius - innerRingRadius * 0.5);
        }
    }
}
