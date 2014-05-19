using WinRTXamlToolkit.Debugging.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace WinRTXamlToolkit.Debugging.Views
{
    [ContentProperty(Name = "Resources")]
    public class PropertyTemplateSelector : DataTemplateSelector
    {
        public ResourceDictionary Resources { get; set; }

        public PropertyTemplateSelector()
        {
            this.Resources = new ResourceDictionary();
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var propertyViewModel = item as BasePropertyViewModel;

            if (propertyViewModel != null)
            {
                return (DataTemplate)this.Resources["Property"];
            }

            var propertyGroupViewModel = item as PropertyGroupViewModel;

            if (propertyGroupViewModel != null)
            {
                return (DataTemplate)this.Resources["Group"];
            }

            return base.SelectTemplateCore(item, container);
        }
    }
}
