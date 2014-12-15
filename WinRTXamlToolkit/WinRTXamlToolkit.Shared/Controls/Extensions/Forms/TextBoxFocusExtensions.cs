using System;
using System.ComponentModel;
using System.Diagnostics;
#if WINDOWS_APP
using Windows.ApplicationModel.Search;
#endif
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Contains an extension for automatically switching focus to next field when the cursor reaches the MexLength position.
    /// </summary>
    public static class TextBoxFocusExtensions
    {
        #region AutoTabOnMaxLength
        /// <summary>
        /// AutoTabOnMaxLength Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty AutoTabOnMaxLengthProperty =
            DependencyProperty.RegisterAttached(
                "AutoTabOnMaxLength",
                typeof(bool),
                typeof(TextBoxFocusExtensions),
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
            var handler = GetAutoTabOnMaxLengthHandler(d);

            if (handler != null)
            {
                handler.Detach();
                SetAutoTabOnMaxLengthHandler(d, null);
            }

            if (newAutoTabOnMaxLength)
            {
                handler = new AutoTabOnMaxLengthHandler((TextBox)d);
                SetAutoTabOnMaxLengthHandler(d, handler);
            }
        }
        #endregion

        #region AutoTabOnMaxLengthHandler
        /// <summary>
        /// AutoTabOnMaxLengthHandler Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty AutoTabOnMaxLengthHandlerProperty =
            DependencyProperty.RegisterAttached(
                "AutoTabOnMaxLengthHandler",
                typeof(AutoTabOnMaxLengthHandler),
                typeof(TextBoxFocusExtensions),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the AutoTabOnMaxLengthHandler property. This dependency property 
        /// indicates the handler object for the AutoTabOnMaxLength property.
        /// There needs to be a separate attached object to avoid memory leaks.
        /// This property should not be set manually - only in the OnAutoTabOnMaxLengthChanged method.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static AutoTabOnMaxLengthHandler GetAutoTabOnMaxLengthHandler(DependencyObject d)
        {
            return (AutoTabOnMaxLengthHandler)d.GetValue(AutoTabOnMaxLengthHandlerProperty);
        }

        /// <summary>
        /// Sets the AutoTabOnMaxLengthHandler property. This dependency property 
        /// indicates the handler object for the AutoTabOnMaxLength property.
        /// There needs to be a separate attached object to avoid memory leaks.
        /// This property should not be set manually - only in the OnAutoTabOnMaxLengthChanged method.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetAutoTabOnMaxLengthHandler(DependencyObject d, AutoTabOnMaxLengthHandler value)
        {
            d.SetValue(AutoTabOnMaxLengthHandlerProperty, value);
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
                typeof(TextBoxFocusExtensions),
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

            var handler = GetAutoSelectOnFocusHandler(d);

            if (handler != null)
            {
                handler.Detach();
                SetAutoSelectOnFocusHandler(d, null);
            }

            if (newAutoSelectOnFocus)
            {
                handler = new AutoSelectOnFocusHandler((TextBox)d);
                SetAutoSelectOnFocusHandler(d, handler);
            }
        }
        #endregion

        #region AutoSelectOnFocusHandler
        /// <summary>
        /// AutoSelectOnFocusHandler Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty AutoSelectOnFocusHandlerProperty =
            DependencyProperty.RegisterAttached(
                "AutoSelectOnFocusHandler",
                typeof(AutoSelectOnFocusHandler),
                typeof(TextBoxFocusExtensions),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the AutoSelectOnFocusHandler property. This dependency property 
        /// indicates the handler for the AutoSelectOnFocus property.
        /// There needs to be a separate attached object to avoid memory leaks.
        /// This property should not be set manually - only in the OnAutoSelectOnFocusChanged method..
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static AutoSelectOnFocusHandler GetAutoSelectOnFocusHandler(DependencyObject d)
        {
            return (AutoSelectOnFocusHandler)d.GetValue(AutoSelectOnFocusHandlerProperty);
        }

        /// <summary>
        /// Sets the AutoSelectOnFocusHandler property. This dependency property 
        /// indicates the handler for the AutoSelectOnFocus property.
        /// There needs to be a separate attached object to avoid memory leaks.
        /// This property should not be set manually - only in the OnAutoSelectOnFocusChanged method..
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetAutoSelectOnFocusHandler(DependencyObject d, AutoSelectOnFocusHandler value)
        {
            d.SetValue(AutoSelectOnFocusHandlerProperty, value);
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
                typeof(TextBoxFocusExtensions),
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

            var handler = GetDisableSearchPaneOnFocusHandler(d);

            if (handler != null)
            {
                handler.Detach();
                SetDisableSearchPaneOnFocusHandler(d, null);
            }

            if (newDisableSearchPaneOnFocus)
            {
                handler = new DisableSearchPaneOnFocusHandler((TextBox)d);
                SetDisableSearchPaneOnFocusHandler(d, handler);
            }
#endif
        }
        #endregion

#if WINDOWS_APP
        #region DisableSearchPaneOnFocusHandler
        /// <summary>
        /// DisableSearchPaneOnFocusHandler Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty DisableSearchPaneOnFocusHandlerProperty =
            DependencyProperty.RegisterAttached(
                "DisableSearchPaneOnFocusHandler",
                typeof(DisableSearchPaneOnFocusHandler),
                typeof(TextBoxFocusExtensions),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the DisableSearchPaneOnFocusHandler property. This dependency property 
        /// indicates the handler for the DisableSearchPaneOnFocus property.
        /// There needs to be a separate attached object to avoid memory leaks.
        /// This property should not be set manually - only in the OnDisableSearchPaneOnFocusChanged method..
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static DisableSearchPaneOnFocusHandler GetDisableSearchPaneOnFocusHandler(DependencyObject d)
        {
            return (DisableSearchPaneOnFocusHandler)d.GetValue(DisableSearchPaneOnFocusHandlerProperty);
        }

        /// <summary>
        /// Sets the DisableSearchPaneOnFocusHandler property. This dependency property 
        /// indicates the handler for the DisableSearchPaneOnFocus property.
        /// There needs to be a separate attached object to avoid memory leaks.
        /// This property should not be set manually - only in the OnDisableSearchPaneOnFocusChanged method..
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetDisableSearchPaneOnFocusHandler(DependencyObject d, DisableSearchPaneOnFocusHandler value)
        {
            d.SetValue(DisableSearchPaneOnFocusHandlerProperty, value);
        }
        #endregion
#endif
    }

    /// <summary>
    /// Handler object type for the AutoTabOnMaxLength property.
    /// </summary>
    public class AutoTabOnMaxLengthHandler
    {
        private TextBox _associatedObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoTabOnMaxLengthHandler"/> class.
        /// </summary>
        /// <param name="associatedObject">The associated object.</param>
        public AutoTabOnMaxLengthHandler(TextBox associatedObject)
        {
            Attach(associatedObject);
        }

        private void Attach(TextBox associatedObject)
        {
            Detach();
            _associatedObject = associatedObject;
            _associatedObject.SelectionChanged += AssociatedObjectOnSelectionChanged;
        }

        private void AssociatedObjectOnSelectionChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_associatedObject.SelectionStart == _associatedObject.MaxLength)
            {
                _associatedObject.MoveFocusForward();
            }
        }

        /// <summary>
        /// Detaches this instance.
        /// </summary>
        public void Detach()
        {
            if (_associatedObject == null)
                return;

            _associatedObject.SelectionChanged -= AssociatedObjectOnSelectionChanged;
            _associatedObject = null;
        }


        /// <summary>
        /// Handler object type for the AutoSelectOnFocus property.
        /// </summary>
        public class AutoSelectOnFocusHandler
        {
            private TextBox _associatedObject;

            /// <summary>
            /// Initializes a new instance of the <see cref="AutoSelectOnFocusHandler"/> class.
            /// </summary>
            /// <param name="associatedObject">The associated object.</param>
            public AutoSelectOnFocusHandler(TextBox associatedObject)
            {
                Attach(associatedObject);
            }

            private void Attach(TextBox associatedObject)
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
                _associatedObject.GotFocus -= AssociatedObjectOnGotFocus;
                _associatedObject = null;
            }
        }
    }

    /// <summary>
    /// Handler object type for the AutoSelectOnFocus property.
    /// </summary>
    public class AutoSelectOnFocusHandler
    {
        private TextBox _associatedObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoSelectOnFocusHandler"/> class.
        /// </summary>
        /// <param name="associatedObject">The associated object.</param>
        public AutoSelectOnFocusHandler(TextBox associatedObject)
        {
            Attach(associatedObject);
        }

        private void Attach(TextBox associatedObject)
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
    public class DisableSearchPaneOnFocusHandler
    {
        private TextBox _associatedObject;
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
                        Debug.WriteLine("Checking for Search capability throws exceptions when the capability is missing. To avoid it set WinRTXamlToolkit.Controls.Extensions.DisableSearchPaneOnFocusHandler.IsSearchEnabled explicitly before WinRTXamlToolkit.Controls.Extensions.TextBoxFocusExtensions.DisableSearchPaneOnFocus behavior is applied.");
                        Debug.WriteLine("Set DisableSearchPaneOnFocusHandler.IsSearchEnabled = false; to avoid the exception in the future");
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
        /// Initializes a new instance of the <see cref="DisableSearchPaneOnFocusHandler"/> class.
        /// </summary>
        /// <param name="associatedObject">The associated object.</param>
        public DisableSearchPaneOnFocusHandler(TextBox associatedObject)
        {
            Attach(associatedObject);
        }

        private void Attach(TextBox associatedObject)
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
