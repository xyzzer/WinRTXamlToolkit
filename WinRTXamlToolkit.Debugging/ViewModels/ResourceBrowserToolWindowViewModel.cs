using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    internal class ResourceBrowserToolWindowViewModel : ToolWindowViewModel
    {
        private readonly ResourceDictionary _resourceDictionary;

        #region SelectedResource
        private object _selectedResource;
        /// <summary>
        /// Gets or sets a value indicating the selected resource.
        /// </summary>
        public object SelectedResource
        {
            get { return this._selectedResource; }
            set { this.SetProperty(ref this._selectedResource, value); }
        }
        #endregion

        public ResourceBrowserToolWindowViewModel(ResourceDictionary resourceDictionary)
        {
            _resourceDictionary = resourceDictionary;
        }
    }
}
