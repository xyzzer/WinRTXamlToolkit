using System;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Converters
{
    public class MultiplyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var x = System.Convert.ToDouble(value);
            var a = System.Convert.ToDouble(parameter);
            return a * x;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var x = System.Convert.ToDouble(value);
            var a = System.Convert.ToDouble(parameter);
            return a / x;
        }
    }
}
