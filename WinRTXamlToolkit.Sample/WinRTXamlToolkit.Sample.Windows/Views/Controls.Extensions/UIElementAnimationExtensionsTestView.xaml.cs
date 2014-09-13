using System;
using System.Threading.Tasks;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class UIElementAnimationExtensionsTestView : UserControl
    {
        public UIElementAnimationExtensionsTestView()
        {
            this.InitializeComponent();
        }

        private void OnFadeInThemeButtonClick(object sender, RoutedEventArgs e)
        {
            testBorder.FadeIn();
        }

        private void OnFadeOutThemeButtonClick(object sender, RoutedEventArgs e)
        {
            testBorder.FadeOut();
        }

        private void OnFadeInCustomButtonClick(object sender, RoutedEventArgs e)
        {
            testBorder.FadeInCustom(TimeSpan.FromSeconds(0.2));
        }

        private void OnFadeOutCustomButtonClick(object sender, RoutedEventArgs e)
        {
            testBorder.FadeOutCustom(TimeSpan.FromSeconds(0.2));
        }
    }
}
