#if NETFX_CORE
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

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

            // Need to set it to null so that the compiler does not
            // complain about use of unassigned local variable.
            WebViewNavigationFailedEventHandler nfeh = null;
            LoadCompletedEventHandler lceh = null;

            //webView.NavigationFailed
            //webView.LoadCompleted

            nfeh = (s, e) =>
            {
                webView.NavigationFailed -= nfeh;
                webView.LoadCompleted -= lceh;
                tcs.SetResult(null);
            };

            lceh = (s, e) =>
            {
                webView.NavigationFailed -= nfeh;
                webView.LoadCompleted -= lceh;
                tcs.SetResult(null);
            };

            webView.NavigationFailed += nfeh;
            webView.LoadCompleted += lceh;

            webView.Navigate(source);

            await tcs.Task; 
        }
    }
}
#endif
