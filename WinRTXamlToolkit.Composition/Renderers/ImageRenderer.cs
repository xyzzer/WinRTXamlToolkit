using System.Threading.Tasks;
using WinRTXamlToolkit.Controls.Extensions;
using D2D = SharpDX.Direct2D1;
using WIC = SharpDX.WIC;
using Jupiter = Windows.UI.Xaml;

namespace WinRTXamlToolkit.Composition.Renderers
{
    public static class ImageRenderer
    {
        internal static async Task Render(CompositionEngine compositionEngine, SharpDX.Direct2D1.RenderTarget renderTarget, Jupiter.FrameworkElement rootElement, Jupiter.Controls.Image image)
        {
            var rect = image.GetBoundingRect(rootElement).ToSharpDX();
            if (rect.Width == 0 ||
                rect.Height == 0)
            {
                return;
            }

            var bitmap = await image.Source.ToSharpDX(renderTarget);

            if (bitmap == null)
            {
                return;
            }

            try
            {
                //var layer = new Layer(renderTarget);
                //var layerParameters = new LayerParameters();
                //layerParameters.ContentBounds = rect;
                //renderTarget.PushLayer(ref layerParameters, layer);
                renderTarget.DrawBitmap(
                    bitmap,
                    rect,
                    (float)image.Opacity,
                    D2D.BitmapInterpolationMode.Linear);

                //renderTarget.PopLayer();
            }
            finally
            {
                bitmap.Dispose();
            }
        }
    }
}
