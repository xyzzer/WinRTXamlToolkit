using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Converters
{
    /// <summary>
    /// Selects a DataTemplate based on a boolean value.
    /// </summary>
    public class BooleanToDataTemplateConverter : DependencyObject, IValueConverter
    {
        /// <summary>
        /// Gets or sets the template to use for false value.
        /// </summary>
        /// <value>
        /// The false template.
        /// </value>
        public DataTemplate FalseTemplate { get; set; }

        /// <summary>
        /// Gets or sets the template to use for true value.
        /// </summary>
        /// <value>
        /// The true template.
        /// </value>
        public DataTemplate TrueTemplate { get; set; }

        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The type of the target property, specified by a helper structure that wraps the type name.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? TrueTemplate : FalseTemplate;
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object. This method is called only in <c>TwoWay</c> bindings. 
        /// </summary>
        /// <param name="value">The target data being passed to the source..</param>
        /// <param name="targetType">The type of the target property, specified by a helper structure that wraps the type name.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the source object.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value == FalseTemplate ? false : true;
        }
    }
}
