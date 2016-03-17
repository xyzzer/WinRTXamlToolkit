using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Debugging.ViewModels.ResourceBrowser
{
    internal class ResourceDictionaryTreeItemViewModel : TreeItemViewModel
    {
        public string QualifierString { get; }

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <remarks>
        /// The name "Properties" is important because that's what the PropertiesView needs and we reuse it from the visual tree view.
        /// </remarks>
        public ObservableCollection<BasePropertyViewModel> Properties { get; } = new ObservableCollection<BasePropertyViewModel>();

        public ResourceDictionaryTreeItemViewModel(
            ITreeViewModel treeViewModel,
            TreeItemViewModel parent,
            ResourceDictionary resourceDictionary,
            object key = null)
            : base(treeViewModel, parent)
        {
            this.QualifierString = String.Empty;

            if (key != null)
            {
                this.QualifierString += $" [{key}]";
            }

            if (resourceDictionary.Source != null)
            {
                this.QualifierString += $" Source: {resourceDictionary.Source}";
            }

            if (resourceDictionary.MergedDictionaries != null &&
                resourceDictionary.MergedDictionaries.Count > 0)
            {
                this.Children.Add(new MergedDictionariesTreeItemViewModel(treeViewModel, this, resourceDictionary.MergedDictionaries));
            }

            if (resourceDictionary.ThemeDictionaries != null &&
                resourceDictionary.ThemeDictionaries.Count > 0)
            {
                this.Children.Add(new ThemeDictionariesTreeItemViewModel(treeViewModel, null, resourceDictionary.ThemeDictionaries));
            }

            foreach (var kvp in resourceDictionary.ToList())
            {
                this.Properties.Add(new ResourceViewModel(kvp.Key, kvp.Value, resourceDictionary));
            }
        }
    }
}