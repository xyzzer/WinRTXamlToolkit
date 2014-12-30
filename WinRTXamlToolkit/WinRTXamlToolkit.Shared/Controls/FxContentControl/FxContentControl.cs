using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Controls.Fx;
using WinRTXamlToolkit.Imaging;
using WinRTXamlToolkit.Tools;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// Control that applies shader effects to its content. Uses CPU for shading.
    /// </summary>
    public class FxContentControl : ContentControl
    {
        private readonly EventThrottler updateThrottler = new EventThrottler();
        private Image _backgroundFxImage;
        private Image _foregroundFxImage;
        private ContentPresenter _contentPresenter;
        private Grid _renderedGrid;

        #region BackgroundFx
        /// <summary>
        /// BackgroundFx Dependency Property
        /// </summary>
        private static readonly DependencyProperty _BackgroundFxProperty =
            DependencyProperty.Register(
                "BackgroundFx",
                typeof(CpuShaderEffect),
                typeof(FxContentControl),
                new PropertyMetadata(null, OnBackgroundFxChanged));

        /// <summary>
        /// Identifies the BackgroundFx dependency property.
        /// </summary>
        public static DependencyProperty BackgroundFxProperty { get { return _BackgroundFxProperty; } }

        /// <summary>
        /// Gets or sets the shader effect for bitmap shown behind the content.
        /// </summary>
        public CpuShaderEffect BackgroundFx
        {
            get { return (CpuShaderEffect)this.GetValue(BackgroundFxProperty); }
            set { this.SetValue(BackgroundFxProperty, value); }
        }

        /// <summary>
        /// Handles changes to the BackgroundFx property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnBackgroundFxChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (FxContentControl)d;
            CpuShaderEffect oldBackgroundFx = (CpuShaderEffect)e.OldValue;
            CpuShaderEffect newBackgroundFx = target.BackgroundFx;
            target.OnBackgroundFxChanged(oldBackgroundFx, newBackgroundFx);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the BackgroundFx property.
        /// </summary>
        /// <param name="oldBackgroundFx">The old BackgroundFx value</param>
        /// <param name="newBackgroundFx">The new BackgroundFx value</param>
        private async void OnBackgroundFxChanged(
            CpuShaderEffect oldBackgroundFx, CpuShaderEffect newBackgroundFx)
        {
            if (_renderedGrid != null &&
                _renderedGrid.ActualHeight > 0)
            {
                await this.UpdateFxAsync();
            }
        }
        #endregion

        #region ForegroundFx
        /// <summary>
        /// ForegroundFx Dependency Property
        /// </summary>
        private static readonly DependencyProperty _ForegroundFxProperty =
            DependencyProperty.Register(
                "ForegroundFx",
                typeof(CpuShaderEffect),
                typeof(FxContentControl),
                new PropertyMetadata(null, OnForegroundFxChanged));

        /// <summary>
        /// Identifies the ForegroundFx dependency property.
        /// </summary>
        public static DependencyProperty ForegroundFxProperty { get { return _ForegroundFxProperty; } }

        /// <summary>
        /// Gets or sets the shader effect for bitmap shown in front of the content.
        /// </summary>
        public CpuShaderEffect ForegroundFx
        {
            get { return (CpuShaderEffect)this.GetValue(ForegroundFxProperty); }
            set { this.SetValue(ForegroundFxProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ForegroundFx property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnForegroundFxChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (FxContentControl)d;
            CpuShaderEffect oldForegroundFx = (CpuShaderEffect)e.OldValue;
            CpuShaderEffect newForegroundFx = target.ForegroundFx;
            target.OnForegroundFxChanged(oldForegroundFx, newForegroundFx);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the ForegroundFx property.
        /// </summary>
        /// <param name="oldForegroundFx">The old ForegroundFx value</param>
        /// <param name="newForegroundFx">The new ForegroundFx value</param>
        private async void OnForegroundFxChanged(
            CpuShaderEffect oldForegroundFx, CpuShaderEffect newForegroundFx)
        {
            if (_renderedGrid != null &&
                _renderedGrid.ActualHeight > 0)
            {
                await this.UpdateFxAsync();
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="FxContentControl"/> class,
        /// </summary>
        public FxContentControl()
        {
            this.DefaultStyleKey = typeof(FxContentControl);
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
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
                await this.UpdateFxAsync();
            }
        }

        private async void OnContentPresenterSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            await this.UpdateFxAsync();
        }

        /// <summary>
        /// Updates the effects asynchronously.
        /// </summary>
        /// <returns>A task that completes once the effects have been applied.</returns>
        public async Task UpdateFxAsync()
        {
            if (_renderedGrid.ActualHeight < 2 ||
                _renderedGrid.ActualWidth < 2 ||
                _backgroundFxImage == null ||
                _foregroundFxImage == null)
            {
                if (_backgroundFxImage != null)
                {
                    _backgroundFxImage.Source = null;
                }

                if (_foregroundFxImage != null)
                {
                    _foregroundFxImage.Source = null;
                }

                return;
            }

            var rtb = new RenderTargetBitmap();
            await rtb.RenderAsync(_renderedGrid);

            if (rtb.PixelHeight == 0)
            {
                {
                    _backgroundFxImage.Source = null;
                }

                if (_foregroundFxImage != null)
                {
                    _foregroundFxImage.Source = null;
                }

                return;
            }

            await this.UpdateBackgroundFx(rtb);
            await this.UpdateForegroundFx(rtb);
        }

        private async Task UpdateBackgroundFx(RenderTargetBitmap rtb)
        {
            if (_renderedGrid.ActualHeight < 1 ||
                _backgroundFxImage == null)
            {
                return;
            }

            if (this.BackgroundFx == null)
            {
                _backgroundFxImage.Source = null;
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

            await OnProcessBackgroundImage(rtb, wb, pw, ph);

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

            if (this.ForegroundFx == null)
            {
                _foregroundFxImage.Source = null;
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

        private Task OnProcessBackgroundImage(RenderTargetBitmap rtb, WriteableBitmap wb, int pw, int ph)
        {
            return this.BackgroundFx.ProcessBitmap(rtb, wb, pw, ph);
        }

        private Task ProcessForegroundImage(RenderTargetBitmap rtb, WriteableBitmap wb, int pw, int ph)
        {
            return this.ForegroundFx.ProcessBitmap(rtb, wb, pw, ph);
        }
    }
}
