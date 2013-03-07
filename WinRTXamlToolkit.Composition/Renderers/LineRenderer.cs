using System.Threading.Tasks;
using SharpDX;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;

namespace WinRTXamlToolkit.Composition.Renderers
{
    public static class LineRenderer
    {
        internal static async Task Render(CompositionEngine compositionEngine, SharpDX.Direct2D1.RenderTarget renderTarget, FrameworkElement rootElement, Line line)
        {
            var rect = line.GetBoundingRect(rootElement).ToSharpDX();
            //var fill = line.Fill.ToSharpDX(renderTarget, rect);
            var stroke = await line.Stroke.ToSharpDX(renderTarget, rect);

            if (stroke == null ||
                line.StrokeThickness <= 0)
            {
                return;
            }

            //var layer = new Layer(renderTarget);
            //var layerParameters = new LayerParameters();
            //layerParameters.ContentBounds = rect;
            //renderTarget.PushLayer(ref layerParameters, layer);

            renderTarget.DrawLine(
                new DrawingPointF(
                    rect.Left + (float)line.X1,
                    rect.Top + (float)line.Y1),
                new DrawingPointF(
                    rect.Left + (float)line.X2,
                    rect.Top + (float)line.Y2),
                    stroke,
                    (float)line.StrokeThickness,
                    line.GetStrokeStyle(compositionEngine.D2DFactory));

            //renderTarget.PopLayer();
        }
    }
}
