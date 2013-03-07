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
        /// <summary>
        /// Reads a string from a text file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Writes a string to a text file.
        /// </summary>
        /// <param name="text">The text to write.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="options">
        /// The enum value that determines how responds if the fileName is the same
        /// as the name of an existing file in the current folder. Defaults to ReplaceExisting.
        /// </param>
        /// <returns></returns>
        public static async Task WriteToFile(
            this string text,
            string fileName,
            StorageFolder folder = null,
            CreationCollisionOption options = CreationCollisionOption.ReplaceExisting)
        {
            folder = folder ?? ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(
                fileName,
                options);
            using (var fs = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                using (var outStream = fs.GetOutputStreamAt(0))
                {
                    using (var dataWriter = new DataWriter(outStream))
                    {
                        if (text != null)
                            dataWriter.WriteString(text);

                        await dataWriter.StoreAsync();
                        dataWriter.DetachStream();
                    }

                    await outStream.FlushAsync();
                }
            }
        }
    }
}
