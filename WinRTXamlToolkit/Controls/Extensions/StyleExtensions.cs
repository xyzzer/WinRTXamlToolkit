using System.Linq;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Controls.Extensions
{
    public static class StyleExtensions
    {
        public static object GetPropertyValue(this Style style, DependencyProperty property)
        {
            var setter =
                style.Setters.Cast<Setter>().FirstOrDefault(
                    s => s.Property == property);
            var value = setter != null ? setter.Value : null;

            if (setter == null &&
                style.BasedOn != null)
            {
                value = style.BasedOn.GetPropertyValue(property);
            }

            return value;
        }
    }
}
