using System;
using System.Diagnostics;
using System.IO;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SharpDX.WIC;
using WinRTXamlToolkit.Composition.Renderers;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Bitmap = SharpDX.WIC.Bitmap;
using Color = SharpDX.Color;
using D2DPixelFormat = SharpDX.Direct2D1.PixelFormat;
using Ellipse = Windows.UI.Xaml.Shapes.Ellipse;
using WicPixelFormat = SharpDX.WIC.PixelFormat;

namespace WinRTXamlToolkit.Composition
{
    public class CompositionEngine
    {
        private readonly ImagingFactory _wicFactory;
        private readonly SharpDX.Direct2D1.Factory _d2DFactory;
        private readonly SharpDX.DirectWrite.Factory _dWriteFactory;

        public CompositionEngine()
        {
            _wicFactory = new ImagingFactory();
            _d2DFactory = new SharpDX.Direct2D1.Factory();
            this._dWriteFactory = new SharpDX.DirectWrite.Factory();
        }

        public ImagingFactory WicFactory
        {
            get
            {
                return this._wicFactory;
            }
        }

        public SharpDX.Direct2D1.Factory D2DFactory
        {
            get
            {
                return this._d2DFactory;
            }
        }

        public SharpDX.DirectWrite.Factory DWriteFactory
        {
            get
            {
                return this._dWriteFactory;
            }
        }

        public MemoryStream RenderToPngStream(FrameworkElement fe)
        {
            var width = (int)Math.Ceiling(fe.ActualWidth);
            var height = (int)Math.Ceiling(fe.ActualHeight);

            // pixel format with transparency/alpha channel and RGB values premultiplied by alpha
            var pixelFormat = WicPixelFormat.Format32bppPRGBA;

            // pixel format without transparency, but one that works with Cleartype antialiasing
            //var pixelFormat = WicPixelFormat.Format32bppBGR;

            var wicBitmap = new Bitmap(
                this.WicFactory,
                width,
                height,
                pixelFormat,
                BitmapCreateCacheOption.CacheOnLoad);

            var renderTargetProperties = new RenderTargetProperties(
                RenderTargetType.Default,
                new D2DPixelFormat(Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied),
                //new D2DPixelFormat(Format.Unknown, AlphaMode.Unknown), // use this for non-alpha, cleartype antialiased text
                0,
                0,
                RenderTargetUsage.None,
                FeatureLevel.Level_DEFAULT);
            var renderTarget = new WicRenderTarget(
                this.D2DFactory,
                wicBitmap,
                renderTargetProperties)
            {
                //TextAntialiasMode = TextAntialiasMode.Cleartype // this only works with the pixel format with no alpha channel
                TextAntialiasMode = TextAntialiasMode.Grayscale // this is the best we can do for bitmaps with alpha channels
            };

            Compose(renderTarget, fe);
            // TODO: There is no need to encode the bitmap to PNG - we could just copy the texture pixel buffer to a WriteableBitmap pixel buffer.
            var ms = new MemoryStream();

            var stream = new WICStream(
                this.WicFactory,
                ms);

            var encoder = new PngBitmapEncoder(WicFactory);
            encoder.Initialize(stream);

            var frameEncoder = new BitmapFrameEncode(encoder);
            frameEncoder.Initialize();
            frameEncoder.SetSize(width, height);
            var format = WicPixelFormat.Format32bppBGRA;
            //var format = WicPixelFormat.FormatDontCare;
            frameEncoder.SetPixelFormat(ref format);
            frameEncoder.WriteSource(wicBitmap);
            frameEncoder.Commit();

            encoder.Commit();

            frameEncoder.Dispose();
            encoder.Dispose();
            stream.Dispose();

            ms.Position = 0;
            return ms;
        }

        public void Compose(RenderTarget renderTarget, FrameworkElement fe)
        {
            renderTarget.BeginDraw();
            renderTarget.Clear(new Color(0, 0, 0, 0));
            Render(renderTarget, fe, fe);
            renderTarget.EndDraw();
        }

        public void Render(RenderTarget renderTarget, FrameworkElement rootElement, FrameworkElement fe)
        {
            var textBlock = fe as TextBlock;

            if (textBlock != null)
            {
                TextBlockRenderer.Render(this, renderTarget, rootElement, textBlock);
                return;
            }

            var rectangle = fe as Rectangle;

            if (rectangle != null)
            {
                RectangleRenderer.Render(this, renderTarget, rootElement, rectangle);
                return;
            }

            var border = fe as Border;

            if (border != null)
            {
                BorderRenderer.Render(this, renderTarget, rootElement, border);
                return;
            }

            var ellipse = fe as Ellipse;

            if (ellipse != null)
            {
                EllipseRenderer.Render(this, renderTarget, rootElement, ellipse);
                return;
            }

            FrameworkElementRenderer.Render(this, renderTarget, rootElement, fe);
        }

        internal void RenderChildren(RenderTarget renderTarget, FrameworkElement rootElement, FrameworkElement fe)
        {
            var children = fe.GetChildrenByZIndex();

            foreach (var dependencyObject in children)
            {
                var child = dependencyObject as FrameworkElement;

                Debug.Assert(child != null);

                if (child != null)
                {
                    this.Render(renderTarget, rootElement, child);
                }
            }
        }
    }
}
