#if SILVERLIGHT
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
#elif NETFX_CORE
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
#elif WPF
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
#endif

namespace WinRTXamlToolkit.AwaitableUI
{
    /// <summary>
    /// Extension methods for waiting for button clicks
    /// on one or one of a collection of buttons.
    /// </summary>
    public static class ButtonExtensions
    {
        /// <summary>
        /// Waits for the button Click event.
        /// </summary>
        public static async Task<RoutedEventArgs> WaitForClickAsync(this ButtonBase button)
        {
            return await EventAsync.FromRoutedEvent(
                eh => button.Click += eh,
                eh => button.Click -= eh);
        }

        /// <summary>
        /// Waits for the Click event from any of the buttons and returns the first button clicked.
        /// </summary>
        public static async Task<ButtonBase> WaitForClickAsync(this IEnumerable<ButtonBase> buttons)
        {
            var tcs = new TaskCompletionSource<ButtonBase>();

            RoutedEventHandler reh = null;

            reh = (s, e) =>
            {
                foreach (var button in buttons)
                {
                    button.Click -= reh;
                }

                tcs.SetResult((ButtonBase)s);
            };

            foreach (var button in buttons)
            {
                button.Click += reh;
            }

            return await tcs.Task;
        }

        /// <summary>
        /// Waits for the Click event from any of the buttons and returns the first button clicked.
        /// </summary>
        public static async Task<ButtonBase> WaitForClickAsync(params ButtonBase[] buttons)
        {
            return await buttons.WaitForClickAsync();
        }
    }
}
