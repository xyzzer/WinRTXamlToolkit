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
    public sealed partial class CameraCaptureControlTestView : UserControl
    {
        public CameraCaptureControlTestView()
        {
            this.InitializeComponent();
        }

        private void OnCycleCamerasButtonClick(object sender, RoutedEventArgs e)
        {
            TestedControl.CycleCamerasAsync();
        }

        private delegate Task TaskDelegate();

        private async void OnCapturePhotoButtonClick(object sender, RoutedEventArgs e)
        {
            var file = await TestedControl.CapturePhotoToStorageFileAsync(ApplicationData.Current.TemporaryFolder);
            var bi = new BitmapImage();

            IRandomAccessStreamWithContentType stream = null;

            try
            {
                stream = await TryCatchRetry.RunWithDelayAsync<Exception, IRandomAccessStreamWithContentType>(
                    file.OpenReadAsync(),
                    TimeSpan.FromSeconds(0.5),
                    10,
                    true);
                await bi.SetSourceAsync(stream);
            }
            catch (Exception ex)
            {
                // Seems like a bug with WinRT not closing the file sometimes that writes the photo to.
#pragma warning disable 4014
                new MessageDialog(ex.Message, "Error").ShowAsync();
#pragma warning restore 4014

                return;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Dispose();
                }
            }

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

                if (this.CapturedVideoElement == null)
                {
                    return;
                }

                this.CapturedVideoElement.SetSource(stream, _videoFile.ContentType);
            }
            else
            {
                CaptureVideoButton.Content = "Record";
                _capturingVideo = false;

                await TestedControl.StopCapture();
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

        private async void OnShowHidePreviewButtonClick(object sender, RoutedEventArgs e)
        {
            this.ShowHidePreviewButton.IsEnabled = false;

            if (this.ShowHidePreviewButton.IsChecked == true)
            {
                await TestedControl.HideAsync();
                this.ShowHidePreviewButton.Content = "Show";
            }
            else
            {
                var result = await TestedControl.ShowAsync();
                this.ShowHidePreviewButton.Content = "Hide";
            }

            this.ShowHidePreviewButton.IsEnabled = true;
        }

        private void Sv_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            sp.Width = sv.ViewportWidth;
        }
    }
}
