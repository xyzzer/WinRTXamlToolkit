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

        private async void OnFadeInThemeButtonClick(object sender, RoutedEventArgs e)
        {
            await testBorder.FadeIn();
        }

        private async void OnFadeOutThemeButtonClick(object sender, RoutedEventArgs e)
        {
            await testBorder.FadeOut();
        }

        private async void OnFadeInCustomButtonClick(object sender, RoutedEventArgs e)
        {
            await testBorder.FadeInCustom(TimeSpan.FromSeconds(0.2));
        }

        private async void OnFadeOutCustomButtonClick(object sender, RoutedEventArgs e)
        {
            await testBorder.FadeOutCustom(TimeSpan.FromSeconds(0.2));
        }
    }
}
