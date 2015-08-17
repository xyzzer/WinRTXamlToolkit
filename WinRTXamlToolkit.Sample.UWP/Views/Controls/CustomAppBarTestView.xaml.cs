using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class CustomAppBarTestView : UserControl
    {
        public CustomAppBarTestView()
        {
            this.InitializeComponent();
        }

        private void OnLeftButtonClick(object sender, RoutedEventArgs e)
        {
            LeftAppBar.IsOpen = false;
        }

        private void OnRightButtonClick(object sender, RoutedEventArgs e)
        {
            RightAppBar.IsOpen = false;
        }

        private void OnIsLightDismissEnabledCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            BottomAppBar.IsLightDismissEnabled =
                TopAppBar.IsLightDismissEnabled =
                InnerTopAppBar.IsLightDismissEnabled =
                InnerBottomAppBar.IsLightDismissEnabled =
                LeftAppBar.IsLightDismissEnabled =
                RightAppBar.IsLightDismissEnabled = true;
        }

        private void OnIsLightDismissEnabledCheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            BottomAppBar.IsLightDismissEnabled =
                TopAppBar.IsLightDismissEnabled =
                InnerTopAppBar.IsLightDismissEnabled =
                InnerBottomAppBar.IsLightDismissEnabled =
                LeftAppBar.IsLightDismissEnabled =
                RightAppBar.IsLightDismissEnabled = false;
        }
    }
}
