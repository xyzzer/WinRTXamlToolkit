using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Composition.Renderers
{
    public static class TextBlockRenderer
    {
        internal static void Render(CompositionEngine compositionEngine, RenderTarget renderTarget, FrameworkElement rootElement, TextBlock textBlock)
        {
            var textFormat = new TextFormat(
                compositionEngine.DWriteFactory,
                textBlock.FontFamily.Source,
                (float)textBlock.FontSize)
            {
                TextAlignment = textBlock.TextAlignment.ToSharpDX(),
                ParagraphAlignment = ParagraphAlignment.Near
            };

            var rect = textBlock.GetBoundingRect(rootElement).ToSharpDX();
            var textBrush = textBlock.Foreground.ToSharpDX(renderTarget, rect);

            var layer = new Layer(renderTarget);
            var layerParameters = new LayerParameters();
            layerParameters.ContentBounds = rect;
            //renderTarget.PushLayer(ref layerParameters, layer);

            // You can render the bounding rectangle to debug composition
            //renderTarget.DrawRectangle(
            //    rect,
            //    textBrush);
            renderTarget.DrawText(
                textBlock.Text,
                textFormat,
                rect,
                textBrush);

            //renderTarget.PopLayer();
        }
    }
}
