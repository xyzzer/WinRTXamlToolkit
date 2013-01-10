using System;
using System.Threading.Tasks;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class UIElementAnimationExtensionsTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public UIElementAnimationExtensionsTestPage()
        {
            this.InitializeComponent();
        }

        protected override async Task OnNavigatedTo(AlternativeNavigationEventArgs e)
        {
            await base.OnNavigatedTo(e);
        }

        protected override async Task OnNavigatingFrom(AlternativeNavigatingCancelEventArgs e)
        {
            await base.OnNavigatingFrom(e);
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
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
