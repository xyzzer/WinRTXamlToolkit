using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Extensions/attached properties for a ContentControl
    /// </summary>
    public static class ContentControlExtensions
    {
        #region FadeTransitioningContentTemplate
        /// <summary>
        /// FadeTransitioningContentTemplate Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty FadeTransitioningContentTemplateProperty =
            DependencyProperty.RegisterAttached(
                "FadeTransitioningContentTemplate",
                typeof(DataTemplate),
                typeof(ContentControlExtensions),
                new PropertyMetadata(null, OnFadeTransitioningContentTemplateChanged));

        /// <summary>
        /// Gets the FadeTransitioningContentTemplate property. This dependency property 
        /// indicates the value to set the ContentTemplate to between fade out/fade in transitions.
        /// </summary>
        public static DataTemplate GetFadeTransitioningContentTemplate(DependencyObject d)
        {
            return (DataTemplate)d.GetValue(FadeTransitioningContentTemplateProperty);
        }

        /// <summary>
        /// Sets the FadeTransitioningContentTemplate property. This dependency property 
        /// indicates the value to set the ContentTemplate to between fade out/fade in transitions.
        /// </summary>
        public static void SetFadeTransitioningContentTemplate(DependencyObject d, DataTemplate value)
        {
            d.SetValue(FadeTransitioningContentTemplateProperty, value);
        }

        /// <summary>
        /// Handles changes to the FadeTransitioningContentTemplate property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static async void OnFadeTransitioningContentTemplateChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataTemplate oldFadeTransitioningContentTemplate = (DataTemplate)e.OldValue;
            DataTemplate newFadeTransitioningContentTemplate = (DataTemplate)d.GetValue(FadeTransitioningContentTemplateProperty);
            var control = (ContentControl)d;
            await control.FadeOut();
            control.ContentTemplate = newFadeTransitioningContentTemplate;
            await control.FadeIn();
        }
        #endregion

        #region FadeInTransitioningContentTemplate
        /// <summary>
        /// FadeInTransitioningContentTemplate Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty FadeInTransitioningContentTemplateProperty =
            DependencyProperty.RegisterAttached(
                "FadeInTransitioningContentTemplate",
                typeof(DataTemplate),
                typeof(ContentControlExtensions),
                new PropertyMetadata(null, OnFadeInTransitioningContentTemplateChanged));

        /// <summary>
        /// Gets the FadeInTransitioningContentTemplate property. This dependency property 
        /// indicates the value to set the ContentTemplate to between fade out/fade in transitions.
        /// </summary>
        public static DataTemplate GetFadeInTransitioningContentTemplate(DependencyObject d)
        {
            return (DataTemplate)d.GetValue(FadeInTransitioningContentTemplateProperty);
        }

        /// <summary>
        /// Sets the FadeInTransitioningContentTemplate property. This dependency property 
        /// indicates the value to set the ContentTemplate to between fade out/fade in transitions.
        /// </summary>
        public static void SetFadeInTransitioningContentTemplate(DependencyObject d, DataTemplate value)
        {
            d.SetValue(FadeInTransitioningContentTemplateProperty, value);
        }

        /// <summary>
        /// Handles changes to the FadeInTransitioningContentTemplate property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static async void OnFadeInTransitioningContentTemplateChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DataTemplate oldFadeInTransitioningContentTemplate = (DataTemplate)e.OldValue;
            DataTemplate newFadeInTransitioningContentTemplate = (DataTemplate)d.GetValue(FadeTransitioningContentTemplateProperty);
            var control = (ContentControl)d;
            await control.FadeOut(TimeSpan.FromSeconds(0));
            control.ContentTemplate = newFadeInTransitioningContentTemplate;
            await control.FadeIn();
        }
        #endregion
    }
}
