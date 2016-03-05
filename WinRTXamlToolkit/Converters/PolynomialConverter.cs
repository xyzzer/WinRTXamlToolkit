using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Converters
{
    /// <summary>
    /// Converts given value to a double-typed result of a polynomial given its coefficients.
    /// </summary>
    public class PolynomialConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the coefficients of the polynomial.
        /// </summary>
        /// <value>
        /// The coefficients.
        /// </value>
        public DoubleCollection Coefficients { get; set; }

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
            double x = (double)value;
            double output = 0;

            for (int i = Coefficients.Count - 1; i >= 0; i--)
            {
                output += Coefficients[i] * Math.Pow(x, (Coefficients.Count - 1) - i);
            }

            return output;
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
            throw new NotSupportedException();
        }
    }
}
