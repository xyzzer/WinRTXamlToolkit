using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace WinRTXamlToolkit.Common
{
    /// <summary>
    /// An implicit <see cref="DataTemplateSelector"/> implementation that gets the <see cref="DataTemplate"/>
    /// from the resource dictionary defined in the <see cref="Resources"/> property or in <see cref="Application"/>
    /// resource dictionary that has the same name as the type name of the data item.
    /// </summary>
    [ContentProperty(Name = "Resources")]
    public class ImplicitDataTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// A resource dictionary that defines data templates to use with keys matching type names of the data items.
        /// </summary>
        public ResourceDictionary Resources { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImplicitDataTemplateSelector"/> class.
        /// </summary>
        public ImplicitDataTemplateSelector()
        {
            this.Resources = new ResourceDictionary();
        }

        /// <summary>
        /// When implemented by a derived class,
        /// returns a specific DataTemplate for a given item or container.
        /// This implementation gets the <see cref="DataTemplate"/>
        /// from the resource dictionary defined in the <see cref="Resources"/> property
        /// or in <see cref="Application"/> resource dictionary that has the same name
        /// as the type name of the data item.
        /// </summary>
        /// <param name="item">The item to return a template for.</param>
        /// <param name="container">The parent container for the templated item.</param>
        /// <returns>
        /// The template to use for the given item and/or container.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// </exception>
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item != null)
            {
                object templateResource;
                var key = item.GetType().Name;

                if (this.Resources.TryGetValue(key, out templateResource))
                {
                    var template = templateResource as DataTemplate;

                    if (template == null)
                    {
                        throw new ArgumentException(string.Format("{0} resource defined in the ImplicitDataTemplateSelector needs to be of DataTemplate type.", key));
                    }

                    return template;
                }

                if (Application.Current.Resources.TryGetValue(key, out templateResource))
                {
                    var template = templateResource as DataTemplate;

                    if (template == null)
                    {
                        throw new ArgumentException(string.Format("{0} resource defined in the application resources needs to be of DataTemplate type, or one needs to be defined in the ImplicitDataTemplateSelector.", key));
                    }

                    return template;
                }
            }

            return base.SelectTemplateCore(item, container);
        }
    }
}
