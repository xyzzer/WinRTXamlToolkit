using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Controls.Fx
{
    /// <summary>
    /// Base class for shader effects used in FxContentControl.
    /// </summary>
    public abstract class CpuShaderEffect
    {
        /// <summary>
        /// Processes the RenderTargetBitmap and outputs the result to the output WriteableBitmap.
        /// </summary>
        /// <param name="rtb">The RenderTargetBitmap that typically includes a screen grab of the portion of UI.</param>
        /// <param name="wb">The WriteableBitmap that the effect output is written to.</param>
        /// <param name="pw">The pixel width of both bitmaps.</param>
        /// <param name="ph">The pixel height of both bitmaps.</param>
        /// <returns>A task that completes when the processing is complete.</returns>
        public abstract Task ProcessBitmap(RenderTargetBitmap rtb, WriteableBitmap wb, int pw, int ph);
    }
}