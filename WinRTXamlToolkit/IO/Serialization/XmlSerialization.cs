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
    public static class XmlSerialization
    {
        public async static Task SerializeAsXmlDataContract<T>(
            this T objectGraph,
            string fileName,
            StorageFolder folder = null,
            bool overwriteIfNull = true)
        {
            string xmlString = objectGraph.SerializeAsXmlDataContract();

            if (xmlString != null || overwriteIfNull)
            {
                await xmlString.WriteToFile(fileName, folder);
            }
        }

        public static string SerializeAsXmlDataContract<T>(this T graph)
        {
            var ser = new DataContractSerializer(typeof(T));
            var ms = new MemoryStream();
            ser.WriteObject(ms, graph);
            var bytes = ms.ToArray();
            return UTF8Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

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

        public async static Task SerializeAsXml<T>(
            this T objectGraph,
            string fileName,
            StorageFolder folder = null,
            bool overwriteIfNull = true)
        {
            string xmlString;

            try
            {
                xmlString = objectGraph.SerializeAsXml();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());

                if (Debugger.IsAttached)
                    Debugger.Break();

                throw;
            }

            if (xmlString != null || overwriteIfNull)
            {
                await xmlString.WriteToFile(fileName, folder);
            }
        }

        public static string SerializeAsXml<T>(this T graph)
        {
            var ser = new XmlSerializer(typeof(T));
            var ms = new MemoryStream();
            ser.Serialize(ms, graph);
            var bytes = ms.ToArray();
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

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
