using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.IO;

namespace WinRTXamlToolkit.Imaging
{
    public static class WriteableBitmapLoadExtensions
    {
        public static async Task<WriteableBitmap> LoadAsync(
            string relativePath)
        {
            return await new WriteableBitmap(1, 1).LoadAsync(relativePath);
        }

        public static async Task<WriteableBitmap> LoadAsync(
            string relativePath,
            uint decodePixelWidth,
            uint decodePixelHeight)
        {
            return await new WriteableBitmap(1, 1).LoadAsync(relativePath, decodePixelWidth, decodePixelHeight);
        }

        public static async Task<WriteableBitmap> LoadAsync(
            StorageFile file)
        {
            return await new WriteableBitmap(1, 1).LoadAsync(file);
        }

        public static async Task<WriteableBitmap> LoadAsync(
            StorageFile file,
            uint decodePixelWidth,
            uint decodePixelHeight)
        {
            return await new WriteableBitmap(1, 1).LoadAsync(file, decodePixelWidth, decodePixelHeight);
        }

        public static async Task<WriteableBitmap> LoadAsync(
            this WriteableBitmap writeableBitmap,
            string relativePath)
        {
            var resolvedFile = await ScaledImageFile.Get(relativePath);

            return await writeableBitmap.LoadAsync(resolvedFile);
        }

        public static async Task<WriteableBitmap> LoadAsync(
            this WriteableBitmap writeableBitmap,
            StorageFile storageFile)
        {
            var wb = writeableBitmap;

            using (var stream = await storageFile.OpenReadAsync())
            {
                await wb.SetSourceAsync(stream);
            }

            return wb;
        }

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
        public static async Task SetSourceAsync(
            this WriteableBitmap writeableBitmap,
            IRandomAccessStream streamSource,
            uint decodePixelWidth,
            uint decodePixelHeight)
        {
            var decoder = await BitmapDecoder.CreateAsync(streamSource);
            var inMemoryStream = new InMemoryRandomAccessStream();
            var encoder = await BitmapEncoder.CreateForTranscodingAsync(inMemoryStream, decoder);
            encoder.BitmapTransform.ScaledWidth = decodePixelWidth;
            encoder.BitmapTransform.ScaledHeight = decodePixelHeight;
            await encoder.FlushAsync();
            inMemoryStream.Seek(0);

            await writeableBitmap.SetSourceAsync(inMemoryStream);
        }
    }
}
