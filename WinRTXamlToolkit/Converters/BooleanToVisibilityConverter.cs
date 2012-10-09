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
        public bool IsReversed { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (IsReversed)
            {
                return value is Visibility && (Visibility)value == Visibility.Visible;
            }

            return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (IsReversed)
            {
                return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
            }

            return value is Visibility && (Visibility)value == Visibility.Visible;
        }
    }
}
