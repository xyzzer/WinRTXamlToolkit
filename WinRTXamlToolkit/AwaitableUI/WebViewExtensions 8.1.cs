#if NETFX_CORE
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.AwaitableUI
{
    /// <summary>
    /// Extension methods for WebView class.
    /// </summary>
    public static class WebViewExtensions
    {
        /// <summary>
        /// Navigates to the given source URI and waits for the loading to complete or fail.
        /// </summary>
        public static async Task NavigateAsync(this WebView webView, Uri source)
        {
            var tcs = new TaskCompletionSource<object>();

            TypedEventHandler<WebView, WebViewNavigationCompletedEventArgs> nceh = null;

            nceh = (s, e) =>
            {
                webView.NavigationCompleted -= nceh;
                tcs.SetResult(null);
            };

            webView.NavigationCompleted += nceh;
            webView.Navigate(source);

            await tcs.Task; 
        }
    }
}
#endif
