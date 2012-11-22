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
    /// <summary>
    /// A camera capture control that looks similar to the CameraCaptureUI,
    /// but works as an embeddable control that you can blend into your app's UI.
    /// </summary>
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

        private CameraCaptureControlStates _internalState = CameraCaptureControlStates.Hidden;

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

        private MediaCapture _mediaCapture;
        /// <summary>
        /// Media capture class associated with current capture.
        /// </summary>
        private MediaCapture MediaCapture
        {
            get
            {
                return _mediaCapture;
            }
            set
            {
                _mediaCapture = value;
            }
        }

        private TaskCompletionSource<bool> _recordingTaskSource;
        #endregion

        #region Dependency properties

        #region CameraFailedEvent

        /// <summary>
        /// Occurs when there is a problem working with the camera.
        /// </summary>
        public event CameraFailedHandler CameraFailed;

        /// <summary>
        /// Handles CameraFailed events occuring when there is a problem working with the camera
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MediaCaptureFailedEventArgs" /> instance containing the event data.</param>
        public delegate void CameraFailedHandler(object sender, MediaCaptureFailedEventArgs e);

        private void OnCameraFailed(object sender, MediaCaptureFailedEventArgs e)
        {
            if (CameraFailed != null)
                CameraFailed(sender, e);
        }

        #endregion

        #region ShowOnLoad
        /// <summary>
        /// The ShowOnLoadProperty.
        /// </summary>
        public static readonly DependencyProperty ShowOnLoadProperty =
            DependencyProperty.Register(
                "ShowOnLoad",
                typeof(bool),
                typeof(CameraCaptureControl),
                new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets a value indicating whether the camera preview
        /// should show when the control is loaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if it should show on load; otherwise, <c>false</c>.
        /// </value>
        public bool ShowOnLoad
        {
            get { return (bool)GetValue(ShowOnLoadProperty); }
            set { SetValue(ShowOnLoadProperty, value); }
        }
        #endregion

        #region PreferredCameraType
        /// <summary>
        /// The preferred camera type property.
        /// </summary>
        public static readonly DependencyProperty PreferredCameraTypeProperty =
            DependencyProperty.Register(
                "PreferredCameraType",
                typeof(Windows.Devices.Enumeration.Panel),
                typeof(CameraCaptureControl),
                new PropertyMetadata(Windows.Devices.Enumeration.Panel.Unknown));

        /// <summary>
        /// Gets or sets the preferred type (panel location) of the default camera.
        /// </summary>
        /// <value>
        /// The type of the preferred camera.
        /// </value>
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
        /// <summary>
        /// Initializes a new instance of the <see cref="CameraCaptureControl" /> class.
        /// </summary>
        public CameraCaptureControl()
        {
            this.DefaultStyleKey = typeof(CameraCaptureControl);
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }
        #endregion

        #region Public methods
        #region InitializeAsync()
        private TaskCompletionSource<CameraInitializationResult> _initializationTaskSource;

        /// <summary>
        /// Performs the one time initialization and shows the preview (if video).
        /// </summary>
        /// <returns></returns>
        private async Task<CameraInitializationResult> InitializeAsync()
        {
            CameraInitializationResult result;

            try
            {
                Debug.Assert(Dispatcher.HasThreadAccess);

                if (_initializationTaskSource != null)
                {
                    // Already initializing or initialized
                    await _initializationTaskSource.Task;
                    _initializationTaskSource = new TaskCompletionSource<CameraInitializationResult>();
                }

                _internalState = CameraCaptureControlStates.Initializing;
                _initializationTaskSource = new TaskCompletionSource<CameraInitializationResult>();

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

                            result = new CameraInitializationResult(false, "No audio devices found.");
                            _initializationTaskSource.SetResult(result);

                            return result;
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

                            result = new CameraInitializationResult(false, "No video devices found.");
                            _initializationTaskSource.SetResult(result);

                            return result;
                        }

                        // Audio only
                        result = new CameraInitializationResult(true);
                        _initializationTaskSource.SetResult(result);

                        return result;
                    }

                    if (_videoCaptureDevices.Length == 1)
                    {
                        _currentVideoCaptureDeviceIndex = 0;
                        result = await this.StartPreviewAsync();
                        _initializationTaskSource.SetResult(result);

                        return result;
                    }

                    if (this.VideoDeviceId != null)
                    {
                        var device = _videoCaptureDevices.FirstOrDefault(d => d.Id == this.VideoDeviceId);

                        if (device != null)
                        {
                            _currentVideoCaptureDeviceIndex = Array.IndexOf(_videoCaptureDevices, device);
                            result = await this.StartPreviewAsync();
                            _initializationTaskSource.SetResult(result);

                            return result;
                        }

                        // If requested device was not found - don't give up, but look for another one
                    }

                    if (_preferredVideoCaptureDevice != null)
                    {
                        _currentVideoCaptureDeviceIndex = Array.IndexOf(_videoCaptureDevices, _preferredVideoCaptureDevice);
                        result = await this.StartPreviewAsync();
                        _initializationTaskSource.SetResult(result);

                        return result;
                    }

                    if (PickVideoDeviceAutomatically)
                    {
                        _currentVideoCaptureDeviceIndex = 0;
                        result = await this.StartPreviewAsync();
                        _initializationTaskSource.SetResult(result);

                        return result;
                    }

                    bool success = await ShowWebCamSelector();

                    if (success)
                    {
                        result = new CameraInitializationResult(true);
                        _initializationTaskSource.SetResult(result);

                        return result;
                        
                    }

                    result = new CameraInitializationResult(false, "Unable to select video device.");
                    _initializationTaskSource.SetResult(result);

                    return result;
                }

                // Audio only recording
                result = await this.StartPreviewAsync();
                _initializationTaskSource.SetResult(result);

                return result;
            }
            catch (Exception ex)
            {
                result = new CameraInitializationResult(false, "An unkown error has occured.", ex);

                if (_initializationTaskSource != null)
                {
                    _initializationTaskSource.SetResult(result);
                }

                return result;
            }
        }
        #endregion

        #region ShowAsync()
        /// <summary>
        /// Shows the preview asynchronously (completes when the camera is initialized)..
        /// </summary>
        /// <returns></returns>
        public async Task<CameraInitializationResult> ShowAsync()
        {
            await this.WaitForLoadedAsync();
            await this.WaitForNonZeroSizeAsync();
            var result = await InitializeAsync();

            if (!result.Success)
            {
                return result;
            }

            try
            {
                _internalState = CameraCaptureControlStates.Shown;

                if (_captureElement != null)
                {
                    _captureElement.Source = MediaCapture;
                    await MediaCapture.StartPreviewAsync();
                }

                return result;
            }
            catch (Exception ex)
            {
                return new CameraInitializationResult(false, "Camera display failed", ex);
            }
        }
        #endregion

        #region HideAsync()
        /// <summary>
        /// Hides the camera preview asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task HideAsync()
        {
            _internalState = CameraCaptureControlStates.Deinitializing;

            if (MediaCapture != null)
            {
                try
                {
                    await MediaCapture.StopPreviewAsync();
                }
                catch {}

                //_mediaCapture.Failed -= OnMediaCaptureFailed;
                //_mediaCapture = null;
            }

            if (_captureElement != null)
            {
                _captureElement.Source = null;
            }

            _internalState = CameraCaptureControlStates.Hidden;
        }
        #endregion

        #region CapturePhotoToStorageFileAsync()
        /// <summary>
        /// Captures the photo to storage file asynchronously.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="defaultExtension">The default extension.</param>
        /// <returns></returns>
        public async Task<StorageFile> CapturePhotoToStorageFileAsync(StorageFolder folder = null, string fileName = null, string defaultExtension = ".jpg")
        {
            if (_countdownControl != null &&
                this.PhotoCaptureCountdownSeconds > 0)
            {
#pragma warning disable 4014
                _countdownControl.FadeInCustom();
                await _countdownControl.StartCountdownAsync(this.PhotoCaptureCountdownSeconds);
                _countdownControl.FadeOutCustom();
#pragma warning restore 4014
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
                await MediaCapture.CapturePhotoToStorageFileAsync(imageEncodingProperties, photoFile);
            }
            catch
            {
                OnCameraFailed(null, null);
                return null;
            }

            return photoFile;
        }
        #endregion

        #region CapturePhotoToStreamAsync()
        /// <summary>
        /// Captures a photo to a stream asynchronously.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="imageEncodingProperties">The image encoding properties.</param>
        /// <returns></returns>
        public async Task CapturePhotoToStreamAsync(IRandomAccessStream stream, ImageEncodingProperties imageEncodingProperties)
        {
            await MediaCapture.CapturePhotoToStreamAsync(imageEncodingProperties, stream);
        }
        #endregion

        #region StartVideoCaptureAsync()
        /// <summary>
        /// Starts the video capture asynchronously.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public async Task<StorageFile> StartVideoCaptureAsync(StorageFolder folder = null, string fileName = null)
        {
            if (_internalState == CameraCaptureControlStates.Recording)
            {
                // Note: Already recording - ignoring the folder and filename and returning result of previous request.
                await _recordingTaskSource.Task;
                return _videoFile;
            }

            if (_internalState != CameraCaptureControlStates.Shown)
            {
                var result = await ShowAsync();

                if (!result.Success)
                {
                    //TODO: Add error handling here.

                    return null;
                }
            }

            _internalState = CameraCaptureControlStates.Recording;
            _recordingTaskSource = new TaskCompletionSource<bool>(false);

            if (MediaCapture == null)
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

            await MediaCapture.StartRecordToStorageFileAsync(encodingProfile, _videoFile);
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
        /// <summary>
        /// Stops the video capture.
        /// </summary>
        /// <returns></returns>
        public async Task<StorageFile> StopCapture()
        {
            if (_internalState != CameraCaptureControlStates.Recording)
            {
                _recordingTaskSource.SetResult(true);
                return null;
            }

            await MediaCapture.StopRecordAsync();
            _recordingTaskSource.SetResult(true);
            return _videoFile;
        }
        #endregion

        #region CycleCamerasAsync()
        /// <summary>
        /// Cycles the cameras asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task<CameraInitializationResult> CycleCamerasAsync()
        {
            if (_videoCaptureDevices.Length <= 1)
            {
                return new CameraInitializationResult(true);
            }

            // Need to set the ID to null so that the current device does not get set again.
            if (_internalState == CameraCaptureControlStates.Shown)
            {
                await HideAsync();
                _currentVideoCaptureDeviceIndex = (_currentVideoCaptureDeviceIndex + 1) % _videoCaptureDevices.Length;

                return await ShowAsync();
            }
            else
            {
                _currentVideoCaptureDeviceIndex = (_currentVideoCaptureDeviceIndex + 1) % _videoCaptureDevices.Length;
            }

            this.VideoDeviceId = _videoCaptureDevices[_currentVideoCaptureDeviceIndex].Id;

            return new CameraInitializationResult(true);
        }
        #endregion
        #endregion

        #region Private/Protected/Internal methods
        #region OnApplyTemplate()
        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate.
        /// In simplest terms, this means the method is called just before a UI element displays in your app.
        /// Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _captureElement = (CaptureElement)GetTemplateChild(CaptureElementName);

            if (MediaCapture != null)
            {
                _captureElement.Source = MediaCapture;
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
                await ShowAsync();
            }
        }
        #endregion

        #region OnUnloaded()
        private async void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_internalState == CameraCaptureControlStates.Initializing &&
                _initializationTaskSource != null)
            {
                await _initializationTaskSource.Task;
            }

            if (_internalState == CameraCaptureControlStates.Shown)
            {
#pragma warning disable 4014
                HideAsync();
#pragma warning restore 4014
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

            await this.StartPreviewAsync();

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
                if (MediaCapture != null)
                {
                    MediaCapture.Failed -= OnMediaCaptureFailed;
                }

                MediaCapture = new MediaCapture();
                MediaCapture.Failed += OnMediaCaptureFailed;

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

                await MediaCapture.InitializeAsync(settings);
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

    /// <summary>
    /// The result of camera initialization.
    /// </summary>
    public class CameraInitializationResult
    {
        /// <summary>
        /// Gets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public Exception Error { get; private set; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="CameraInitializationResult" /> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraInitializationResult" /> class.
        /// </summary>
        /// <param name="success">if set to <c>true</c> if initialization was successful.</param>
        /// <param name="error">The error.</param>
        /// <param name="exception">The exception.</param>
        internal CameraInitializationResult(bool success, string error = null, Exception exception = null)
        {
            this.Error = exception;
            this.ErrorMessage = error;
            this.Success = success;
        }
    }
}
