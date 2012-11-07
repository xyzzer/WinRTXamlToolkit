using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
    /// <summary>
    /// WriteableBitmap extensions for creating or populating a WriteableBitmap from a BitmapImage (or rather the source Uri of the image).
    /// </summary>
    public static class WriteableBitmapFromBitmapImageExtension
    {
        /// <summary>
        /// Creates a new WriteableBitmap from the source URI of the given BitmapImage.
        /// </summary>
        /// <param name="source">The source BitmapImage.</param>
        /// <returns></returns>
        public static async Task<WriteableBitmap> FromBitmapImage(BitmapImage source)
        {
            var ret = new WriteableBitmap(1, 1);
            return await FromBitmapImage(ret, source);
        }

        /// <summary>
        /// Loads the WriteableBitmap from the source URI of the given BitmapImage
        /// </summary>
        /// <param name="target">The target WriteableBitmap.</param>
        /// <param name="source">The source BitmapImage.</param>
        /// <returns></returns>
        public static async Task<WriteableBitmap> FromBitmapImage(this WriteableBitmap target, BitmapImage source)
        {
            string installedFolderImageSourceUri = source.UriSource.OriginalString.Replace("ms-appx:/", "");
            await target.LoadAsync(installedFolderImageSourceUri);
            return target;
        }

        /// <summary>
        /// Creates a new WriteableBitmap from the source URI of the given BitmapImage.
        /// </summary>
        /// <param name="source">The source BitmapImage.</param>
        /// <param name="decodePixelWidth">Width to decode to.</param>
        /// <param name="decodePixelHeight">Height to decode to.</param>
        /// <returns></returns>
        public static async Task<WriteableBitmap> FromBitmapImage(
            BitmapImage source,
            uint decodePixelWidth,
            uint decodePixelHeight)
        {
            var ret = new WriteableBitmap(1, 1);
            return await FromBitmapImage(ret, source, decodePixelWidth, decodePixelHeight);
        }

        /// <summary>
        /// Loads the WriteableBitmap from the source URI of the given BitmapImage
        /// </summary>
        /// <param name="target">The target WriteableBitmap.</param>
        /// <param name="source">The source BitmapImage.</param>
        /// <param name="decodePixelWidth">Width to decode to.</param>
        /// <param name="decodePixelHeight">Height to decode to.</param>
        /// <returns></returns>
        public static async Task<WriteableBitmap> FromBitmapImage(
            this WriteableBitmap target,
            BitmapImage source,
            uint decodePixelWidth,
            uint decodePixelHeight)
        {
            string installedFolderImageSourceUri = source.UriSource.OriginalString.Replace("ms-appx:/", "");
            await target.LoadAsync(
                installedFolderImageSourceUri,
                decodePixelWidth,
                decodePixelHeight);
            return target;
        }
    }
}
