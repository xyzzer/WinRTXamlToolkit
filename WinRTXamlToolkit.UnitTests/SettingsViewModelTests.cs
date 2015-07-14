using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using WinRTXamlToolkit.Debugging.ViewModels;

namespace WinRTXamlToolkit.UnitTests
{
    [TestClass]
    public class SettingsViewModelTests
    {
        #region class TestPropertySet
        public class TestPropertySet : IPropertySet
        {
            private static readonly Dictionary<string, object> dictionary = new Dictionary<string, object>();

            public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
            {
                return dictionary.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Add(KeyValuePair<string, object> item)
            {
                dictionary.Add(item.Key, item.Value);
            }

            public void Clear()
            {
                dictionary.Clear();
            }

            public bool Contains(KeyValuePair<string, object> item)
            {
                return dictionary.Contains(item);
            }

            public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public bool Remove(KeyValuePair<string, object> item)
            {
                throw new NotImplementedException();
            }

            public int Count
            {
                get { return dictionary.Count; }
            }

            public bool IsReadOnly { get; private set; }
            public void Add(string key, object value)
            {
                dictionary.Add(key, value);
            }

            public bool ContainsKey(string key)
            {
                return dictionary.ContainsKey(key);
            }

            public bool Remove(string key)
            {
                return dictionary.Remove(key);
            }

            public bool TryGetValue(string key, out object value)
            {
                return dictionary.TryGetValue(key, out value);
            }

            public object this[string key]
            {
                get { return dictionary[key]; }
                set { dictionary[key] = value; }
            }

            public ICollection<string> Keys
            {
                get { return dictionary.Keys; }
            }

            public ICollection<object> Values
            {
                get { return dictionary.Values; }
            }

            public event MapChangedEventHandler<string, object> MapChanged;
        } 
        #endregion

        #region class TestSettingsViewModel
        public class TestSettingsViewModel : SettingsViewModel
        {
            public TestSettingsViewModel()
                : base(isDeserializing: false)
            {
                this.IsSavingUnchangingSettingsInSetter = true;
                this.SettingsValues = new TestPropertySet();
            }

            #region StringProperty
            /// <summary>
            /// Backing field for the StringProperty property.
            /// </summary>
            private string stringProperty;

            /// <summary>
            /// Gets or sets a value indicating a test string property.
            /// </summary>
            public string StringProperty
            {
                get { return this.GetProperty(ref this.stringProperty); }
                set { this.SetProperty(ref this.stringProperty, value); }
            }
            #endregion

            #region BoolProperty
            /// <summary>
            /// Backing field for the BoolProperty property.
            /// </summary>
            private bool boolProperty;

            /// <summary>
            /// Gets or sets a value indicating whether something is true or false.
            /// </summary>
            public bool BoolProperty
            {
                get { return this.GetProperty(ref this.boolProperty); }
                set { this.SetProperty(ref this.boolProperty, value); }
            }
            #endregion
        } 
        #endregion

        #region SettingsViewModel_Accessors()
        /// <summary>
        /// Tests if the accessors work.
        /// </summary>
        [TestMethod]
        public void SettingsViewModel_Accessors()
        {
            var tsvm = new TestSettingsViewModel();
            tsvm.StringProperty = "string";
            tsvm.BoolProperty = true;
            Assert.AreEqual(tsvm.StringProperty, "string");
            Assert.AreEqual(tsvm.BoolProperty, true);
            tsvm.StringProperty = "string2";
            tsvm.BoolProperty = false;
            Assert.AreEqual(tsvm.StringProperty, "string2");
            Assert.AreEqual(tsvm.BoolProperty, false);
        }
        #endregion

        #region SettingsViewModel_LocalSettings()
        /// <summary>
        /// Tests if the settings are saved to local settings.
        /// </summary>
        [TestMethod]
        public void SettingsViewModel_LocalSettings()
        {
            var tsvm1 = new TestSettingsViewModel();
            var tsvm2 = new TestSettingsViewModel();

            // setting a property on one instance should work on another instance since they share local settings
            tsvm1.StringProperty = "string";
            tsvm1.BoolProperty = true;
            Assert.AreEqual("string", tsvm2.StringProperty);
            Assert.AreEqual(true, tsvm2.BoolProperty);
            tsvm2.StringProperty = "string2";
            tsvm2.BoolProperty = false;
            Assert.AreEqual("string2", tsvm1.StringProperty);
            Assert.AreEqual(false, tsvm1.BoolProperty);

        }
        #endregion

        #region SettingsViewModel_SaveAndLoad()
        /// <summary>
        /// Tests if the settings are saved and loaded from a file correctly
        /// </summary>
        [TestMethod]
        public async Task SettingsViewModel_SaveAndLoad()
        {
            var tsvm1 = new TestSettingsViewModel();
            tsvm1.AutoSaveSettingsToFile = false;
            var settingsFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(Guid.NewGuid().ToString());

            // In the first part of the test we
            // 1. Set some settings and save them to a file
            // 2. Change the settings
            // 3. Load settings from the file
            // 4. Confirm the original values are restored
            tsvm1.StringProperty = "string1";
            tsvm1.BoolProperty = true;
            await tsvm1.SaveSettingsAsync(settingsFile);
            tsvm1.StringProperty = "string2";
            tsvm1.BoolProperty = false;

            await TestSettingsViewModel.LoadSettingsAsync<TestSettingsViewModel>(settingsFile);
            var tsvm2 = (TestSettingsViewModel)SettingsViewModel.Instance;

            Assert.AreEqual("string1", tsvm2.StringProperty);
            Assert.AreEqual(true, tsvm2.BoolProperty);

            // In the second part we test AutoSaveSettingsToFile
            // 1. Enable AutoSaveSettingsToFile
            // 2. Set some settings
            // 3. Disable AutoSaveSettingsToFile
            // 4. Set some new values that should not be saved
            // 5. Load settings
            // 6. Confirm the original values are restored
            tsvm2.AutoSaveSettingsToFile = true;
            tsvm2.StringProperty = "string3";
            tsvm2.BoolProperty = true;
            tsvm2.AutoSaveSettingsToFile = false;
            tsvm2.StringProperty = "string4";
            tsvm2.BoolProperty = false;

            await TestSettingsViewModel.LoadSettingsAsync<TestSettingsViewModel>(settingsFile);
            var tsvm3 = (TestSettingsViewModel)SettingsViewModel.Instance;

            Assert.AreEqual("string3", tsvm3.StringProperty);
            Assert.AreEqual(true, tsvm3.BoolProperty);

        }
        #endregion
    }
}
