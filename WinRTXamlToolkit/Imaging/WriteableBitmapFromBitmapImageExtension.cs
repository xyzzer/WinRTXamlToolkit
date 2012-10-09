using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
    public static class WriteableBitmapFromBitmapImageExtension
    {
        public static async Task<WriteableBitmap> FromBitmapImage(BitmapImage source)
        {
            var ret = new WriteableBitmap(1, 1);
            return await FromBitmapImage(ret, source);
        }

        public static async Task<WriteableBitmap> FromBitmapImage(this WriteableBitmap target, BitmapImage source)
        {
            string installedFolderImageSourceUri = source.UriSource.OriginalString.Replace("ms-appx:/", "");
            await target.LoadAsync(installedFolderImageSourceUri);
            return target;
        }

        public static async Task<WriteableBitmap> FromBitmapImage(
            BitmapImage source,
            uint decodePixelWidth,
            uint decodePixelHeight)
        {
            var ret = new WriteableBitmap(1, 1);
            return await FromBitmapImage(ret, source, decodePixelWidth, decodePixelHeight);
        }

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
