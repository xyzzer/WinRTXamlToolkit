using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Controls.Extensions;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ImageExtensionsTestView : UserControl
    {
        public ImageExtensionsTestView()
        {
            this.InitializeComponent();
            this.Loaded += (sender, args) => this.GetFirstAncestorOfType<AlternativePage>().ShouldWaitForImagesToLoad = false;
        }
    }
}
