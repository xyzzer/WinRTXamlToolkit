using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Imaging;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// TODO: Control that applies shader effects to its content.
    /// </summary>
    public class FxContentControl : ContentControl
    {
        private Image _backgroundFxImage;
        private Image _foregroundFxImage;
        private ContentPresenter _contentPresenter;
        private Grid _renderedGrid;

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
            _renderedGrid = this.GetTemplateChild("RenderedGrid") as Grid;

            if (_renderedGrid != null)
            {
                _renderedGrid.SizeChanged += this.OnContentPresenterSizeChanged;
            }

            if (_renderedGrid.ActualHeight > 0)
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
            if (_renderedGrid.ActualHeight < 1 ||
                _backgroundFxImage == null ||
                _foregroundFxImage == null)
            {
                return;
            }

            var rtb = new RenderTargetBitmap();
            await rtb.RenderAsync(_renderedGrid);

            await this.UpdateBackgroundFx(rtb);
            await this.UpdateForegroundFx(rtb);
        }

        private async Task UpdateBackgroundFx(RenderTargetBitmap rtb)
        {
            ////await Task.Delay(1000);
            if (_renderedGrid.ActualHeight < 1 ||
                _backgroundFxImage == null)
            {
                return;
            }

            var pw = rtb.PixelWidth;
            var ph = rtb.PixelHeight;

            var wb = _backgroundFxImage.Source as WriteableBitmap;

            if (wb == null ||
                wb.PixelWidth != pw ||
                wb.PixelHeight != ph)
            {
                wb = new WriteableBitmap(pw, ph);
            }

            await ProcessBackgroundImage(rtb, wb, pw, ph);

            _backgroundFxImage.Source = wb;
        }

        private async Task UpdateForegroundFx(RenderTargetBitmap rtb)
        {
            ////await Task.Delay(1000);
            if (_renderedGrid.ActualHeight < 1 ||
                _foregroundFxImage == null)
            {
                return;
            }

            var pw = rtb.PixelWidth;
            var ph = rtb.PixelHeight;

            var wb = _foregroundFxImage.Source as WriteableBitmap;

            if (wb == null ||
                wb.PixelWidth != pw ||
                wb.PixelHeight != ph)
            {
                wb = new WriteableBitmap(pw, ph);
            }

            await ProcessForegroundImage(rtb, wb, pw, ph);

            _foregroundFxImage.Source = wb;
        }

        protected virtual async Task ProcessBackgroundImage(RenderTargetBitmap rtb, WriteableBitmap wb, int pw, int ph)
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

        protected virtual async Task ProcessForegroundImage(RenderTargetBitmap rtb, WriteableBitmap wb, int pw, int ph)
        {
        }
    }
}
