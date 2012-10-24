using System;
using Windows.Foundation;
using Windows.UI.Popups;

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
    }
}
