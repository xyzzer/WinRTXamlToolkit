using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    internal class ResourceBrowserToolWindowViewModel : ToolWindowViewModel, ITreeViewModel
    {
        #region SelectedItem
        private TreeItemViewModel _selectedItem;
        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public TreeItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (this.SetProperty(ref _selectedItem, value))
                {
                    this.SelectedResourceViewModel = (value as ResourceTreeItemViewModel)?.PropertiesViewModel;
                }
            }
        }
        #endregion

        public class MergedDictionariesTreeItemViewModel : TreeItemViewModel
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

        public class ThemeDictionariesTreeItemViewModel : TreeItemViewModel
        {
            public ThemeDictionariesTreeItemViewModel(ITreeViewModel treeViewModel, TreeItemViewModel parent, IDictionary<object, object> themeDictionaries) : base(treeViewModel, parent)
            {
                foreach (var themeDictionary in themeDictionaries)
                {
                    this.Children.Add(new ResourceDictionaryTreeItemViewModel(treeViewModel, this, (ResourceDictionary)themeDictionary.Value, themeDictionary.Key));
                }
            }
        }

        public class ResourceDictionaryTreeItemViewModel : TreeItemViewModel
        {
            public object Key { get; }

            public ResourceDictionaryTreeItemViewModel(ITreeViewModel treeViewModel, TreeItemViewModel parent, ResourceDictionary dictionary, object key = null)
                : base(treeViewModel, parent)
            {
                this.Key = key ?? dictionary.Source.ToString();

                foreach (var kvp in dictionary)
                {
                    this.Children.Add(new ResourceTreeItemViewModel(treeViewModel, this, kvp.Value, kvp.Key));
                }
            }
        }

        public class ResourceTreeItemViewModel : TreeItemViewModel
        {
            public object Key { get; }
            public string ValueTypeName { get; }
            public object Value { get; }
            public DependencyObjectViewModel PropertiesViewModel { get; }

            public ResourceTreeItemViewModel(ITreeViewModel treeViewModel, ResourceDictionaryTreeItemViewModel parent, object value, object key)
                : base(treeViewModel, parent)
            {
                this.Value = value;
                this.ValueTypeName = this.Value.GetType().Name;
                this.Key = key;
                this.PropertiesViewModel = new DependencyObjectViewModel(null, null, value);
            }
        }
        #region SelectedResourceViewModel
        private DependencyObjectViewModel _selectedResourceViewModel;
        /// <summary>
        /// Gets or sets a value indicating the selected resource.
        /// </summary>
        public DependencyObjectViewModel SelectedResourceViewModel
        {
            get { return _selectedResourceViewModel; }
            set { this.SetProperty(ref _selectedResourceViewModel, value); }
        }
        #endregion

        #region RootElements
        private readonly ObservableCollection<TreeItemViewModel> _rootElements = new ObservableCollection<TreeItemViewModel>();
        /// <summary>
        /// Gets or sets the root elements in the visual tree.
        /// </summary>
        public ObservableCollection<TreeItemViewModel> RootElements => _rootElements;
        #endregion

        public ResourceBrowserToolWindowViewModel(ResourceDictionary resourceDictionary)
        {
            var resourceDictionary1 = resourceDictionary;

            if (resourceDictionary1.MergedDictionaries != null &&
                resourceDictionary1.MergedDictionaries.Count > 0)
            {
                _rootElements.Add(new MergedDictionariesTreeItemViewModel(this, null, resourceDictionary1.MergedDictionaries));
            }

            if (resourceDictionary1.ThemeDictionaries != null &&
                resourceDictionary1.ThemeDictionaries.Count > 0)
            {
                _rootElements.Add(new ThemeDictionariesTreeItemViewModel(this, null, resourceDictionary1.ThemeDictionaries));
            }

            foreach (var resourceKeyValue in resourceDictionary)
            {
                _rootElements.Add(new ResourceTreeItemViewModel(this, null, resourceKeyValue.Value, resourceKeyValue.Key));
            }
        }
    }
}
