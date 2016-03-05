using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Defines an attached property that controls the visual state of the element based on the value.
    /// </summary>
    public static class VisualStateExtensions
    {
        #region State
        /// <summary>
        /// State Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.RegisterAttached(
                "State",
                typeof(string),
                typeof(VisualStateExtensions),
                new PropertyMetadata(null, OnStateChanged));

        /// <summary>
        /// Gets the State property. This dependency property 
        /// indicates the VisualState that the associated control should be set to.
        /// </summary>
        public static string GetState(DependencyObject d)
        {
            return (string)d.GetValue(StateProperty);
        }

        /// <summary>
        /// Sets the State property. This dependency property 
        /// indicates the VisualState that the associated control should be set to.
        /// </summary>
        public static void SetState(DependencyObject d, string value)
        {
            d.SetValue(StateProperty, value);
        }

        /// <summary>
        /// Handles changes to the State property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnStateChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var stateName = (string)e.NewValue;
            var ctrl = (Control)d;
            VisualStateManager.GoToState(ctrl, stateName, true);
        }
        #endregion
    }
}
