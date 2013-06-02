using System.Linq;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Style type extension methods.
    /// </summary>
    public static class StyleExtensions
    {
        /// <summary>
        /// Gets the property value for the given property in the given style.
        /// </summary>
        /// <param name="style">The style.</param>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        public static object GetPropertyValue(this Style style, DependencyProperty property)
        {
            var setter =
                style.Setters.Cast<Setter>().FirstOrDefault(
                    s => s.Property == property);

            if (setter != null)
            {
                return setter.Value;
            }

            if (style.BasedOn != null)
            {
                return style.BasedOn.GetPropertyValue(property);
            }

            return DependencyProperty.UnsetValue;
        }
    }
}
