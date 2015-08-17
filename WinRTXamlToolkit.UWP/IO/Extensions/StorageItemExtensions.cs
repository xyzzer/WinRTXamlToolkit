using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace WinRTXamlToolkit.IO.Extensions
{
    /// <summary>
    /// Extension methods for IStorageItem (StorageFile and others).
    /// </summary>
    public static class StorageItemExtensions
    {
        #region GetFreeSpace()
        /// <summary>
        /// Returns the free space of the storage associate with the given storage item.
        /// </summary>
        /// <param name="sf">Storage item</param>
        /// <returns>Free space.</returns>
        public static async Task<UInt64> GetFreeSpace(this IStorageItem sf)
        {
            var properties = await sf.GetBasicPropertiesAsync();
            var filteredProperties = await properties.RetrievePropertiesAsync(new[] { "System.FreeSpace" });
            var freeSpace = filteredProperties["System.FreeSpace"];
            return (UInt64)freeSpace;
        } 
        #endregion

        #region GetSize()
        /// <summary>
        /// Gets the file size in bytes.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static async Task<ulong> GetSize(this IStorageItem file)
        {
            var props = await file.GetBasicPropertiesAsync();
            ulong sizeInB = props.Size;

            return sizeInB;
        } 
        #endregion

        #region GetSizeString()
        /// <summary>
        /// Gets the file size string given size in bytes.
        /// </summary>
        /// <param name="sizeInB">The size in B.</param>
        /// <returns></returns>
        public static string GetSizeString(this long sizeInB)
        {
            return GetSizeString((ulong)sizeInB);
        } 
        #endregion

        #region GetSizeString()
        /// <summary>
        /// Gets the file size string given size in bytes.
        /// </summary>
        /// <param name="sizeInB">The size in B.</param>
        /// <param name="promoteLimit">Specifies the number of units at which to promote to the next unit. Default is 1024. E.g. if the value is 1024 - for sizeInB of 1023 the result is 1023B, while for 1024 - 1kB.</param>
        /// <param name="decimalLimit">Defines the minimum number of units for which to produce results without decimal places. For lower values - results will include decimal places. Default is 10 which yields 9.8kB or 9.9kB, but yields "10kB" for both 10240B and 11263B.</param>
        /// <param name="separator">Separator between the value and the unit. Space by default. Typically should be either space or empty string.</param>
        /// <returns>The string that specifies a size in B/kB/MB/GB/TB depending on the size in B.</returns>
        public static string GetSizeString(this ulong sizeInB, double promoteLimit = 1024, double decimalLimit = 10, string separator = " ")
        {
            if (sizeInB < promoteLimit)
                return string.Format("{0}{1}B", sizeInB, separator);

            var sizeInKB = sizeInB / 1024.0;

            if (sizeInKB < decimalLimit)
                return string.Format("{0:F1}{1}KB", sizeInKB, separator);

            if (sizeInKB < promoteLimit)
                return string.Format("{0:F0}{1}KB", sizeInKB, separator);

            var sizeInMB = sizeInKB / 1024.0;

            if (sizeInMB < decimalLimit)
                return string.Format("{0:F1}{1}MB", sizeInMB, separator);

            if (sizeInMB < promoteLimit)
                return string.Format("{0:F0}{1}MB", sizeInMB, separator);

            var sizeInGB = sizeInMB / 1024.0;

            if (sizeInGB < decimalLimit)
                return string.Format("{0:F1}{1}GB", sizeInGB, separator);

            if (sizeInGB < promoteLimit)
                return string.Format("{0:F0}{1}GB", sizeInGB, separator);

            var sizeInTB = sizeInGB / 1024.0;

            if (sizeInTB < decimalLimit)
                return string.Format("{0:F1}{1}TB", sizeInTB, separator);

            return string.Format("{0:F0}{1}TB", sizeInTB, separator);
        } 
        #endregion
    }
}
