using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;

namespace WinRTXamlToolkit.Composition.Renderers
{
    public static class RectangleRenderer
    {
        internal static void Render(CompositionEngine compositionEngine, SharpDX.Direct2D1.RenderTarget renderTarget, FrameworkElement rootElement, Rectangle rectangle)
        {
            var rect = rectangle.GetBoundingRect(rootElement).ToSharpDX();

            var roundedRect = new SharpDX.Direct2D1.RoundedRect();
            roundedRect.Rect = rect;
            roundedRect.RadiusX = (float)rectangle.RadiusX;
            roundedRect.RadiusY = (float)rectangle.RadiusY;
            var brush = rectangle.Fill.ToSharpDX(renderTarget, rect);

            //var layer = new Layer(renderTarget);
            //var layerParameters = new LayerParameters();
            //layerParameters.ContentBounds = rect;
            //renderTarget.PushLayer(ref layerParameters, layer);

            renderTarget.FillRoundedRectangle(roundedRect, brush);

            //renderTarget.PopLayer();
        }
    }
}
