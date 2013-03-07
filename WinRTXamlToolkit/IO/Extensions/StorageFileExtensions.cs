using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace WinRTXamlToolkit.IO.Extensions
{
    /// <summary>
    /// Extension methods for StorageFile.
    /// </summary>
    public static class StorageFileExtensions
    {
        /// <summary>
        /// Gets the file size in bytes.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static async Task<ulong> GetSize(this StorageFile file)
        {
            var props = await file.GetBasicPropertiesAsync();
            ulong sizeInB = props.Size;

            return sizeInB;
        }

        /// <summary>
        /// Gets the file size string given size in bytes.
        /// </summary>
        /// <param name="sizeInB">The size in B.</param>
        /// <returns></returns>
        public static string GetSizeString(this long sizeInB)
        {
            return GetSizeString((ulong)sizeInB);
        }

        /// <summary>
        /// Gets the file size string given size in bytes.
        /// </summary>
        /// <param name="sizeInB">The size in B.</param>
        /// <returns></returns>
        public static string GetSizeString(this ulong sizeInB)
        {
            if (sizeInB < 128)
                return string.Format("{0}B", sizeInB);

            var sizeInKB = sizeInB / 1024.0;

            if (sizeInKB < 10)
                return string.Format("{0:F1}KB", sizeInKB);

            if (sizeInKB < 128)
                return string.Format("{0:F0}KB", sizeInKB);

            var sizeInMB = sizeInKB / 1024.0;

            if (sizeInMB < 10)
                return string.Format("{0:F1}MB", sizeInMB);

            if (sizeInMB < 128)
                return string.Format("{0:F0}MB", sizeInMB);

            var sizeInGB = sizeInMB / 1024.0;

            if (sizeInGB < 10)
                return string.Format("{0:F1}GB", sizeInGB);

            if (sizeInGB < 128)
                return string.Format("{0:F0}GB", sizeInGB);

            var sizeInTB = sizeInGB / 1024.0;

            if (sizeInTB < 10)
                return string.Format("{0:F1}TB", sizeInTB);

            return string.Format("{0:F0}TB", sizeInTB);
        }
    }
}
