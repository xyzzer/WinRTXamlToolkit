using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Imaging;

namespace WinRTXamlToolkit.Controls
{
    public class FxContentControl : ContentControl
    {
        private Image _backgroundFxImage;
        private Image _foregroundFxImage;
        private ContentPresenter _contentPresenter;

        public FxContentControl()
        {
            this.DefaultStyleKey = typeof(FxContentControl);
        }

        protected override async void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _backgroundFxImage = this.GetTemplateChild("BackgroundFxImage") as Image;
            _foregroundFxImage = this.GetTemplateChild("ForegroundFxImage") as Image;
            _contentPresenter = this.GetTemplateChild("ContentPresenter") as ContentPresenter;

            if (_contentPresenter != null)
            {
                _contentPresenter.SizeChanged += this.OnContentPresenterSizeChanged;
            }

            if (_contentPresenter.ActualHeight > 0)
            {
                await this.UpdateFx();
            }
        }

        private async void OnContentPresenterSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            await this.UpdateFx();
        }

        private async Task UpdateFx()
        {
            await this.UpdateBackgroundFx();
        }

        private async Task UpdateBackgroundFx()
        {
            ////await Task.Delay(1000);
            if (_contentPresenter.ActualHeight < 1 ||
                _backgroundFxImage == null)
            {
                return;
            }

            var rtb = new RenderTargetBitmap();
            await rtb.RenderAsync(_contentPresenter);

            var pw = rtb.PixelWidth;
            var ph = rtb.PixelHeight;

            var wb = _backgroundFxImage.Source as WriteableBitmap;

            if (wb == null ||
                wb.PixelWidth != pw ||
                wb.PixelHeight != ph)
            {
                wb = new WriteableBitmap(pw, ph);
            }

            await ProcessContentImage(rtb, wb, pw, ph);

            _backgroundFxImage.Source = wb;
        }

        protected virtual async Task ProcessContentImage(RenderTargetBitmap rtb, WriteableBitmap wb, int pw, int ph)
        {
            var rtbBuffer = await rtb.GetPixelsAsync();
            var rtbPixels = rtbBuffer.GetPixels();
            var wbBuffer = wb.PixelBuffer;
            var wbPixels = wbBuffer.GetPixels();

            // Expand
            int expansion = 1;

            for (int x = 0; x < pw; x++)
                for (int y = 0; y < ph; y++)
                {
                    int x1min = Math.Max(0, x - expansion);
                    int x1max = Math.Min(x + expansion, pw - 1);
                    int y1min = Math.Max(0, y - expansion);
                    int y1max = Math.Min(y + expansion, ph - 1);
                    //bool found = false;
                    byte maxa = 0;

                    for (int x1 = x1min; x1 <= x1max; x1++)
                        for (int y1 = y1min; y1 <= y1max; y1++)
                        {
                            var a = rtbPixels.Bytes[4 * (y1 * pw + x1) + 3];
                            if (a > maxa)
                                maxa = a;
                        }
                    wbPixels.Bytes[4 * (y * pw + x)] = 0;
                    wbPixels.Bytes[4 * (y * pw + x) + 1] = 0;
                    wbPixels.Bytes[4 * (y * pw + x) + 2] = 0;
                    wbPixels.Bytes[4 * (y * pw + x) + 3] = maxa;
                }

            wbPixels.UpdateFromBytes();
        }
    }
}
