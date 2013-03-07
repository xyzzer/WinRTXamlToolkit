using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using SharpDX;
using WinRTXamlToolkit.Imaging;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Jupiter = Windows.UI.Xaml;
using D2D = SharpDX.Direct2D1;

namespace WinRTXamlToolkit.Composition
{
    public static class Conversion
    {
        public static SharpDX.DirectWrite.TextAlignment ToSharpDX(
            this Jupiter.TextAlignment alignment)
        {
            switch (alignment)
            {
                case Jupiter.TextAlignment.Center:
                    return SharpDX.DirectWrite.TextAlignment.Center;
                case Jupiter.TextAlignment.Right:
                    return SharpDX.DirectWrite.TextAlignment.Trailing;
                case Jupiter.TextAlignment.Justify:
                    return SharpDX.DirectWrite.TextAlignment.Justified;
                case Jupiter.TextAlignment.Left:
                    return SharpDX.DirectWrite.TextAlignment.Leading;
                default:
                    throw new NotSupportedException("Unexpected TextAlignment value - not available in Windows 8 RTM.");
            }
        }

        public static async Task<D2D.Brush> ToSharpDX(
            this Jupiter.Media.Brush brush,
            D2D.RenderTarget renderTarget,
            RectangleF rect)
        {
            if (brush == null)
                return null;

            var solidColorBrush = brush as Jupiter.Media.SolidColorBrush;

            if (solidColorBrush != null)
            {
                var color = solidColorBrush.Color.ToSharpDX();
                
                return new D2D.SolidColorBrush(
                    renderTarget,
                    color,
                    new D2D.BrushProperties
                    {
                        Opacity = (float)solidColorBrush.Opacity
                    });
            }

            var linearGradientBrush = brush as Jupiter.Media.LinearGradientBrush;

            if (linearGradientBrush != null)
            {
                var properties = new D2D.LinearGradientBrushProperties();
                //properties.StartPoint =
                //    new DrawingPointF(
                //        (float)(linearGradientBrush.StartPoint.X * renderTarget.Size.Width),
                //        (float)(linearGradientBrush.StartPoint.Y * renderTarget.Size.Height));
                //properties.EndPoint =
                //    new DrawingPointF(
                //        (float)(linearGradientBrush.EndPoint.X * renderTarget.Size.Width),
                //        (float)(linearGradientBrush.EndPoint.Y * renderTarget.Size.Height));
                properties.StartPoint =
                    new DrawingPointF(
                        rect.Left + (float)(linearGradientBrush.StartPoint.X * rect.Width),
                        rect.Top + (float)(linearGradientBrush.StartPoint.Y * rect.Height));
                properties.EndPoint =
                    new DrawingPointF(
                        rect.Left + (float)(linearGradientBrush.EndPoint.X * rect.Width),
                        rect.Top + (float)(linearGradientBrush.EndPoint.Y * rect.Height));

                var brushProperties = new D2D.BrushProperties();

                brushProperties.Opacity = (float)linearGradientBrush.Opacity;

                if (linearGradientBrush.Transform != null)
                {
                    brushProperties.Transform = linearGradientBrush.Transform.ToSharpDX();
                }

                var gradientStopCollection = linearGradientBrush.GradientStops.ToSharpDX(renderTarget);

                return new D2D.LinearGradientBrush(
                    renderTarget,
                    properties,
                    brushProperties,
                    gradientStopCollection);
            }

            var imageBrush = brush as Jupiter.Media.ImageBrush;

            if (imageBrush != null)
            {
                var bitmap = await imageBrush.ImageSource.ToSharpDX(renderTarget);

                var w = bitmap.PixelSize.Width;
                var h = bitmap.PixelSize.Height;
                Matrix3x2 transform = Matrix3x2.Identity;

                switch (imageBrush.Stretch)
                {
                    case Stretch.None:
                        transform.M31 += rect.Left + rect.Width * 0.5f - w / 2;
                        transform.M32 += rect.Top + rect.Height * 0.5f - h / 2;
                        break;
                    case Stretch.Fill:
                        transform = Matrix3x2.Scaling(
                            rect.Width / w,
                            rect.Height / h);
                        transform.M31 += rect.Left;
                        transform.M32 += rect.Top;
                        break;
                    case Stretch.Uniform:
                        var bitmapAspectRatio = (float)w / h;
                        var elementAspectRatio = rect.Width / rect.Height;

                        if (bitmapAspectRatio > elementAspectRatio)
                        {
                            var scale = rect.Width / w;
                            transform = Matrix3x2.Scaling(scale);
                            transform.M31 += rect.Left;
                            transform.M32 += rect.Top + rect.Height * 0.5f - scale * h / 2;
                        }
                        else // (elementAspectRatio >= bitmapAspectRatio)
                        {
                            var scale = rect.Height / h;
                            transform = Matrix3x2.Scaling(scale);
                            transform.M31 += rect.Left + rect.Width * 0.5f - scale * w / 2;
                            transform.M32 += rect.Top;
                        }

                        break;
                    case Stretch.UniformToFill:
                        var bitmapAspectRatio2 = (float)w / h;
                        var elementAspectRatio2 = rect.Width / rect.Height;

                        if (bitmapAspectRatio2 > elementAspectRatio2)
                        {
                            var scale = rect.Height / h;
                            transform = Matrix3x2.Scaling(scale);
                            transform.M31 += rect.Left + rect.Width * 0.5f - scale * w / 2;
                            transform.M32 += rect.Top;
                        }
                        else // (elementAspectRatio >= bitmapAspectRatio)
                        {
                            var scale = rect.Width / w;
                            transform = Matrix3x2.Scaling(scale);
                            transform.M31 += rect.Left;
                            transform.M32 += rect.Top + rect.Height * 0.5f - scale * h / 2;
                        }

                        break;
                }
                

                return new D2D.BitmapBrush1(
                    (D2D.DeviceContext)renderTarget,
                    bitmap,
                    new D2D.BitmapBrushProperties1
                    {
                        ExtendModeX = D2D.ExtendMode.Clamp,
                        ExtendModeY = D2D.ExtendMode.Clamp,
                        InterpolationMode = D2D.InterpolationMode.HighQualityCubic
                    })
                    {
                        Opacity = (float)imageBrush.Opacity,
                        Transform = transform
                    };
                //    var writeableBitmap = imageBrush.ImageSource as WriteableBitmap;
                //    var bitmapImage = imageBrush.ImageSource as BitmapImage;

                //    if (bitmapImage != null)
                //    {
                //        writeableBitmap =
                //            await WriteableBitmapFromBitmapImageExtension.FromBitmapImage(bitmapImage);
                //    }
                //    CompositionEngine c;

                //    return new D2D.BitmapBrush(
                //        renderTarget,
                //        writeableBitmap.ToSharpDX(),
                //}
            }

#if DEBUG
            throw new NotSupportedException("Only SolidColorBrush supported for now");
#else
            return new D2D.SolidColorBrush(renderTarget, Color.Transparent);
#endif
        }

        public static SharpDX.Matrix3x2 ToSharpDX(
            this Jupiter.Media.Transform transform)
        {
            var matrixTransform = transform as Jupiter.Media.MatrixTransform;

            if (matrixTransform != null)
            {
                return matrixTransform.Matrix.ToSharpDX();
            }

            throw new NotImplementedException();
        }

        public static SharpDX.Matrix3x2 ToSharpDX(
            this Jupiter.Media.Matrix matrix)
        {
                return new Matrix3x2(
                    (float)matrix.M11,
                    (float)matrix.M12,
                    (float)matrix.M21,
                    (float)matrix.M22,
                    (float)matrix.OffsetX,
                    (float)matrix.OffsetY);
        }

        public static D2D.GradientStopCollection ToSharpDX(
            this Jupiter.Media.GradientStopCollection gradientStopCollection,
            D2D.RenderTarget renderTarget)
        {
            var gradientStops = new D2D.GradientStop[gradientStopCollection.Count];

            for (int i = 0; i < gradientStopCollection.Count; i++)
            {
                gradientStops[i] = gradientStopCollection[i].ToSharpDX();
            }

            return new D2D.GradientStopCollection(renderTarget, gradientStops);
        }

        public static D2D.GradientStop ToSharpDX(
            this Jupiter.Media.GradientStop gradientStop)
        {
            return new D2D.GradientStop
                   {
                       Color = gradientStop.Color.ToSharpDX(),
                       Position = (float)gradientStop.Offset
                   };
        }

        public static SharpDX.Color ToSharpDX(
            this Windows.UI.Color color)
        {
            return new SharpDX.Color(color.R, color.G, color.B, color.A);
        }

        public static RectangleF ToSharpDX(this Rect rect)
        {
            return new RectangleF((float)rect.Left, (float)rect.Top, (float)rect.Right, (float)rect.Bottom);
        }

        public static D2D.CapStyle ToSharpDX(
            this Jupiter.Media.PenLineCap lineCap)
        {
            switch (lineCap)
            {
                case Jupiter.Media.PenLineCap.Flat:
                    return D2D.CapStyle.Flat;
                case Jupiter.Media.PenLineCap.Round:
                    return D2D.CapStyle.Round;
                case Jupiter.Media.PenLineCap.Square:
                    return D2D.CapStyle.Square;
                case Jupiter.Media.PenLineCap.Triangle:
                    return D2D.CapStyle.Triangle;
                default:
                    throw new NotSupportedException("Unexpected PenLineCap value - not available in Windows 8 RTM.");
            }
        }

        public static D2D.LineJoin ToSharpDX(
            this Jupiter.Media.PenLineJoin lineJoin)
        {
            switch (lineJoin)
            {
                case Jupiter.Media.PenLineJoin.Miter:
                    return D2D.LineJoin.Miter;
                case Jupiter.Media.PenLineJoin.Bevel:
                    return D2D.LineJoin.Bevel;
                case Jupiter.Media.PenLineJoin.Round:
                    return D2D.LineJoin.Round;
                default:
                    throw new NotSupportedException("Unexpected PenLineJoin value - not available in Windows 8 RTM.");
            }
        }

        public static SharpDX.DrawingPointF ToSharpDX(
            this Windows.Foundation.Point point)
        {
            return new DrawingPointF(
                (float)point.X,
                (float)point.Y);
        }

        public static SharpDX.DrawingSizeF ToSharpDX(
            this Windows.Foundation.Size size)
        {
            return new DrawingSizeF(
                (float)size.Width,
                (float)size.Height);
        }

        public static D2D.SweepDirection ToSharpDX(
            this Jupiter.Media.SweepDirection sweepDirection)
        {
            return
                sweepDirection == Jupiter.Media.SweepDirection.Clockwise
                    ? D2D.SweepDirection.Clockwise
                    : D2D.SweepDirection.CounterClockwise;
        }

        public static async Task<D2D.Bitmap1> ToSharpDX(this ImageSource imageSource, D2D.RenderTarget renderTarget)
        {
            var wb = imageSource as Jupiter.Media.Imaging.WriteableBitmap;

            if (wb == null)
            {
                var bi = imageSource as Jupiter.Media.Imaging.BitmapImage;

                if (bi == null)
                {
                    return null;
                }

                wb = await WriteableBitmapFromBitmapImageExtension.FromBitmapImage(bi);

                if (wb == null)
                {
                    return null;
                }
            }

            int width = wb.PixelWidth;
            int height = wb.PixelHeight;
            //var cpuReadBitmap = CompositionEngine.CreateCpuReadBitmap(width, height);
            var cpuReadBitmap = CompositionEngine.CreateRenderTargetBitmap(width, height);
            //var mappedRect = cpuReadBitmap.Map(D2D.MapOptions.Write | D2D.MapOptions.Read | D2D.MapOptions.Discard);

            using (var readStream = wb.PixelBuffer.AsStream())
            {
                var pitch = width * 4;
                //using (var writeStream =
                //    new DataStream(
                //        userBuffer: mappedRect.DataPointer,
                //        sizeInBytes: mappedRect.Pitch * height,
                //        canRead: false,
                //        canWrite: true))
                {
                    var buffer = new byte[pitch * height];
                    readStream.Read(buffer, 0, buffer.Length);
                    cpuReadBitmap.CopyFromMemory(buffer, pitch);

                    //for (int i = 0; i < height; i++)
                    //{
                    //    readStream.Read(buffer, 0, mappedRect.Pitch);
                    //    writeStream.Write(buffer, 0, buffer.Length);
                    //}
                    
                }
            }
            //cpuReadBitmap.CopyFromMemory();

            return cpuReadBitmap;
        }
    }
}
