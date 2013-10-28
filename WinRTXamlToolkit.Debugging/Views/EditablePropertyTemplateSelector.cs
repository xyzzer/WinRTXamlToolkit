using System;
using System.Reflection;
using Windows.UI.Text;
using WinRTXamlToolkit.Debugging.Controls;
using WinRTXamlToolkit.Debugging.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Debugging.Views
{
    [ContentProperty(Name = "Resources")]
    public class EditablePropertyTemplateSelector : DataTemplateSelector
    {
        public ResourceDictionary Resources { get; set; }

        public EditablePropertyTemplateSelector()
        {
            this.Resources = new ResourceDictionary();
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var propertyViewModel = item as BasePropertyViewModel;
            var dpvm = item as DependencyPropertyViewModel;

            if (propertyViewModel != null)
            {
                if (!propertyViewModel.IsReadOnly)
                {
                    var type = propertyViewModel.PropertyType;
                    var typeInfo = type.GetTypeInfo();

                    if (type == typeof(string) ||
                        type == typeof(object) && (propertyViewModel.Value == null || propertyViewModel.GetType() == typeof(string)) ||
                        dpvm != null && dpvm.DependencyProperty == ToolTipService.ToolTipProperty)
                    {
                        return (DataTemplate)this.Resources["StringPropertyEditor"];
                    }

                    if (type == typeof(bool) || type == typeof(bool?))
                    {
                        return (DataTemplate)this.Resources["BooleanPropertyEditor"];
                    }

                    if (typeof(CacheMode).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
                    {
                        return (DataTemplate)this.Resources["CacheModePropertyEditor"];
                    }

                    if (type == typeof(Thickness))
                    {
                        return (DataTemplate)this.Resources["ThicknessPropertyEditor"];
                    }

                    if (type == typeof(CornerRadius))
                    {
                        return (DataTemplate)this.Resources["CornerRadiusPropertyEditor"];
                    }

                    if (type == typeof(byte) ||
                        type == typeof (Int16) ||
                        type == typeof (UInt16) ||
                        type == typeof (int) ||
                        type == typeof (uint) ||
                        type == typeof (Int64) ||
                        type == typeof (UInt64) ||
                        type == typeof (float) ||
                        type == typeof (double)
                        )
                    {
                        return (DataTemplate)this.Resources["NumericPropertyEditor"];
                    }

                    if (typeInfo.IsEnum ||
                        typeInfo.IsGenericType &&
                        type.GetGenericTypeDefinition() == typeof (Nullable<>) &&
                        type.GenericTypeArguments[0].GetTypeInfo().IsEnum)
                    {
                        return (DataTemplate)this.Resources["EnumPropertyEditor"];
                    }

                    if (typeof(Brush).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
                    {
                        return (DataTemplate)this.Resources["BrushPropertyEditor"];
                    }

                    if (typeof(ImageSource).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()))
                    {
                        return (DataTemplate)this.Resources["ImageSourcePropertyEditor"];
                    }

                    if (typeof(DependencyObject).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo()) &&
                        propertyViewModel.Value != null)
                    {
                        return (DataTemplate)this.Resources["DependencyObjectPropertyEditor"];
                    }

                    if (type == typeof (FontWeight))
                    {
                        return (DataTemplate)this.Resources["FontWeightPropertyEditor"];
                    }

                    return (DataTemplate)this.Resources["DefaultPropertyEditor"];
                }

                //return base.SelectTemplateCore(item, container);
                var editableListBoxItem = (EditableListBoxItem)container;

                return editableListBoxItem.SlimContentTemplate;
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
