using System.Collections.Generic;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Debugging.ViewModels.ResourceBrowser
{
    internal class ThemeDictionariesTreeItemViewModel : TreeItemViewModel
    {
        public ThemeDictionariesTreeItemViewModel(ITreeViewModel treeViewModel, TreeItemViewModel parent, IDictionary<object, object> themeDictionaries) : base(treeViewModel, parent)
        {
            foreach (var themeDictionary in themeDictionaries)
            {
                this.Children.Add(new ResourceDictionaryTreeItemViewModel(treeViewModel, this, (ResourceDictionary)themeDictionary.Value, themeDictionary.Key));
            }
        }
    }
}