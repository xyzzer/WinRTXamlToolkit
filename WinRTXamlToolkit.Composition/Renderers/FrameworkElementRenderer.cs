using System.Diagnostics;
using SharpDX.Direct2D1;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Composition.Renderers
{
    public static class FrameworkElementRenderer
    {
        internal static void Render(CompositionEngine compositionEngine, RenderTarget renderTarget, FrameworkElement rootElement, FrameworkElement fe)
        {
            compositionEngine.RenderChildren(renderTarget, rootElement, fe);
        }
    }
}
