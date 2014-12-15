using System;
using System.ComponentModel;
using System.Diagnostics;
#if WINDOWS_APP
using Windows.ApplicationModel.Search;
#endif
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Contains an extension for automatically switching focus to next field when the cursor reaches the MexLength position.
    /// </summary>
    public static class PasswordBoxFocusExtensions
    {
        #region AutoTabOnMaxLength
        /// <summary>
        /// AutoTabOnMaxLength Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty AutoTabOnMaxLengthProperty =
            DependencyProperty.RegisterAttached(
                "AutoTabOnMaxLength",
                typeof(bool),
                typeof(PasswordBoxFocusExtensions),
                new PropertyMetadata(false, OnAutoTabOnMaxLengthChanged));

        /// <summary>
        /// Gets the AutoTabOnMaxLength property. This dependency property 
        /// indicates whether the focus should switch to next element 
        /// when the cursor moves to the position at maximum length of the field..
        /// </summary>
        public static bool GetAutoTabOnMaxLength(DependencyObject d)
        {
            return (bool)d.GetValue(AutoTabOnMaxLengthProperty);
        }

        /// <summary>
        /// Sets the AutoTabOnMaxLength property. This dependency property 
        /// indicates whether the focus should switch to next element 
        /// when the cursor moves to the position at maximum length of the field..
        /// </summary>
        public static void SetAutoTabOnMaxLength(DependencyObject d, bool value)
        {
            d.SetValue(AutoTabOnMaxLengthProperty, value);
        }

        /// <summary>
        /// Handles changes to the AutoTabOnMaxLength property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnAutoTabOnMaxLengthChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool newAutoTabOnMaxLength = (bool)d.GetValue(AutoTabOnMaxLengthProperty);
            var handler = GetPasswordAutoTabOnMaxLengthHandler(d);

            if (handler != null)
            {
                handler.Detach();
                SetPasswordAutoTabOnMaxLengthHandler(d, null);
            }

            if (newAutoTabOnMaxLength)
            {
                handler = new PasswordAutoTabOnMaxLengthHandler((PasswordBox)d);
                SetPasswordAutoTabOnMaxLengthHandler(d, handler);
            }
        }
        #endregion

        #region PasswordAutoTabOnMaxLengthHandler
        /// <summary>
        /// PasswordAutoTabOnMaxLengthHandler Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty PasswordAutoTabOnMaxLengthHandlerProperty =
            DependencyProperty.RegisterAttached(
                "PasswordAutoTabOnMaxLengthHandler",
                typeof(PasswordAutoTabOnMaxLengthHandler),
                typeof(PasswordBoxFocusExtensions),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the PasswordAutoTabOnMaxLengthHandler property. This dependency property 
        /// indicates the handler object for the AutoTabOnMaxLength property.
        /// There needs to be a separate attached object to avoid memory leaks.
        /// This property should not be set manually - only in the OnAutoTabOnMaxLengthChanged method.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static PasswordAutoTabOnMaxLengthHandler GetPasswordAutoTabOnMaxLengthHandler(DependencyObject d)
        {
            return (PasswordAutoTabOnMaxLengthHandler)d.GetValue(PasswordAutoTabOnMaxLengthHandlerProperty);
        }

        /// <summary>
        /// Sets the PasswordAutoTabOnMaxLengthHandler property. This dependency property 
        /// indicates the handler object for the AutoTabOnMaxLength property.
        /// There needs to be a separate attached object to avoid memory leaks.
        /// This property should not be set manually - only in the OnAutoTabOnMaxLengthChanged method.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetPasswordAutoTabOnMaxLengthHandler(DependencyObject d, PasswordAutoTabOnMaxLengthHandler value)
        {
            d.SetValue(PasswordAutoTabOnMaxLengthHandlerProperty, value);
        }
        #endregion

        #region AutoSelectOnFocus
        /// <summary>
        /// AutoSelectOnFocus Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty AutoSelectOnFocusProperty =
            DependencyProperty.RegisterAttached(
                "AutoSelectOnFocus",
                typeof(bool),
                typeof(PasswordBoxFocusExtensions),
                new PropertyMetadata(false, OnAutoSelectOnFocusChanged));

        /// <summary>
        /// Gets the AutoSelectOnFocus property. This dependency property 
        /// indicates whether the content should be selected when the control receives focus.
        /// </summary>
        public static bool GetAutoSelectOnFocus(DependencyObject d)
        {
            return (bool)d.GetValue(AutoSelectOnFocusProperty);
        }

        /// <summary>
        /// Sets the AutoSelectOnFocus property. This dependency property 
        /// indicates whether the content should be selected when the control receives focus.
        /// </summary>
        public static void SetAutoSelectOnFocus(DependencyObject d, bool value)
        {
            d.SetValue(AutoSelectOnFocusProperty, value);
        }

        /// <summary>
        /// Handles changes to the AutoSelectOnFocus property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnAutoSelectOnFocusChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool newAutoSelectOnFocus = (bool)d.GetValue(AutoSelectOnFocusProperty);

            var handler = GetPasswordAutoSelectOnFocusHandler(d);

            if (handler != null)
            {
                handler.Detach();
                SetPasswordAutoSelectOnFocusHandler(d, null);
            }

            if (newAutoSelectOnFocus)
            {
                handler = new PasswordAutoSelectOnFocusHandler((PasswordBox)d);
                SetPasswordAutoSelectOnFocusHandler(d, handler);
            }
        }
        #endregion

        #region PasswordAutoSelectOnFocusHandler
        /// <summary>
        /// PasswordAutoSelectOnFocusHandler Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty PasswordAutoSelectOnFocusHandlerProperty =
            DependencyProperty.RegisterAttached(
                "PasswordAutoSelectOnFocusHandler",
                typeof(PasswordAutoSelectOnFocusHandler),
                typeof(PasswordBoxFocusExtensions),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the PasswordAutoSelectOnFocusHandler property. This dependency property 
        /// indicates the handler for the AutoSelectOnFocus property.
        /// There needs to be a separate attached object to avoid memory leaks.
        /// This property should not be set manually - only in the OnAutoSelectOnFocusChanged method..
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static PasswordAutoSelectOnFocusHandler GetPasswordAutoSelectOnFocusHandler(DependencyObject d)
        {
            return (PasswordAutoSelectOnFocusHandler)d.GetValue(PasswordAutoSelectOnFocusHandlerProperty);
        }

        /// <summary>
        /// Sets the PasswordAutoSelectOnFocusHandler property. This dependency property 
        /// indicates the handler for the AutoSelectOnFocus property.
        /// There needs to be a separate attached object to avoid memory leaks.
        /// This property should not be set manually - only in the OnAutoSelectOnFocusChanged method..
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetPasswordAutoSelectOnFocusHandler(DependencyObject d, PasswordAutoSelectOnFocusHandler value)
        {
            d.SetValue(PasswordAutoSelectOnFocusHandlerProperty, value);
        }
        #endregion

        #region DisableSearchPaneOnFocus
        /// <summary>
        /// DisableSearchPaneOnFocus Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty DisableSearchPaneOnFocusProperty =
            DependencyProperty.RegisterAttached(
                "DisableSearchPaneOnFocus",
                typeof(bool),
                typeof(PasswordBoxFocusExtensions),
                new PropertyMetadata(false, OnDisableSearchPaneOnFocusChanged));

        /// <summary>
        /// Gets the DisableSearchPaneOnFocus property. This dependency property 
        /// indicates whether SearchPane.ShowOnKeyboardInput should be disabled when the control is focused.
        /// </summary>
        public static bool GetDisableSearchPaneOnFocus(DependencyObject d)
        {
            return (bool)d.GetValue(DisableSearchPaneOnFocusProperty);
        }

        /// <summary>
        /// Sets the DisableSearchPaneOnFocus property. This dependency property 
        /// indicates whether SearchPane.ShowOnKeyboardInput should be disabled when the control is focused.
        /// </summary>
        public static void SetDisableSearchPaneOnFocus(DependencyObject d, bool value)
        {
            d.SetValue(DisableSearchPaneOnFocusProperty, value);
        }

        /// <summary>
        /// Handles changes to the DisableSearchPaneOnFocus property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnDisableSearchPaneOnFocusChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
#if WINDOWS_APP
            bool newDisableSearchPaneOnFocus = (bool)d.GetValue(DisableSearchPaneOnFocusProperty);

            var handler = GetPasswordDisableSearchPaneOnFocusHandler(d);

            if (handler != null)
            {
                handler.Detach();
                SetPasswordDisableSearchPaneOnFocusHandler(d, null);
            }

            if (newDisableSearchPaneOnFocus)
            {
                handler = new PasswordDisableSearchPaneOnFocusHandler((PasswordBox)d);
                SetPasswordDisableSearchPaneOnFocusHandler(d, handler);
            }
#endif
        }
        #endregion

#if WINDOWS_APP
        #region PasswordDisableSearchPaneOnFocusHandler
        /// <summary>
        /// PasswordDisableSearchPaneOnFocusHandler Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty PasswordDisableSearchPaneOnFocusHandlerProperty =
            DependencyProperty.RegisterAttached(
                "PasswordDisableSearchPaneOnFocusHandler",
                typeof(PasswordDisableSearchPaneOnFocusHandler),
                typeof(PasswordBoxFocusExtensions),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the PasswordDisableSearchPaneOnFocusHandler property. This dependency property 
        /// indicates the handler for the DisableSearchPaneOnFocus property.
        /// There needs to be a separate attached object to avoid memory leaks.
        /// This property should not be set manually - only in the OnDisableSearchPaneOnFocusChanged method..
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static PasswordDisableSearchPaneOnFocusHandler GetPasswordDisableSearchPaneOnFocusHandler(DependencyObject d)
        {
            return (PasswordDisableSearchPaneOnFocusHandler)d.GetValue(PasswordDisableSearchPaneOnFocusHandlerProperty);
        }

        /// <summary>
        /// Sets the PasswordDisableSearchPaneOnFocusHandler property. This dependency property 
        /// indicates the handler for the DisableSearchPaneOnFocus property.
        /// There needs to be a separate attached object to avoid memory leaks.
        /// This property should not be set manually - only in the OnDisableSearchPaneOnFocusChanged method..
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetPasswordDisableSearchPaneOnFocusHandler(DependencyObject d, PasswordDisableSearchPaneOnFocusHandler value)
        {
            d.SetValue(PasswordDisableSearchPaneOnFocusHandlerProperty, value);
        }
        #endregion
#endif
    }

    /// <summary>
    /// Handler object type for the AutoTabOnMaxLength property.
    /// </summary>
    public class PasswordAutoTabOnMaxLengthHandler
    {
        private PasswordBox _associatedObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordAutoTabOnMaxLengthHandler"/> class.
        /// </summary>
        /// <param name="associatedObject">The associated object.</param>
        public PasswordAutoTabOnMaxLengthHandler(PasswordBox associatedObject)
        {
            Attach(associatedObject);
        }

        private void Attach(PasswordBox associatedObject)
        {
            Detach();
            _associatedObject = associatedObject;
            _associatedObject.KeyUp += AssociatedObjectOnKeyUp;
        }

        private void AssociatedObjectOnKeyUp(object sender, KeyRoutedEventArgs keyRoutedEventArgs)
        {
            if (_associatedObject.Password.Length == _associatedObject.MaxLength &&
                !IsSystemKey(keyRoutedEventArgs.Key))
            {
                _associatedObject.MoveFocusForward();
            }
        }

        private static bool IsSystemKey(VirtualKey key)
        {
            return
                key == VirtualKey.Menu ||
                key == VirtualKey.LeftMenu ||
                key == VirtualKey.RightMenu ||
                key == VirtualKey.LeftControl ||
                key == VirtualKey.RightControl ||
                key == VirtualKey.LeftWindows ||
                key == VirtualKey.RightWindows ||
                key == VirtualKey.Shift ||
                key == VirtualKey.Up ||
                key == VirtualKey.Right ||
                key == VirtualKey.Down ||
                key == VirtualKey.Left ||
                key == VirtualKey.LeftShift ||
                key == VirtualKey.RightShift ||
                key == VirtualKey.Tab ||
                key == VirtualKey.Back ||
                key == VirtualKey.Delete ||
                key == VirtualKey.F1 ||
                key == VirtualKey.F2 ||
                key == VirtualKey.F3 ||
                key == VirtualKey.F4 ||
                key == VirtualKey.F5 ||
                key == VirtualKey.F6 ||
                key == VirtualKey.F7 ||
                key == VirtualKey.F8 ||
                key == VirtualKey.F9 ||
                key == VirtualKey.F10 ||
                key == VirtualKey.F11 ||
                key == VirtualKey.F12 ||
                key == VirtualKey.F13 ||
                key == VirtualKey.F14 ||
                key == VirtualKey.F15 ||
                key == VirtualKey.F16 ||
                key == VirtualKey.F17 ||
                key == VirtualKey.F18 ||
                key == VirtualKey.F19 ||
                key == VirtualKey.F20 ||
                key == VirtualKey.F21 ||
                key == VirtualKey.F22 ||
                key == VirtualKey.F23 ||
                key == VirtualKey.F24 ||
                key == VirtualKey.PageDown ||
                key == VirtualKey.PageUp ||
                key == VirtualKey.Home ||
                key == VirtualKey.End ||
                key == VirtualKey.Scroll ||
                key == VirtualKey.Insert ||
                key == VirtualKey.Escape;
        }

        /// <summary>
        /// Detaches this instance.
        /// </summary>
        public void Detach()
        {
            if (_associatedObject == null)
                return;

            _associatedObject.KeyUp -= AssociatedObjectOnKeyUp;
            _associatedObject = null;
        }
    }

    /// <summary>
    /// Handler object type for the AutoSelectOnFocus property.
    /// </summary>
    public class PasswordAutoSelectOnFocusHandler
    {
        private PasswordBox _associatedObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordAutoSelectOnFocusHandler"/> class.
        /// </summary>
        /// <param name="associatedObject">The associated object.</param>
        public PasswordAutoSelectOnFocusHandler(PasswordBox associatedObject)
        {
            Attach(associatedObject);
        }

        private void Attach(PasswordBox associatedObject)
        {
            Detach();
            _associatedObject = associatedObject;
            _associatedObject.GotFocus += AssociatedObjectOnGotFocus;
        }

        private void AssociatedObjectOnGotFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            _associatedObject.SelectAll();
        }

        /// <summary>
        /// Detaches this instance.
        /// </summary>
        public void Detach()
        {
            if (_associatedObject == null)
                return;

            _associatedObject.KeyUp -= AssociatedObjectOnGotFocus;
            _associatedObject = null;
        }
    }

#if WINDOWS_APP
    /// <summary>
    /// Handler object type for the DisableSearchPaneOnFocus property.
    /// </summary>
    public class PasswordDisableSearchPaneOnFocusHandler
    {
        private PasswordBox _associatedObject;
        private bool _searchPaneShowOnKeyboardInput;

        private static bool? _isSearchEnabled;

        /// <summary>
        /// Gets or sets a value indicating whether search is currently enabled in the app.
        /// </summary>
        public static bool IsSearchEnabled
        {
            get
            {
                if (_isSearchEnabled == null)
                {
                    try
                    {
                        _isSearchEnabled = SearchPane.GetForCurrentView() != null;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        Debug.WriteLine("Checking for Search capability throws exceptions when the capability is missing. To avoid it set WinRTXamlToolkit.Controls.Extensions.PasswordDisableSearchPaneOnFocusHandler.IsSearchEnabled explicitly before WinRTXamlToolkit.Controls.Extensions.PasswordBoxFocusExtensions.DisableSearchPaneOnFocus behavior is applied.");
                        Debug.WriteLine("Set PasswordDisableSearchPaneOnFocusHandler.IsSearchEnabled = false; to avoid the exception in the future");
                        _isSearchEnabled = false;
                    }
                }

                return _isSearchEnabled.Value;
            }
            set
            {
                _isSearchEnabled = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordDisableSearchPaneOnFocusHandler"/> class.
        /// </summary>
        /// <param name="associatedObject">The associated object.</param>
        public PasswordDisableSearchPaneOnFocusHandler(PasswordBox associatedObject)
        {
            Attach(associatedObject);
        }

        private void Attach(PasswordBox associatedObject)
        {
            Detach();

            if (!IsSearchEnabled)
            {
                return;
            }

            _associatedObject = associatedObject;
            _associatedObject.GotFocus += AssociatedObjectOnGotFocus;
            _associatedObject.LostFocus += AssociatedObjectOnLostFocus;
        }

        private void AssociatedObjectOnGotFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            _searchPaneShowOnKeyboardInput = SearchPane.GetForCurrentView().ShowOnKeyboardInput;
            SearchPane.GetForCurrentView().ShowOnKeyboardInput = false;
        }

        private void AssociatedObjectOnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            SearchPane.GetForCurrentView().ShowOnKeyboardInput = _searchPaneShowOnKeyboardInput;
        }

        /// <summary>
        /// Detaches this instance.
        /// </summary>
        public void Detach()
        {
            if (_associatedObject == null)
                return;

            _associatedObject.GotFocus -= AssociatedObjectOnGotFocus;
            _associatedObject.LostFocus -= AssociatedObjectOnLostFocus;
            _associatedObject = null;
        }
    }
#endif
}
