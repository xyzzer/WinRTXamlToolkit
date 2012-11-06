using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ColorPickerTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public ColorPickerTestPage()
        {
            this.InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
