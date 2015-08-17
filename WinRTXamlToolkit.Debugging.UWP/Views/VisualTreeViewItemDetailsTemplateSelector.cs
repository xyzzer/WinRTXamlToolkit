using WinRTXamlToolkit.Debugging.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Debugging.Views
{
    public class VisualTreeViewItemDetailsTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DependencyPropertyDetailsTemplate { get; set; }
        public DataTemplate DefaultDetailsTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is DependencyObjectViewModel)
            {
                return DependencyPropertyDetailsTemplate;
            }

            return DefaultDetailsTemplate;
        }
    }
}
