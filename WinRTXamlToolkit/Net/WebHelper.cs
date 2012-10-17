using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Net
{
    /// <summary>
    /// A few Web utilities
    /// </summary>
    public static class WebHelper
    {
        /// <summary>
        /// Checks if Internet connection is available. May not be the most best way to do it though
        /// </summary>
        /// <returns></returns>
        public static bool IsConnectedToInternet()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }

        /// <summary>
        /// Downloads content from given URL and returns it as string
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> DownloadPageAsync(string url)
        {
            HttpClientHandler handler = new HttpClientHandler { UseDefaultCredentials = true, AllowAutoRedirect = true };
            HttpClient client = new HttpClient(handler);
            client.MaxResponseContentBufferSize = 196608;
            HttpResponseMessage response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }

}
