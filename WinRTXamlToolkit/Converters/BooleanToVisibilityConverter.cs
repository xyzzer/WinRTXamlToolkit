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
        /// <summary>
        /// If true - converts from Visibility to Boolean.
        /// </summary>
        public bool IsReversed { get; set; }

        /// <summary>
        /// If true - converts true to Collapsed and false to Visible.
        /// </summary>
        public bool IsInversed { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (IsReversed)
            {
                return (value is Visibility) ^ IsInversed && (Visibility)value == Visibility.Visible;
            }

            return (value is bool && (bool)value) ^ IsInversed ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (IsReversed)
            {
                return (value is bool && (bool)value) ^ IsInversed ? Visibility.Visible : Visibility.Collapsed;
            }

            return (value is Visibility && (Visibility)value == Visibility.Visible) ^ IsInversed;
        }
    }
}
