using System.Net.Http;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

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
            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();

            return
                (connectionProfile != null &&
                connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
        }

        /// <summary>
        /// Downloads content from given URL and returns it as string
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> DownloadStringAsync(string url)
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
