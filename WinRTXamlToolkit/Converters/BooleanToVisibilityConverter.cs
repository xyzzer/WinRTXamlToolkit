using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Converters
{
    /// <summary>
    /// Value converter that translates true to <see cref="Visibility.Visible"/> and false to
    /// <see cref="Visibility.Collapsed"/>.
    /// </summary>
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// If true - converts from Visibility to Boolean.
        /// </summary>
        public bool IsReversed { get; set; }

        /// <summary>
        /// If true - converts true to Collapsed and false to Visible.
        /// </summary>
        public bool IsInversed { get; set; }

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
                return (value is Visibility) ^ IsInversed && (Visibility)value == Visibility.Visible;
            }

            return (value is bool && (bool)value) ^ IsInversed ? Visibility.Visible : Visibility.Collapsed;
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
                return (value is bool && (bool)value) ^ IsInversed ? Visibility.Visible : Visibility.Collapsed;
            }

            return (value is Visibility && (Visibility)value == Visibility.Visible) ^ IsInversed;
        }
    }
}
