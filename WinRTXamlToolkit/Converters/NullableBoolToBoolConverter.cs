using System;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Converters
{
    public class NullableBoolToBoolConverter : IValueConverter
    {
        public bool IsReversed { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (IsReversed)
            {
                return (bool?) (value);
            }
            
            var val = value as bool?;
            return val.HasValue && val.Value;
        }

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
