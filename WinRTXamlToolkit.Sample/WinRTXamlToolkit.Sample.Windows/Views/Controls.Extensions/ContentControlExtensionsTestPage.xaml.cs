using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ContentControlExtensionsTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public ContentControlExtensionsTestPage()
        {
            this.InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
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
