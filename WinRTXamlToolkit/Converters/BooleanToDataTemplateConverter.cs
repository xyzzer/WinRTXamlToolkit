using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Converters
{
    public class BooleanToDataTemplateConverter : DependencyObject, IValueConverter
    {
        public DataTemplate FalseTemplate { get; set; }
        public DataTemplate TrueTemplate { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? TrueTemplate : FalseTemplate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value == FalseTemplate ? false : true;
        }
    }
}
