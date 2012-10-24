using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;

namespace WinRTXamlToolkit.Imaging
{
// ReSharper disable InconsistentNaming - This class extends IBuffer
    public static class IBufferExtensions
// ReSharper restore InconsistentNaming
    {
        public class PixelBufferInfo
        {
            private readonly Stream _pixelStream;

            public byte[] Bytes;
            //public int[] Pixels;
            public int this[int i]
            {
                get
                {
                    //return Pixels[i];
                    return ColorExtensions.IntColorFromBytes(
                        Bytes[i * 4 + 3],
                        Bytes[i * 4 + 2],
                        Bytes[i * 4 + 1],
                        Bytes[i * 4 + 0]);
                }
                set
                {
                    //Pixels[i] = value;
                    Bytes[i * 4 + 3] = (byte)((value >> 24) & 0xff);
                    Bytes[i * 4 + 2] = (byte)((value >> 16) & 0xff);
                    Bytes[i * 4 + 1] = (byte)((value >> 8) & 0xff);
                    Bytes[i * 4 + 0] = (byte)((value) & 0xff);
                    _pixelStream.Seek(i * 4, SeekOrigin.Begin);
                    _pixelStream.Write(Bytes, i * 4, 4);
                }
            }

            /// <summary>
            /// Returns the maximum difference between any of the R/G/B/A components
            /// of a color at given index and the one passed as parameter.
            /// </summary>
            /// <param name="i">Pixel index</param>
            /// <param name="color">Color to compare to</param>
            /// <returns>E.g. for 0x01010101 and 0x00010203 returns 0x01.</returns>
            public byte MaxDiff(int i, int color)
            {
                byte maxDiff = (byte)Math.Abs(Bytes[i * 4 + 3] - ((color >> 24) & 0xff));
                maxDiff = Math.Max(maxDiff, (byte)Math.Abs(Bytes[i * 4 + 2] - ((color >> 16) & 0xff)));
                maxDiff = Math.Max(maxDiff, (byte)Math.Abs(Bytes[i * 4 + 1] - ((color >> 8) & 0xff)));
                return Math.Max(maxDiff, (byte)Math.Abs(Bytes[i * 4 + 0] - ((color) & 0xff)));
            }

            public PixelBufferInfo(IBuffer pixelBuffer)
            {
                this._pixelStream = pixelBuffer.AsStream();
                this.Bytes = new byte[this._pixelStream.Length];
                this._pixelStream.Seek(0, SeekOrigin.Begin);
                this._pixelStream.Read(this.Bytes, 0, Bytes.Length);
                //this.Pixels = bytes.ToPixels();
            }

            public void UpdateFromBytes()
            {
                _pixelStream.Seek(0, SeekOrigin.Begin);
                _pixelStream.Write(Bytes, 0, Bytes.Length);
            }
        }

        public static PixelBufferInfo GetPixels(this IBuffer pixelBuffer)
        {
            return new PixelBufferInfo(pixelBuffer);
        }
    }
}
