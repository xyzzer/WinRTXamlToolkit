using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Storage;

namespace WinRTXamlToolkit.Tools
{
    public static class PackageHelper
    {
        public static async Task<List<Assembly>> GetPackageAssembliesAsync()
        {
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;

            List<Assembly> list = new List<Assembly>();

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
                    catch (Exception ex)
                    {
                        
                    }
                }
            }

            return list;
        }
    }
}
