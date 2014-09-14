using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Controls.Fx
{
    /// <summary>
    /// Base class for shader effects used in FxContentControl.
    /// </summary>
    public abstract class CpuShaderEffect
    {
        public abstract Task ProcessBitmap(RenderTargetBitmap rtb, WriteableBitmap wb, int pw, int ph);
    }
}