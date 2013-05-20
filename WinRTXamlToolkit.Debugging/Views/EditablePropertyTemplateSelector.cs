using System;
using System.Reflection;
using WinRTXamlToolkit.Debugging.Controls;
using WinRTXamlToolkit.Debugging.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace WinRTXamlToolkit.Debugging.Views
{
    [ContentProperty]
    public class EditablePropertyTemplateSelector : DataTemplateSelector
    {
        public ResourceDictionary Resources { get; set; }

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
                        dpvm.DependencyProperty == ToolTipService.ToolTipProperty)
                    {
                        return (DataTemplate)this.Resources["StringPropertyEditor"];
                    }

                    if (type == typeof (bool))
                    {
                        return (DataTemplate)this.Resources["BooleanPropertyEditor"];
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
                        propertyViewModel.PropertyType.GetGenericTypeDefinition() == typeof (Nullable<>))
                    {
                        return (DataTemplate)this.Resources["EnumPropertyEditor"];
                    }
                }

                //return base.SelectTemplateCore(item, container);
                var editableListBoxItem = (EditableListBoxItem)container;

                return editableListBoxItem.SlimContentTemplate;
            }

            return base.SelectTemplateCore(item, container);
        }
    }
}
