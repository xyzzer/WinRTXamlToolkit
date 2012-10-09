using System;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Converters
{
    public class SecondsToTimeSpanStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var seconds = (double)value;
            var timeSpan = TimeSpan.FromSeconds(seconds);
            var str = string.Format(
                "{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                timeSpan.Hours,
                timeSpan.Minutes,
                timeSpan.Seconds,
                timeSpan.Milliseconds);
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return TimeSpan.Parse((string)value);
        }
    }
}
