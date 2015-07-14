using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    [XmlRoot(ElementName = "Settings")]
    public class SettingsViewModel : BindableBase
    {
        #region Instance (singleton implementation)
        private static SettingsViewModel _instance;
        public static SettingsViewModel Instance
        {
            get { return _instance ?? (_instance = new SettingsViewModel(false)); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        /// <remarks>
        /// Normally singleton constructors are private, but we want to be able to serialize the class,
        /// so we have to trust that it's not called explicitly.
        /// </remarks>
        internal SettingsViewModel(bool isDeserializing)
        {
            this.isDeserializing = isDeserializing;
            this.SettingsValues = ApplicationData.Current.LocalSettings.Values;
        }

        public SettingsViewModel() : this(true)
        {
        }
        #endregion

        #region IsSavingUnchangingSettingsInSetter
#if DEBUG
        private bool isSavingUnchangingSettingsInSetter = false;
        internal bool IsSavingUnchangingSettingsInSetter
        {
            get { return this.GetProperty(ref this.isSavingUnchangingSettingsInSetter); }
            set { this.isSavingUnchangingSettingsInSetter = value; }
        }
#endif 
    	#endregion

        internal IPropertySet SettingsValues { get; set; }

        private bool isDeserializing = true;
        private bool isSavingPending = false;

        #region SavingSettingsTask
        private Task savingSettingsTask;
        public Task SavingSettingsTask
        {
            get { return this.savingSettingsTask; }
        } 
        #endregion

        #region SetProperty()
        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            System.Diagnostics.Debug.Assert(propertyName != null, "propertyName != null");
// ReSharper disable once ExplicitCallerInfoArgument
            base.SetProperty(ref storage, value, propertyName);
            this.SettingsValues[propertyName] = value;

            if (this.AutoSaveSettingsToFile &&
                !isDeserializing)
            {
                // We don't save settings while we're deserializing.
                // Deserialization should be quick anyways, so no one
                // else should be trying to set settings while it's happening.
#pragma warning disable 4014
                this.SaveSettingsAsync();
#pragma warning restore 4014
            }

            // We can't tell whether the property has changed in the settings view model
            // because we would first have to check what the original value was in the settings which kills
            // the gain we would get from not saving it on setter calls that don't change it.
            // Let's just always assume it has changed.
            return true;
        }
        #endregion

        #region GetProperty()
        protected T GetProperty<T>(ref T storage, [CallerMemberName] string propertyName = null)
        {
            object savedSettingValue;

            if (!this.SettingsValues.TryGetValue(propertyName, out savedSettingValue))
            {
                return storage;
            }

            return (T)savedSettingValue;
        }
        #endregion

        #region AutoSaveSettingsToFile
        private bool _autoSaveSettingsToFile;
        /// <summary>
        /// Gets or sets a value indicating whether the settings should be automatically
        /// saved to a file when a setting value changes.
        /// </summary>
        [XmlAttribute]
        public bool AutoSaveSettingsToFile
        {
            get { return this.GetProperty(ref this._autoSaveSettingsToFile); }
            set { this.SetProperty(ref this._autoSaveSettingsToFile, value); }
        }
        #endregion

        #region SettingsFileAccessToken
        private string _settingsFileAccessToken;

        /// <summary>
        /// Gets or sets the AccessToken of the settings file if settings are loaded from a file.
        /// </summary>
        [XmlAttribute]
        public string SettingsFileAccessToken
        {
            get { return this.GetProperty(ref this._settingsFileAccessToken); }
            set { this.SetProperty(ref this._settingsFileAccessToken, value); }
        }
        #endregion

        #region LoadSettingsAsync()
        /// <summary>
        /// Loads settings.
        /// </summary>
        internal static async Task LoadSettingsAsync<T>(StorageFile file) where T : SettingsViewModel
        {
            using (var stream = await file.OpenStreamForReadAsync())
            {
                var ser = new XmlSerializer(typeof(T));

                var settings = (SettingsViewModel)ser.Deserialize(stream);
                settings.isDeserializing = false;
                stream.Flush();
                _instance = settings;
            }

            var fal = StorageApplicationPermissions.FutureAccessList;
            _instance.SettingsFileAccessToken = fal.Add(file);
        }
        #endregion

        #region SaveSettingsAsync()
        /// <summary>
        /// Saves settings to the settings file based on SettingsFileAccessToken
        /// used to get a file from future access list.
        /// </summary>
        /// <returns></returns>
        internal async Task SaveSettingsAsync(StorageFile settingsFile = null)
        {
            if (settingsFile != null)
            {
                var fal = StorageApplicationPermissions.FutureAccessList;
                this.SettingsFileAccessToken = fal.Add(settingsFile);
            }

            Debug.Assert(!string.IsNullOrEmpty(this.SettingsFileAccessToken));

            this.isSavingPending = true;

            if (this.savingSettingsTask != null)
            {
                await this.savingSettingsTask;
                return;
            }

            var tcs = new TaskCompletionSource<bool>();
            this.savingSettingsTask = tcs.Task;

            try
            {
                while (this.isSavingPending)
                {
                    try
                    {
                        if (settingsFile == null)
                        {
                            var fal = StorageApplicationPermissions.FutureAccessList;
                            settingsFile = await fal.GetFileAsync(this.SettingsFileAccessToken);
                        }

                        using (var stream = await settingsFile.OpenStreamForWriteAsync())
                        {
                            var ser = new XmlSerializer(typeof(SettingsViewModel));
                            ser.Serialize(stream, this);
                            stream.Flush();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("Saving settings failed.\r\n" + e);

                        if (Debugger.IsAttached)
                        {
                            Debugger.Break();
                        }
                    }
                }
            }
            finally
            {
                tcs.SetResult(true);
                this.savingSettingsTask = null;
            }
        }
        #endregion
    }
}
