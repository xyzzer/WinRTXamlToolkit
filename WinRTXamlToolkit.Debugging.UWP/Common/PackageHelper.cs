using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Storage;

namespace WinRTXamlToolkit.Debugging.Common
{
    /// <summary>
    /// Implements helper methods for getting properties of the application package.
    /// </summary>
    public static class PackageHelper
    {
        /// <summary>
        /// Gets the list of package assemblies asynchronously.
        /// </summary>
        /// <remarks>
        /// The side effect of this method is that it tries to load all the assemblies found in the package.
        /// For that reason it should be used cautiously - mostly in debugging and exploration scenarios and should not be used at runtime.
        /// </remarks>
        /// <returns>The list of package assemblies.</returns>
        public static async Task<List<Assembly>> GetPackageAssembliesAsync()
        {
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;

            var list = new List<Assembly>();

            foreach (StorageFile file in (await folder.GetFilesAsync()))
            {
                if (file.FileType == ".dll" ||
                    file.FileType == ".exe")
                {
                    try
                    {
                        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.Name);
                        var assemblyName = new AssemblyName { Name = fileNameWithoutExtension };
                        var assembly = Assembly.Load(assemblyName);
                        list.Add(assembly);
                    }
// ReSharper disable EmptyGeneralCatchClause
                    catch
// ReSharper restore EmptyGeneralCatchClause
                    {
                    }
                }
            }

            return list;
        }
    }
}
