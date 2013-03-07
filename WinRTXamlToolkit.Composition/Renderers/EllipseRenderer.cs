using System.Threading.Tasks;
using SharpDX;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
using Jupiter = Windows.UI.Xaml;
using D2D = SharpDX.Direct2D1;

namespace WinRTXamlToolkit.Composition.Renderers
{
    public static class EllipseRenderer
    {
        internal static async Task Render(CompositionEngine compositionEngine, SharpDX.Direct2D1.RenderTarget renderTarget, FrameworkElement rootElement, Ellipse ellipse)
        {
            var rect = ellipse.GetBoundingRect(rootElement).ToSharpDX();

            var d2dEllipse = new D2D.Ellipse(
                new DrawingPointF(
                    (float)((rect.Left + rect.Right) * 0.5),
                    (float)((rect.Top + rect.Bottom) * 0.5)),
                (float)(0.5 * rect.Width),
                (float)(0.5 * rect.Height));
            var fill = await ellipse.Fill.ToSharpDX(renderTarget, rect);

            //var layer = new Layer(renderTarget);
            //var layerParameters = new LayerParameters();
            //layerParameters.ContentBounds = rect;
            //renderTarget.PushLayer(ref layerParameters, layer);

            var stroke = await ellipse.Stroke.ToSharpDX(renderTarget, rect);

            if (ellipse.StrokeThickness > 0 &&
                stroke != null)
            {
                var halfStrokeThickness = (float)(ellipse.StrokeThickness * 0.5);
                d2dEllipse.RadiusX -= halfStrokeThickness;
                d2dEllipse.RadiusY -= halfStrokeThickness;

                if (fill != null)
                {
                    renderTarget.FillEllipse(d2dEllipse, fill);
                }

                renderTarget.DrawEllipse(
                    d2dEllipse,
                    stroke,
                    (float)ellipse.StrokeThickness,
                    ellipse.GetStrokeStyle(compositionEngine.D2DFactory));
            }
            else if (fill != null)
            {
                renderTarget.FillEllipse(d2dEllipse, fill);
            }

            //renderTarget.PopLayer();
        }
    }
}
