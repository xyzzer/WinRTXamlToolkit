using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.Storage;

namespace WinRTXamlToolkit.IO
{
    /// <summary>
    /// Contains a helper method for getting a file with the applicable scale qualifier.
    /// </summary>
    public static class ScaledImageFile
    {
        /// <summary>
        /// Used to retrieve a StorageFile that uses qualifiers in the naming convention.
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static async Task<StorageFile> Get(string relativePath)
        {
            string resourceKey = string.Format("Files/{0}", relativePath);
            var mainResourceMap = ResourceManager.Current.MainResourceMap;

            if (!mainResourceMap.ContainsKey(resourceKey))
                return null;
            return await mainResourceMap[resourceKey].Resolve(ResourceContext.GetForCurrentView()).GetValueAsFileAsync();
        }
    }
}
