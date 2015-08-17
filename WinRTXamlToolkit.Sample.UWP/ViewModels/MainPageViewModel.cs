using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.IO.Extensions;
using WinRTXamlToolkit.Net;
using WinRTXamlToolkit.Sample.Views;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Popups;

namespace WinRTXamlToolkit.Sample.ViewModels
{
    public class MainPageViewModel : ViewModel
    {
        #region Instance (singleton implementation)
        private static MainPageViewModel _instance;
        public static MainPageViewModel Instance
        {
            get { return _instance ?? (_instance = new MainPageViewModel()); }
        }
        #endregion

        public List<SampleButtonViewModel> UngroupedSamples { get; private set; }
        public ObservableCollection<ButtonViewModel> SampleGroups { get; private set; }

        private MainPageViewModel()
        {
            this.UngroupedSamples = new List<SampleButtonViewModel>
            {
                new SampleButtonViewModel
                (
                    "AlternativeFrame",
                    typeof(AlternativeFrameTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "AnimatingContainer",
                    typeof(AnimatingContainerTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "AutoCompleteTextBox",
                    typeof(AutoCompleteTextBoxTestPage),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "BackgroundTimer",
                    typeof(BackgroundTimerTestView),
                    SampleTypes.Miscellaneous
                ),
                new SampleButtonViewModel
                (
                    "EventThrottler",
                    typeof(EventThrottlerTestView),
                    SampleTypes.Miscellaneous
                ),
                new SampleButtonViewModel
                (
                    "Behavior",
                    typeof(BehaviorsTestView),
                    SampleTypes.Extensions
                ),
                new SampleButtonViewModel
                (
                    "BindingDebugConverter",
                    typeof(BindingDebugConverterTestView),
                    SampleTypes.Debugging
                ),
                new SampleButtonViewModel
                (
                    "ButtonBaseExtensions",
                    typeof(ButtonBaseExtensionsTestView),
                    SampleTypes.Extensions
                ),
                new SampleButtonViewModel
                (
                    "Calendar",
                    typeof(CalendarTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "CameraCaptureControl",
                    typeof(CameraCaptureControlTestView),
                    SampleTypes.Controls
                ),
                //new SampleButtonViewModel
                //(
                //    "CameraCaptureUI",
                //    Command = new RelayCommand(StartCameraCaptureUITest),
                //    SampleTypes.Controls
                //),
                new SampleButtonViewModel
                (
                    "CascadingImage",
                    typeof(CascadingImageTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "CascadingTextBlock",
                    typeof(CascadingTextBlockTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "Chart",
                    typeof(ChartTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "ColorPicker",
                    typeof(ColorPickerTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "ColorPicker Primitives",
                    typeof(ColorPickerPrimitivesTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "Content Fade",
                    typeof(ContentControlExtensionsTestView),
                    SampleTypes.Extensions
                ),
                new SampleButtonViewModel
                (
                    "CountdownControl",
                    typeof(CountdownTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "CustomAppBar",
                    typeof(CustomAppBarTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "CustomGridSplitter",
                    typeof(CustomGridSplitterTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "DebugConsole Log",
                    typeof(DebugConsoleTestView),
                    SampleTypes.Debugging
                ),
                new SampleButtonViewModel
                (
                    "DebugConsole Visual Tree",
                    typeof(DebugTreeViewTestView),
                    SampleTypes.Debugging
                ),
                new SampleButtonViewModel
                (
                    "DockPanel",
                    typeof(DockPanelTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "FrameworkElementExtensions.(System)Cursor",
                    typeof(FrameworkElementExtensionsCursorTestView),
                    SampleTypes.Extensions
                ),
                new SampleButtonViewModel
                (
                    "FxContentControl",
                    typeof(FxContentControlTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "Gauge",
                    typeof(GaugeTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "HighlightBehavior",
                    typeof(HighlightBehaviorTestView),
                    SampleTypes.Extensions
                ),
                new SampleButtonViewModel
                (
                    "ImageButton",
                    typeof(ImageButtonTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "ImageToggleButton",
                    typeof(ImageToggleButtonTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "ImageExtensions",
                    typeof(ImageExtensionsTestView),
                    SampleTypes.Extensions
                ),
                new SampleButtonViewModel
                (
                    "Imaging",
                    typeof(ImagingTestView),
                    SampleTypes.Imaging
                ),
                new SampleButtonViewModel
                (
                    "InputDialog",
                    typeof(InputDialogTestView),
                    SampleTypes.Controls
                ),
                //new SampleButtonViewModel
                //(
                //    "I/O",
                //    Command = new RelayCommand(InputDialogAndStringIOTest),
                //    SampleTypes.Miscellaneous
                //),
                new SampleButtonViewModel
                (
                    "ItemsControlExtensions",
                    typeof(ItemsControlExtensionsTestView),
                    SampleTypes.Extensions
                ),
                new SampleButtonViewModel
                (
                    "LayoutTransformControl",
                    typeof(LayoutTransformControlTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "ListViewExtensions",
                    typeof(ListViewExtensionsTestView),
                    SampleTypes.Extensions
                ),
                new SampleButtonViewModel
                (
                    "ListViewItemExtensions.ItemToBringIntoView",
                    typeof(ListViewExtensionsItemToBringIntoViewTestView),
                    SampleTypes.Extensions
                ),
                new SampleButtonViewModel
                (
                    "ListViewItemExtensions",
                    typeof(ListViewItemExtensionsTestView),
                    SampleTypes.Extensions
                ),
                new SampleButtonViewModel
                (
                    "MessageDialogExtensions",
                    typeof(MessageDialogExtensionsTestView),
                    SampleTypes.Extensions
                ),
                new SampleButtonViewModel
                (
                    "NumericUpDown",
                    typeof(NumericUpDownTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "ParallaxBackgroundBehavior",
                    typeof(ParallaxBackgroundBehaviorTestView),
                    SampleTypes.Extensions
                ),
                new SampleButtonViewModel
                (
                    "RingSlice/PieSlice",
                    typeof(RingSliceTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "ScrollViewerExtensions",
                    typeof(ScrollViewerExtensionsTestView),
                    SampleTypes.Extensions
                ),
                new SampleButtonViewModel
                (
                    "SquareGrid",
                    typeof(SquareGridTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "TextBox Validation",
                    typeof(TextBoxValidationTestView),
                    SampleTypes.Extensions
                ),
                new SampleButtonViewModel
                (
                    "TextBoxFocusExtensions",
                    typeof(TextBoxFocusExtensionsTestView),
                    SampleTypes.Extensions
                ),
                new SampleButtonViewModel
                (
                    "ToolStrip",
                    typeof(ToolStripTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "ToolWindow",
                    typeof(ToolWindowTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "TreeView",
                    typeof(TreeViewTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "UIElementAnimationExtensions",
                    typeof(UIElementAnimationExtensionsTestView),
                    SampleTypes.Extensions
                ),
                new SampleButtonViewModel
                (
                    "UniformGrid",
                    typeof(UniformGridTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "WatermarkPasswordBox",
                    typeof(WatermarkPasswordBoxTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "WatermarkTextBox",
                    typeof(WatermarkTextBoxTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "WebBrowser",
                    typeof(WebBrowserTestView),
                    SampleTypes.Controls
                ),
                //new SampleButtonViewModel
                //(
                //    "WebFile",
                //    Command = new RelayCommand(RunWebFileTest),
                //    SampleTypes.Miscellaneous
                //),
                new SampleButtonViewModel
                (
                    "WrapPanel",
                    typeof(WrapPanelTestView),
                    SampleTypes.Controls
                ),
                new SampleButtonViewModel
                (
                    "WriteableBitmapLoadExtensions",
                    typeof(WriteableBitmapLoadExtensionsTestView),
                    SampleTypes.Extensions
                ),
                new SampleButtonViewModel
                (
                    "XML DataContract Serialization",
                    typeof(XmlDataContractSerializerTestView),
                    SampleTypes.Miscellaneous
                ),
                new SampleButtonViewModel
                (
                    "XML Serialization",
                    typeof(XmlSerializerTestView),
                    SampleTypes.Miscellaneous
                )
            };

            this.SampleGroups =
                new ObservableCollection<ButtonViewModel>(
                    this.UngroupedSamples
                        .OrderBy(s => s.Caption) // samples ordered by name
                        .GroupBy(s => s.SampleType) // samples grouped by group type in the order based on enum value order
                        .Select(g => new SampleGroupViewModel(
                            g.Key,
                            g))
                        .OrderBy(g => (int)g.SampleType)); // groups ordered by name

            foreach (var sampleGroupViewModel in this.SampleGroups.Cast<SampleGroupViewModel>())
            {
                sampleGroupViewModel.ParentList = this.SampleGroups;
            }
        }

        private static async void RunWebFileTest()
        {
            try
            {
                var file = await WebFile.SaveAsync(new Uri("http://lh5.googleusercontent.com/-HcYBzdacEBY/TWLkpYMRZ-I/AAAAAAAAhzg/ZwAws7m2IpQ/s1152/IMG_7246.JPG"));

                if (file != null)
                    await new MessageDialog("Downloaded file. Name determined: " + file.Name).ShowAsync();
                else
                    throw new InvalidOperationException();

                file = await WebFile.SaveAsync(
                    new Uri("http://lh5.googleusercontent.com/-HcYBzdacEBY/TWLkpYMRZ-I/AAAAAAAAhzg/ZwAws7m2IpQ/s1152/IMG_7246.JPG"),
                    ApplicationData.Current.TemporaryFolder,
                    "some file.jpg");

                if (file != null)
                    await new MessageDialog("Downloaded file. Name given/resolved: " + file.Name).ShowAsync();
                else
                    throw new InvalidOperationException();
            }
            catch (Exception ex)
            {
#pragma warning disable 4014
                new MessageDialog("Failed to download file. Exception: \r\n" + ex).ShowAsync();
#pragma warning restore 4014
            }
        }

        private static async void InputDialogAndStringIOTest()
        {
            var dialog = new InputDialog();
            await dialog.ShowAsync("Title", "Text", "OK", "Cancel");
            await new MessageDialog(dialog.InputText, "WinRTXamlToolkit.Sample").ShowAsync();
            await "ABC".WriteToFile("abc.txt", KnownFolders.DocumentsLibrary);
            await "DEF".WriteToFile("abc.txt", KnownFolders.DocumentsLibrary);
            await "ABC2".WriteToFile("abc2.txt", KnownFolders.DocumentsLibrary);
            await new MessageDialog("Test files written", "WinRTXamlToolkit.Sample").ShowAsync();
            var abc = await StringIOExtensions.ReadFromFile("abc.txt", KnownFolders.DocumentsLibrary);
            await new MessageDialog(abc, "Test file read").ShowAsync();
        }

        private async void StartCameraCaptureUITest()
        {
            var dialog = new CameraCaptureUI();
            await dialog.CaptureFileAsync(CameraCaptureUIMode.PhotoOrVideo);
        }
    }
}
