////using Windows.System;
////using Windows.UI.Core;
////using Windows.UI.Xaml;

////namespace WinRTXamlToolkit.Input
////{
////    /// <summary>
////    /// TODO: Keyboard helpers.
////    /// </summary>
////    public static class Keyboard
////    {
////        static Keyboard()
////        {
////        }

////        private static void Test()
////        {
////            var currentWindow = Window.Current;
////            var coreWindow = currentWindow.CoreWindow;
////            var s = coreWindow.GetAsyncKeyState(VirtualKey.Shift);

////            if (s == CoreVirtualKeyStates.None)
////            {
                
////            }

////            coreWindow.GetKeyState(VirtualKey.Shift);
////        }

////        #region KeyCommands
////        /// <summary>
////        /// KeyCommands Attached Dependency Property
////        /// </summary>
////        public static readonly DependencyProperty KeyCommandsProperty =
////            DependencyProperty.RegisterAttached(
////                "KeyCommands",
////                typeof(KeyCommandCollection),
////                typeof(Keyboard),
////                new PropertyMetadata(null, OnKeyCommandsChanged));

////        /// <summary>
////        /// Gets the KeyCommands property. This dependency property 
////        /// indicates the collection of commands to invoke with key combinations.
////        /// </summary>
////        public static KeyCommandCollection GetKeyCommands(DependencyObject d)
////        {
////            return (KeyCommandCollection)d.GetValue(KeyCommandsProperty);
////        }

////        /// <summary>
////        /// Sets the KeyCommands property. This dependency property 
////        /// indicates the collection of key commands associated with this element.
////        /// </summary>
////        public static void SetKeyCommands(DependencyObject d, KeyCommandCollection value)
////        {
////            d.SetValue(KeyCommandsProperty, value);
////        }

////        /// <summary>
////        /// Handles changes to the KeyCommands property.
////        /// </summary>
////        /// <param name="d">
////        /// The <see cref="DependencyObject"/> on which
////        /// the property has changed value.
////        /// </param>
////        /// <param name="e">
////        /// Event data that is issued by any event that
////        /// tracks changes to the effective value of this property.
////        /// </param>
////        private static void OnKeyCommandsChanged(
////            DependencyObject d, DependencyPropertyChangedEventArgs e)
////        {
////            var oldKeyCommands = (KeyCommandCollection)e.OldValue;
////            var newKeyCommands = (KeyCommandCollection)d.GetValue(KeyCommandsProperty);
////        }
////        #endregion
////    }
////}
