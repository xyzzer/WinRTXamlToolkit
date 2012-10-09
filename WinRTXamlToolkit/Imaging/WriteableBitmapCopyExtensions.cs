using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
    public static class WriteableBitmapExtensions
    {
        public static WriteableBitmap Copy(this WriteableBitmap source)
        {
            var copiedBytes =
                new byte[4 * source.PixelWidth * source.PixelHeight];
            var inputStream = source.PixelBuffer.AsStream();
            inputStream.Seek(0, SeekOrigin.Begin);
            inputStream.Read(copiedBytes, 0, copiedBytes.Length);

            var target = new WriteableBitmap(source.PixelWidth, source.PixelHeight);
            var outputStream = target.PixelBuffer.AsStream();
            outputStream.Seek(0, SeekOrigin.Begin);
            outputStream.Write(copiedBytes, 0, copiedBytes.Length);

            return target;
        }
    }
}
