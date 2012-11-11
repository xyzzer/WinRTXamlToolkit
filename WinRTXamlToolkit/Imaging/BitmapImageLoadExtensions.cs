using System;
using System.Threading.Tasks;
using WinRTXamlToolkit.IO.Extensions;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
    /// <summary>
    /// Extension and helper methods for loading a BitmapImage.
    /// </summary>
    public static class BitmapImageLoadExtensions
    {
        /// <summary>
        /// Loads a BitmapImage asynchronously given a specific file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static async Task<BitmapImage> LoadAsync(StorageFile file)
        {
            BitmapImage bitmap = new BitmapImage();
            return await bitmap.SetSourceAsync(file);
        }

        /// <summary>
        /// Loads a BitmapImage asynchronously given a specific folder and file name.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static async Task<BitmapImage> LoadAsync(StorageFolder folder, string fileName)
        {
            BitmapImage bitmap = new BitmapImage();

            if (await folder.ContainsFileAsync(fileName))
            {
                var file = await folder.GetFileByPathAsync(fileName);
                return await bitmap.SetSourceAsync(file);
            }
            
            return null;
        }

        /// <summary>
        /// Sets the source image for a BitmapImage by opening
        /// a given file and processing the result asynchronously.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static async Task<BitmapImage> SetSourceAsync(this BitmapImage bitmap, StorageFile file)
        {
            using (var stream = await file.OpenReadAsync())
            {
                await bitmap.SetSourceAsync(stream);
            }

            return bitmap;
        }
    }
}
