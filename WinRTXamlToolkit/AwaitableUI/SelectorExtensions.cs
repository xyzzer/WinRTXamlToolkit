#if SILVERLIGHT
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
#elif NETFX_CORE
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
#elif WPF
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
#endif

namespace WinRTXamlToolkit.AwaitableUI
{
    /// <summary>
    /// Contains an extension method for awaiting selection changes on a Selector control, such as a ListBox.
    /// </summary>
    public static class SelectorExtensions
    {
        /// <summary>
        /// Waits for a selection change event in a Selector (eg. a ListBox).
        /// </summary>
        public static async Task<SelectionChangedEventArgs> WaitForSelectionChangedAsync(this Selector selector)
        {
            var tcs = new TaskCompletionSource<SelectionChangedEventArgs>();

            // Need to set it to noll so that the compiler does not
            // complain about use of unassigned local variable.
            SelectionChangedEventHandler sceh = null;

            sceh = (s, e) =>
            {
                selector.SelectionChanged -= sceh;
                tcs.SetResult(e);
            };

            selector.SelectionChanged += sceh;
            return await tcs.Task;
        }
    }
}
