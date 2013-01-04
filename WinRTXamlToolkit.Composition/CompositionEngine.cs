using System;
using System.Diagnostics;
using System.IO;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Composition.Renderers;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
using Path = Windows.UI.Xaml.Shapes.Path;
using D2D = SharpDX.Direct2D1;
using SharpDX.DXGI;
using WIC = SharpDX.WIC;
using Jupiter = Windows.UI.Xaml;

namespace WinRTXamlToolkit.Composition
{
    public class CompositionEngine
    {
        private readonly SharpDX.DXGI.Factory1 _dxgiFactory;
        private readonly SharpDX.WIC.ImagingFactory _wicFactory;
        private readonly SharpDX.Direct2D1.Factory _d2DFactory;
        private readonly SharpDX.DirectWrite.Factory _dWriteFactory;

        public CompositionEngine()
        {
            _dxgiFactory = new SharpDX.DXGI.Factory1();
            //_dxgiFactory.Adapters1[0].Description1.
            //_dxgiFactory = new SharpDX.DXGI.Factory();
            //new 
            _wicFactory = new SharpDX.WIC.ImagingFactory();
            _d2DFactory = new SharpDX.Direct2D1.Factory();
            _dWriteFactory = new SharpDX.DirectWrite.Factory();
        }

        public WIC.ImagingFactory WicFactory
        {
            get
            {
                return _wicFactory;
            }
        }

        public SharpDX.Direct2D1.Factory D2DFactory
        {
            get
            {
                return _d2DFactory;
            }
        }

        public SharpDX.DirectWrite.Factory DWriteFactory
        {
            get
            {
                return _dWriteFactory;
            }
        }

        public WriteableBitmap RenderToWriteableBitmap(FrameworkElement fe)
        {
            var width = (int)Math.Ceiling(fe.ActualWidth);
            var height = (int)Math.Ceiling(fe.ActualHeight);

            var renderTargetProperties = new D2D.RenderTargetProperties(
                D2D.RenderTargetType.Default,
                new D2D.PixelFormat(Format.B8G8R8A8_UNorm, D2D.AlphaMode.Premultiplied),
                0,
                0,
                D2D.RenderTargetUsage.None,
                D2D.FeatureLevel.Level_DEFAULT);

            var pixelFormat = WIC.PixelFormat.Format32bppPRGBA;

            //new D2D.Device()

            //new DeviceContext()

            //new DeviceContext()
            //var bitmap = new Bitmap1(
            //    _d2DFactory,
            //    width,
            //    height,
            //    pixelFormat,
            //    BitmapCreateCacheOption.CacheOnLoad);

            var wb = new WriteableBitmap(width, height);

            return wb;
        }

        public MemoryStream RenderToPngStream(FrameworkElement fe)
        {
            var width = (int)Math.Ceiling(fe.ActualWidth);
            var height = (int)Math.Ceiling(fe.ActualHeight);

            // pixel format with transparency/alpha channel and RGB values premultiplied by alpha
            var pixelFormat = WIC.PixelFormat.Format32bppPRGBA;

            // pixel format without transparency, but one that works with Cleartype antialiasing
            //var pixelFormat = WicPixelFormat.Format32bppBGR;

            var wicBitmap = new WIC.Bitmap(
                this.WicFactory,
                width,
                height,
                pixelFormat,
                WIC.BitmapCreateCacheOption.CacheOnLoad);

            var renderTargetProperties = new D2D.RenderTargetProperties(
                D2D.RenderTargetType.Default,
                new D2D.PixelFormat(Format.R8G8B8A8_UNorm, D2D.AlphaMode.Premultiplied),
                //new D2DPixelFormat(Format.Unknown, AlphaMode.Unknown), // use this for non-alpha, cleartype antialiased text
                0,
                0,
                D2D.RenderTargetUsage.None,
                D2D.FeatureLevel.Level_DEFAULT);
            var renderTarget = new D2D.WicRenderTarget(
                this.D2DFactory,
                wicBitmap,
                renderTargetProperties)
            {
                //TextAntialiasMode = TextAntialiasMode.Cleartype // this only works with the pixel format with no alpha channel
                TextAntialiasMode = D2D.TextAntialiasMode.Grayscale // this is the best we can do for bitmaps with alpha channels
            };

            Compose(renderTarget, fe);
            // TODO: There is no need to encode the bitmap to PNG - we could just copy the texture pixel buffer to a WriteableBitmap pixel buffer.
            var ms = new MemoryStream();

            var stream = new WIC.WICStream(
                this.WicFactory,
                ms);

            var encoder = new WIC.PngBitmapEncoder(WicFactory);
            encoder.Initialize(stream);

            var frameEncoder = new WIC.BitmapFrameEncode(encoder);
            frameEncoder.Initialize();
            frameEncoder.SetSize(width, height);
            var format = WIC.PixelFormat.Format32bppBGRA;
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

        public void Compose(D2D.RenderTarget renderTarget, FrameworkElement fe)
        {
            renderTarget.BeginDraw();
            renderTarget.Clear(new SharpDX.Color(0, 0, 0, 0));
            this.Render(renderTarget, fe, fe);
            renderTarget.EndDraw();
        }

        public void Render(D2D.RenderTarget renderTarget, FrameworkElement rootElement, FrameworkElement fe)
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

            var line = fe as Line;

            if (line != null)
            {
                LineRenderer.Render(this, renderTarget, rootElement, line);
                return;
            }

            var path = fe as Path;

            if (path != null)
            {
                PathRenderer.Render(this, renderTarget, rootElement, path);
                return;
            }

            FrameworkElementRenderer.Render(this, renderTarget, rootElement, fe);
        }

        internal void RenderChildren(D2D.RenderTarget renderTarget, FrameworkElement rootElement, FrameworkElement fe)
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
