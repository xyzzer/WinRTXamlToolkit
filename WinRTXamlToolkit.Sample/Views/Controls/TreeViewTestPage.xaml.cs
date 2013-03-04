using WinRTXamlToolkit.Sample.ViewModels.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class TreeViewTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public TreeViewTestPage()
        {
            this.InitializeComponent();
            this.DataContext = new TreeViewPageViewModel();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var toggleButton = (ToggleButton)sender;
            var styleName = (string)toggleButton.Content;
            var style = this.Resources[styleName] as Style;
            if (style != null &&
                tvDataBound != null)
                tvDataBound.Style = style;
        }
    }
}
