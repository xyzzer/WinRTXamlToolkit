#if NETFX_CORE
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace WinRTXamlToolkit.AwaitableUI
{
    public static class WebViewExtensions
    {
        /// <summary>
        /// Begins a storyboard and waits for it to complete.
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
