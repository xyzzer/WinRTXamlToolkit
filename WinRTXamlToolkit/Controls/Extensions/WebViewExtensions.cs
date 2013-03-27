using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Extension methods for WebView control.
    /// </summary>
    public static class WebViewExtensions
    {
        /// <summary>
        /// Gets the title of the currently displayed page.
        /// </summary>
        /// <param name="webView">The web view.</param>
        /// <returns></returns>
        public static string GetTitle(this WebView webView)
        {
            return webView.InvokeScript("eval", new[] {"document.title"});
        }

        /// <summary>
        /// Gets the address of the current page.
        /// </summary>
        /// <param name="webView">The web view.</param>
        /// <returns></returns>
        public static string GetAddress(this WebView webView)
        {
            var address = webView.InvokeScript("eval", new[] { "document.location.href" });

            if (address == null)
            {
                return webView.Source.ToString();
            }

            return address;
        }

        /// <summary>
        /// Gets the HTML head inner string of the current page.
        /// </summary>
        /// <param name="webView">The web view.</param>
        /// <returns></returns>
        public static string GetHead(this WebView webView)
        {
            //var headCount = webView.InvokeScript(
            //    "eval", new[] {"document.getElementsByTagName('head').innerHTML"});
            //Debug.WriteLine(headCount);

            try
            {
                var head = webView.InvokeScript("eval", new[] { "document.getElementsByTagName('head')[0].innerHTML" });
                return head;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the tag attribute value for specific other attribute name and value.
        /// </summary>
        /// <param name="htmlFragment">The HTML fragment.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="testAttributeName">Name of the test attribute.</param>
        /// <param name="testAttributeValue">The test attribute value.</param>
        /// <param name="attributeToGet">The attribute to get.</param>
        /// <returns></returns>
        private static string GetTagAttributeBySpecificAttribute(
            string htmlFragment,
            string tagName,
            string testAttributeName,
            string testAttributeValue,
            string attributeToGet)
        {
            var regex = new Regex(
                string.Format(
                    "\\<{0}[^\\>]+{1}=\\\"{2}\\\"[^\\>]+{3}=\\\"(?<retGroup>[^\\>]+?)\\\"",
                    tagName,
                    testAttributeName,
                    testAttributeValue,
                    attributeToGet),
                    RegexOptions.Multiline);
            var match = regex.Match(htmlFragment);

            if (match.Success)
            {
                return match.Groups["retGroup"].Value;
            }

            regex = new Regex(
                string.Format(
                    "\\<{0}[^\\>]+{3}=\\\"(?<retGroup>[^\\>]+?)\\\"[^\\>]+{1}=\\\"{2}\\\"",
                    tagName,
                    testAttributeName,
                    testAttributeValue,
                    attributeToGet),
                    RegexOptions.Multiline);
            match = regex.Match(htmlFragment);

            if (match.Success)
            {
                return match.Groups["retGroup"].Value;
            }

            return null;
        }

        /// <summary>
        /// Gets the fav icon URI.
        /// </summary>
        /// <param name="webView">The web view.</param>
        /// <returns></returns>
        public static Uri GetFavIconLink(this WebView webView)
        {
            var head = webView.GetHead();

            if (head == null)
                return null;

            head = head.ToLower();
            var favIconString = GetTagAttributeBySpecificAttribute(
                head, "link", "rel", "shortcut icon", "href");

            //if (favIconString == null)
            //    favIconString = GetTagAttributeBySpecificAttribute(
            //    head, "meta", "itemprop", "image", "content");

            var address = webView.GetAddress();
            var uri = new Uri(address);

            if (favIconString != null)
            {
                if (!favIconString.ToLower().StartsWith("http://") &&
                    !favIconString.ToLower().StartsWith("https://"))
                {
                    favIconString = string.Format(
                        "{0}://{1}/{2}",
                        uri.Scheme,
                        uri.Host,
                        favIconString.TrimStart('/'));
                }

                return new Uri(favIconString);
            }

            return new Uri(string.Format("{0}://{1}/favicon.ico", uri.Scheme, uri.Host));
            //return new Uri("http://www.google.com/s2/favicons?domain=" + webView.GetAddress());
        }
    }
}
