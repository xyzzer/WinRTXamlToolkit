using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ContentControlExtensionsTestView : UserControl
    {
        public ContentControlExtensionsTestView()
        {
            this.InitializeComponent();
        }

        private void TestButtonClick(object sender, RoutedEventArgs e)
        {
            if (TestButton.ContentTemplate == Resources["ContentTemplate1"])
                ContentControlExtensions.SetFadeTransitioningContentTemplate(
                    TestButton,
                   (DataTemplate)Resources["ContentTemplate2"]);
            else
                ContentControlExtensions.SetFadeTransitioningContentTemplate(
                    TestButton,
                   (DataTemplate)Resources["ContentTemplate1"]);
        }
    }
}
