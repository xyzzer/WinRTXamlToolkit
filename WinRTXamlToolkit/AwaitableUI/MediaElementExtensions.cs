using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.AwaitableUI
{
    public static class MediaElementExtensions
    {
        /// <summary>
        /// Waits for the MediaElement.CurrentState to change to any (default) or specific MediaElementState value.
        /// </summary>
        /// <param name="mediaElement"></param>
        /// <param name="newState">The MediaElementState value to wait for. Null by default causes the metod to wait for a change to any other state.</param>
        /// <returns></returns>
        public static async Task<MediaElement> WaitForStateAsync(this MediaElement mediaElement, MediaElementState? newState = null)
        {
            if (newState != null &&
                mediaElement.CurrentState == newState.Value)
            {
                return null;
            }

            var tcs = new TaskCompletionSource<MediaElement>();
            RoutedEventHandler reh = null;

            reh = (s, e) =>
            {
                if (newState != null && mediaElement.CurrentState != newState.Value)
                {
                    return;
                }

                mediaElement.CurrentStateChanged -= reh;
                tcs.SetResult((MediaElement)s);
            };

            mediaElement.CurrentStateChanged += reh;

            return await tcs.Task;
        }

        /// <summary>
        /// Waits for the MediaElement to complete playback.
        /// </summary>
        /// <param name="mediaElement"></param>
        /// <returns></returns>
        public static async Task<MediaElement> WaitToComplete(this MediaElement mediaElement)
        {
            var tcs = new TaskCompletionSource<MediaElement>();
            RoutedEventHandler reh = null;

            reh = (s, e) =>
            {
                if (mediaElement.CurrentState == MediaElementState.Buffering ||
                    mediaElement.CurrentState == MediaElementState.Opening ||
                    mediaElement.CurrentState == MediaElementState.Playing)
                {
                    return;
                }

                mediaElement.CurrentStateChanged -= reh;
                tcs.SetResult((MediaElement)s);
            };

            mediaElement.CurrentStateChanged += reh;

            return await tcs.Task;
        }
    }
}
