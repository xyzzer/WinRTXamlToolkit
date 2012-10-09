using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
    public static class WriteableBitmapBlitBlockExtensions
    {
        public static void BlitBlock(this WriteableBitmap target, int targetVerticalOffset, WriteableBitmap source, int sourceVerticalOffset, int verticalBlockHeight, bool autoInvalidate = false)
        {
            if (source.PixelWidth != target.PixelWidth)
            {
                throw new ArgumentException(
                    "BlitBlock only supports copying blocks of pixels between bitmaps of equal size.",
                    "source");
            }

            if (sourceVerticalOffset + verticalBlockHeight > source.PixelHeight ||
                targetVerticalOffset + verticalBlockHeight > target.PixelHeight ||
                verticalBlockHeight <= 0)
            {
                throw new ArgumentException();
            }

            var copiedBytes =
                new byte[4 * source.PixelWidth * verticalBlockHeight];
            var inputStream = source.PixelBuffer.AsStream();
            inputStream.Seek(4 * source.PixelWidth * sourceVerticalOffset, SeekOrigin.Begin);
            inputStream.Read(copiedBytes, 0, copiedBytes.Length);

            var outputStream = target.PixelBuffer.AsStream();
            outputStream.Seek(4 * target.PixelWidth * targetVerticalOffset, SeekOrigin.Begin);
            outputStream.Write(copiedBytes, 0, copiedBytes.Length);

            if (autoInvalidate)
            {
                target.Invalidate();
            }
        }
    }
}
