using System.Reflection;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public static class VisualTreeViewModelBuilder
    {
        public static Assembly UserAssembly { get; set; }

        public static DependencyObjectViewModel Build(VisualTreeViewModel treeModel, TreeItemViewModel parent, UIElement element)
        {
            if (element == null)
            {
                return null;
            }

            DependencyObjectViewModel vm;

            vm = new DependencyObjectViewModel(treeModel, parent, element);

            return vm;
        }
    }
}