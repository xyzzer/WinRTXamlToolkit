using System;
using System.IO;
using System.Threading.Tasks;
using WinRTXamlToolkit.AwaitableUI;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.IO;

namespace WinRTXamlToolkit.Imaging
{
    /// <summary>
    /// Contains extension and helper methods for loading WriteableBitmaps.
    /// </summary>
    public static class WriteableBitmapLoadExtensions
    {
        /// <summary>
        /// Loads the WriteableBitmap asynchronously given the path relative to the installation folder.
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        /// <returns></returns>
        public static async Task<WriteableBitmap> LoadAsync(
            string relativePath)
        {
            return await new WriteableBitmap(1, 1).LoadAsync(relativePath);
        }

        /// <summary>
        /// Loads the WriteableBitmap asynchronously given the path relative to the installation folder and the dimensions.
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        /// <param name="decodePixelWidth">Width in pixels of the decoded bitmap.</param>
        /// <param name="decodePixelHeight">Height in pixels of the decoded bitmap.</param>
        /// <returns></returns>
        public static async Task<WriteableBitmap> LoadAsync(
            string relativePath,
            uint decodePixelWidth,
            uint decodePixelHeight)
        {
            return await new WriteableBitmap(1, 1).LoadAsync(relativePath, decodePixelWidth, decodePixelHeight);
        }

        /// <summary>
        /// Loads the WriteableBitmap asynchronously given the storage file.
        /// </summary>
        /// <param name="storageFile">The storage file.</param>
        /// <returns></returns>
        public static async Task<WriteableBitmap> LoadAsync(
            StorageFile storageFile)
        {
            return await new WriteableBitmap(1, 1).LoadAsync(storageFile);
        }

        /// <summary>
        /// Loads the WriteableBitmap asynchronously given the storage file and the dimensions.
        /// </summary>
        /// <param name="storageFile">The storage file.</param>
        /// <param name="decodePixelWidth">Width in pixels of the decoded bitmap.</param>
        /// <param name="decodePixelHeight">Height in pixels of the decoded bitmap.</param>
        /// <returns></returns>
        public static async Task<WriteableBitmap> LoadAsync(
            StorageFile storageFile,
            uint decodePixelWidth,
            uint decodePixelHeight)
        {
            return await new WriteableBitmap(1, 1).LoadAsync(storageFile, decodePixelWidth, decodePixelHeight);
        }

        /// <summary>
        /// Loads the WriteableBitmap asynchronously given the storage file and the dimensions.
        /// </summary>
        /// <param name="writeableBitmap">The writeable bitmap.</param>
        /// <param name="relativePath">The relative path.</param>
        /// <returns></returns>
        public static async Task<WriteableBitmap> LoadAsync(
            this WriteableBitmap writeableBitmap,
            string relativePath)
        {
            var resolvedFile = await ScaledImageFile.Get(relativePath);

            if (resolvedFile == null)
                throw new FileNotFoundException("Could not load image.", relativePath);

            return await writeableBitmap.LoadAsync(resolvedFile);
        }

        /// <summary>
        /// Loads the WriteableBitmap asynchronously given the storage file.
        /// </summary>
        /// <param name="writeableBitmap">The writeable bitmap.</param>
        /// <param name="storageFile">The storage file.</param>
        /// <returns></returns>
        public static async Task<WriteableBitmap> LoadAsync(
            this WriteableBitmap writeableBitmap,
            StorageFile storageFile)
        {
            var wb = writeableBitmap;

            using (var stream = await storageFile.OpenReadAsync())
            {
                await wb.SetSourceAsync(stream);
            }

            //await wb.WaitForLoadedAsync();

            return wb;
        }

        /// <summary>
        /// Loads the WriteableBitmap asynchronously given the storage file and the dimensions.
        /// </summary>
        /// <param name="writeableBitmap">The writeable bitmap.</param>
        /// <param name="storageFile">The storage file.</param>
        /// <param name="decodePixelWidth">Width in pixels of the decoded bitmap.</param>
        /// <param name="decodePixelHeight">Height in pixels of the decoded bitmap.</param>
        /// <returns></returns>
        public static async Task<WriteableBitmap> LoadAsync(
            this WriteableBitmap writeableBitmap,
            StorageFile storageFile,
            uint decodePixelWidth,
            uint decodePixelHeight)
        {
            using (var stream = await storageFile.OpenReadAsync())
            {
                await writeableBitmap.SetSourceAsync(
                    stream,
                    decodePixelWidth,
                    decodePixelHeight);
            }

            return writeableBitmap;
        }

        /// <summary>
        /// Loads the WriteableBitmap asynchronously given the file path relative to install location and the dimensions.
        /// </summary>
        /// <param name="writeableBitmap">The writeable bitmap.</param>
        /// <param name="relativePath">The relative path.</param>
        /// <param name="decodePixelWidth">Width in pixels of the decoded bitmap.</param>
        /// <param name="decodePixelHeight">Height in pixels of the decoded bitmap.</param>
        /// <returns></returns>
        public static async Task<WriteableBitmap> LoadAsync(
            this WriteableBitmap writeableBitmap,
            string relativePath,
            uint decodePixelWidth,
            uint decodePixelHeight)
        {
            var resolvedFile = await ScaledImageFile.Get(relativePath);

            return await writeableBitmap.LoadAsync(
                resolvedFile,
                decodePixelWidth,
                decodePixelHeight);
        }

        // The Tim Heuer method (see https://twitter.com/timheuer/status/217521386720198656)
        /// <summary>
        /// Sets the WriteableBitmap source asynchronously given a stream and dimensions.
        /// </summary>
        /// <param name="writeableBitmap">The writeable bitmap.</param>
        /// <param name="streamSource">The stream source.</param>
        /// <param name="decodePixelWidth">Width in pixels of the decoded bitmap.</param>
        /// <param name="decodePixelHeight">Height in pixels of the decoded bitmap.</param>
        /// <returns></returns>
        public static async Task SetSourceAsync(
            this WriteableBitmap writeableBitmap,
            IRandomAccessStream streamSource,
            uint decodePixelWidth,
            uint decodePixelHeight)
        {
            var decoder = await BitmapDecoder.CreateAsync(streamSource);

            using (var inMemoryStream = new InMemoryRandomAccessStream())
            {
                var encoder = await BitmapEncoder.CreateForTranscodingAsync(inMemoryStream, decoder);
                encoder.BitmapTransform.ScaledWidth = decodePixelWidth;
                encoder.BitmapTransform.ScaledHeight = decodePixelHeight;
                await encoder.FlushAsync();
                inMemoryStream.Seek(0);

                await writeableBitmap.SetSourceAsync(inMemoryStream);
            }
        }
    }
}
