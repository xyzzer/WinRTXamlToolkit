using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources.Core;
using Windows.Storage;

namespace WinRTXamlToolkit.IO
{
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
            var mainResourceMap = ResourceManager.Current.MainResourceMap ;

            if (!mainResourceMap.ContainsKey(resourceKey))
                return null;

            return await mainResourceMap[resourceKey].Resolve().GetValueAsFileAsync();
        }
    }
}
