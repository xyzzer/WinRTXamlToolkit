using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Extensions that apply to Control classes.
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Moves the focus forward (to next control that can get focus).
        /// </summary>
        /// <remarks>
        /// This method ignores TabIndex and TabNavigation properties.
        /// </remarks>
        /// <param name="control">The control.</param>
        public static void MoveFocusForward(this Control control)
        {
            //TODO: Add support for TabIndex and TabNavigation.
            var root = Window.Current.Content;
            var focusable =
                root.GetDescendantsOfType<Control>()
                    .Where(d => d.IsTabStop)
                    .ToList();

            if (focusable.Count == 0)
                return;

            var i = focusable.IndexOf(control);

            if (i < 0)
            {
                focusable[0].Focus(FocusState.Programmatic);
                return;
            }

            if (i + 1 < focusable.Count)
            {
                focusable[i + 1].Focus(FocusState.Programmatic);
                return;
            }

            focusable[0].Focus(FocusState.Programmatic);
        }
    }
}
