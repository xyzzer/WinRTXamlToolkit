using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WinRTXamlToolkit.IO.Extensions;
using WinRTXamlToolkit.IO.Serialization;
using Windows.Storage;
using Windows.UI.Xaml;
using WinRTXamlToolkit.IO;
using System.Xml.Serialization;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class XmlSerializerTestPage
    {
        public XmlSerializerTestPage()
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
            var data = new SampleXmlSerializableData();
            this.serializedDataTextBox.Text = data.SerializeAsXml();
            this.classDefinitionTextBox.Text =
                @"    [XmlRoot(ElementName = ""RootElement"")]
    public class SampleXmlSerializableData : List<SampleXmlSerializableDataItem>
    {
        [XmlAttribute(AttributeName = ""w"")]
        public int Width { get; set; }

        [XmlAttribute(AttributeName = ""h"")]
        public int Height { get; set; }

        public SampleXmlSerializableData()
        {
            Width = 1024;
            Height = 1024;

            this.Add(new SampleXmlSerializableDataItem
            {
                Width = 512,
                Height = 512,
                X = 0,
                Y = 0
            });
            this.Add(new SampleXmlSerializableDataItem
            {
                Width = 512,
                Height = 512,
                X = 512,
                Y = 0
            });
            this.Add(new SampleXmlSerializableDataItem
            {
                Width = 512,
                Height = 512,
                X = 0,
                Y = 512
            });
            this.Add(new SampleXmlSerializableDataItem
            {
                Width = 512,
                Height = 512,
                X = 512,
                Y = 512
            });
        }
    }

    [XmlType(TypeName = ""Item"")]
    public class SampleXmlSerializableDataItem
    {
        [XmlAttribute(AttributeName = ""x"")]
        public int X { get; set; }

        [XmlAttribute(AttributeName = ""y"")]
        public int Y { get; set; }

        [XmlAttribute(AttributeName = ""w"")]
        public int Width { get; set; }

        [XmlAttribute(AttributeName = ""h"")]
        public int Height { get; set; }
    }";
        }

        private static async Task RunTestOnTempFile()
        {
            var data = new SampleXmlSerializableData {Width = 12345};
            var folder = ApplicationData.Current.TemporaryFolder;
            string fileName = await folder.CreateTempFileNameAsync(".xml");
            await data.SerializeAsXml(
                fileName,
                folder);
            var deserializedData = await XmlSerialization.LoadFromXmlFile<SampleXmlSerializableData>(
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

    [XmlRoot(ElementName = "RootElement")]
    public class SampleXmlSerializableData : List<SampleXmlSerializableDataItem>
    {
        [XmlAttribute(AttributeName = "w")]
        public int Width { get; set; }

        [XmlAttribute(AttributeName = "h")]
        public int Height { get; set; }

        public SampleXmlSerializableData()
        {
            Width = 1024;
            Height = 1024;

            this.Add(new SampleXmlSerializableDataItem
            {
                Width = 512,
                Height = 512,
                X = 0,
                Y = 0
            });
            this.Add(new SampleXmlSerializableDataItem
            {
                Width = 512,
                Height = 512,
                X = 512,
                Y = 0
            });
            this.Add(new SampleXmlSerializableDataItem
            {
                Width = 512,
                Height = 512,
                X = 0,
                Y = 512
            });
            this.Add(new SampleXmlSerializableDataItem
            {
                Width = 512,
                Height = 512,
                X = 512,
                Y = 512
            });
        }
    }

    [XmlType(TypeName = "Item")]
    public class SampleXmlSerializableDataItem
    {
        [XmlAttribute(AttributeName = "x")]
        public int X { get; set; }

        [XmlAttribute(AttributeName = "y")]
        public int Y { get; set; }

        [XmlAttribute(AttributeName = "w")]
        public int Width { get; set; }

        [XmlAttribute(AttributeName = "h")]
        public int Height { get; set; }
    }
}
