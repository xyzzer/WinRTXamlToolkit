//#define TRACEDEVICEDETAILS
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.IO.Extensions;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using WinRTXamlToolkit.AwaitableUI;
using LayoutPanel = Windows.UI.Xaml.Controls.Panel;
using Panel = Windows.Devices.Enumeration.Panel;

namespace WinRTXamlToolkit.Controls
{
    [TemplatePart(Name = CaptureElementName, Type = typeof(CaptureElement))]
    [TemplatePart(Name = WebCamSelectorPopupName, Type = typeof(Popup))]
    [TemplatePart(Name = WebCamSelectorName, Type = typeof(Selector))]
    [TemplatePart(Name = RecordingIndicatorName, Type = typeof(LayoutPanel))]
    [TemplatePart(Name = RecordingAnimationName, Type = typeof(Storyboard))]
    [TemplatePart(Name = CountdownControlName, Type = typeof(CountdownControl))]
    public class CameraCaptureControl : Control
    {
        #region enum CameraCaptureControlStates
        private enum CameraCaptureControlStates
        {
            Hidden = 0,
            Initializing,
            Shown,
            Recording,
            Deinitializing
        }
        #endregion

        #region Private fields
        #region Template part names
        private const string CaptureElementName = "PART_CaptureElement";
        private const string WebCamSelectorPopupName = "PART_WebCamSelectorPopup";
        private const string WebCamSelectorName = "PART_WebCamSelector";
        private const string RecordingIndicatorName = "PART_RecordingIndicator";
        private const string RecordingAnimationName = "PART_RecordingAnimation";
        private const string CountdownControlName = "PART_CountdownControl";
        private const string FlashAnimationName = "PART_FlashAnimation";
        #endregion

        #region Template parts
        private CaptureElement _captureElement;
        private Popup _webCamSelectorPopup;
        private Selector _webCamSelector;
        private LayoutPanel _recordingIndicator;
        private Storyboard _recordingAnimation;
        private CountdownControl _countdownControl;
        private Storyboard _flashAnimation;
        #endregion

        private CameraCaptureControlStates _internalState;

        /// <summary>
        /// List of audio devices found.
        /// </summary>
        private DeviceInformation[] _audioCaptureDevices;

        /// <summary>
        /// List of video devices found.
        /// </summary>
        private DeviceInformation[] _videoCaptureDevices;

        /// <summary>
        /// Index of the currently used device in the _audioCaptureDevices list.
        /// </summary>
        private int _currentAudioCaptureDeviceIndex = -1;

        /// <summary>
        /// Index of the currently used video device in the _videoCaptureDevices list.
        /// </summary>
        private int _currentVideoCaptureDeviceIndex = -1;

        /// <summary>
        /// Video device found to be of preferred kind.
        /// </summary>
        private DeviceInformation _preferredVideoCaptureDevice;

        /// <summary>
        /// Media capture class associated with current capture.
        /// </summary>
        private MediaCapture _mediaCapture;

        private TaskCompletionSource<bool> _recordingTaskSource;
        #endregion

        #region Dependency properties

        #region CameraFailedEvent

        public event CameraFailedHandler CameraFailed;

        public delegate void CameraFailedHandler(object sender, MediaCaptureFailedEventArgs e);

        public void OnCameraFailed(object sender, MediaCaptureFailedEventArgs e)
        {
            if (CameraFailed != null)
                CameraFailed(sender, e);
        }

        #endregion

        #region ShowOnLoad
        public static readonly DependencyProperty ShowOnLoadProperty =
            DependencyProperty.Register(
                "ShowOnLoad",
                typeof(bool),
                typeof(CameraCaptureControl),
                new PropertyMetadata(true));

        public bool ShowOnLoad
        {
            get { return (bool)GetValue(ShowOnLoadProperty); }
            set { SetValue(ShowOnLoadProperty, value); }
        }
        #endregion

        #region PreferredCameraType
        public static readonly DependencyProperty PreferredCameraTypeProperty =
            DependencyProperty.Register(
                "PreferredCameraType",
                typeof(Windows.Devices.Enumeration.Panel),
                typeof(CameraCaptureControl),
                new PropertyMetadata(Windows.Devices.Enumeration.Panel.Unknown));

        public Windows.Devices.Enumeration.Panel PreferredCameraType
        {
            get
            {
                return (Windows.Devices.Enumeration.Panel)GetValue(PreferredCameraTypeProperty);
            }
            set
            {
                SetValue(PreferredCameraTypeProperty, value);
            }
        }
        #endregion

        #region VideoDevice
        /// <summary>
        /// VideoDevice Dependency Property
        /// </summary>
        public static readonly DependencyProperty VideoDeviceProperty =
            DependencyProperty.Register(
                "VideoDevice",
                typeof(DeviceInformation),
                typeof(CameraCaptureControl),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the VideoDevice property. This dependency property 
        /// indicates the currently used video capture device.
        /// </summary>
        public DeviceInformation VideoDevice
        {
            get { return (DeviceInformation)GetValue(VideoDeviceProperty); }
            private set { SetValue(VideoDeviceProperty, value); }
        }
        #endregion

        #region AudioDevice
        /// <summary>
        /// AudioDevice Dependency Property
        /// </summary>
        public static readonly DependencyProperty AudioDeviceProperty =
            DependencyProperty.Register(
                "AudioDevice",
                typeof(DeviceInformation),
                typeof(CameraCaptureControl),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the AudioDevice property. This dependency property 
        /// indicates the currently used audio capture device..
        /// </summary>
        public DeviceInformation AudioDevice
        {
            get { return (DeviceInformation)GetValue(AudioDeviceProperty); }
            set { SetValue(AudioDeviceProperty, value); }
        }
        #endregion

        #region VideoDeviceId
        /// <summary>
        /// VideoDeviceId Dependency Property
        /// </summary>
        public static readonly DependencyProperty VideoDeviceIdProperty =
            DependencyProperty.Register(
                "VideoDeviceId",
                typeof(string),
                typeof(CameraCaptureControl),
                new PropertyMetadata(null, OnVideoDeviceIdChanged));

        /// <summary>
        /// Gets or sets the VideoDeviceId property. This dependency property 
        /// indicates the ID of the video device.
        /// </summary>
        public string VideoDeviceId
        {
            get { return (string)GetValue(VideoDeviceIdProperty); }
            set { SetValue(VideoDeviceIdProperty, value); }
        }

        /// <summary>
        /// Handles changes to the VideoDeviceId property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnVideoDeviceIdChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (CameraCaptureControl)d;
            string oldVideoDeviceId = (string)e.OldValue;
            string newVideoDeviceId = target.VideoDeviceId;
            target.OnVideoDeviceIdChanged(oldVideoDeviceId, newVideoDeviceId);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the VideoDeviceId property.
        /// </summary>
        /// <param name="oldVideoDeviceId">The old VideoDeviceId value</param>
        /// <param name="newVideoDeviceId">The new VideoDeviceId value</param>
        protected void OnVideoDeviceIdChanged(
            string oldVideoDeviceId, string newVideoDeviceId)
        {
        }
        #endregion

        #region AudioDeviceId
        /// <summary>
        /// AudioDeviceId Dependency Property
        /// </summary>
        public static readonly DependencyProperty AudioDeviceIdProperty =
            DependencyProperty.Register(
                "AudioDeviceId",
                typeof(string),
                typeof(CameraCaptureControl),
                new PropertyMetadata(null, OnAudioDeviceIdChanged));

        /// <summary>
        /// Gets or sets the AudioDeviceId property. This dependency property 
        /// indicates the ID of the audio device used for recording.
        /// </summary>
        public string AudioDeviceId
        {
            get { return (string)GetValue(AudioDeviceIdProperty); }
            set { SetValue(AudioDeviceIdProperty, value); }
        }

        /// <summary>
        /// Handles changes to the AudioDeviceId property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnAudioDeviceIdChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (CameraCaptureControl)d;
            string oldAudioDeviceId = (string)e.OldValue;
            string newAudioDeviceId = target.AudioDeviceId;
            target.OnAudioDeviceIdChanged(oldAudioDeviceId, newAudioDeviceId);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the AudioDeviceId property.
        /// </summary>
        /// <param name="oldAudioDeviceId">The old AudioDeviceId value</param>
        /// <param name="newAudioDeviceId">The new AudioDeviceId value</param>
        protected virtual void OnAudioDeviceIdChanged(
            string oldAudioDeviceId, string newAudioDeviceId)
        {
        }
        #endregion

        #region VideoDeviceName
        /// <summary>
        /// VideoDeviceName Dependency Property
        /// </summary>
        public static readonly DependencyProperty VideoDeviceNameProperty =
            DependencyProperty.Register(
                "VideoDeviceName",
                typeof(string),
                typeof(CameraCaptureControl),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the VideoDeviceName property. This dependency property 
        /// indicates the name of the currently used video device.
        /// </summary>
        public string VideoDeviceName
        {
            get { return (string)GetValue(VideoDeviceNameProperty); }
            private set { SetValue(VideoDeviceNameProperty, value); }
        }
        #endregion

        #region AudioDeviceName
        /// <summary>
        /// AudioDeviceName Dependency Property
        /// </summary>
        public static readonly DependencyProperty AudioDeviceNameProperty =
            DependencyProperty.Register(
                "AudioDeviceName",
                typeof(string),
                typeof(CameraCaptureControl),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the AudioDeviceName property. This dependency property 
        /// indicates the name of the audio device name.
        /// </summary>
        public string AudioDeviceName
        {
            get { return (string)GetValue(AudioDeviceNameProperty); }
            private set { SetValue(AudioDeviceNameProperty, value); }
        }
        #endregion

        #region PickVideoDeviceAutomatically
        /// <summary>
        /// PickVideoDeviceAutomatically Dependency Property
        /// </summary>
        public static readonly DependencyProperty PickVideoDeviceAutomaticallyProperty =
            DependencyProperty.Register(
                "PickVideoDeviceAutomatically",
                typeof(bool),
                typeof(CameraCaptureControl),
                new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets the PickVideoDeviceAutomatically property. This dependency property 
        /// indicates whether the video device should be selected automatically.
        /// A selection UI shows up if false.
        /// </summary>
        public bool PickVideoDeviceAutomatically
        {
            get { return (bool)GetValue(PickVideoDeviceAutomaticallyProperty); }
            set { SetValue(PickVideoDeviceAutomaticallyProperty, value); }
        }
        #endregion

        #region PickAudioDeviceAutomatically
        /// <summary>
        /// PickAudioDeviceAutomatically Dependency Property
        /// </summary>
        public static readonly DependencyProperty PickAudioDeviceAutomaticallyProperty =
            DependencyProperty.Register(
                "PickAudioDeviceAutomatically",
                typeof(bool),
                typeof(CameraCaptureControl),
                new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets the PickAudioDeviceAutomatically property. This dependency property 
        /// indicates whether the video device should be selected automatically.
        /// A selection UI shows up if false.
        /// </summary>
        public bool PickAudioDeviceAutomatically
        {
            get { return (bool)GetValue(PickAudioDeviceAutomaticallyProperty); }
            set { SetValue(PickAudioDeviceAutomaticallyProperty, value); }
        }
        #endregion

        #region StreamingCaptureMode
        /// <summary>
        /// StreamingCaptureMode Dependency Property
        /// </summary>
        public static readonly DependencyProperty StreamingCaptureModeProperty =
            DependencyProperty.Register(
                "StreamingCaptureMode",
                typeof(StreamingCaptureMode),
                typeof(CameraCaptureControl),
                new PropertyMetadata(StreamingCaptureMode.Video, OnStreamingCaptureModeChanged));

        /// <summary>
        /// Gets or sets the StreamingCaptureMode property. This dependency property 
        /// indicates the streaming mode.
        /// </summary>
        public StreamingCaptureMode StreamingCaptureMode
        {
            get { return (StreamingCaptureMode)GetValue(StreamingCaptureModeProperty); }
            set { SetValue(StreamingCaptureModeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the StreamingCaptureMode property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnStreamingCaptureModeChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (CameraCaptureControl)d;
            StreamingCaptureMode oldStreamingCaptureMode = (StreamingCaptureMode)e.OldValue;
            StreamingCaptureMode newStreamingCaptureMode = target.StreamingCaptureMode;
            target.OnStreamingCaptureModeChanged(oldStreamingCaptureMode, newStreamingCaptureMode);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the StreamingCaptureMode property.
        /// </summary>
        /// <param name="oldStreamingCaptureMode">The old StreamingCaptureMode value</param>
        /// <param name="newStreamingCaptureMode">The new StreamingCaptureMode value</param>
        protected virtual void OnStreamingCaptureModeChanged(
            StreamingCaptureMode oldStreamingCaptureMode, StreamingCaptureMode newStreamingCaptureMode)
        {
        }
        #endregion

        #region VideoEncodingQuality
        /// <summary>
        /// VideoEncodingQuality Dependency Property
        /// </summary>
        public static readonly DependencyProperty VideoEncodingQualityProperty =
            DependencyProperty.Register(
                "VideoEncodingQuality",
                typeof(VideoEncodingQuality),
                typeof(CameraCaptureControl),
                new PropertyMetadata(VideoEncodingQuality.Vga));

        private StorageFile _videoFile;

        /// <summary>
        /// Gets or sets the VideoEncodingQuality property. This dependency property 
        /// indicates the quality of the video recording.
        /// </summary>
        public VideoEncodingQuality VideoEncodingQuality
        {
            get { return (VideoEncodingQuality)GetValue(VideoEncodingQualityProperty); }
            set { SetValue(VideoEncodingQualityProperty, value); }
        }
        #endregion

        #region VideoDeviceEnclosureLocation
        /// <summary>
        /// VideoDeviceEnclosureLocation Dependency Property
        /// </summary>
        public static readonly DependencyProperty VideoDeviceEnclosureLocationProperty =
            DependencyProperty.Register(
                "VideoDeviceEnclosureLocation",
                typeof(EnclosureLocation),
                typeof(CameraCaptureControl),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the VideoDeviceEnclosureLocation property. This dependency property 
        /// indicates the location of the video capture device.
        /// </summary>
        public EnclosureLocation VideoDeviceEnclosureLocation
        {
            get { return (EnclosureLocation)GetValue(VideoDeviceEnclosureLocationProperty); }
            private set { SetValue(VideoDeviceEnclosureLocationProperty, value); }
        }
        #endregion

        #region PhotoCaptureCountdownSeconds
        /// <summary>
        /// PhotoCaptureCountdownSeconds Dependency Property
        /// </summary>
        public static readonly DependencyProperty PhotoCaptureCountdownSecondsProperty =
            DependencyProperty.Register(
                "PhotoCaptureCountdownSeconds",
                typeof(int),
                typeof(CameraCaptureControl),
                new PropertyMetadata(3));

        /// <summary>
        /// Gets or sets the PhotoCaptureCountdownSeconds property. This dependency property 
        /// indicates the number of countdown seconds before a photo gets captured.
        /// </summary>
        public int PhotoCaptureCountdownSeconds
        {
            get { return (int)GetValue(PhotoCaptureCountdownSecondsProperty); }
            set { SetValue(PhotoCaptureCountdownSecondsProperty, value); }
        }
        #endregion

        #endregion

        #region CTOR
        public CameraCaptureControl()
        {
            this.DefaultStyleKey = typeof(CameraCaptureControl);
            this.Loaded += OnLoaded;
        }
        #endregion

        #region Public methods
        #region InitializeAsync()
        public async Task<CameraInitializationResult> InitializeAsync()
        {
            try
            {
                _internalState = CameraCaptureControlStates.Initializing;

                if (_currentAudioCaptureDeviceIndex < 0 &&
                    (this.StreamingCaptureMode == StreamingCaptureMode.AudioAndVideo ||
                    this.StreamingCaptureMode == StreamingCaptureMode.Audio))
                {
                    await FindAudioCaptureDevicesAsync();

                    if (_audioCaptureDevices.Length == 0)
                    {
                        if (this.StreamingCaptureMode == StreamingCaptureMode.Audio)
                        {
                            _internalState = CameraCaptureControlStates.Hidden;
                            Debugger.Break();

                            return new CameraInitializationResult(false, "No audio devices found.");
                        }
                    }
                    else if (this.AudioDeviceId != null)
                    {
                        var device = _audioCaptureDevices.FirstOrDefault(d => d.Id == this.AudioDeviceId);

                        if (device != null)
                        {
                            _currentAudioCaptureDeviceIndex = Array.IndexOf(_audioCaptureDevices, device);
                        }
                    }
                    else if (this.PickAudioDeviceAutomatically)
                    {
                        _currentAudioCaptureDeviceIndex = 0;
                    }
                    else
                    {
                        await ShowMicrophoneSelector();
                    }
                }

                if (_currentVideoCaptureDeviceIndex < 0 &&
                    (this.StreamingCaptureMode == StreamingCaptureMode.AudioAndVideo ||
                    this.StreamingCaptureMode == StreamingCaptureMode.Video))
                {
                    await FindVideoCaptureDevicesAsync();

                    if (_videoCaptureDevices.Length == 0)
                    {
                        if (this.StreamingCaptureMode == StreamingCaptureMode.Video)
                        {
                            Debugger.Break();
                            _internalState = CameraCaptureControlStates.Hidden;

                            return new CameraInitializationResult(false, "No video devices found.");
                        }

                        // Audio only
                        return new CameraInitializationResult(true);
                    }

                    if (_videoCaptureDevices.Length == 1)
                    {
                        _currentVideoCaptureDeviceIndex = 0;
                        return await StartPreviewAsync();
                    }

                    if (this.VideoDeviceId != null)
                    {
                        var device = _videoCaptureDevices.FirstOrDefault(d => d.Id == this.VideoDeviceId);

                        if (device != null)
                        {
                            _currentVideoCaptureDeviceIndex = Array.IndexOf(_videoCaptureDevices, device);
                            return await StartPreviewAsync();
                        }

                        // If requested device was not found - don't give up, but look for another one
                    }

                    if (_preferredVideoCaptureDevice != null)
                    {
                        _currentVideoCaptureDeviceIndex = Array.IndexOf(_videoCaptureDevices, _preferredVideoCaptureDevice);
                        return await StartPreviewAsync();
                    }

                    if (PickVideoDeviceAutomatically)
                    {
                        _currentVideoCaptureDeviceIndex = 0;
                        return await StartPreviewAsync();
                    }

                    bool success = await ShowWebCamSelector();

                    if (success)
                    {
                        return new CameraInitializationResult(true);
                    }

                    return new CameraInitializationResult(false, "Unable to select video device.");
                }

                // Audio only recording
                return await StartPreviewAsync();
            }
            catch (Exception ex)
            {
                return new CameraInitializationResult(false, "An unkown error has occured.", ex);
            }
        }
        #endregion

        #region ShowAsync()
        public async Task<bool> ShowAsync()
        {
            if (_internalState != CameraCaptureControlStates.Initializing)
            {
                //TODO: Handle this case better - wait for the state to be hidden if deinitializing or wait for initialization to complete before returning if initializing
                return false;
            }

            try
            {
                _internalState = CameraCaptureControlStates.Shown;

                if (_captureElement != null)
                {
                    _captureElement.Source = _mediaCapture;
                    await _mediaCapture.StartPreviewAsync();
                }


                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region HideAsync()
        public async Task HideAsync()
        {
            _internalState = CameraCaptureControlStates.Deinitializing;

            if (_mediaCapture != null)
            {
                await _mediaCapture.StopPreviewAsync();
                _mediaCapture = null;
            }

            if (_captureElement != null)
            {
                _captureElement.Source = null;
            }

            _internalState = CameraCaptureControlStates.Hidden;
        }
        #endregion

        #region CapturePhotoToStorageFileAsync()
        public async Task<StorageFile> CapturePhotoToStorageFileAsync(StorageFolder folder = null, string fileName = null, string defaultExtension = ".jpg")
        {
            if (_countdownControl != null &&
                this.PhotoCaptureCountdownSeconds > 0)
            {
                _countdownControl.FadeInCustom();
                await _countdownControl.StartCountdown(this.PhotoCaptureCountdownSeconds);
                _countdownControl.FadeOutCustom();
            }

            if (_flashAnimation != null)
                _flashAnimation.Begin();

            if (folder == null)
            {
                folder = KnownFolders.PicturesLibrary;
            }

            if (fileName == null)
            {
                fileName = await folder.CreateTempFileNameAsync(defaultExtension);
            }

            var photoFile = await folder.CreateFileAsync(
                fileName,
                CreationCollisionOption.FailIfExists);

            ImageEncodingProperties imageEncodingProperties;

            switch (Path.GetExtension(fileName))
            {
                case ".png":
                    imageEncodingProperties = ImageEncodingProperties.CreatePng();
                    break;
                default:
                    imageEncodingProperties = ImageEncodingProperties.CreateJpeg();
                    break;
            }

            try
            {
                await _mediaCapture.CapturePhotoToStorageFileAsync(imageEncodingProperties, photoFile);
            }
            catch
            {
                OnCameraFailed(null, null);
                return null;
            }

            return photoFile;
        }
        #endregion

        #region CapturePhotoToStorageFileAsync()
        public async Task CapturePhotoToStreamAsync(IRandomAccessStream stream, ImageEncodingProperties imageEncodingProperties)
        {
            await _mediaCapture.CapturePhotoToStreamAsync(imageEncodingProperties, stream);
        }
        #endregion

        #region StartVideoCaptureAsync()
        public async Task<StorageFile> StartVideoCaptureAsync(StorageFolder folder = null, string fileName = null)
        {
            if (_internalState == CameraCaptureControlStates.Recording)
            {
                // Note: Already recording - ignoring the folder and filename and returning result of previous request.
                await _recordingTaskSource.Task;
                return _videoFile;
            }

            _internalState = CameraCaptureControlStates.Recording;
            _recordingTaskSource = new TaskCompletionSource<bool>(false);

            if (_internalState != CameraCaptureControlStates.Shown)
            {
                CameraInitializationResult result = await InitializeAsync();

                if (result.Success)
                {
                    await ShowAsync();
                }
                else
                {
                    //TODO: Add error handling here.
                }
            }

            if (_mediaCapture == null)
            {
                throw new InvalidOperationException();
            }

            if (folder == null)
            {
                folder = KnownFolders.VideosLibrary;
            }

            if (fileName == null)
            {
                fileName = await folder.CreateTempFileNameAsync(".mp4");
            }

            _videoFile = await folder.CreateFileAsync(
                fileName,
                CreationCollisionOption.FailIfExists);

            if (_recordingIndicator != null)
            {
                _recordingIndicator.Visibility = Visibility.Visible;
            }

            if (_recordingAnimation != null)
            {
                _recordingAnimation.Begin();
            }

            var encodingProfile =
                Path.GetExtension(fileName).Equals(".wmv", StringComparison.OrdinalIgnoreCase) ?
                MediaEncodingProfile.CreateWmv(VideoEncodingQuality) :
                MediaEncodingProfile.CreateMp4(VideoEncodingQuality);

            await _mediaCapture.StartRecordToStorageFileAsync(encodingProfile, _videoFile);
            await _recordingTaskSource.Task;
            _recordingTaskSource = null;

            if (_recordingAnimation != null)
            {
                _recordingAnimation.Stop();
            }

            if (_recordingIndicator != null)
            {
                _recordingIndicator.Visibility = Visibility.Collapsed;
            }

            _internalState = CameraCaptureControlStates.Shown;
            return _videoFile;
        }
        #endregion

        #region StopCapture()
        public async Task<StorageFile> StopCapture()
        {
            if (_internalState != CameraCaptureControlStates.Recording)
            {
                _recordingTaskSource.SetResult(true);
                return null;
            }

            await _mediaCapture.StopRecordAsync();
            _recordingTaskSource.SetResult(true);
            return _videoFile;
        }
        #endregion

        #region CycleCamerasAsync()
        public async Task CycleCamerasAsync()
        {
            if (_videoCaptureDevices.Length <= 1)
            {
                return;
            }

            // Need to set the ID to null so that the current device does not get set again.
            if (_internalState == CameraCaptureControlStates.Shown)
            {
                await HideAsync();
                _currentVideoCaptureDeviceIndex = (_currentVideoCaptureDeviceIndex + 1) % _videoCaptureDevices.Length;

                var result = await InitializeAsync();

                if (result.Success)
                {
                    await ShowAsync();
                }
                else
                {
                    //TODO: Add error handling here
                }
            }
            else
            {
                _currentVideoCaptureDeviceIndex = (_currentVideoCaptureDeviceIndex + 1) % _videoCaptureDevices.Length;
            }

            this.VideoDeviceId = _videoCaptureDevices[_currentVideoCaptureDeviceIndex].Id;
        }
        #endregion
        #endregion

        #region Private/Protected/Internal methods
        #region OnApplyTemplate()
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _captureElement = (CaptureElement)GetTemplateChild(CaptureElementName);

            if (_mediaCapture != null)
            {
                _captureElement.Source = _mediaCapture;
                _mediaCapture.StartPreviewAsync();
            }

            _webCamSelectorPopup = GetTemplateChild(WebCamSelectorPopupName) as Popup;
            _webCamSelector = GetTemplateChild(WebCamSelectorName) as Selector;
            _recordingIndicator = GetTemplateChild(RecordingIndicatorName) as LayoutPanel;
            _recordingAnimation = GetTemplateChild(RecordingAnimationName) as Storyboard;
            _countdownControl = GetTemplateChild(CountdownControlName) as CountdownControl;
            _flashAnimation = GetTemplateChild(FlashAnimationName) as Storyboard;
        }
        #endregion

        #region OnLoaded()
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.ShowOnLoad)
            {
                CameraInitializationResult args = await InitializeAsync();

                if (args.Success)
                {
                    await ShowAsync();
                }
                else
                {
                    // TODO: Add error handling here
                    await HideAsync();
                }
            }
        }
        #endregion

        #region ShowMicrophoneSelector()
        private async Task<bool> ShowMicrophoneSelector()
        {
            if (_webCamSelector == null)
                return false;

            _webCamSelector.Items.Clear();

            foreach (var device in _audioCaptureDevices)
            {
                _webCamSelector.Items.Add(
                    device.EnclosureLocation == null || (device.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Unknown)
                        ? device.Name
                        : string.Format("{0} - {1}", device.EnclosureLocation.Panel, device.Name));
            }

            if (_webCamSelectorPopup != null)
            {
                _webCamSelectorPopup.IsOpen = true;
            }

            await _webCamSelector.WaitForSelectionChangedAsync();
            _currentAudioCaptureDeviceIndex = _webCamSelector.SelectedIndex;

            if (_webCamSelectorPopup != null)
            {
                _webCamSelectorPopup.IsOpen = false;
            }

            await StartPreviewAsync();
            return true;
        }
        #endregion

        #region ShowWebCamSelector()
        private async Task<bool> ShowWebCamSelector()
        {
            if (_webCamSelector == null)
                return false;

            _webCamSelector.Items.Clear();

            foreach (var device in _videoCaptureDevices)
            {
                _webCamSelector.Items.Add(
                    device.EnclosureLocation == null || (device.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Unknown)
                        ? device.Name
                        : string.Format("{0} - {1}", device.EnclosureLocation.Panel, device.Name));
            }

            if (_webCamSelectorPopup != null)
            {
                _webCamSelectorPopup.IsOpen = true;
            }

            await _webCamSelector.WaitForSelectionChangedAsync();
            _currentVideoCaptureDeviceIndex = _webCamSelector.SelectedIndex;

            if (_webCamSelectorPopup != null)
            {
                _webCamSelectorPopup.IsOpen = false;
            }

            await StartPreviewAsync();
            return true;
        }
        #endregion

        #region FindVideoCaptureDevicesAsync()
        private async Task FindVideoCaptureDevicesAsync()
        {
            if (_videoCaptureDevices == null)
            {
                _videoCaptureDevices = (await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture)).ToArray();

                if (this.PreferredCameraType != Panel.Unknown)

                    foreach (var device in _videoCaptureDevices)
                    {
                        if (device.EnclosureLocation != null &&
                            device.EnclosureLocation.Panel == this.PreferredCameraType)
                        {
                            _preferredVideoCaptureDevice = device;
                            break;
                        }
                    }

#if TRACEDEVICEDETAILS
                TraceDeviceDetails();
#endif
            }
        }
        #endregion

        #region FindAudioCaptureDevicesAsync()
        private async Task FindAudioCaptureDevicesAsync()
        {
            if (_audioCaptureDevices == null)
            {
                _audioCaptureDevices = (await DeviceInformation.FindAllAsync(DeviceClass.AudioCapture)).ToArray();
            }
        }
        #endregion

        #region StartPreviewAsync()
        private async Task<CameraInitializationResult> StartPreviewAsync()
        {
            try
            {
                if (_mediaCapture != null)
                {
                    _mediaCapture.Failed -= OnMediaCaptureFailed;
                }

                _mediaCapture = new MediaCapture();
                _mediaCapture.Failed += OnMediaCaptureFailed;

                if (_currentVideoCaptureDeviceIndex >= 0)
                {
                    this.VideoDevice = _videoCaptureDevices[_currentVideoCaptureDeviceIndex];
                    this.VideoDeviceId = this.VideoDevice.Id;
                    this.VideoDeviceName = this.VideoDevice.Name;
                    this.VideoDeviceEnclosureLocation = this.VideoDevice.EnclosureLocation;
                }

                if (_currentAudioCaptureDeviceIndex >= 0)
                {
                    this.AudioDevice = _audioCaptureDevices[_currentAudioCaptureDeviceIndex];
                    this.AudioDeviceId = this.AudioDevice.Id;
                    this.AudioDeviceName = this.AudioDevice.Name;
                }

                if (CheckStreamingCaptureModeConfiguration())
                {
                    return new CameraInitializationResult(true);
                }

                var settings = new MediaCaptureInitializationSettings
                {
                    StreamingCaptureMode = this.StreamingCaptureMode,
                    PhotoCaptureSource = PhotoCaptureSource.Auto //TODO: Figure out if this is worth exposing
                };

                if (this.StreamingCaptureMode == StreamingCaptureMode.AudioAndVideo)
                {
                    settings.VideoDeviceId = this.VideoDeviceId;
                    settings.AudioDeviceId = this.AudioDeviceId;
                }
                else if (this.StreamingCaptureMode == StreamingCaptureMode.Video)
                {
                    settings.VideoDeviceId = this.VideoDeviceId;
                }
                else if (this.StreamingCaptureMode == StreamingCaptureMode.Audio)
                {
                    settings.AudioDeviceId = this.AudioDeviceId;
                }

                await _mediaCapture.InitializeAsync(settings);
            }
            catch (Exception ex)
            {
                string error = "An unkown error has occured.";

                if (ex.Message.ToLower().Contains("denied"))
                    error = "Camera acccess permission not granted.";
                else if (ex.Message.ToLower().Contains("revoked"))
                    error = "Camera acccess permission has been revoked.";

                return new CameraInitializationResult(false, error);
            }

            return new CameraInitializationResult(true);
        }
        #endregion

        #region OnMediaCaptureFailed()
        private void OnMediaCaptureFailed(MediaCapture sender, MediaCaptureFailedEventArgs errorEventArgs)
        {
            OnCameraFailed(sender, errorEventArgs);
        }
        #endregion

        #region CheckStreamingCaptureModeConfiguration()
        private bool CheckStreamingCaptureModeConfiguration()
        {
            if (this.StreamingCaptureMode == StreamingCaptureMode.Video ||
                this.StreamingCaptureMode == StreamingCaptureMode.AudioAndVideo)
            {
                Debug.Assert(!string.IsNullOrEmpty(this.VideoDeviceId));

                if (string.IsNullOrEmpty(this.VideoDeviceId))
                {
                    // Fallbacks
                    if (this.StreamingCaptureMode == StreamingCaptureMode.Video)
                        return true;
                    this.StreamingCaptureMode = StreamingCaptureMode.Audio;
                }
            }

            if (this.StreamingCaptureMode == StreamingCaptureMode.Audio ||
                this.StreamingCaptureMode == StreamingCaptureMode.AudioAndVideo)
            {
                Debug.Assert(!string.IsNullOrEmpty(this.AudioDeviceId));

                if (string.IsNullOrEmpty(this.AudioDeviceId))
                {
                    // Fallbacks
                    if (this.StreamingCaptureMode == StreamingCaptureMode.Audio)
                        return true;
                    this.StreamingCaptureMode = StreamingCaptureMode.Video;
                }
            }
            return false;
        }

        #endregion

        #region TraceDeviceDetails()
#if TRACEDEVICEDETAILS
        [Conditional("DEBUG")]
        private void TraceDeviceDetails()
        {
            int i = 0;

            try
            {
                foreach (var device in _videoCaptureDevices)
                {
                    Debug.WriteLine("* Device [{0}]", i++);

                    if (device.EnclosureLocation == null)
                    {
                        Debug.WriteLine("EnclosureLocation: null");
                    }
                    else
                    {
                        Debug.WriteLine("EnclosureLocation.InDock: " + device.EnclosureLocation.InDock);
                        Debug.WriteLine("EnclosureLocation.InLid: " + device.EnclosureLocation.InLid);
                        Debug.WriteLine("EnclosureLocation.Panel: " + device.EnclosureLocation.Panel);
                    }

                    Debug.WriteLine("Id: " + device.Id);
                    Debug.WriteLine("IsDefault: " + device.IsDefault);
                    Debug.WriteLine("IsEnabled: " + device.IsEnabled);
                    Debug.WriteLine("Name: " + device.Name);
                    Debug.WriteLine("Properties:");

                    foreach (var property in device.Properties)
                    {
                        Debug.WriteLine("\t{0}({1}): {2}", property.Key, property.Value.GetType(), property.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
            }
        }
#endif
        #endregion
        #endregion
    }

    public class CameraInitializationResult
    {
        public Exception Error { get; private set; }
        public string ErrorMessage { get; private set; }
        public bool Success { get; private set; }

        public CameraInitializationResult(bool success, string error = null, Exception exception = null)
        {
            this.Error = exception;
            this.ErrorMessage = error;
            this.Success = success;
        }
    }
}
