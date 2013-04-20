using System.Collections.ObjectModel;
using System.Reflection;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class VisualTreeViewModel : BindableBase
    {
        #region RootElements
        private readonly ObservableCollection<TreeItemViewModel> _rootElements = new ObservableCollection<TreeItemViewModel>();
        /// <summary>
        /// Gets or sets the root elements in the visual tree.
        /// </summary>
        public ObservableCollection<TreeItemViewModel> RootElements
        {
            get { return _rootElements; }
        }
        #endregion

        #region SelectedItem
        private TreeItemViewModel _selectedItem;
        public TreeItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set { this.SetProperty(ref _selectedItem, value); }
        }
        #endregion

        public VisualTreeViewModel()
        {
            if (Window.Current.Content != null)
            {
                VisualTreeViewModelBuilder.UserAssembly =
                    Window.Current.Content.GetType().GetTypeInfo().Assembly;
            }

            this.RootElements.Add(VisualTreeViewModelBuilder.Build(this, null, Window.Current.Content));}
        }
}
