using System;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.AwaitableUI;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Page extensions. Allows to specify Page's top and bottom app bars by setting them on its descendant.
    /// </summary>
    public static class PageExtensions
    {
        #region BottomAppBar
        /// <summary>
        /// BottomAppBar Attached Dependency Property
        /// </summary>
        private static readonly DependencyProperty _BottomAppBarProperty =
            DependencyProperty.RegisterAttached(
                "BottomAppBar",
                typeof(AppBar),
                typeof(PageExtensions),
                new PropertyMetadata(null, OnBottomAppBarChanged));

        /// <summary>
        /// Identifies the BottomAppBar dependency property.
        /// </summary>
        public static DependencyProperty BottomAppBarProperty { get { return _BottomAppBarProperty; } }

        /// <summary>
        /// Gets the BottomAppBar property. This dependency property 
        /// indicates the bottom app bar to set on the page for the control on which this property is defined.
        /// </summary>
        public static AppBar GetBottomAppBar(DependencyObject d)
        {
            return (AppBar)d.GetValue(BottomAppBarProperty);
        }

        /// <summary>
        /// Sets the BottomAppBar property. This dependency property 
        /// indicates the bottom app bar to set on the page for the control on which this property is defined.
        /// </summary>
        public static void SetBottomAppBar(DependencyObject d, AppBar value)
        {
            d.SetValue(BottomAppBarProperty, value);
        }

        /// <summary>
        /// Handles changes to the BottomAppBar property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static async void OnBottomAppBarChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldBottomAppBar = (AppBar)e.OldValue;
            var newBottomAppBar = (AppBar)d.GetValue(BottomAppBarProperty);

            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            var fe = (FrameworkElement)d;
            var parentPage = fe.GetFirstAncestorOfType<Page>();

            if (parentPage == null)
            {
                await fe.WaitForLoadedAsync();
                parentPage = fe.GetFirstAncestorOfType<Page>();

                if (parentPage == null)
                {
                    throw new InvalidOperationException("PageExtensions.BottomAppBar is used to set the BottomAppBar on a parent page control and so it needs to be used in a control that is hosted in a Page.");
                }
            }

            parentPage.BottomAppBar = newBottomAppBar;
        }
        #endregion

        #region TopAppBar
        /// <summary>
        /// TopAppBar Attached Dependency Property
        /// </summary>
        private static readonly DependencyProperty _TopAppBarProperty =
            DependencyProperty.RegisterAttached(
                "TopAppBar",
                typeof(AppBar),
                typeof(PageExtensions),
                new PropertyMetadata(null, OnTopAppBarChanged));

        /// <summary>
        /// Identifies the TopAppBar dependency property.
        /// </summary>
        public static DependencyProperty TopAppBarProperty { get { return _TopAppBarProperty; } }

        /// <summary>
        /// Gets the TopAppBar property. This dependency property 
        /// indicates the Top app bar to set on the page for the control on which this property is defined.
        /// </summary>
        public static AppBar GetTopAppBar(DependencyObject d)
        {
            return (AppBar)d.GetValue(TopAppBarProperty);
        }

        /// <summary>
        /// Sets the TopAppBar property. This dependency property 
        /// indicates the Top app bar to set on the page for the control on which this property is defined.
        /// </summary>
        public static void SetTopAppBar(DependencyObject d, AppBar value)
        {
            d.SetValue(TopAppBarProperty, value);
        }

        /// <summary>
        /// Handles changes to the TopAppBar property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static async void OnTopAppBarChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldTopAppBar = (AppBar)e.OldValue;
            var newTopAppBar = (AppBar)d.GetValue(TopAppBarProperty);

            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            var fe = (FrameworkElement)d;
            var parentPage = fe.GetFirstAncestorOfType<Page>();

            if (parentPage == null)
            {
                await fe.WaitForLoadedAsync();
                parentPage = fe.GetFirstAncestorOfType<Page>();

                if (parentPage == null)
                {
                    throw new InvalidOperationException("PageExtensions.TopAppBar is used to set the TopAppBar on a parent page control and so it needs to be used in a control that is hosted in a Page.");
                }
            }

            parentPage.TopAppBar = newTopAppBar;
        }
        #endregion
    }
}
