using System;
using SharpDX;
using WinRTXamlToolkit.Imaging;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Jupiter = Windows.UI.Xaml;
using D2D = SharpDX.Direct2D1;

namespace WinRTXamlToolkit.Composition
{
    public static class Conversion
    {
        public static SharpDX.DirectWrite.TextAlignment ToSharpDX(this Jupiter.TextAlignment alignment)
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

        public static D2D.Brush ToSharpDX(
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
                    color);
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
                    //brushProperties,
                    gradientStopCollection);
            }

            //var imageBrush = brush as Jupiter.Media.ImageBrush;

            //if (imageBrush != null)
            //{
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

        public static SharpDX.Color ToSharpDX(this Windows.UI.Color color)
        {
            return new SharpDX.Color(color.R, color.G, color.B, color.A);
        }

        public static SharpDX.RectangleF ToSharpDX(this Rect rect)
        {
            return new RectangleF((float)rect.Left, (float)rect.Top, (float)rect.Right, (float)rect.Bottom);
        }

        public static D2D.CapStyle ToSharpDX(this Jupiter.Media.PenLineCap lineCap)
        {
            switch (lineCap)
            {
                case PenLineCap.Flat:
                    return D2D.CapStyle.Flat;
                case PenLineCap.Round:
                    return D2D.CapStyle.Round;
                case PenLineCap.Square:
                    return D2D.CapStyle.Square;
                case PenLineCap.Triangle:
                    return D2D.CapStyle.Triangle;
                default:
                    throw new NotSupportedException("Unexpected PenLineCap value - not available in Windows 8 RTM.");
            }
        }

        public static D2D.LineJoin ToSharpDX(this Jupiter.Media.PenLineJoin lineJoin)
        {
            switch (lineJoin)
            {
                case PenLineJoin.Miter:
                    return D2D.LineJoin.Miter;
                case PenLineJoin.Bevel:
                    return D2D.LineJoin.Bevel;
                case PenLineJoin.Round:
                    return D2D.LineJoin.Round;
                default:
                    throw new NotSupportedException("Unexpected PenLineJoin value - not available in Windows 8 RTM.");
            }
        }
    }
}
