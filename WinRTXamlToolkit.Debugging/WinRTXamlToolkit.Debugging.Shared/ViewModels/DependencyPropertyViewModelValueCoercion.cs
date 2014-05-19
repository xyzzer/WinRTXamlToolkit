using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public partial class DependencyPropertyViewModel
    {
        // This coercion thing is a second level fail-safe for setting the property values.
        // The coercion should really be enforced in the UI of the property editors.
        // The third level is just by eating exceptions when trying to set the value in the DependencyPropertyViewModel.Value setter.
        internal static class ValueCoercionHelperFactory
        {
            public static IValueCoercionHelper GetValueCoercionHelper(DependencyProperty dp)
            {
                if (dp == FrameworkElement.WidthProperty ||
                    dp == FrameworkElement.HeightProperty ||
                    dp == FrameworkElement.MinWidthProperty ||
                    dp == FrameworkElement.MaxWidthProperty ||
                    dp == FrameworkElement.MinHeightProperty ||
                    dp == FrameworkElement.MaxHeightProperty)
                {
                    return new DoubleCoercionHelper(0, double.MaxValue);
                }

                if (dp == FrameworkElement.OpacityProperty)
                {
                    return new DoubleCoercionHelper(0, 1);
                }

                if (dp == Canvas.ZIndexProperty)
                {
                    return new IntCoercionHelper(int.MinValue, 1000000);
                }

                return null;
            }
        }

        internal interface IValueCoercionHelper
        {
            void CoerceValue(ref object value);
        }

        internal class DoubleCoercionHelper : IValueCoercionHelper
        {
            private readonly double _minimum;
            private readonly double _maximum;

            internal DoubleCoercionHelper(double minimum, double maximum)
            {
                _minimum = minimum;
                _maximum = maximum;
            }

            public void CoerceValue(ref object value)
            {
                var d = (double)value;

                if (d < _minimum)
                {
                    value = _minimum;
                }

                if (d > _maximum)
                {
                    value = _maximum;
                }
            }
        }

        internal class IntCoercionHelper : IValueCoercionHelper
        {
            private readonly int _minimum;
            private readonly int _maximum;

            internal IntCoercionHelper(int minimum, int maximum)
            {
                _minimum = minimum;
                _maximum = maximum;
            }

            public void CoerceValue(ref object value)
            {
                var i = (int)value;

                if (i < _minimum)
                {
                    value = _minimum;
                }

                if (i > _maximum)
                {
                    value = _maximum;
                }
            }
        }
    }
}