using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Text;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class TreeItemViewModel : BindableBase
    {
        public VisualTreeViewModel TreeModel { get; protected set; }
        private bool _everExpanded;
        private bool _everSelected;

        #region Parent
        private TreeItemViewModel _parent;
        public TreeItemViewModel Parent
        {
            get { return _parent; }
            set { this.SetProperty(ref _parent, value); }
        }
        #endregion

        #region Children
        private ObservableCollection<TreeItemViewModel> _children = new ObservableCollection<TreeItemViewModel>();
        public ObservableCollection<TreeItemViewModel> Children
        {
            get { return _children; }
            set { this.SetProperty(ref _children, value); }
        }
        #endregion

        #region DisplayName
        public string DisplayName
        {
            get { return this.ToString(); }
        }
        #endregion

        #region IsSelected
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (this.SetProperty(ref _isSelected, value) && value)
                {
                    if (!_everSelected)
                    {
                        _everSelected = true;
#pragma warning disable 4014
                        LoadPropertiesAsync();
#pragma warning restore 4014
                    }

                    this.TreeModel.SelectedItem = this;
                }
            }
        }
        #endregion

        #region IsExpanded
        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (this.SetProperty(ref _isExpanded, value) &&
                    value &&
                    !_everExpanded)
                {
                    _everExpanded = true;
#pragma warning disable 4014
                    LoadChildrenAsync();
#pragma warning restore 4014
                }
            }
        }
        #endregion

        #region FontWeight
        private FontWeight _fontWeight = FontWeights.Normal;
        public FontWeight FontWeight
        {
            get { return _fontWeight; }
            set { this.SetProperty(ref _fontWeight, value); }
        }
        #endregion

        public TreeItemViewModel(
            VisualTreeViewModel treeModel,
            TreeItemViewModel parent)
        {
            this.TreeModel = treeModel;
            this.Parent = parent;
        }

#pragma warning disable 1998
        internal virtual async Task LoadPropertiesAsync()
        {
        }

        internal virtual async Task LoadChildrenAsync()
        {
        }

        internal virtual async Task RefreshAsync()
        {
        }
#pragma warning restore 1998
    }
}