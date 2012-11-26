using System;
using System.IO;
using System.Threading.Tasks;

using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using SharpDX.WIC;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Controls.Extensions;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Bitmap = SharpDX.WIC.Bitmap;
using Color = SharpDX.Color;
using D2DPixelFormat = SharpDX.Direct2D1.PixelFormat;
using SolidColorBrush = Windows.UI.Xaml.Media.SolidColorBrush;
using TextAlignment = Windows.UI.Xaml.TextAlignment;
using WicPixelFormat = SharpDX.WIC.PixelFormat;

namespace WinRTXamlToolkit.Composition
{
    // Thanks to Christoph Wille and his WinRT-snippets
    // https://github.com/christophwille/winrt-snippets/tree/master/RenderTextToBitmap
    // http://stackoverflow.com/questions/9151615/how-does-one-use-a-memory-stream-instead-of-files-when-rendering-direct2d-images

    public static class WriteableBitmapRenderExtensions
    {
        public static async Task Render(this WriteableBitmap wb, FrameworkElement fe)
        {
            var ms = RenderToStream(fe);
            var msrandom = new MemoryRandomAccessStream(ms);
            await wb.SetSourceAsync(msrandom);
        }

        public static MemoryStream RenderToStream(FrameworkElement fe)
        {
            var width = (int)Math.Ceiling(fe.ActualWidth);
            var height = (int)Math.Ceiling(fe.ActualHeight);

            // pixel format with transparency/alpha channel and RGB values premultiplied by alpha
            var pixelFormat = WicPixelFormat.Format32bppPRGBA;

            // pixel format without transparency, but one that works with Cleartype antialiasing
            //var pixelFormat = WicPixelFormat.Format32bppBGR;

            var wicFactory = new ImagingFactory();
            var dddFactory = new SharpDX.Direct2D1.Factory();
            var dwFactory = new SharpDX.DirectWrite.Factory();

            var wicBitmap = new Bitmap(
                wicFactory,
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
                dddFactory,
                wicBitmap,
                renderTargetProperties)
            {
                //TextAntialiasMode = TextAntialiasMode.Cleartype // this only works with the pixel format with no alpha channel
                TextAntialiasMode = TextAntialiasMode.Grayscale // this is the best we can do for bitmaps with alpha channels
            };

            renderTarget.BeginDraw();
            renderTarget.Clear(new Color(0, 0, 0, 0));

            var textBlocks = fe.GetDescendantsOfType<TextBlock>();

            foreach (var textBlock in textBlocks)
            {
                var textFormat = new TextFormat(dwFactory, textBlock.FontFamily.Source, (float)textBlock.FontSize)
                {
                    TextAlignment = textBlock.TextAlignment.ToSharpDX(),
                    ParagraphAlignment = ParagraphAlignment.Near
                };

                var textBrush = textBlock.Foreground.ToSharpDX(renderTarget);
                var rect = textBlock.GetBoundingRect(fe).ToSharpDX();

                // You can render the bounding rectangle to debug composition
                //renderTarget.DrawRectangle(
                //    rect,
                //    textBrush);
                renderTarget.DrawText(
                    textBlock.Text,
                    textFormat,
                    rect,
                    textBrush);
            }

            renderTarget.EndDraw();

            // TODO: There is no need to encode the bitmap to PNG - we could just copy the texture pixel buffer to a WriteableBitmap pixel buffer.
            var ms = new MemoryStream();

            var stream = new WICStream(
                wicFactory,
                ms);

            var encoder = new PngBitmapEncoder(wicFactory);
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

        private static SharpDX.DirectWrite.TextAlignment ToSharpDX(this Windows.UI.Xaml.TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Center:
                    return SharpDX.DirectWrite.TextAlignment.Center;
                case TextAlignment.Right:
                    return SharpDX.DirectWrite.TextAlignment.Trailing;
                case TextAlignment.Justify:
                    return SharpDX.DirectWrite.TextAlignment.Justified;
                case TextAlignment.Left:
                    return SharpDX.DirectWrite.TextAlignment.Leading;
                default:
                    throw new NotSupportedException("Unexpected TextAlignment value - not available in Windows 8 RTM.");
            }
        }

        private static SharpDX.Direct2D1.Brush ToSharpDX(
            this Windows.UI.Xaml.Media.Brush brush,
            RenderTarget renderTarget)
        {
            var solidColorBrush = brush as SolidColorBrush;

            if (solidColorBrush != null)
            {
                var color = solidColorBrush.Color.ToSharpDX();

                return new SharpDX.Direct2D1.SolidColorBrush(
                    renderTarget,
                    color);
            }

#if DEBUG
            throw new NotSupportedException("Only SolidColorBrush supported for now");
#else
            return new SharpDX.Direct2D1.SolidColorBrush(renderTarget, Color.Transparent);
#endif
        }

        private static SharpDX.Color ToSharpDX(this Windows.UI.Color color)
        {
            return new SharpDX.Color(color.R, color.G, color.B, color.A);
        }

        private static SharpDX.RectangleF ToSharpDX(this Rect rect)
        {
            return new RectangleF((float)rect.Left, (float)rect.Top, (float)rect.Right + 1, (float)rect.Bottom + 1);
        }
    }
}
