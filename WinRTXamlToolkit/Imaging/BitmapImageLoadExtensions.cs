using System;
using System.Threading.Tasks;
using WinRTXamlToolkit.IO.Extensions;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
    public static class BitmapImageLoadExtensions
    {
        public static async Task<BitmapImage> LoadAsync(StorageFile file)
        {
            BitmapImage bitmap = new BitmapImage();
            return await bitmap.SetSourceAsync(file);
        }

        public static async Task<BitmapImage> LoadAsync(StorageFolder folder, string fileName)
        {
            BitmapImage bitmap = new BitmapImage();

            if (await folder.ContainsFileAsync(fileName))
            {
                var file = await folder.GetFileByPathAsync(fileName);
                return await bitmap.SetSourceAsync(file);
            }
            else
                return null;
        }

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
