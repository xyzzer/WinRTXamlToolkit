using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class BindingDebugConverterTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public BindingDebugConverterTestPage()
        {
            this.InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
