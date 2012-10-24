using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Converters
{
    public class PolynomialConverter : IValueConverter
    {
        public DoubleCollection Coefficients { get; set; }

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

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
