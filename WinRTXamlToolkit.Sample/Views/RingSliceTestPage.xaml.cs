using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class RingSliceTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public RingSliceTestPage()
        {
            this.InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
