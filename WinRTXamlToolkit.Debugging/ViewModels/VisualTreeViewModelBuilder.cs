using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using WinRTXamlToolkit.Tools;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public static class VisualTreeViewModelBuilder
    {

        public static async Task<DependencyObjectViewModel> Build(VisualTreeViewModel treeModel, TreeItemViewModel parent, UIElement element)
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