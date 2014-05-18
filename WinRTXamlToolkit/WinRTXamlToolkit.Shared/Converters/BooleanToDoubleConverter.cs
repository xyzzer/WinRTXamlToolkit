using System;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Converters
{
    /// <summary>
    /// Converts boolean values to double values.
    /// </summary>
    public class BooleanToDoubleConverter : IValueConverter
    {
        private const double Epsilon = 0.001;
        /// <summary>
        /// Gets or sets the double value to use when the input value is false.
        /// </summary>
        /// <value>
        /// The double value to use when the input value is false.
        /// </value>
        public double FalseValue { get; set; }

        /// <summary>
        /// Gets or sets the double value to use when the input value is true.
        /// </summary>
        /// <value>
        /// The double value to use when the input value is true.
        /// </value>
        public double TrueValue { get; set; }

        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? TrueValue : FalseValue;
        }

        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">value</exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var v = (double)value;

            if (Math.Abs(v - TrueValue) < Epsilon)
                return true;

            if (Math.Abs(v - FalseValue) < Epsilon)
                return false;

            throw new ArgumentException(
                string.Format(
                    "BooleanToDoubleConverter configured to convert FalseValue={0} or TrueValue={1}. Value passed to convert back: {2}",
                    FalseValue,
                    TrueValue,
                    value),
                "value");
        }
    }
}
