using System.Collections.ObjectModel;
using System.Linq;
using WinRTXamlToolkit.Sample.Commands;
using WinRTXamlToolkit.Sample.Views;

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

        public ObservableCollection<SampleButtonViewModel> Samples { get; private set; }

        private MainPageViewModel()
        {
            var samples = new ObservableCollection<SampleButtonViewModel>
            {
//                new SampleButtonViewModel
//                {
//                    Caption = "AlternativeFrame",
//                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(AlternativeFrameTestPage))),
//                    SampleType = SampleTypes.Controls
//                },
//                new SampleButtonViewModel
//                {
//                    Caption = "AnimatingContainer",
//                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(AnimatingContainerTestPage))),
//                    SampleType = SampleTypes.Controls
//                },
//                new SampleButtonViewModel
//                {
//                    Caption = "BackgroundTimer",
//                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(BackgroundTimerTestPage))),
//                    SampleType = SampleTypes.Miscellaneous
//                },
//                new SampleButtonViewModel
//                {
//                    Caption = "EventThrottler",
//                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(EventThrottlerTestPage))),
//                    SampleType = SampleTypes.Miscellaneous
//                },
//                new SampleButtonViewModel
//                {
//                    Caption = "Behavior",
//                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(BehaviorsTestPage))),
//                    SampleType = SampleTypes.Extensions
//                },
//                new SampleButtonViewModel
//                {
//                    Caption = "BindingDebugConverter",
//                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(BindingDebugConverterTestPage))),
//                    SampleType = SampleTypes.Debugging
//                },
//                new SampleButtonViewModel
//                {
//                    Caption = "ButtonBaseExtensions",
//                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ButtonBaseExtensionsTestPage))),
//                    SampleType = SampleTypes.Extensions
//                },
                new SampleButtonViewModel
                (
                    "Calendar",
                    SampleTypes.Controls,
                    typeof(CalendarTestView)
                ),
                new SampleButtonViewModel
                (
                    "CameraCaptureControl",
                    SampleTypes.Controls,
                    typeof(CameraCaptureControlTestView)
                ),
//                new SampleButtonViewModel
//                {
//                    Caption = "CameraCaptureUI",
//                    Command = new RelayCommand(StartCameraCaptureUITest),
//                    SampleType = SampleTypes.Controls
//                },
//                new SampleButtonViewModel
//                {
//                    Caption = "CascadingImage",
//                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(CascadingImageTestPage))),
//                    SampleType = SampleTypes.Controls
//                },
//                new SampleButtonViewModel
//                {
//                    Caption = "CascadingTextBlock",
//                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(CascadingTextBlockTestPage))),
//                    SampleType = SampleTypes.Controls
//                },
                new SampleButtonViewModel
                (
                    "Chart",
                    SampleTypes.Controls,
                    typeof(ChartTestView)
                ),
                //new SampleButtonViewModel
                //{
                //    Caption = "ColorPicker",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ColorPickerTestPage))),
                //    SampleType = SampleTypes.Controls
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "ColorPicker Primitives",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ColorPickerPrimitivesTestPage))),
                //    SampleType = SampleTypes.Controls
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "Content Fade",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ContentControlExtensionsTestPage))),
                //    SampleType = SampleTypes.Extensions
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "CountdownControl",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(CountdownTestPage))),
                //    SampleType = SampleTypes.Controls
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "CustomAppBar",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(CustomAppBarTestPage))),
                //    SampleType = SampleTypes.Controls
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "CustomGridSplitter",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(CustomGridSplitterTestPage))),
                //    SampleType = SampleTypes.Controls
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "DebugConsole Log",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(DebugConsoleTestPage))),
                //    SampleType = SampleTypes.Debugging
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "DebugConsole Visual Tree",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(DebugTreeViewTestPage))),
                //    SampleType = SampleTypes.Debugging
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "FrameworkElementExtensions.(System)Cursor",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(FrameworkElementExtensionsCursorTestPage))),
                //    SampleType = SampleTypes.Extensions
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "Gauge",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(GaugeTestPage))),
                //    SampleType = SampleTypes.Controls
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "HighlightBehavior",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(HighlightBehaviorTestPage))),
                //    SampleType = SampleTypes.Extensions
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "ImageButton",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ImageButtonTestPage))),
                //    SampleType = SampleTypes.Controls
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "ImageToggleButton",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ImageToggleButtonTestPage))),
                //    SampleType = SampleTypes.Controls
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "ImageExtensions",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ImageExtensionsTestPage))),
                //    SampleType = SampleTypes.Extensions
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "Imaging",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ImagingTestPage))),
                //    SampleType = SampleTypes.Imaging
                //},
                new SampleButtonViewModel
                (
                    "InputDialog",
                    SampleTypes.Controls,
                    typeof(InputDialogTestView)
                ),
                //new SampleButtonViewModel
                //{
                //    Caption = "I/O",
                //    Command = new RelayCommand(InputDialogAndStringIOTest),
                //    SampleType = SampleTypes.Miscellaneous
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "LayoutTransformControl",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(LayoutTransformControlTestPage))),
                //    SampleType = SampleTypes.Controls
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "ListViewExtensions",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ListViewExtensionsTestPage))),
                //    SampleType = SampleTypes.Extensions
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "ListViewItemExtensions.ItemToBringIntoView",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ListViewExtensionsItemToBringIntoViewTestPage))),
                //    SampleType = SampleTypes.Extensions
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "ListViewItemExtensions",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ListViewItemExtensionsTestPage))),
                //    SampleType = SampleTypes.Extensions
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "MessageDialogExtensions",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(MessageDialogExtensionsTestPage))),
                //    SampleType = SampleTypes.Extensions
                //},
                new SampleButtonViewModel
                (
                    "NumericUpDown",
                    SampleTypes.Controls,
                    typeof(NumericUpDownTestView)
                ),
                //new SampleButtonViewModel
                //{
                //    Caption = "ParallaxBackgroundBehavior",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ParallaxBackgroundBehaviorTestPage))),
                //    SampleType = SampleTypes.Extensions
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "RingSlice/PieSlice",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(RingSliceTestPage))),
                //    SampleType = SampleTypes.Controls
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "ScrollViewerExtensions",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ScrollViewerExtensionsTestPage))),
                //    SampleType = SampleTypes.Extensions
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "SquareGrid",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(SquareGridTestPage))),
                //    SampleType = SampleTypes.Controls
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "TextBox Validation",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(TextBoxValidationTestPage))),
                //    SampleType = SampleTypes.Extensions
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "TextBoxFocusExtensions",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(TextBoxFocusExtensionsTestPage))),
                //    SampleType = SampleTypes.Extensions
                //},
                new SampleButtonViewModel
                (
                    "TextBoxFocusExtensions",
                    SampleTypes.Extensions,
                    typeof(TextBoxFocusExtensionsTestView)
                ),
                //new SampleButtonViewModel
                //{
                //    Caption = "ToolWindow",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ToolWindowTestPage))),
                //    SampleType = SampleTypes.Controls
                //},
                new SampleButtonViewModel
                (
                    "TreeView",
                    SampleTypes.Controls,
                    typeof(TreeViewTestView)
                ),
                //new SampleButtonViewModel
                //{
                //    Caption = "UIElementAnimationExtensions",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(UIElementAnimationExtensionsTestPage))),
                //    SampleType = SampleTypes.Extensions
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "UniformGrid",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(UniformGridTestPage))),
                //    SampleType = SampleTypes.Controls
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "WatermarkPasswordBox",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(WatermarkPasswordBoxTestPage))),
                //    SampleType = SampleTypes.Controls
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "WatermarkTextBox",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(WatermarkTextBoxTestPage))),
                //    SampleType = SampleTypes.Controls
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "WebBrowser",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(WebBrowserTestPage))),
                //    SampleType = SampleTypes.Controls
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "WebFile",
                //    Command = new RelayCommand(RunWebFileTest),
                //    SampleType = SampleTypes.Miscellaneous
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "WrapPanel",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(WrapPanelTestPage))),
                //    SampleType = SampleTypes.Controls
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "WriteableBitmapLoadExtensions",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(WriteableBitmapLoadExtensionsTestPage))),
                //    SampleType = SampleTypes.Extensions
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "XML DataContract Serialization",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(XmlDataContractSerializerTestPage))),
                //    SampleType = SampleTypes.Miscellaneous
                //},
                //new SampleButtonViewModel
                //{
                //    Caption = "XML Serialization",
                //    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(XmlSerializerTestPage))),
                //    SampleType = SampleTypes.Miscellaneous
                //}
            };

            this.Samples =
                new ObservableCollection<SampleButtonViewModel>(
                    samples
                        .OrderBy(s => s.Caption) // samples ordered by name
                    );
                        //.GroupBy(s => s.SampleType) // samples grouped by group type in the order based on enum value order
                        //.Select(g => new SampleGroupViewModel(
                        //    g.Key,
                        //    g))
                        //.OrderBy(g => (int)g.SampleType)); // groups ordered by name

            //foreach (var sampleButtonViewModel in this.Samples.Cast<SampleGroupViewModel>())
            //{
            //    sampleButtonViewModel.ParentList = this.Samples;
            //}
        }

//        private static async void RunWebFileTest()
//        {
//            try
//            {
//                var file = await WebFile.SaveAsync(new Uri("http://lh5.googleusercontent.com/-HcYBzdacEBY/TWLkpYMRZ-I/AAAAAAAAhzg/ZwAws7m2IpQ/s1152/IMG_7246.JPG"));

//                if (file != null)
//                    await new MessageDialog("Downloaded file. Name determined: " + file.Name).ShowAsync();
//                else
//                    throw new InvalidOperationException();

//                file = await WebFile.SaveAsync(
//                    new Uri("http://lh5.googleusercontent.com/-HcYBzdacEBY/TWLkpYMRZ-I/AAAAAAAAhzg/ZwAws7m2IpQ/s1152/IMG_7246.JPG"),
//                    ApplicationData.Current.TemporaryFolder,
//                    "some file.jpg");

//                if (file != null)
//                    await new MessageDialog("Downloaded file. Name given/resolved: " + file.Name).ShowAsync();
//                else
//                    throw new InvalidOperationException();
//            }
//            catch (Exception ex)
//            {
//#pragma warning disable 4014
//                new MessageDialog("Failed to download file. Exception: \r\n" + ex).ShowAsync();
//#pragma warning restore 4014
//            }
//        }

//        private static async void InputDialogAndStringIOTest()
//        {
//            var dialog = new InputDialog();
//            await dialog.ShowAsync("Title", "Text", "OK", "Cancel");
//            await new MessageDialog(dialog.InputText, "WinRTXamlToolkit.Sample").ShowAsync();
//            await "ABC".WriteToFile("abc.txt", KnownFolders.DocumentsLibrary);
//            await "DEF".WriteToFile("abc.txt", KnownFolders.DocumentsLibrary);
//            await "ABC2".WriteToFile("abc2.txt", KnownFolders.DocumentsLibrary);
//            await new MessageDialog("Test files written", "WinRTXamlToolkit.Sample").ShowAsync();
//            var abc = await StringIOExtensions.ReadFromFile("abc.txt", KnownFolders.DocumentsLibrary);
//            await new MessageDialog(abc, "Test file read").ShowAsync();
//        }

        //private async void StartCameraCaptureUITest()
        //{
        //    var dialog = new CameraCaptureUI();
        //    await dialog.CaptureFileAsync(CameraCaptureUIMode.PhotoOrVideo);
        //}
    }
}
