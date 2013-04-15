using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Composition.Renderers;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
using D2D = SharpDX.Direct2D1;
using SharpDX.DXGI;
using WIC = SharpDX.WIC;
using Jupiter = Windows.UI.Xaml;

namespace WinRTXamlToolkit.Composition
{
    public class CompositionEngine : Component
    {
// ReSharper disable InconsistentNaming
        private static readonly SharpDX.WIC.ImagingFactory _wicFactory;
        private static readonly SharpDX.Direct2D1.Factory _d2DFactory;
        private static readonly SharpDX.DirectWrite.Factory _dWriteFactory;
        private static readonly SharpDX.Direct2D1.DeviceContext _d2DDeviceContext;
// ReSharper restore InconsistentNaming

        static CompositionEngine()
        {
            _wicFactory = new SharpDX.WIC.ImagingFactory();
            _dWriteFactory = new SharpDX.DirectWrite.Factory();

            var d3DDevice = new SharpDX.Direct3D11.Device(
                DriverType.Hardware,
                DeviceCreationFlags.BgraSupport
#if DEBUG
                | DeviceCreationFlags.Debug
#endif
,
                FeatureLevel.Level_11_1,
                FeatureLevel.Level_11_0,
                FeatureLevel.Level_10_1,
                FeatureLevel.Level_10_0,
                FeatureLevel.Level_9_3,
                FeatureLevel.Level_9_2,
                FeatureLevel.Level_9_1
                );

            var dxgiDevice = ComObject.As<SharpDX.DXGI.Device>(d3DDevice.NativePointer);
                //new SharpDX.DXGI.Device2(d3DDevice.NativePointer);
            var d2DDevice = new SharpDX.Direct2D1.Device(dxgiDevice);
            _d2DFactory = d2DDevice.Factory;
            _d2DDeviceContext = new SharpDX.Direct2D1.DeviceContext(d2DDevice, D2D.DeviceContextOptions.None);
            _d2DDeviceContext.DotsPerInch = new DrawingSizeF(
                Windows.Graphics.Display.DisplayProperties.LogicalDpi,
                Windows.Graphics.Display.DisplayProperties.LogicalDpi);
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

        public async Task<WriteableBitmap> RenderToWriteableBitmap(FrameworkElement fe)
        {
            var width = (int)Math.Ceiling(fe.ActualWidth);
            var height = (int)Math.Ceiling(fe.ActualHeight);

            if (width == 0 ||
                height == 0)
            {
                throw new InvalidOperationException("Can't render an empty element. ActualWidth or ActualHeight equal 0. Consider awaiting a WaitForNonZeroSizeAsync() call or invoking Measure()/Arrange() before the call to Render().");
            }

            using (var renderTargetBitmap = CreateRenderTargetBitmap(width, height))
            {
                _d2DDeviceContext.Target = renderTargetBitmap;
                _d2DDeviceContext.AntialiasMode = D2D.AntialiasMode.PerPrimitive;
                _d2DDeviceContext.TextAntialiasMode = D2D.TextAntialiasMode.Grayscale;

                await Compose(_d2DDeviceContext, fe);

                using (var cpuReadBitmap = CreateCpuReadBitmap(width, height))
                {
                    cpuReadBitmap.CopyFromRenderTarget(
                        _d2DDeviceContext,
                        new DrawingPoint(0, 0),
                        new SharpDX.Rectangle(0, 0, width, height));
                    var mappedRect = cpuReadBitmap.Map(D2D.MapOptions.Read);

                    try
                    {
                        using (var readStream =
                            new DataStream(
                                userBuffer: mappedRect.DataPointer,
                                sizeInBytes: mappedRect.Pitch * height,
                                canRead: true,
                                canWrite: false))
                        {
                            var wb = new WriteableBitmap(width, height);

                            using (var writeStream = wb.PixelBuffer.AsStream())
                            {
                                var buffer = new byte[mappedRect.Pitch];

                                for (int i = 0; i < height; i++)
                                {
                                    readStream.Read(buffer, 0, mappedRect.Pitch);
                                    writeStream.Write(buffer, 0, buffer.Length);
                                }
                            }

                            wb.Invalidate();

                            return wb;
                        }
                    }
                    finally
                    {
                        cpuReadBitmap.Unmap();
                    }
                }
            }
        }

        internal static D2D.Bitmap1 CreateRenderTargetBitmap(int width, int height)
        {
            var renderTargetBitmap = new D2D.Bitmap1(
                _d2DDeviceContext,
                new DrawingSize(width, height),
                new D2D.BitmapProperties1(
                    new D2D.PixelFormat(
                        Format.B8G8R8A8_UNorm, D2D.AlphaMode.Premultiplied),
                        Windows.Graphics.Display.DisplayProperties.LogicalDpi,
                        Windows.Graphics.Display.DisplayProperties.LogicalDpi,
                        D2D.BitmapOptions.Target));

            return renderTargetBitmap;
        }

        internal static D2D.Bitmap1 CreateCpuReadBitmap(int width, int height)
        {
            var cpuReadBitmap = new D2D.Bitmap1(
                _d2DDeviceContext,
                new DrawingSize(width, height),
                new D2D.BitmapProperties1(
                    new D2D.PixelFormat(
                        SharpDX.DXGI.Format.B8G8R8A8_UNorm, D2D.AlphaMode.Premultiplied),
                        Windows.Graphics.Display.DisplayProperties.LogicalDpi,
                        Windows.Graphics.Display.DisplayProperties.LogicalDpi,
                        D2D.BitmapOptions.CpuRead | D2D.BitmapOptions.CannotDraw));

            return cpuReadBitmap;
        }

        public async Task<MemoryStream> RenderToPngStream(FrameworkElement fe)
        {
            var width = (int)Math.Ceiling(fe.ActualWidth);
            var height = (int)Math.Ceiling(fe.ActualHeight);

            if (width == 0 ||
                height == 0)
            {
                throw new InvalidOperationException("Can't render an empty element. ActualWidth or ActualHeight equal 0. Consider awaiting a WaitForNonZeroSizeAsync() call or invoking Measure()/Arrange() before the call to Render().");
            }

            // pixel format with transparency/alpha channel and RGB values premultiplied by alpha
            var pixelFormat = WIC.PixelFormat.Format32bppPRGBA;

            using (var wicBitmap = new WIC.Bitmap(
                this.WicFactory,
                width,
                height,
                pixelFormat,
                WIC.BitmapCreateCacheOption.CacheOnLoad))
            {
                var renderTargetProperties = new D2D.RenderTargetProperties(
                    D2D.RenderTargetType.Default,
                    new D2D.PixelFormat(
                        Format.R8G8B8A8_UNorm, D2D.AlphaMode.Premultiplied),
                    //new D2DPixelFormat(Format.Unknown, AlphaMode.Unknown), // use this for non-alpha, cleartype antialiased text
                    0,
                    0,
                    D2D.RenderTargetUsage.None,
                    D2D.FeatureLevel.Level_DEFAULT);
                using (var renderTarget = new D2D.WicRenderTarget(
                    this.D2DFactory,
                    wicBitmap,
                    renderTargetProperties)
                    {
                        //TextAntialiasMode = TextAntialiasMode.Cleartype // this only works with the pixel format with no alpha channel
                        TextAntialiasMode =
                            D2D.TextAntialiasMode.Grayscale
                        // this is the best we can do for bitmaps with alpha channels
                    })
                {
                    await Compose(renderTarget, fe);
                }

                // TODO: There is no need to encode the bitmap to PNG - we could just copy the texture pixel buffer to a WriteableBitmap pixel buffer.
                return GetBitmapAsStream(wicBitmap);
            }
        }

        private MemoryStream GetBitmapAsStream(WIC.Bitmap wicBitmap)
        {
            int width = wicBitmap.Size.Width;
            int height = wicBitmap.Size.Height;
            var ms = new MemoryStream();

            using (var stream = new WIC.WICStream(
                this.WicFactory,
                ms))
            {
                using (var encoder = new WIC.PngBitmapEncoder(WicFactory))
                {
                    encoder.Initialize(stream);

                    using (var frameEncoder = new WIC.BitmapFrameEncode(encoder))
                    {
                        frameEncoder.Initialize();

                        frameEncoder.SetSize(width, height);
                        var format = WIC.PixelFormat.Format32bppBGRA;
                        frameEncoder.SetPixelFormat(ref format);
                        frameEncoder.WriteSource(wicBitmap);
                        frameEncoder.Commit();
                    }

                    encoder.Commit();
                }
            }

            ms.Position = 0;
            return ms;
        }

        public async Task Compose(D2D.RenderTarget renderTarget, FrameworkElement fe)
        {
            renderTarget.BeginDraw();
            renderTarget.Clear(new SharpDX.Color(0, 0, 0, 0));
            await this.Render(renderTarget, fe, fe);
            renderTarget.EndDraw();
        }

        public async Task Render(D2D.RenderTarget renderTarget, FrameworkElement rootElement, FrameworkElement fe)
        {
            var textBlock = fe as TextBlock;

            if (textBlock != null)
            {
                await TextBlockRenderer.Render(this, renderTarget, rootElement, textBlock);
                return;
            }

            var rectangle = fe as Jupiter.Shapes.Rectangle;

            if (rectangle != null)
            {
                await RectangleRenderer.Render(this, renderTarget, rootElement, rectangle);
                return;
            }

            var border = fe as Border;

            if (border != null)
            {
                await BorderRenderer.Render(this, renderTarget, rootElement, border);
                return;
            }

            var image = fe as Image;

            if (image != null)
            {
                await ImageRenderer.Render(this, renderTarget, rootElement, image);
                return;
            }

            var ellipse = fe as Ellipse;

            if (ellipse != null)
            {
#pragma warning disable 4014
                EllipseRenderer.Render(this, renderTarget, rootElement, ellipse);
#pragma warning restore 4014
                return;
            }

            var line = fe as Line;

            if (line != null)
            {
                await LineRenderer.Render(this, renderTarget, rootElement, line);
                return;
            }

            var path = fe as Jupiter.Shapes.Path;

            if (path != null)
            {
                await PathRenderer.Render(this, renderTarget, rootElement, path);
                return;
            }

            await FrameworkElementRenderer.Render(this, renderTarget, rootElement, fe);
        }

        internal async Task RenderChildren(D2D.RenderTarget renderTarget, FrameworkElement rootElement, FrameworkElement fe)
        {
            var children = fe.GetChildrenByZIndex();

            foreach (var dependencyObject in children)
            {
                var child = dependencyObject as FrameworkElement;

                Debug.Assert(child != null);

                if (child != null)
                {
                    await this.Render(renderTarget, rootElement, child);
                }
            }
        }
    }
}
