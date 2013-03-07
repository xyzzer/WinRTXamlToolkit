using System.Diagnostics;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Composition.Renderers
{
    public static class FrameworkElementRenderer
    {
        internal static async Task Render(CompositionEngine compositionEngine, RenderTarget renderTarget, FrameworkElement rootElement, FrameworkElement fe)
        {
            await compositionEngine.RenderChildren(renderTarget, rootElement, fe);
        }
    }
}
