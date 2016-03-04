using System;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class MessageDialogExtensionsTestView : UserControl
    {
        public MessageDialogExtensionsTestView()
        {
            this.InitializeComponent();
        }

        private void ShowAsyncQueueButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("Dialog 1 ShowQueuedAsync", "Dialog 1");
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            dialog.ShowQueuedAsync();
            dialog = new MessageDialog("Dialog 2 ShowQueuedAsync", "Dialog 2");
            dialog.ShowQueuedAsync();
            dialog = new MessageDialog("Dialog 3 ShowQueuedAsync", "Dialog 3");
            dialog.ShowQueuedAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private void ShowAsyncIfPossibleButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("Dialog 1 ShowIfPossibleAsync", "Dialog 1");
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            dialog.ShowIfPossibleAsync();
            dialog = new MessageDialog("Dialog 2 ShowIfPossibleAsync", "Dialog 2");
            dialog.ShowIfPossibleAsync();
            dialog = new MessageDialog("Dialog 3 ShowIfPossibleAsync", "Dialog 3");
            dialog.ShowIfPossibleAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
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
            dialog = new MessageDialog("ShowIfPossibleAsync", "Dialog 3");
#pragma warning disable 4014
            dialog.ShowIfPossibleAsync();

            // This will not show because there is a dialog shown at this time
            dialog = new MessageDialog("ShowIfPossibleAsync", "Dialog 4");
            dialog.ShowIfPossibleAsync();

            // This will show after Dialog 3 is dismissed
            dialog = new MessageDialog("ShowQueuedAsync", "Dialog 5");
            dialog.ShowQueuedAsync();

            // This will not show because there is a dialog shown at this time (Dialog 3)
            dialog = new MessageDialog("ShowIfPossibleAsync", "Dialog 6");
            dialog.ShowIfPossibleAsync();

            // This will show after Dialog 5 is dismissed
            dialog = new MessageDialog("ShowQueuedAsync", "Dialog 7");
            dialog.ShowQueuedAsync();

            // This will show after Dialog 7 is dismissed
            dialog = new MessageDialog("ShowQueuedAsync", "Dialog 8");
            dialog.ShowQueuedAsync();
#pragma warning restore 4014
        }
    }
}
