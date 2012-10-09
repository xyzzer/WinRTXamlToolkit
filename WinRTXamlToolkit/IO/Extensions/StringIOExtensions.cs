using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace WinRTXamlToolkit.IO.Extensions
{
    /// <summary>
    /// Extensions for simple writing and reading of strings to/from files.
    /// </summary>
    /// <remarks>
    /// Note that these were created before FileIO class existed in WinRT, but they still serve a purpose.
    /// </remarks>
    public static class StringIOExtensions
    {
        public static async Task<string> ReadFromFile(
            string fileName,
            StorageFolder folder = null)
        {
            folder = folder ?? ApplicationData.Current.LocalFolder;
            var file = await folder.GetFileAsync(fileName);

            using (var fs = await file.OpenAsync(FileAccessMode.Read))
            {
                using (var inStream = fs.GetInputStreamAt(0))
                {
                    using (var reader = new DataReader(inStream))
                    {
                        await reader.LoadAsync((uint)fs.Size);
                        string data = reader.ReadString((uint)fs.Size);
                        reader.DetachStream();
                        return data;
                    }
                }
            }
        }

        public static async Task WriteToFile(
            this string contents,
            string fileName,
            StorageFolder folder = null)
        {
            folder = folder ?? ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(
                fileName,
                CreationCollisionOption.ReplaceExisting);
            using (var fs = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (var outStream = fs.GetOutputStreamAt(0))
                {
                    using (var dataWriter = new DataWriter(outStream))
                    {
                        if (contents != null)
                            dataWriter.WriteString(contents);

                        await dataWriter.StoreAsync();
                        dataWriter.DetachStream();
                    }

                    await outStream.FlushAsync();
                }
            }
        }
    }
}
