using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using WinRTXamlToolkit.IO.Extensions;
using Windows.Storage;

namespace WinRTXamlToolkit.IO.Serialization
{
    /// <summary>
    /// Extension and helper methods for serializing an object graph
    /// to JSON or loading one from a JSON file.
    /// </summary>
    public static class JsonSerialization
    {
        /// <summary>
        /// Serializes the object graph as JSON and saves.
        /// </summary>
        /// <typeparam name="T">The type of the object graph reference.</typeparam>
        /// <param name="objectGraph">The object graph.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folder">The folder to save to.</param>
        /// <param name="overwriteIfNull">
        /// if set to <c>true</c> - the method will
        /// overwrite the old file if the serialized value is a null reference.
        /// </param>
        /// <returns></returns>
        public async static Task SerializeAsJson<T>(
            this T objectGraph,
            string fileName,
            StorageFolder folder = null,
            bool overwriteIfNull = true)
        {
            string json = null;

            if (objectGraph != null)
                json = objectGraph.SerializeAsJson();

            if (json != null || overwriteIfNull)
            {
                await json.WriteToFile(fileName, folder);
            }
        }

        /// <summary>
        /// Serializes the object graph as json.
        /// </summary>
        /// <typeparam name="T">The type of the object graph reference.</typeparam>
        /// <param name="graph">The object graph.</param>
        /// <returns></returns>
        public static string SerializeAsJson<T>(this T graph)
        {
            if (graph == null)
                return null;

            var ser = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream();
            ser.WriteObject(ms, graph);
            var bytes = ms.ToArray();
            return UTF8Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Loads an object graph from a JSON file.
        /// </summary>
        /// <typeparam name="T">The type of the expected object graph reference.</typeparam>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
        public static async Task<T> LoadFromJsonFile<T>(
            string fileName,
            StorageFolder folder = null)
        {
            var json = await StringIOExtensions.ReadFromFile(fileName, folder);
            var ms = new MemoryStream(UTF8Encoding.UTF8.GetBytes(json));
            var ser = new DataContractJsonSerializer(typeof(T));
            T result = (T)ser.ReadObject(ms);
            return result;
        }
    }
}
