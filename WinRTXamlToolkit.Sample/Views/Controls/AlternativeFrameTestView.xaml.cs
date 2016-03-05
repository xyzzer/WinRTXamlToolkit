using System;
using System.Linq;
using System.Threading.Tasks;
using WinRTXamlToolkit.AwaitableUI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class AlternativeFrameTestView : UserControl
    {
        public AlternativeFrameTestView()
        {
            this.InitializeComponent();
            this.subFrame.Navigated += this.OnSubFrameNavigatedAsync;
            InitializeTest();
        }

        private async Task OnSubFrameNavigatedAsync(object sender, Controls.AlternativeNavigationEventArgs args)
        {
            await this.UpdateStacksAsync();
        }

        private async Task UpdateStacksAsync()
        {
            // Wait for the transition to complete to keep it smooth
            await Task.Delay(400);
            BackStackItemsControl.ItemsSource = subFrame.BackStack.Reverse().ToList();
            CurrentJournalEntryContentControl.Content = subFrame.CurrentJournalEntry;
            ForwardStackItemsControl.ItemsSource = subFrame.ForwardStack.ToList();
        }

        private async void InitializeTest()
        {
            await this.WaitForLoadedAsync();
            subFrame.Visibility = Visibility.Visible;
            await subFrame.NavigateAsync(typeof(AlternativeFrameTestPageSubPage1), (int)1);
        }

        private void GetNavigationStateButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationStateTextBox.Text = subFrame.GetNavigationState();
        }

        private async void SetNavigationStateButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                await subFrame.SetNavigationStateAsync(NavigationStateTextBox.Text);
            }
            catch(Exception ex)
            {
#pragma warning disable 4014
                new MessageDialog(ex.ToString(), "Exception").ShowAsync();
#pragma warning restore 4014
            }
        }
    }
}
