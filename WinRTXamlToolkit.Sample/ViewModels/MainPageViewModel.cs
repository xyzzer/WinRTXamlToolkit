using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
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
        public ObservableCollection<ButtonViewModel> Samples { get; private set; }

        public MainPageViewModel()
        {
            this.Samples = new ObservableCollection<ButtonViewModel>
            {
                new ButtonViewModel
                {
                    Caption = "AlternativeFrame",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(AlternativeFrameTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "AnimatingContainer",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(AnimatingContainerTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "BackgroundTimer",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(BackgroundTimerTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "Behavior",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(BehaviorsTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "BindingDebugConverter",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(BindingDebugConverterTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "ButtonBaseExtensions",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ButtonBaseExtensionsTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "CameraCaptureControl",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(CameraCaptureControlPage)))
                },
                new ButtonViewModel
                {
                    Caption = "CameraCaptureUI",
                    Command = new RelayCommand(StartCameraCaptureUITest)
                },
                new ButtonViewModel
                {
                    Caption = "CascadingImage",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(CascadingImageTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "CascadingTextBlock",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(CascadingTextBlockTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "Chart",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ChartTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "ColorPicker",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ColorPickerTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "ColorPicker Primitives",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ColorPickerPrimitivesTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "Content Fade",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ContentControlExtensionsTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "CountdownControl",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(CountdownTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "CustomAppBar",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(CustomAppBarTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "CustomGridSplitter",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(CustomGridSplitterTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "DebugConsole",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(DebugConsoleTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "FrameworkElementExtensions.(System)Cursor",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(FrameworkElementExtensionsCursorTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "HighlightBehavior",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(HighlightBehaviorTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "ImageButton",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ImageButtonTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "ImageToggleButton",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ImageToggleButtonTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "ImageExtensions",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ImageExtensionsTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "Imaging",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ImagingTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "InputDialog",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(InputDialogTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "I/O",
                    Command = new RelayCommand(InputDialogAndStringIOTest)
                },
                new ButtonViewModel
                {
                    Caption = "LayoutTransformControl",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(LayoutTransformControlTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "ListViewExtensions",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ListViewExtensionsTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "ListViewExtensionsItem.ToBringIntoView",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ListViewExtensionsItemToBringIntoViewTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "ListViewItemExtensions",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ListViewItemExtensionsTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "MessageDialogExtensions",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(MessageDialogExtensionsTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "NumericUpDown",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(NumericUpDownTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "ParallaxBackgroundBehavior",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ParallaxBackgroundBehaviorTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "RingSlice/PieSlice",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(RingSliceTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "ScrollViewerExtensions",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(ScrollViewerExtensionsTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "TextBox Validation",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(TextBoxValidationTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "TreeView",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(TreeViewTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "UIElementAnimationExtensions",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(UIElementAnimationExtensionsTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "UniformGrid",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(UniformGridTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "WatermarkTextBox",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(WatermarkTextBoxTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "WebBrowser",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(WebBrowserTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "WebFile",
                    Command = new RelayCommand(RunWebFileTest)
                },
                new ButtonViewModel
                {
                    Caption = "WrapPanel",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(WrapPanelTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "WriteableBitmapLoadExtensions",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(WriteableBitmapLoadExtensionsTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "WriteableBitmap.Render()",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(WriteableBitmapRenderTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "XML DataContract Serialization",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(XmlDataContractSerializerTestPage)))
                },
                new ButtonViewModel
                {
                    Caption = "XML Serialization",
                    Command = new RelayCommand(() => AppShell.Frame.Navigate(typeof(XmlSerializerTestPage)))
                }
            };
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
