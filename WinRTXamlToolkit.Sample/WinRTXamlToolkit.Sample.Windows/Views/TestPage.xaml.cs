using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Sample.ViewModels;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class TestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public TestPage()
        {
            this.InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        protected override Task OnNavigatedTo(AlternativeNavigationEventArgs e)
        {
            var title = e.Parameter as string;
            this.TitleTextBlock.Text = title;
            var sampleButton = MainPageViewModel.Instance.UngroupedSamples.First(s => s.Caption == title);
            var content = (FrameworkElement)Activator.CreateInstance(sampleButton.ViewType);
            this.ContentGrid.Children.Add(content);
            return base.OnNavigatedTo(e);
        }
    }
}
