using System.Threading.Tasks;
using WinRTXamlToolkit.Sample.ViewModels.Controls.Extensions;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class TextBoxFocusExtensionsTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public TextBoxFocusExtensionsTestPage()
        {
            this.InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
