using System.ComponentModel;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Extensions for the AppBar class
    /// </summary>
    public static class AppBarExtensions
    {
        #region HideWhenSnapped
        /// <summary>
        /// HideWhenSnapped Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty HideWhenSnappedProperty =
            DependencyProperty.RegisterAttached(
                "HideWhenSnapped",
                typeof(bool),
                typeof(AppBarExtensions),
                new PropertyMetadata(false, OnHideWhenSnappedChanged));

        /// <summary>
        /// Gets the HideWhenSnapped property. This dependency property 
        /// indicates whether the AppBar should be hidden if the current view is Snapped.
        /// </summary>
        /// <remarks>
        /// Note that it will still invisibly open/close on regular gestures,
        /// so wheh the app gets unsnapped - it might be opened or closed depending on what happens in between.
        /// The CustomAppBar might be a better choice for more control with its CanOpenInSnappedView property.
        /// </remarks>
        public static bool GetHideWhenSnapped(DependencyObject d)
        {
            return (bool)d.GetValue(HideWhenSnappedProperty);
        }

        /// <summary>
        /// Sets the HideWhenSnapped property. This dependency property 
        /// indicates whether the AppBar should be hidden if the current view is Snapped.
        /// </summary>
        /// <remarks>
        /// Note that it will still invisibly open/close on regular gestures,
        /// so wheh the app gets unsnapped - it might be opened or closed depending on what happens in between.
        /// The CustomAppBar might be a better choice for more control with its CanOpenInSnappedView property.
        /// </remarks>
        public static void SetHideWhenSnapped(DependencyObject d, bool value)
        {
            d.SetValue(HideWhenSnappedProperty, value);
        }

        /// <summary>
        /// Handles changes to the HideWhenSnapped property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnHideWhenSnappedChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool oldHideWhenSnapped = (bool)e.OldValue;
            bool newHideWhenSnapped = (bool)d.GetValue(HideWhenSnappedProperty);

            if (newHideWhenSnapped)
            {
                var handler = new HideWhenSnappedHandler((AppBar)d);
                SetHideWhenSnappedHandler(d, handler);
            }
            else
            {
                var handler = GetHideWhenSnappedHandler(d);
                SetHideWhenSnappedHandler(d, null);
                handler.Detach();
            }
        }
        #endregion

        #region HideWhenSnappedHandler
        /// <summary>
        /// HideWhenSnappedHandler Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty HideWhenSnappedHandlerProperty =
            DependencyProperty.RegisterAttached(
                "HideWhenSnappedHandler",
                typeof(HideWhenSnappedHandler),
                typeof(AppBarExtensions),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the HideWhenSnappedHandler property. This dependency property 
        /// indicates the handler for HideWhenSnapped behavior.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static HideWhenSnappedHandler GetHideWhenSnappedHandler(DependencyObject d)
        {
            return (HideWhenSnappedHandler)d.GetValue(HideWhenSnappedHandlerProperty);
        }

        /// <summary>
        /// Sets the HideWhenSnappedHandler property. This dependency property 
        /// indicates the handler for HideWhenSnapped behavior.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void SetHideWhenSnappedHandler(DependencyObject d, HideWhenSnappedHandler value)
        {
            d.SetValue(HideWhenSnappedHandlerProperty, value);
        }
        #endregion
    }

    /// <summary>
    /// Handles hiding an AppBar when the app goes to a snapped view.
    /// </summary>
    public class HideWhenSnappedHandler
    {
        private AppBar _appBar;

        public HideWhenSnappedHandler(AppBar appBar)
        {
            Attach(appBar);
        }

        private void Attach(AppBar appBar)
        {
            _appBar = appBar;
            Window.Current.SizeChanged += WindowSizeChanged;
            _appBar.Unloaded += OnAppBarUnloaded;
            UpdateAppBarVisibility();
        }

        private void OnAppBarUnloaded(object sender, RoutedEventArgs e)
        {
            Detach();
        }

        internal void Detach()
        {
            Window.Current.SizeChanged -= WindowSizeChanged;
            _appBar.Unloaded -= OnAppBarUnloaded;
            _appBar = null;
        }

        private void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            if (_appBar.IsInVisualTree())
            {
                UpdateAppBarVisibility();
            }
            else
            {
                Detach();
            }
        }

        private void UpdateAppBarVisibility()
        {
            _appBar.Visibility = ApplicationView.Value != ApplicationViewState.Snapped ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
