using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
    public static class WriteableBitmapCropExtensions
    {
        public static WriteableBitmap Crop(this WriteableBitmap source, int x1, int y1, int x2, int y2)
        {
            if (x1 >= x2 ||
                y1 >= y2 ||
                x1 < 0 ||
                y1 < 0 ||
                x2 < 0 ||
                y2 < 0 ||
                x1 > source.PixelWidth ||
                y1 > source.PixelHeight ||
                x2 > source.PixelWidth ||
                y2 > source.PixelHeight)
            {
                throw new ArgumentException();
            }

            //var buffer = source.PixelBuffer.GetPixels();
            var cw = x2 - x1;
            var ch = y2 - y1;
            var target = new WriteableBitmap(cw, ch);

            var croppedBytes =
                new byte[4 * cw * ch];
            var inputStream = source.PixelBuffer.AsStream();
            inputStream.Seek(4 * (source.PixelWidth * y1 + x1), SeekOrigin.Current);
            for (int i = 0; i < ch; i++)
            {
                inputStream.Read(croppedBytes, 4 * cw * i, 4 * cw);
                inputStream.Seek(4 * (source.PixelWidth - cw), SeekOrigin.Current);
            }

            var outputStream = target.PixelBuffer.AsStream();
            outputStream.Seek(0, SeekOrigin.Begin);
            outputStream.Write(croppedBytes, 0, croppedBytes.Length);
            target.Invalidate();

            return target;
        }
    }
}
