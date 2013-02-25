using System.ComponentModel;
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
}
