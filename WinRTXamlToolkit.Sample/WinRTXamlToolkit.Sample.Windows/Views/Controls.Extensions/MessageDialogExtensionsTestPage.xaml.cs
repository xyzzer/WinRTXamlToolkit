using System;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class MessageDialogExtensionsTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public MessageDialogExtensionsTestPage()
        {
            this.InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void ShowAsyncQueueButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("Dialog 1 ShowAsyncQueue", "Dialog 1");
            dialog.ShowAsyncQueue();
            dialog = new MessageDialog("Dialog 2 ShowAsyncQueue", "Dialog 2");
            dialog.ShowAsyncQueue();
            dialog = new MessageDialog("Dialog 3 ShowAsyncQueue", "Dialog 3");
            dialog.ShowAsyncQueue();
        }

        private void ShowAsyncIfPossibleButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("Dialog 1 ShowAsyncIfPossible", "Dialog 1");
            dialog.ShowAsyncIfPossible();
            dialog = new MessageDialog("Dialog 2 ShowAsyncIfPossible", "Dialog 2");
            dialog.ShowAsyncIfPossible();
            dialog = new MessageDialog("Dialog 3 ShowAsyncIfPossible", "Dialog 3");
            dialog.ShowAsyncIfPossible();
        }

        private async void ShowAsyncQueuePlusIfPossibleButton_OnClick(object sender, RoutedEventArgs e)
        {
            // This should obviously be displayed
            var dialog = new MessageDialog("await ShowAsync", "Dialog 1");
            await dialog.ShowAsync();

            // This should be displayed because we awaited the previous request to return
            dialog = new MessageDialog("await ShowAsync", "Dialog 2");
            await dialog.ShowAsync(); 

            // All other requests below are invoked without awaiting
            // the preceding ones to complete (dialogs being closed)

            // This will show because there is no dialog shown at this time
            dialog = new MessageDialog("ShowAsyncIfPossible", "Dialog 3");
#pragma warning disable 4014
            dialog.ShowAsyncIfPossible();

            // This will not show because there is a dialog shown at this time
            dialog = new MessageDialog("ShowAsyncIfPossible", "Dialog 4");
            dialog.ShowAsyncIfPossible();

            // This will show after Dialog 3 is dismissed
            dialog = new MessageDialog("ShowAsyncQueue", "Dialog 5");
            dialog.ShowAsyncQueue();

            // This will not show because there is a dialog shown at this time (Dialog 3)
            dialog = new MessageDialog("ShowAsyncIfPossible", "Dialog 6");
            dialog.ShowAsyncIfPossible();

            // This will show after Dialog 5 is dismissed
            dialog = new MessageDialog("ShowAsyncQueue", "Dialog 7");
            dialog.ShowAsyncQueue();

            // This will show after Dialog 7 is dismissed
            dialog = new MessageDialog("ShowAsyncQueue", "Dialog 8");
            dialog.ShowAsyncQueue();
#pragma warning restore 4014
        }
    }
}
