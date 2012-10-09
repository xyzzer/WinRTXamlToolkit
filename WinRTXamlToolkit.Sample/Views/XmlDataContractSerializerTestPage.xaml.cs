using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using WinRTXamlToolkit.IO.Extensions;
using WinRTXamlToolkit.IO.Serialization;
using Windows.Storage;
using Windows.UI.Xaml;
using WinRTXamlToolkit.IO;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class XmlDataContractSerializerTestPage
    {
        public XmlDataContractSerializerTestPage()
        {
            this.InitializeComponent();
            RunTest();
        }

        private async void RunTest()
        {
            await RunTestOnTempFile();
            RunTestInMemory();
        }

        private void RunTestInMemory()
        {
            var data = new SampleSerializableData();
            this.serializedDataTextBox.Text = data.SerializeAsXmlDataContract();
            this.classDefinitionTextBox.Text =
                @"    [DataContract(Namespace = """", Name = ""RootElement"")]
    public class SampleSerializableData
    {
        [DataMember(Name = ""w"")]
        public int Width { get; set; }

        [DataMember(Name = ""h"")]
        public int Height { get; set; }

        [DataMember(Name = ""Items"")]
        public List<SampleSerializableDataItem> Items { get; set; }

        public SampleSerializableData()
        {
            Width = 1024;
            Height = 1024;

            Items = new List<SampleSerializableDataItem>();
            Items.Add(new SampleSerializableDataItem
            {
                Width = 512,
                Height = 512,
                X = 0,
                Y = 0
            });
            Items.Add(new SampleSerializableDataItem
            {
                Width = 512,
                Height = 512,
                X = 512,
                Y = 0
            });
            Items.Add(new SampleSerializableDataItem
            {
                Width = 512,
                Height = 512,
                X = 0,
                Y = 512
            });
            Items.Add(new SampleSerializableDataItem
            {
                Width = 512,
                Height = 512,
                X = 512,
                Y = 512
            });
        }
    }

    [DataContract(Namespace = """", Name = ""ItemElement"")]
    public class SampleSerializableDataItem
    {
        [DataMember(Name = ""x"")]
        public int X { get; set; }

        [DataMember(Name = ""y"")]
        public int Y { get; set; }

        [DataMember(Name = ""w"")]
        public int Width { get; set; }

        [DataMember(Name = ""h"")]
        public int Height { get; set; }
    }";
        }

        private static async Task RunTestOnTempFile()
        {
            var data = new SampleSerializableData {Width = 12345};
            var folder = ApplicationData.Current.TemporaryFolder;
            string fileName = await folder.CreateTempFileNameAsync(".xml");
            await data.SerializeAsXmlDataContract(
                fileName,
                folder);
            var deserializedData = await XmlSerialization.LoadFromXmlDataContractFile<SampleSerializableData>(
                fileName,
                folder);

            Debug.Assert(deserializedData.Width == data.Width);
            var file = await folder.GetFileAsync(fileName);
            await file.DeleteAsync();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }

    [DataContract(Namespace = "", Name = "RootElement")]
    public class SampleSerializableData
    {
        [DataMember(Name = "w")]
        public int Width { get; set; }

        [DataMember(Name = "h")]
        public int Height { get; set; }

        [DataMember(Name = "Items")]
        public List<SampleSerializableDataItem> Items { get; set; }

        public SampleSerializableData()
        {
            Width = 1024;
            Height = 1024;

            Items = new List<SampleSerializableDataItem>
            {
                new SampleSerializableDataItem
                    {
                        Width = 512,
                        Height = 512,
                        X = 0,
                        Y = 0
                    },
                new SampleSerializableDataItem
                    {
                        Width = 512,
                        Height = 512,
                        X = 512,
                        Y = 0
                    },
                new SampleSerializableDataItem
                    {
                        Width = 512,
                        Height = 512,
                        X = 0,
                        Y = 512
                    },
                new SampleSerializableDataItem
                    {
                        Width = 512,
                        Height = 512,
                        X = 512,
                        Y = 512
                    }
            };
        }
    }

    [DataContract(Namespace = "", Name = "ItemElement")]
    public class SampleSerializableDataItem
    {
        [DataMember(Name = "x")]
        public int X { get; set; }

        [DataMember(Name = "y")]
        public int Y { get; set; }

        [DataMember(Name = "w")]
        public int Width { get; set; }

        [DataMember(Name = "h")]
        public int Height { get; set; }
    }
}
