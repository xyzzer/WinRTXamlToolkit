using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Converters
{
    public class DoubleToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (int)(double)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (double)(int)value;
        }
    }
}
