using System.Collections.Generic;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Debugging.ViewModels.ResourceBrowser
{
    internal class MergedDictionariesTreeItemViewModel : TreeItemViewModel
    {
        public MergedDictionariesTreeItemViewModel(ITreeViewModel treeViewModel, TreeItemViewModel parent, IList<ResourceDictionary> mergedDictionaries)
            : base(treeViewModel, parent)
        {
            foreach (var mergedDictionary in mergedDictionaries)
            {
                this.Children.Add(new ResourceDictionaryTreeItemViewModel(treeViewModel, this, mergedDictionary));
            }
        }
    }
}