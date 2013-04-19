using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Text;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class VisualTreeViewModel : BindableBase
    {
        #region RootElement
        private readonly ObservableCollection<TreeItemViewModel> _rootElements = new ObservableCollection<TreeItemViewModel>();
        /// <summary>
        /// Gets or sets the root elements in the visual tree.
        /// </summary>
        public ObservableCollection<TreeItemViewModel> RootElements
        {
            get { return _rootElements; }
        }
        #endregion

        public VisualTreeViewModel()
        {
            if (Window.Current.Content != null)
            {
                VisualTreeViewModelBuilder.UserAssembly =
                    Window.Current.Content.GetType().GetTypeInfo().Assembly;
            }
            this.RootElements.Add(VisualTreeViewModelBuilder.Build(Window.Current.Content));}
        }

    public static class VisualTreeViewModelBuilder
    {
        public static Assembly UserAssembly { get; set; }

        public static UIElementViewModel Build(UIElement element)
        {
            if (element == null)
            {
                return null;
            }

            UIElementViewModel vm;

            var fe = element as FrameworkElement;

            if (fe != null)
            {
                vm = new FrameworkElementViewModel(fe);

                return vm;
            }

            vm = new UIElementViewModel(element);

            return vm;
        }
    }

    public class FrameworkElementViewModel : UIElementViewModel
    {
        #region Name
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                this.SetProperty(ref _name, value);
                OnPropertyChanged("DisplayName");
            }
        }
        #endregion

        #region DescendantCount
        private int _descendantCount;
        /// <summary>
        /// Gets or sets the number of descendants.
        /// </summary>
        public int DescendantCount
        {
            get { return _descendantCount; }
            protected set { this.SetProperty(ref _descendantCount, value); }
        }
        #endregion
        
        public FrameworkElementViewModel(FrameworkElement model)
            : base (model)
        {
            _name = model.Name;
            this.DescendantCount = model.GetDescendants().Count();

            if (this.DescendantCount == 0)
            {
                this.Children.Clear();
            }
        }

        public override string ToString()
        {
            if (_name == null)
            {
                return base.ToString();
            }

            return
                string.Format(
                    "{0}{1}{2}",
                    base.ToString(),
                    !string.IsNullOrEmpty(_name) ? string.Format(" ({0})", _name) : string.Empty,
                    _descendantCount != 0 ? string.Format(" [{0}]", _descendantCount) : string.Empty);
        }
    }

    public class UIElementViewModel : DependencyObjectViewModel
    {
        public UIElementViewModel(UIElement model)
            : base (model)
        {
        }
    }

    public class DependencyObjectViewModel : TreeItemViewModel
    {
        private readonly DependencyObject _model;

        public DependencyObjectViewModel(DependencyObject model)
        {
            _model = model;

            if (_model.GetType().GetTypeInfo().Assembly ==
                VisualTreeViewModelBuilder.UserAssembly)
            {
                FontWeight = FontWeights.Bold;
            }

            this.Children.Add(new StubTreeItemViewModel());
        }

        public override string ToString()
        {
            return _model.GetType().Name;
        }

        protected override void LoadProperties()
        {
        }

        protected override void LoadChildren()
        {
            this.Children.Clear();

            foreach (var childElement in _model.GetChildren())
            {
                var childVM = VisualTreeViewModelBuilder.Build((UIElement)childElement);
                this.Children.Add(childVM);
            }
        }
    }

    public class StubTreeItemViewModel : TreeItemViewModel
    {
        private readonly string _displayName;
        private readonly string _description;

        public string Description { get { return _description; } }

        public StubTreeItemViewModel(string displayName = "Loading...", string description = "Please wait while more content is loaded...")
        {
            _displayName = displayName;
            _description = description;
        }

        public override string ToString()
        {
            return _displayName;
        }
    }

    public class TreeItemViewModel : BindableBase
    {
        private bool _everExpanded;
        private bool _everSelected;

        #region Children
        private readonly ObservableCollection<TreeItemViewModel> _children = new ObservableCollection<TreeItemViewModel>();
        public ObservableCollection<TreeItemViewModel> Children
        {
            get { return _children; }
            //set { this.SetProperty(ref _children, value); }
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
                if (this.SetProperty(ref _isSelected, value) &&
                    value &&
                    !_everSelected)
                {
                    _everSelected = true;
                    LoadProperties();
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
                    LoadChildren();
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

        protected virtual void LoadProperties()
        {
        }

        protected virtual void LoadChildren()
        {
        }
    }

    public class PropertyViewModel : BindableBase
    {
        #region Name
        private string _name;
        public string Name
        {
            get { return _name; }
            protected set { this.SetProperty(ref _name, value); }
        }
        #endregion

        protected PropertyInfo _propertyInfo;

        protected PropertyViewModel()
        {
        }

        public PropertyViewModel(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
            this.Name = propertyInfo.Name;
        }
    }
}
