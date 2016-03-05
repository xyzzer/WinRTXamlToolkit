using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
    /// <summary>
    /// Extension methods for WriteableBitmap to copy pixels from a source bitmap.
    /// </summary>
    public static class WriteableBitmapCopyExtensions
    {
        /// <summary>
        /// Copies the specified source bitmap into a new bitmap.
        /// </summary>
        /// <param name="source">The source bitmap.</param>
        /// <returns></returns>
        public static WriteableBitmap Copy(this WriteableBitmap source)
        {
            source.Invalidate();
            var copiedBytes =
                new byte[4 * source.PixelWidth * source.PixelHeight];
            var inputStream = source.PixelBuffer.AsStream();
            inputStream.Seek(0, SeekOrigin.Begin);
            inputStream.Read(copiedBytes, 0, copiedBytes.Length);

            var target = new WriteableBitmap(source.PixelWidth, source.PixelHeight);
            var outputStream = target.PixelBuffer.AsStream();
            outputStream.Seek(0, SeekOrigin.Begin);
            outputStream.Write(copiedBytes, 0, copiedBytes.Length);
            target.Invalidate();
            return target;
        }
    }
}
