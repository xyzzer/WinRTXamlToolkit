using WinRTXamlToolkit.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class CascadingImageTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public CascadingImageTestPage()
        {
            this.InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ((CascadingImageControl)sender).Cascade();
        }
    }
}
