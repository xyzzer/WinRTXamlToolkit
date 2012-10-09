using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
    public static class WriteableBitmapSaveExtensions
    {
        public static async Task SaveToFile(
            this WriteableBitmap writeableBitmap)
        {
            await writeableBitmap.SaveToFile(
                KnownFolders.PicturesLibrary,
                string.Format(
                    "{0}_{1}.png",
                    DateTime.Now.ToString("yyyyMMdd_HHmmss_fff"),
                    Guid.NewGuid()));
        }

        public static async Task<StorageFile> SaveToFile(
            this WriteableBitmap writeableBitmap,
            StorageFolder storageFolder)
        {
            return await writeableBitmap.SaveToFile(
                storageFolder,
                string.Format(
                    "{0}_{1}.png",
                    DateTime.Now.ToString("yyyyMMdd_HHmmss_fff"),
                    Guid.NewGuid()));
        }

        public static async Task<StorageFile> SaveToFile(
            this WriteableBitmap writeableBitmap,
            StorageFolder storageFolder,
            string fileName)
        {
            StorageFile outputFile =
                await storageFolder.CreateFileAsync(
                    fileName,
                    CreationCollisionOption.ReplaceExisting);

            Guid encoderId;

            var ext = Path.GetExtension(fileName);

            if (new[] { ".bmp", ".dib" }.Contains(ext))
            {
                encoderId = BitmapEncoder.BmpEncoderId;
            }
            else if (new[] { ".tiff", ".tif" }.Contains(ext))
            {
                encoderId = BitmapEncoder.TiffEncoderId;
            }
            else if (new[] { ".gif" }.Contains(ext))
            {
                encoderId = BitmapEncoder.TiffEncoderId;
            }
            else if (new[] { ".jpg", ".jpeg", ".jpe", ".jfif", ".jif" }.Contains(ext))
            {
                encoderId = BitmapEncoder.TiffEncoderId;
            }
            else if (new[] { ".hdp", ".jxr", ".wdp" }.Contains(ext))
            {
                encoderId = BitmapEncoder.JpegXREncoderId;
            }
            else //if (new [] {".png"}.Contains(ext))
            {
                encoderId = BitmapEncoder.PngEncoderId;
            }

            await writeableBitmap.SaveToFile(outputFile, encoderId);

            return outputFile;
        }

        public static async Task SaveToFile(
            this WriteableBitmap writeableBitmap,
            StorageFile outputFile,
            Guid encoderId)
        {
            try
            {
                Stream stream = writeableBitmap.PixelBuffer.AsStream();
                byte[] pixels = new byte[(uint)stream.Length];
                await stream.ReadAsync(pixels, 0, pixels.Length);

                using (var writeStream = await outputFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(encoderId, writeStream);
                    encoder.SetPixelData(
                        BitmapPixelFormat.Bgra8,
                        BitmapAlphaMode.Premultiplied,
                        (uint)writeableBitmap.PixelWidth,
                        (uint)writeableBitmap.PixelHeight,
                        96,
                        96,
                        pixels);
                    await encoder.FlushAsync();

                    using (var outputStream = writeStream.GetOutputStreamAt(0))
                    {
                        await outputStream.FlushAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }
        }
    }
}