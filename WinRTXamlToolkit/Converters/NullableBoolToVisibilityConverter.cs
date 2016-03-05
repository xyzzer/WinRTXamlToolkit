using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Converters
{
    /// <summary>
    /// Converts a nullable bool to Visibility.
    /// </summary>
    public class NullableBoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// If true - converts from Visibility to nullable Boolean.
        /// </summary>
        public bool IsReversed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether true gets converted to Visibility.Visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if true gets converted Visibility.Visible;
        /// otherwise, if <c>false</c> - false will be converted to Visibility.Visible.
        /// </value>
        public bool TrueIsVisible { get; set; }

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
                var vis = (Visibility) value;
                return (bool?)(((vis == Visibility.Visible) && TrueIsVisible) || ((vis == Visibility.Collapsed) && !TrueIsVisible));
            }
            
            var val = value as bool?;

            return 
                (val.HasValue && val.Value) ?
                (TrueIsVisible ? Visibility.Visible : Visibility.Collapsed) :
                (!TrueIsVisible ? Visibility.Visible : Visibility.Collapsed);
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

                return
                    (val.HasValue && val.Value) ?
                    (TrueIsVisible ? Visibility.Visible : Visibility.Collapsed) :
                    (!TrueIsVisible ? Visibility.Visible : Visibility.Collapsed);
            }

            var vis = (Visibility)value;
            return (bool?)(((vis == Visibility.Visible) && TrueIsVisible) || ((vis == Visibility.Collapsed) && !TrueIsVisible));
        }
    }
}
