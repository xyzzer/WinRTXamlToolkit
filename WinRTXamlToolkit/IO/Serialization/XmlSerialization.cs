using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WinRTXamlToolkit.IO.Extensions;
using Windows.Storage;

namespace WinRTXamlToolkit.IO.Serialization
{
    /// <summary>
    /// XML serialization helpers
    /// </summary>
    public static class XmlSerialization
    {
        /// <summary>
        /// Serializes the object graph as XML using DataContractSerializer and writes it to a file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectGraph">The object graph.</param>
        /// <param name="fileName">Name of the file to write to.</param>
        /// <param name="folder">The folder to put the file in.</param>
        /// <param name="options">
        /// The enum value that determines how responds if the fileName is the same
        /// as the name of an existing file in the current folder. Defaults to FailIfExists.
        /// </param>
        /// <returns></returns>
        public async static Task SerializeAsXmlDataContract<T>(
            this T objectGraph,
            string fileName,
            StorageFolder folder = null,
            CreationCollisionOption options = CreationCollisionOption.FailIfExists)
        {
            folder = folder ?? ApplicationData.Current.LocalFolder;

            try
            {
                var file = await folder.CreateFileAsync(fileName, options);

                using (var stream = await file.OpenStreamForWriteAsync())
                {
                    var ser = new DataContractSerializer(typeof(T));
                    ser.WriteObject(stream, objectGraph);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());

                if (Debugger.IsAttached)
                    Debugger.Break();

                throw;
            }
        }

        /// <summary>
        /// Serializes the object graph as XML using DataContractSerializer.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectGraph">The object graph.</param>
        /// <returns></returns>
        public static string SerializeAsXmlDataContract<T>(this T objectGraph)
        {
            var ser = new DataContractSerializer(typeof(T));
            var ms = new MemoryStream();
            ser.WriteObject(ms, objectGraph);
            var bytes = ms.ToArray();
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Loads object graph from XML file serialized using DataContractSerializer.
        /// </summary>
        /// <typeparam name="T">The type of the returned object graph reference.</typeparam>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folder">The folder.</param>
        /// <returns>Object graph reference.</returns>
        public static async Task<T> LoadFromXmlDataContractFile<T>(
            string fileName,
            StorageFolder folder = null)
        {
            var xmlString = await StringIOExtensions.ReadFromFile(fileName, folder);
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));
            var ser = new DataContractSerializer(typeof(T));
            T result = (T)ser.ReadObject(ms);
            return result;
        }

        /// <summary>
        /// Serializes the object graph as XML using XmlSerializer and writes it to a file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectGraph">The object graph.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folder">The folder.</param>
        /// <param name="options">
        /// The enum value that determines how responds if the fileName is the same
        /// as the name of an existing file in the current folder. Defaults to FailIfExists.
        /// </param>
        /// <returns></returns>
        public async static Task SerializeAsXml<T>(
            this T objectGraph,
            string fileName,
            StorageFolder folder = null,
            CreationCollisionOption options = CreationCollisionOption.FailIfExists)
        {
            folder = folder ?? ApplicationData.Current.LocalFolder;

            try
            {
                var file = await folder.CreateFileAsync(fileName, options);

                using (var stream = await file.OpenStreamForWriteAsync())
                {
                    var ser = new XmlSerializer(typeof(T));
                    ser.Serialize(stream, objectGraph);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());

                if (Debugger.IsAttached)
                    Debugger.Break();

                throw;
            }
        }

        /// <summary>
        /// Serializes the object graph as XML using XmlSerializer.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectGraph">The object graph.</param>
        /// <returns></returns>
        public static string SerializeAsXml<T>(this T objectGraph)
        {
            var ser = new XmlSerializer(typeof(T));
            var ms = new MemoryStream();
            ser.Serialize(ms, objectGraph);
            var bytes = ms.ToArray();
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Loads an object graph from XML file serialized using XmlSerializer.
        /// </summary>
        /// <typeparam name="T">Type of the returned object graph reference.</typeparam>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folder">The folder.</param>
        /// <returns>Object graph reference.</returns>
        public static async Task<T> LoadFromXmlFile<T>(
            string fileName,
            StorageFolder folder = null)
        {
            var xmlString = await StringIOExtensions.ReadFromFile(fileName, folder);
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));
            var ser = new XmlSerializer(typeof(T));
            T result = (T)ser.Deserialize(ms);
            return result;
        }
    }
}
