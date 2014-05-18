using System.Threading.Tasks;
using WinRTXamlToolkit.Sample.ViewModels.Controls.Extensions;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class TextBoxValidationTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public TextBoxValidationTestPage()
        {
            this.InitializeComponent();
            this.DataContext = new TextBoxValidationTestPageViewModel();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
