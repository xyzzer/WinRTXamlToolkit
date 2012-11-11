using System;
using System.Linq;
using System.Threading.Tasks;
using WinRTXamlToolkit.AwaitableUI;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class AlternativeFrameTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public AlternativeFrameTestPage()
        {
            this.InitializeComponent();
            this.subFrame.Navigated += subFrame_Navigated;
            InitializeTest();
        }

        private async Task subFrame_Navigated(object sender, Controls.AlternativeNavigationEventArgs args)
        {
            await UpdateStacks();
        }

        private async Task UpdateStacks()
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
            await subFrame.Navigate(typeof(AlternativeFrameTestPageSubPage1), (int)1);
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void GetNavigationStateButton_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationStateTextBox.Text = subFrame.GetNavigationState();
        }

        private void SetNavigationStateButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                subFrame.SetNavigationState(NavigationStateTextBox.Text);
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
