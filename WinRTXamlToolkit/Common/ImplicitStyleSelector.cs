using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace WinRTXamlToolkit.Common
{
    /// <summary>
    /// An implicit <see cref="StyleSelector"/> implementation that gets the <see cref="Style"/>
    /// from the resource dictionary defined in the <see cref="Resources"/> property or in <see cref="Application"/>
    /// resource dictionary that has the same name as the type name of the data item.
    /// </summary>
    [ContentProperty(Name = "Resources")]
    public class ImplicitStyleSelector : StyleSelector
    {
        /// <summary>
        /// A resource dictionary that defines styles to use with keys matching type names of the data items.
        /// </summary>
        public ResourceDictionary Resources { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImplicitStyleSelector"/> class.
        /// </summary>
        public ImplicitStyleSelector()
        {
            this.Resources = new ResourceDictionary();
        }

        /// <summary>
        /// When implemented by a derived class, returns a specific Style based on custom logic.
        /// This implementation gets the <see cref="Style"/>
        /// from the resource dictionary defined in the <see cref="Resources"/> property
        /// or in <see cref="Application"/> resource dictionary that has the same name
        /// as the type name of the data item.
        /// </summary>
        /// <param name="item">The content.</param>
        /// <param name="container">The element to which the style is applied.</param>
        /// <returns>
        /// An application-specific style to apply; may also return null.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// </exception>
        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            if (item != null)
            {
                object styleResource;
                var key = item.GetType().Name;

                if (this.Resources.TryGetValue(key, out styleResource))
                {
                    var style = styleResource as Style;

                    if (style == null)
                    {
                        throw new ArgumentException(string.Format("{0} resource defined in the ImplicitStyleSelector needs to be of Style type.", key));
                    }

                    return style;
                }

                if (Application.Current.Resources.TryGetValue(key, out styleResource))
                {
                    var style = styleResource as Style;

                    if (style == null)
                    {
                        throw new ArgumentException(string.Format("{0} resource defined in the application resources needs to be of Style type, or one needs to be defined in the ImplicitStyleSelector.", key));
                    }

                    return style;
                }
            }

            return base.SelectStyleCore(item, container);
        }
    }
}
