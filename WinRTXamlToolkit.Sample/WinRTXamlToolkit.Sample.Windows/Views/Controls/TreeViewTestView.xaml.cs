using WinRTXamlToolkit.Sample.ViewModels.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class TreeViewTestView : UserControl
    {
        public TreeViewTestView()
        {
            this.InitializeComponent();
            this.DataContext = new TreeViewPageViewModel();
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var toggleButton = (ToggleButton)sender;
            var styleName = (string)toggleButton.Content;
            var style = this.Resources[styleName] as Style;

            if (style != null &&
                tvDataBound != null)
            {
                tvDataBound.Style = style;

                if (toggleButton == MouseThemeRadioButton)
                {
                    tvDataBound.RequestedTheme = ElementTheme.Light;
                }
                else
                {
                    tvDataBound.RequestedTheme = ElementTheme.Default;
                }
            }
        }
    }
}
