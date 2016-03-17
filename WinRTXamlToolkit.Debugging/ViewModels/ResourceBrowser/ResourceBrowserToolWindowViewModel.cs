using System.Collections.ObjectModel;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Debugging.ViewModels.ResourceBrowser
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
                    this.SelectedResourceDictionaryViewModel = (value as ResourceDictionaryTreeItemViewModel);
                }
            }
        }
        #endregion

        #region SelectedResourceDictionaryViewModel
        private ResourceDictionaryTreeItemViewModel _selectedResourceDictionaryViewModel;
        /// <summary>
        /// Gets or sets a value indicating the selected resource.
        /// </summary>
        public ResourceDictionaryTreeItemViewModel SelectedResourceDictionaryViewModel
        {
            get { return _selectedResourceDictionaryViewModel; }
            set { this.SetProperty(ref _selectedResourceDictionaryViewModel, value); }
        }
        #endregion

        /// <summary>
        /// Gets or sets the root elements in the visual tree.
        /// </summary>
        public ObservableCollection<TreeItemViewModel> RootElements { get; } = new ObservableCollection<TreeItemViewModel>();

        public ResourceBrowserToolWindowViewModel(ResourceDictionary resourceDictionary)
        {
            this.RootElements.Add(new ResourceDictionaryTreeItemViewModel(this, null, resourceDictionary));
            this.SelectedResourceDictionaryViewModel = (ResourceDictionaryTreeItemViewModel)this.RootElements[0];
        }
    }
}
