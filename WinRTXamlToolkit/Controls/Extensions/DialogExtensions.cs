using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Dialog (MessageBox) related helpers
    /// </summary>
    public static class DialogExtensions
    {
        /// <summary>
        /// Shows a simple message with specified text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static async Task ShowMessage(string text)
        {
            var dialog = new Windows.UI.Popups.MessageDialog(text);
            var result = await dialog.ShowAsync();
        }

        /// <summary>
        /// Shows a dialog with two options to choose from
        /// </summary>
        /// <param name="text"></param>
        /// <param name="leftButtonText"></param>
        /// <param name="rightButtonText"></param>
        /// <param name="leftButtonAction"></param>
        /// <param name="rightButtonAction"></param>
        /// <returns></returns>
        public static async Task ShowTwoOptionsDialog(string text, string leftButtonText, string rightButtonText, Action leftButtonAction, Action rightButtonAction)
        {
            MessageDialog dialog = new MessageDialog(text);
            UICommandInvokedHandler cmdHandler = new UICommandInvokedHandler(cmd =>
            {
                if (leftButtonAction != null) leftButtonAction.Invoke();
            });

            UICommand yesCmd = new UICommand(leftButtonText, c =>
            {
                if (rightButtonAction != null) rightButtonAction.Invoke();
            });
            UICommand noCmd = new UICommand(rightButtonText, c =>
            {

            });

            dialog.Commands.Add(yesCmd);
            dialog.Commands.Add(noCmd);

            dialog.DefaultCommandIndex = 1;

            await dialog.ShowAsync();
        }
    }
}
