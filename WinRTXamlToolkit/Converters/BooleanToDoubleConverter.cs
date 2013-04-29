using System;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Converters
{
    public class BooleanToDoubleConverter : IValueConverter
    {
        private const double Epsilon = 0.001;
        public double FalseValue { get; set; }
        public double TrueValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? TrueValue : FalseValue;
        }

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
