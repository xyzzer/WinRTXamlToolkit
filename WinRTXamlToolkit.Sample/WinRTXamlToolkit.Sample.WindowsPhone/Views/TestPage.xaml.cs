using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Sample.ViewModels;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class TestPage : Page
    {
        public TestPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var title = e.Parameter as string;
            this.TitleTextBlock.Text = title;
            var sampleButton = MainPageViewModel.Instance.Samples.First(s => s.Caption == title);
            var content = (FrameworkElement)Activator.CreateInstance(sampleButton.ViewType);
            this.ContentGrid.Children.Add(content);
        }
    }
}
