using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Controls.Data
{
    public static class DataTemplateExtensions
    {
        #region Hierarchy
        /// <summary>
        /// Hierarchy Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty HierarchyProperty =
            DependencyProperty.RegisterAttached(
                "Hierarchy",
                typeof(HierarchicalDataTemplate),
                typeof(DataTemplateExtensions),
                new PropertyMetadata(null, OnHierarchyChanged));

        /// <summary>
        /// Gets the Hierarchy property. This dependency property 
        /// indicates the hierarchical template extensions to use for data-bound hierarchical controls.
        /// </summary>
        public static HierarchicalDataTemplate GetHierarchy(DependencyObject d)
        {
            return (HierarchicalDataTemplate)d.GetValue(HierarchyProperty);
        }

        /// <summary>
        /// Sets the Hierarchy property. This dependency property 
        /// indicates the hierarchical template extensions to use for data-bound hierarchical controls.
        /// </summary>
        public static void SetHierarchy(DependencyObject d, HierarchicalDataTemplate value)
        {
            d.SetValue(HierarchyProperty, value);
        }

        /// <summary>
        /// Handles changes to the Hierarchy property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnHierarchyChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HierarchicalDataTemplate oldHierarchy = (HierarchicalDataTemplate)e.OldValue;
            HierarchicalDataTemplate newHierarchy = (HierarchicalDataTemplate)d.GetValue(HierarchyProperty);
            //if (oldHierarchy != null)
            //    oldHierarchy.ClearValue(FrameworkElement.DataContextProperty);
            //if (newHierarchy != null)
            //    newHierarchy.SetBinding(
            //        FrameworkElement.DataContextProperty,
            //        new Binding
            //        {
            //            Path = new PropertyPath("DataContext"),
            //            Source = d
            //        });
        }
        #endregion
    }
}
