using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class CustomAppBarTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public CustomAppBarTestPage()
        {
            this.InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
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
