using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// MessageDialog extension methods
    /// </summary>
    public static class MessageDialogExtensions
    {
        /// <summary>
        /// Shows a dialog with two options to choose from
        /// </summary>
        /// <param name="text"></param>
        /// <param name="leftButtonText"></param>
        /// <param name="rightButtonText"></param>
        /// <param name="leftButtonAction"></param>
        /// <param name="rightButtonAction"></param>
        /// <returns></returns>
        public static IAsyncOperation<IUICommand> ShowTwoOptionsDialog(string text, string leftButtonText, string rightButtonText, Action leftButtonAction, Action rightButtonAction)
        {
            var dialog = new MessageDialog(text);

            dialog.AddButton(leftButtonText, leftButtonAction);
            dialog.AddButton(rightButtonText, rightButtonAction);

            dialog.DefaultCommandIndex = 1;

            return dialog.ShowAsync();
        }

        /// <summary>
        /// Adds a button to the MessageDialog with given caption and action.
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="caption"></param>
        /// <param name="action"></param>
        public static void AddButton(this MessageDialog dialog, string caption, Action action)
        {
            var cmd = new UICommand(
                caption,
                c =>
                {
                    if (action != null)
                        action.Invoke();
                });
            dialog.Commands.Add(cmd);
        }

        private static TaskCompletionSource<MessageDialog> _currentDialogShowRequest;

        /// <summary>
        /// Begins an asynchronous operation showing a dialog.
        /// If another dialog is already shown using
        /// ShowAsyncQueue or ShowAsyncIfPossible method - it will wait
        /// for that previous dialog to be dismissed before showing the new one.
        /// </summary>
        /// <param name="dialog">The dialog.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">This method can only be invoked from UI thread.</exception>
        public static async Task<IUICommand> ShowAsyncQueue(this MessageDialog dialog)
        {
            if (!Window.Current.Dispatcher.HasThreadAccess)
            {
                throw new InvalidOperationException("This method can only be invoked from UI thread.");
            }

            while (_currentDialogShowRequest != null)
            {
                await _currentDialogShowRequest.Task;
            }

            var request = _currentDialogShowRequest = new TaskCompletionSource<MessageDialog>();
            var result = await dialog.ShowAsync();
            _currentDialogShowRequest = null;
            request.SetResult(dialog);

            return result;
        }

        /// <summary>
        /// Begins an asynchronous operation showing a dialog.
        /// If another dialog is already shown using
        /// ShowAsyncQueue or ShowAsyncIfPossible method - it will wait
        /// return immediately and the new dialog won't be displayed.
        /// </summary>
        /// <param name="dialog">The dialog.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">This method can only be invoked from UI thread.</exception>
        public static async Task<IUICommand> ShowAsyncIfPossible(this MessageDialog dialog)
        {
            if (!Window.Current.Dispatcher.HasThreadAccess)
            {
                throw new InvalidOperationException("This method can only be invoked from UI thread.");
            }

            while (_currentDialogShowRequest != null)
            {
                return null;
            }

            var request = _currentDialogShowRequest = new TaskCompletionSource<MessageDialog>();
            var result = await dialog.ShowAsync();
            _currentDialogShowRequest = null;
            request.SetResult(dialog);

            return result;
        }
    }
}
