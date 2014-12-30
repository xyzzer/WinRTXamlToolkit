using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.AwaitableUI
{
    /// <summary>
    /// Provides awaitable extension methods for UIElement objects.
    /// </summary>
    public static class UIElementExtensions
    {
        /// <summary>
        /// Waits for the LostFocus event.
        /// Note that the event might not be raised if the focus has already been lost or is queued to be lost,
        /// so some polling might be advised if that is the case.
        /// </summary>
        /// <param name="control">The control whoe focuse loss is being awaited.</param>
        public static async Task WaitForLostFocusAsync(this UIElement control)
        {
            await EventAsync.FromRoutedEvent(
                eh => control.LostFocus += eh,
                eh => control.LostFocus -= eh);
        }
    }
}
