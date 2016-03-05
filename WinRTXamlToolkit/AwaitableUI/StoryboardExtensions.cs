#if SILVERLIGHT
using System.Windows.Media.Animation;
using System.Threading.Tasks;
#elif NETFX_CORE
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;
#elif WPF
using System.Threading.Tasks;
using System.Windows.Media.Animation;
#endif

namespace WinRTXamlToolkit.AwaitableUI
{
    /// <summary>
    /// Contains an extension method for waiting for Storyboard to complete.
    /// </summary>
    public static class StoryboardExtensions
    {
        /// <summary>
        /// Begins a storyboard and waits for it to complete.
        /// </summary>
        public static async Task BeginAsync(this Storyboard storyboard)
        {
            await EventAsync.FromEvent<object>(
                eh => storyboard.Completed += eh,
                eh => storyboard.Completed -= eh,
                storyboard.Begin);
        }
    }
}
