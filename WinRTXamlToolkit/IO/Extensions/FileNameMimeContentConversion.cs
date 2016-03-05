using System.IO;

namespace WinRTXamlToolkit.IO.Extensions
{
    /// <summary>
    /// Helpers for file name to MIME content type conversion.
    /// </summary>
    public static class FileNameMimeContentConversion
    {
        /// <summary>
        /// Gets the MIME content type based on file name.
        /// </summary>
        /// <remarks>
        /// Consider doing this instead if you are already opening the file:<br/>
        /// var fs = await folder.GetFileAsync(fileName);<br/>
        /// var contentType = fs.ContentType;
        /// </remarks>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetMimeContentTypeFromFileName(this string fileName)
        {
            var ext = Path.GetExtension(fileName);

            switch (ext)
            {
                // Audio
                case ".wav":
                    return "audio/wav";
                case ".au":
                    return "audio/basic";
                case ".snd":
                    return "audio/basic";
                case ".mid":
                    return "audio/mid";
                case ".rmi":
                    return "audio/mid";
                case ".mp3":
                    return "audio/mpeg";
                case ".aif":
                    return "audio/x-aiff";
                case ".aifc":
                    return "audio/x-aiff";
                case ".aiff":
                    return "audio/x-aiff";
                case ".m3u":
                    return "audio/x-mpegurl";
                case ".ra":
                    return "audio/x-pn-realaudio";
                case ".ram":
                    return "audio/x-pn-realaudio";
                default:
                    return null;
            }
        }
    }
}
