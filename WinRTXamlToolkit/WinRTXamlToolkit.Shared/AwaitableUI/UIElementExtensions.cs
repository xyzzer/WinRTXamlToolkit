using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.AwaitableUI
{
    public static class UIElementExtensions
    {
        public static async Task WaitForLostFocusAsync(this UIElement control)
        {
            await EventAsync.FromRoutedEvent(
                eh => control.LostFocus += eh,
                eh => control.LostFocus -= eh);
        }
    }
}
