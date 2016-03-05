using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.AwaitableUI;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Attached properties for use with a ListBoxItem.
    /// </summary>
    /// <remarks>
    /// Note that the IsEnabled property is to be used on an element inside of a ListBox and not the ListBoxItem itself.
    /// </remarks>
    public static class ListBoxItemExtensions
    {
        #region IsEnabled
        /// <summary>
        /// IsEnabled Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(ListBoxItemExtensions),
                new PropertyMetadata(true, OnIsEnabledChanged));

        /// <summary>
        /// Gets the IsEnabled property. This dependency property 
        /// indicates whether the first ListBoxItem found in ancestors is enabled.
        /// </summary>
        /// <remarks>
        /// Note that the IsEnabled property is to be used on an element inside of a ListBox and not the ListBoxItem itself.
        /// Setting this property will update the IsEnabled property of the ListBoxItem making it easier to
        /// disable selection of ListBoxItems in the databound collection scenarios.
        /// </remarks>
        public static bool GetIsEnabled(DependencyObject d)
        {
            return (bool)d.GetValue(IsEnabledProperty);
        }

        /// <summary>
        /// Sets the IsEnabled property. This dependency property 
        /// indicates whether the first ListBoxItem found in ancestors is enabled.
        /// </summary>
        /// <remarks>
        /// Note that the IsEnabled property is to be used on an element inside of a ListBox and not the ListBoxItem itself.
        /// Setting this property will update the IsEnabled property of the ListBoxItem making it easier to
        /// disable selection of ListBoxItems in the databound collection scenarios.
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

            var listBoxItem =
                d as ListBoxItem ??
                d.GetAncestors().OfType<ListBoxItem>().FirstOrDefault();
            if (listBoxItem == null)
                return;
            //throw new InvalidOperationException("ListBoxItemExtensions.IsEnabled can only be set on a ListBoxItem or its descendant in the visual tree");

            listBoxItem.IsEnabled = newIsEnabled;
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
                typeof(ListBoxItemExtensions),
                new PropertyMetadata(false, OnIsSelectedChanged));

        /// <summary>
        /// Gets the IsSelected property. This dependency property 
        /// indicates whether the first ListBoxItem found in ancestors is selected.
        /// </summary>
        /// <remarks>
        /// Note that the IsSelected property is to be used on an element inside of a ListBox and not the ListBoxItem itself.
        /// Setting this property will update the IsSelected property of the ListBoxItem making it easier to
        /// disable selection of ListBoxItems in the databound collection scenarios.
        /// </remarks>
        public static bool GetIsSelected(DependencyObject d)
        {
            return (bool)d.GetValue(IsSelectedProperty);
        }

        /// <summary>
        /// Sets the IsSelected property. This dependency property 
        /// indicates whether the first ListBoxItem found in ancestors is selected.
        /// </summary>
        /// <remarks>
        /// Note that the IsSelected property is to be used on an element inside of a ListBox and not the ListBoxItem itself.
        /// Setting this property will update the IsSelected property of the ListBoxItem making it easier to
        /// disable selection of ListBoxItems in the databound collection scenarios.
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

            var ListBoxItem =
                d as ListBoxItem ??
                d.GetAncestors().OfType<ListBoxItem>().FirstOrDefault();
            if (ListBoxItem == null)
                return;
            //throw new InvalidOperationException("ListBoxItemExtensions.IsSelected can only be set on a ListBoxItem or its descendant in the visual tree");
            ListBoxItem.IsSelected = newIsSelected;
        }
        #endregion
    }
}
