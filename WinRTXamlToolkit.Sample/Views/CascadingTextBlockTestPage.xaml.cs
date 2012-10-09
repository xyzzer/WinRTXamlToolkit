using System.Threading.Tasks;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class CascadingTextBlockTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public CascadingTextBlockTestPage()
        {
            this.InitializeComponent();
            cascadingTextBlock.Loaded += CascadingTextBlockTestPage_Loaded;
        }

        private async void CascadingTextBlockTestPage_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(100);
            while (cascadingTextBlock.IsInVisualTree())
            {
                await cascadingTextBlock.BeginCascadingTransitionAsync();
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
