using WinRTXamlToolkit.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class CascadingImageTestView : UserControl
    {
        public CascadingImageTestView()
        {
            this.InitializeComponent();
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ((CascadingImageControl)sender).Cascade();
        }
    }
}
