using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Converters
{
    public class NullableBoolToVisibilityConverter : IValueConverter
    {
        public bool IsReversed { get; set; }
        public bool TrueIsVisible { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (IsReversed)
            {
                var vis = (Visibility) value;
                return (bool?)(((vis == Visibility.Visible) && TrueIsVisible) || ((vis == Visibility.Collapsed) && !TrueIsVisible));
            }
            
            var val = value as bool?;

            return 
                (val.HasValue && val.Value) ?
                (TrueIsVisible ? Visibility.Visible : Visibility.Collapsed) :
                (!TrueIsVisible ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (IsReversed)
            {
                var val = value as bool?;

                return
                    (val.HasValue && val.Value) ?
                    (TrueIsVisible ? Visibility.Visible : Visibility.Collapsed) :
                    (!TrueIsVisible ? Visibility.Visible : Visibility.Collapsed);
            }

            var vis = (Visibility)value;
            return (bool?)(((vis == Visibility.Visible) && TrueIsVisible) || ((vis == Visibility.Collapsed) && !TrueIsVisible));
        }
    }
}
