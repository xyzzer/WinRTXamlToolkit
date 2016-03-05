using System;
using WinRTXamlToolkit.Debugging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Converters
{
    /// <summary>
    /// Makes elements rotated in a way that they are not facing the viewer - invisible.
    /// </summary>
    public class ProjectionRotationToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var rotation = (double)value;

            while (rotation >= 360)
            {
                rotation -= 360;
            }
            while (rotation < 0)
            {
                rotation += 360;
            }

            var ret = rotation >= 90 && rotation <= 270
                       ? Visibility.Collapsed
                       : Visibility.Visible;

            DC.Trace("Rotation: {0}, Visibility: {1}", (double)value, ret);

            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
