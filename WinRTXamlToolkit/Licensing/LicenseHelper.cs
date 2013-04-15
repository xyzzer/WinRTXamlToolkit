//#define TRIALTEST
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Store;

// Courtesy of Simon (darkside) Jackson
// http://darkgenesis.zenithmoon.com/windows-8-license-checker-tester-thingy/

namespace WinRTXamlToolkit.Licensing
{
    /// <summary>
    /// Helper methods for app license information.
    /// </summary>
    public class LicenseChecker : INotifyPropertyChanged
    {
        private static LicenseInformation _licenseInformation;

        #region IsTrial
        private bool _isTrial;

        /// <summary>
        /// Gets or sets a value indicating whether the app is in trial mode.
        /// </summary>
        /// <value>
        ///   <c>true</c> if in trial mode; otherwise, <c>false</c>.
        /// </value>
        public bool IsTrial
        {
            get
            {
#if TRIALTEST
                InitializeLicense();
#endif

                return _isTrial;
            }
            set
            {
                SetProperty(ref _isTrial, value);
            }
        } 
        #endregion

        #region InitializeLicense()
        /// <summary>
        /// Initializes the license checke.
        /// </summary>
        public void InitializeLicense()
        {
            // Initialize the license info for use in the app that is uploaded to the Store.
            // uncomment for release
            _licenseInformation = CurrentApp.LicenseInformation;

            // Initialize the license info for testing.
            // comment the next line for release
            //licenseInformation = CurrentAppSimulator.LicenseInformation;
            // Register for the license state change event.
            _licenseInformation.LicenseChanged += LicenseChangedEventHandler;

            IsTrial = _licenseInformation.IsTrial;
        } 
        #endregion

        #region TestTrialPurchase()
        public static void TestTrialPurchase()
        {
#pragma warning disable 4014
            CurrentAppSimulator.RequestAppPurchaseAsync(false);
#pragma warning restore 4014
        } 
        #endregion

        #region LicenseChangedEventHandler()
        private void LicenseChangedEventHandler()
        {
            // code is in next steps
            ReloadLicense();
        } 
        #endregion

        #region ReloadLicense()
        /// <summary>
        /// Reloads license information.
        /// </summary>
        public void ReloadLicense()
        {
            if (_licenseInformation.IsActive)
            {
                IsTrial = _licenseInformation.IsTrial;
            }
            else
            {
                // A license is inactive only when there's an error.
                IsTrial = true;
            }
        } 
        #endregion

        #region INotifyPropertyChanged implementation
        /// <summary>
        /// Multicast event for property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Checks if a property already matches a desired value.  Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        } 
        #endregion
    }
}
