using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using WinRTXamlToolkit.IO.Extensions;
using Windows.Storage;

namespace WinRTXamlToolkit.IO.Serialization
{
    public static class JsonSerialization
    {
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
