using System;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class CameraCaptureControlPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public CameraCaptureControlPage()
        {
            this.InitializeComponent();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void OnCycleCamerasButtonClick(object sender, RoutedEventArgs e)
        {
            TestedControl.CycleCamerasAsync();
        }

        private async void OnShowPreviewButtonClick(object sender, RoutedEventArgs e)
        {
            var result = await TestedControl.ShowAsync();
        }

        private void OnHidePreviewButtonClick(object sender, RoutedEventArgs e)
        {
            TestedControl.HideAsync();
        }

        private async void OnCapturePhotoButtonClick(object sender, RoutedEventArgs e)
        {
            var file = await TestedControl.CapturePhotoToStorageFileAsync(ApplicationData.Current.TemporaryFolder);
            var bi = new BitmapImage();
            var stream = await file.OpenReadAsync();
            bi.SetSource(stream);
            PhotoImage.Source = bi;
        }

        private bool _capturingVideo;
        private StorageFile _videoFile;

        private async void OnCaptureVideoButtonClick(object sender, RoutedEventArgs e)
        {
            if (!_capturingVideo)
            {
                CaptureVideoButton.Content = "Stop";
                _capturingVideo = true;
                _videoFile = await TestedControl.StartVideoCaptureAsync(ApplicationData.Current.TemporaryFolder);
                CapturedVideoElement.SetSource(await _videoFile.OpenReadAsync(), _videoFile.ContentType);
            }
            else
            {
                CaptureVideoButton.Content = "Record";
                _capturingVideo = false;
                TestedControl.StopCapture();
            }
        }

        private void OnTestedControlCameraFailed(object sender, MediaCaptureFailedEventArgs e)
        {
            if (this.IsInVisualTree())
            {
                new MessageDialog(e.Message, "Error").ShowAsync();
            }
        }
    }
}
