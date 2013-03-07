using System.Threading.Tasks;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Composition.Renderers
{
    public static class TextBlockRenderer
    {
        internal static async Task Render(CompositionEngine compositionEngine, RenderTarget renderTarget, FrameworkElement rootElement, TextBlock textBlock)
        {
            using (var textFormat = new TextFormat(
                compositionEngine.DWriteFactory,
                textBlock.FontFamily.Source,
                (float)textBlock.FontSize)
            {
                TextAlignment = textBlock.TextAlignment.ToSharpDX(),
                ParagraphAlignment = ParagraphAlignment.Near
            })
            {
                var rect = textBlock.GetBoundingRect(rootElement).ToSharpDX();
                // For some reason we need a bigger rect for the TextBlock rendering to fit in the same boundaries
                rect.Right++;
                rect.Bottom++;

                using (
                    var textBrush = await textBlock.Foreground.ToSharpDX(renderTarget, rect))
                {
                    //using(var layer = new Layer(renderTarget))
                    //{
                    //var layerParameters = new LayerParameters();
                    //layerParameters.ContentBounds = rect;
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
                    //}
                }
            }
        }
    }
}
