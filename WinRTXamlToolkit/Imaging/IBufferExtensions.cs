using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;

namespace WinRTXamlToolkit.Imaging
{
// ReSharper disable InconsistentNaming - This class extends IBuffer
    /// <summary>
    /// Contains extensions for an IBuffer interface in the context of a WriteableBitmap,
    /// which exposes one to access its pixels.
    /// </summary>
    public static class IBufferExtensions
// ReSharper restore InconsistentNaming
    {
        /// <summary>
        /// Gives access to the pixels of a WriteableBitmap given an IBuffer
        /// exposed by Pixels property.
        /// </summary>
        /// <remarks>
        /// Note that creating this object copies the pixels buffer
        /// into the Bytes byte array for quick pixel access
        /// and the array needs to be copied back to the pixels buffer
        /// to update the bitmap with a call to UpdateFromBytes().
        /// This is acceptable for convenience and possibly best for
        /// performance in some scenarios, but it does add some upfront
        /// overhead as well overhead to update the bitmap at the end.
        /// This is only a theory and for better performance it might be
        /// good to test different approaches.
        /// The goal of this approach is code simplicity. For best performance
        /// using native code and/or DirectX is recommended.
        /// </remarks>
        public class PixelBufferInfo
        {
            private readonly Stream _pixelStream;

            /// <summary>
            /// The bytes of the pixel stream.
            /// </summary>
            public byte[] Bytes;

            /// <summary>
            /// Gets or sets the <see cref="System.Int32" /> containing an ARGB format pixel
            /// at index i in the buffer.
            /// </summary>
            /// <value>
            /// The <see cref="System.Int32" /> containing an ARGB format pixel.
            /// </value>
            /// <param name="i">The buffer index.</param>
            /// <remarks>
            /// To access a pixel at position x,y you need to get pixels[wb.PixelWidth * y + x].
            /// </remarks>
            /// <returns></returns>
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

            /// <summary>
            /// Initializes a new instance of the <see cref="PixelBufferInfo" /> class.
            /// </summary>
            /// <param name="pixelBuffer">The pixel buffer returned by WriteableBitmap.PixelBuffer.</param>
            public PixelBufferInfo(IBuffer pixelBuffer)
            {
                _pixelStream = pixelBuffer.AsStream();
                this.Bytes = new byte[_pixelStream.Length];
                _pixelStream.Seek(0, SeekOrigin.Begin);
                _pixelStream.Read(this.Bytes, 0, Bytes.Length);
                //this.Pixels = bytes.ToPixels();
            }

            /// <summary>
            /// Updates the associated pixel buffer from bytes.
            /// </summary>
            public void UpdateFromBytes()
            {
                _pixelStream.Seek(0, SeekOrigin.Begin);
                _pixelStream.Write(Bytes, 0, Bytes.Length);
            }
        }

        /// <summary>
        /// Gets the pixels access wrapper for a PixelBuffer property of a WriteableBitmap.
        /// </summary>
        /// <param name="pixelBuffer">The pixel buffer.</param>
        /// <returns></returns>
        public static PixelBufferInfo GetPixels(this IBuffer pixelBuffer)
        {
            return new PixelBufferInfo(pixelBuffer);
        }
    }
}
