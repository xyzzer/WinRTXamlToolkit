using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WinRTXamlToolkit.IO.Extensions;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace WinRTXamlToolkit.Net
{
    /// <summary>
    /// Contains a helper method for downloading a file from a web uri.
    /// </summary>
    public static class WebFile
    {
        /// <summary>
        /// Downloads a file from the specified address and returns the file.
        /// </summary>
        /// <param name="fileUri">The URI of the file.</param>
        /// <param name="folder">The folder to save the file to.</param>
        /// <param name="fileName">The file name to save the file as.</param>
        /// <param name="option">
        /// A value that indicates what to do
        /// if the filename already exists in the current folder.
        /// </param>
        /// <remarks>
        /// If no file name is given - the method will try to find
        /// the suggested file name in the HTTP response
        /// based on the Content-Disposition HTTP header.
        /// </remarks>
        /// <returns></returns>
        public async static Task<StorageFile> SaveAsync(
            Uri fileUri,
            StorageFolder folder = null,
            string fileName = null,
            NameCollisionOption option = NameCollisionOption.GenerateUniqueName)
        {
            if (folder == null)
            {
                folder = ApplicationData.Current.LocalFolder;
            }

            var file = await folder.CreateTempFileAsync();
            var downloader = new BackgroundDownloader();
            var download = downloader.CreateDownload(
                fileUri,
                file);

            var res = await download.StartAsync();

            if (string.IsNullOrEmpty(fileName))
            {
                // Use temp file name by default
                fileName = file.Name;

                // Try to find a suggested file name in the http response headers
                // and rename the temp file before returning if the name is found.
                var info = res.GetResponseInformation();

                if (info.Headers.ContainsKey("Content-Disposition"))
                {
                    var cd = info.Headers["Content-Disposition"];
                    var regEx = new Regex("filename=\"(?<fileNameGroup>.+?)\"");
                    var match = regEx.Match(cd);

                    if (match.Success)
                    {
                        fileName = match.Groups["fileNameGroup"].Value;
                        await file.RenameAsync(fileName, option);
                        return file;
                    }
                }
            }

            await file.RenameAsync(fileName, option);
            return file;
        }
    }
}
