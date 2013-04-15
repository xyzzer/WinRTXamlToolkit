using System.Threading.Tasks;
using SharpDX;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Jupiter = Windows.UI.Xaml;
using D2D = SharpDX.Direct2D1;

namespace WinRTXamlToolkit.Composition.Renderers
{
    public static class BorderRenderer
    {
        internal static async Task Render(CompositionEngine compositionEngine, SharpDX.Direct2D1.RenderTarget renderTarget, FrameworkElement rootElement, Border border)
        {
            var rect = border.GetBoundingRect(rootElement).ToSharpDX();
            var brush = await border.Background.ToSharpDX(renderTarget, rect);

            if (brush != null)
            {
                var geometry = GetBorderFillGeometry(compositionEngine, border, rect);

                //var layer = new Layer(renderTarget);
                //var layerParameters = new LayerParameters();
                //layerParameters.ContentBounds = rect;
                //renderTarget.PushLayer(ref layerParameters, layer);

                renderTarget.FillGeometry(geometry, brush);

                //renderTarget.PopLayer();
            }

            await compositionEngine.RenderChildren(renderTarget, rootElement, border);
        }

        private static D2D.PathGeometry GetBorderFillGeometry(
            CompositionEngine compositionEngine, Border border, RectangleF rect)
        {
            var topLeftCornerSize = new DrawingSizeF(
                (float)border.CornerRadius.TopLeft,
                (float)border.CornerRadius.TopLeft);
            var topRightCornerSize = new DrawingSizeF(
                (float)border.CornerRadius.TopRight,
                (float)border.CornerRadius.TopRight);
            var bottomLeftCornerSize = new DrawingSizeF(
                (float)border.CornerRadius.BottomLeft,
                (float)border.CornerRadius.BottomLeft);
            var bottomRightCornerSize = new DrawingSizeF(
                (float)border.CornerRadius.BottomRight,
                (float)border.CornerRadius.BottomRight);

            var topCornersWidth = topLeftCornerSize.Width + topRightCornerSize.Width;

            if (topCornersWidth > rect.Width)
            {
                var scale = rect.Width / topCornersWidth;
                topLeftCornerSize.Width *= scale;
                topRightCornerSize.Width *= scale;
            }

            var bottomCornersWidth = bottomLeftCornerSize.Width + bottomRightCornerSize.Width;

            if (bottomCornersWidth > rect.Width)
            {
                var scale = rect.Width / bottomCornersWidth;
                bottomLeftCornerSize.Width *= scale;
                bottomRightCornerSize.Width *= scale;
            }

            var leftCornersHeight = topLeftCornerSize.Height + bottomLeftCornerSize.Height;

            if (leftCornersHeight > rect.Height)
            {
                var scale = rect.Height / leftCornersHeight;
                topLeftCornerSize.Height *= scale;
                bottomLeftCornerSize.Height *= scale;
            }

            var rightCornersHeight = topRightCornerSize.Height + bottomRightCornerSize.Height;

            if (rightCornersHeight > rect.Height)
            {
                var scale = rect.Height / rightCornersHeight;
                topRightCornerSize.Height *= scale;
                bottomRightCornerSize.Height *= scale;
            }

            var geometry = new D2D.PathGeometry(compositionEngine.D2DFactory);

            // Create the geometry of the irregular rounded rectangle.
            var geometrySink = geometry.Open();

            // Start to the right of the topleft corner.
            geometrySink.BeginFigure(
                new DrawingPointF(
                    rect.Left + topLeftCornerSize.Width,
                    rect.Top + 0),
                D2D.FigureBegin.Filled);

            //if (topCornersWidth < rect.Width)
            {
                // Top edge
                geometrySink.AddLine(
                    new DrawingPointF(
                        rect.Left + rect.Width - topRightCornerSize.Width,
                        rect.Top + 0));
            }

            //if (topRightCornerSize.Width > 0)

            // Top-right corner
            geometrySink.AddArc(
                new D2D.ArcSegment
                {
                    Point = new DrawingPointF(
                        rect.Left + rect.Width,
                        rect.Top + topRightCornerSize.Height),
                    Size = topRightCornerSize,
                    RotationAngle = 0,
                    SweepDirection = D2D.SweepDirection.Clockwise,
                    ArcSize = D2D.ArcSize.Small
                });

            // Right edge
            geometrySink.AddLine(
                new DrawingPointF(
                    rect.Left + rect.Width,
                    rect.Top + rect.Height - bottomRightCornerSize.Height));

            // Bottom-right corner
            geometrySink.AddArc(
                new D2D.ArcSegment
                {
                    Point = new DrawingPointF(
                        rect.Left + rect.Width - bottomRightCornerSize.Width,
                        rect.Top + rect.Height),
                    Size = bottomRightCornerSize,
                    RotationAngle = 0,
                    SweepDirection = D2D.SweepDirection.Clockwise,
                    ArcSize = D2D.ArcSize.Small
                });

            // Bottom edge
            geometrySink.AddLine(
                new DrawingPointF(
                    rect.Left + bottomLeftCornerSize.Width,
                    rect.Top + rect.Height));

            // Bottom-left corner
            geometrySink.AddArc(
                new D2D.ArcSegment
                {
                    Point = new DrawingPointF(
                        rect.Left + 0,
                        rect.Top + rect.Height - bottomLeftCornerSize.Height),
                    Size = bottomLeftCornerSize,
                    RotationAngle = 0,
                    SweepDirection = D2D.SweepDirection.Clockwise,
                    ArcSize = D2D.ArcSize.Small
                });

            // Left edge
            geometrySink.AddLine(
                new DrawingPointF(
                    rect.Left + 0,
                    rect.Top + topLeftCornerSize.Height));

            // Top-left corner
            geometrySink.AddArc(
                new D2D.ArcSegment
                {
                    Point = new DrawingPointF(
                        rect.Left + topLeftCornerSize.Width,
                        rect.Top + 0),
                    Size = topLeftCornerSize,
                    RotationAngle = 0,
                    SweepDirection = D2D.SweepDirection.Clockwise,
                    ArcSize = D2D.ArcSize.Small
                });

            geometrySink.EndFigure(D2D.FigureEnd.Closed);
            geometrySink.Close();

            return geometry;
        }
    }
}
