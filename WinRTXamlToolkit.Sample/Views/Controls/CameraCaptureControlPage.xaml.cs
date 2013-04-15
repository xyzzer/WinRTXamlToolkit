using System;
using System.Threading.Tasks;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Tools;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
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

        private delegate Task TaskDelegate();

        private async void OnCapturePhotoButtonClick(object sender, RoutedEventArgs e)
        {
            var file = await TestedControl.CapturePhotoToStorageFileAsync(ApplicationData.Current.TemporaryFolder);
            var bi = new BitmapImage();

            IRandomAccessStreamWithContentType stream;

            try
            {
                stream = await TryCatchRetry.RunWithDelayAsync<Exception, IRandomAccessStreamWithContentType>(
                    file.OpenReadAsync(),
                    TimeSpan.FromSeconds(0.5),
                    10,
                    true);
            }
            catch (Exception ex)
            {
                // Seems like a bug with WinRT not closing the file sometimes that writes the photo to.
#pragma warning disable 4014
                new MessageDialog(ex.Message, "Error").ShowAsync();
#pragma warning restore 4014

                return;
            }

            bi.SetSource(stream);
            PhotoImage.Source = bi;
            CapturedVideoElement.Visibility = Visibility.Collapsed;
            PhotoImage.Visibility = Visibility.Visible;
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
                CapturedVideoElement.Visibility = Visibility.Visible;
                PhotoImage.Visibility = Visibility.Collapsed;

                IRandomAccessStreamWithContentType stream;

                try
                {
                    stream = await TryCatchRetry.RunWithDelayAsync<Exception, IRandomAccessStreamWithContentType>(
                        _videoFile.OpenReadAsync(),
                        TimeSpan.FromSeconds(0.5),
                        10);
                }
                catch (Exception ex)
                {
#pragma warning disable 4014
                    // Seems like a bug with WinRT not closing the file sometimes that it writes the video to.
                    new MessageDialog(ex.Message, "Error").ShowAsync();
#pragma warning restore 4014

                    return;
                }

                CapturedVideoElement.SetSource(stream, _videoFile.ContentType);
            }
            else
            {
                CaptureVideoButton.Content = "Record";
                _capturingVideo = false;
#pragma warning disable 4014
                TestedControl.StopCapture();
#pragma warning restore 4014
            }
        }

        private void OnTestedControlCameraFailed(object sender, MediaCaptureFailedEventArgs e)
        {
            if (this.IsInVisualTree())
            {
#pragma warning disable 4014
                new MessageDialog(e != null ? e.Message : "Camera Capture Failed", "Error").ShowAsync();
#pragma warning restore 4014
            }
        }
    }
}
