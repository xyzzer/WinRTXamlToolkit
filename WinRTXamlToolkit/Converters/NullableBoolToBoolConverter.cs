using System;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Converters
{
    /// <summary>
    /// Converts between bool? and bool.
    /// </summary>
    public class NullableBoolToBoolConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets a value indicating whether it should convert bool too bool? instead.
        /// </summary>
        /// <value>
        /// <c>true</c> if converting from bool? to bool; otherwise, <c>false</c> and converting from bool to bool?.
        /// </value>
        public bool IsReversed { get; set; }

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
            if (IsReversed)
            {
                return (bool?) (value);
            }
            
            var val = value as bool?;
            return val.HasValue && val.Value;
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
            if (IsReversed)
            {
                var val = value as bool?;
                return val.HasValue && val.Value;
            }
            
            return (bool?) (value);
        }
    }
}
