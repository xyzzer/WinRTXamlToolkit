using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.AwaitableUI;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Attached properties for use with a ListViewItem.
    /// </summary>
    /// <remarks>
    /// Note that the IsEnabled property is to be used on an element inside of a ListView and not the ListViewItem itself.
    /// </remarks>
    public static class ListViewItemExtensions
    {
        #region IsEnabled
        /// <summary>
        /// IsEnabled Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(ListViewItemExtensions),
                new PropertyMetadata(true, OnIsEnabledChanged));

        /// <summary>
        /// Gets the IsEnabled property. This dependency property 
        /// indicates whether the first ListViewItem found in ancestors is enabled.
        /// </summary>
        /// <remarks>
        /// Note that the IsEnabled property is to be used on an element inside of a ListView and not the ListViewItem itself.
        /// Setting this property will update the IsEnabled property of the ListViewItem making it easier to
        /// disable selection of ListViewItems in the databound collection scenarios.
        /// </remarks>
        public static bool GetIsEnabled(DependencyObject d)
        {
            return (bool)d.GetValue(IsEnabledProperty);
        }

        /// <summary>
        /// Sets the IsEnabled property. This dependency property 
        /// indicates whether the first ListViewItem found in ancestors is enabled.
        /// </summary>
        /// <remarks>
        /// Note that the IsEnabled property is to be used on an element inside of a ListView and not the ListViewItem itself.
        /// Setting this property will update the IsEnabled property of the ListViewItem making it easier to
        /// disable selection of ListViewItems in the databound collection scenarios.
        /// </remarks>
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

            var listViewItem =
                d as ListViewItem ??
                d.GetAncestors().OfType<ListViewItem>().FirstOrDefault();
            if (listViewItem == null)
                return;
            //throw new InvalidOperationException("ListViewItemExtensions.IsEnabled can only be set on a ListViewItem or its descendant in the visual tree");

            listViewItem.IsEnabled = newIsEnabled;
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
                typeof(ListViewItemExtensions),
                new PropertyMetadata(false, OnIsSelectedChanged));

        /// <summary>
        /// Gets the IsSelected property. This dependency property 
        /// indicates whether the first ListViewItem found in ancestors is selected.
        /// </summary>
        /// <remarks>
        /// Note that the IsSelected property is to be used on an element inside of a ListView and not the ListViewItem itself.
        /// Setting this property will update the IsSelected property of the ListViewItem making it easier to
        /// disable selection of ListViewItems in the databound collection scenarios.
        /// </remarks>
        public static bool GetIsSelected(DependencyObject d)
        {
            return (bool)d.GetValue(IsSelectedProperty);
        }

        /// <summary>
        /// Sets the IsSelected property. This dependency property 
        /// indicates whether the first ListViewItem found in ancestors is selected.
        /// </summary>
        /// <remarks>
        /// Note that the IsSelected property is to be used on an element inside of a ListView and not the ListViewItem itself.
        /// Setting this property will update the IsSelected property of the ListViewItem making it easier to
        /// disable selection of ListViewItems in the databound collection scenarios.
        /// </remarks>
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

            var listViewItem =
                d as ListViewItem ??
                d.GetAncestors().OfType<ListViewItem>().FirstOrDefault();
            if (listViewItem == null)
                return;
            //throw new InvalidOperationException("ListViewItemExtensions.IsSelected can only be set on a ListViewItem or its descendant in the visual tree");
            listViewItem.IsSelected = newIsSelected;
        }
        #endregion
    }
}
