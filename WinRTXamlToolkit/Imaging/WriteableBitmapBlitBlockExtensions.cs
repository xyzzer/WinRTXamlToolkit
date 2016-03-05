using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
    /// <summary>
    /// WriteableBitmap extensions for blitting.
    /// </summary>
    public static class WriteableBitmapBlitBlockExtensions
    {
        /// <summary>
        /// Blits a vertical block of a source bitmap to a target bitmap of same width.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="targetVerticalOffset">The target vertical offset.</param>
        /// <param name="source">The source.</param>
        /// <param name="sourceVerticalOffset">The source vertical offset.</param>
        /// <param name="verticalBlockHeight">Height of the vertical block.</param>
        /// <param name="autoInvalidate">if set to <c>true</c> the bitmap will auto invalidate.</param>
        /// <exception cref="System.ArgumentException">BlitBlock only supports copying blocks of pixels between bitmaps of equal size.;source</exception>
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
