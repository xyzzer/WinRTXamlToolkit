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
            var stroke = await line.Stroke.ToSharpDX(renderTarget, rect);

            if (stroke == null ||
                line.StrokeThickness <= 0)
            {
                return;
            }

            var layer = line.CreateAndPushLayerIfNecessary(renderTarget, rootElement);

            renderTarget.DrawLine(
                new Vector2(
                    rect.Left + (float)line.X1,
                    rect.Top + (float)line.Y1),
                new Vector2(
                    rect.Left + (float)line.X2,
                    rect.Top + (float)line.Y2),
                    stroke,
                    (float)line.StrokeThickness,
                    line.GetStrokeStyle(compositionEngine.D2DFactory));

            if (layer != null)
            {
                renderTarget.PopLayer();
                layer.Dispose();
            }
        }
    }
}
