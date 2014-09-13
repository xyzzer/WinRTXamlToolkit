using System.Threading.Tasks;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class CascadingTextBlockTestView : UserControl
    {
        public CascadingTextBlockTestView()
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
    }
}
