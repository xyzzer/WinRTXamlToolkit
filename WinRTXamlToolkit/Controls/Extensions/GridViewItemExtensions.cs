using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.AwaitableUI;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Extension methods and properties for the GridViewItem class.
    /// </summary>
    public static class GridViewItemExtensions
    {
        #region IsEnabled
        /// <summary>
        /// IsEnabled Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(GridViewItemExtensions),
                new PropertyMetadata(true, OnIsEnabledChanged));

        /// <summary>
        /// Gets the IsEnabled property. This dependency property 
        /// indicates whether the first GridViewItem found in ancestors is enabled.
        /// </summary>
        public static bool GetIsEnabled(DependencyObject d)
        {
            return (bool)d.GetValue(IsEnabledProperty);
        }

        /// <summary>
        /// Sets the IsEnabled property. This dependency property 
        /// indicates whether the first GridViewItem found in ancestors is enabled.
        /// </summary>
        public static void SetIsEnabled(DependencyObject d, bool value)
        {
            d.SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// Handles changes to the IsEnabled property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static async void OnIsEnabledChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool oldIsEnabled = (bool)e.OldValue;
            bool newIsEnabled = (bool)d.GetValue(IsEnabledProperty);

            if (!d.IsInVisualTree())
                await ((FrameworkElement)d).WaitForLoadedAsync();

            var gridViewItem =
                d as GridViewItem ??
                d.GetAncestors().OfType<GridViewItem>().FirstOrDefault();

            if (gridViewItem == null)
                return;
                //throw new InvalidOperationException("GridViewItemExtensions.IsEnabled can only be set on a GridViewItem or its descendant in the visual tree");

            gridViewItem.IsEnabled = newIsEnabled;
        }
        #endregion

        #region IsSelected
        /// <summary>
        /// IsSelected Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.RegisterAttached(
                "IsSelected",
                typeof(bool),
                typeof(GridViewItemExtensions),
                new PropertyMetadata(false, OnIsSelectedChanged));

        /// <summary>
        /// Gets the IsSelected property. This dependency property 
        /// indicates whether the first GridViewItem found in ancestors is selected.
        /// </summary>
        public static bool GetIsSelected(DependencyObject d)
        {
            return (bool)d.GetValue(IsSelectedProperty);
        }

        /// <summary>
        /// Sets the IsSelected property. This dependency property 
        /// indicates whether the first GridViewItem found in ancestors is selected.
        /// </summary>
        public static void SetIsSelected(DependencyObject d, bool value)
        {
            d.SetValue(IsSelectedProperty, value);
        }

        /// <summary>
        /// Handles changes to the IsSelected property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static async void OnIsSelectedChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool oldIsSelected = (bool)e.OldValue;
            bool newIsSelected = (bool)d.GetValue(IsSelectedProperty);

            if (!d.IsInVisualTree())
                await ((FrameworkElement)d).WaitForLoadedAsync();

            var gridViewItem =
                d as GridViewItem ??
                d.GetAncestors().OfType<GridViewItem>().FirstOrDefault();

            if (gridViewItem == null)
                return;
            //throw new InvalidOperationException("GridViewItemExtensions.IsSelected can only be set on a GridViewItem or its descendant in the visual tree");

            gridViewItem.IsSelected = newIsSelected;
        }
        #endregion
    }
}
