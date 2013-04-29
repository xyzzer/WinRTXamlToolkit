using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Debugging.Common;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class DependencyObjectViewModel : TreeItemViewModel
    {
        internal DependencyObject Model { get; private set; }
        private readonly string _description;

        public string Description { get { return _description; } }

        #region Properties
        private List<BasePropertyViewModel> _allProperties;
        public List<BasePropertyViewModel> Properties
        {
            get
            {
                return ShowDefaultedProperties && ShowReadOnlyProperties
                           ? _allProperties
                           : _allProperties.Where(p => (ShowDefaultedProperties || !p.IsDefault) && (ShowReadOnlyProperties || !p.IsReadOnly)).ToList();
            }
        }
        #endregion

        #region ShowDefaultedProperties
        public bool ShowDefaultedProperties
        {
            get { return TreeModel.ShowDefaultedProperties; }
            set
            {
                TreeModel.ShowDefaultedProperties = value;
                // ReSharper disable ExplicitCallerInfoArgument
                OnPropertyChanged();
                OnPropertyChanged("Properties");
                // ReSharper restore ExplicitCallerInfoArgument
            }
        }
        #endregion

        #region ShowReadOnlyProperties
        public bool ShowReadOnlyProperties
        {
            get { return TreeModel.ShowReadOnlyProperties; }
            set
            {
                TreeModel.ShowReadOnlyProperties = value;
                // ReSharper disable ExplicitCallerInfoArgument
                OnPropertyChanged();
                OnPropertyChanged("Properties");
                // ReSharper restore ExplicitCallerInfoArgument
            }
        }
        #endregion
        
        #region Name
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (this.SetProperty(ref _name, value))
                {
                    // ReSharper disable ExplicitCallerInfoArgument
                    OnPropertyChanged("DisplayName");
                    // ReSharper restore ExplicitCallerInfoArgument
                }
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
            protected set
            {
                if (this.SetProperty(ref _descendantCount, value))
                {
                    // ReSharper disable ExplicitCallerInfoArgument
                    OnPropertyChanged("DisplayName");
                    // ReSharper restore ExplicitCallerInfoArgument
                }
            }
        }
        #endregion

        #region Details
        private ObservableCollection<DetailViewModel> _details = new ObservableCollection<DetailViewModel>();
        public ObservableCollection<DetailViewModel> Details
        {
            get { return _details; }
            set { this.SetProperty(ref _details, value); }
        }
        #endregion

        #region PreviewImageSource
        private ImageSource _previewImageSource;
        public ImageSource PreviewImageSource
        {
            get { return _previewImageSource; }
            set { this.SetProperty(ref _previewImageSource, value); }
        }
        #endregion

        public DependencyObjectViewModel(
            VisualTreeViewModel treeModel,
            TreeItemViewModel parent,
            DependencyObject model)
            : base (treeModel, parent)
        {
            this.Model = model;

            if (Model.GetType().GetTypeInfo().Assembly != typeof(FrameworkElement).GetTypeInfo().Assembly)
            {
                this.FontWeight = FontWeights.Bold;
            }

            this.Children.Add(new StubTreeItemViewModel(this.TreeModel, this));

            var fe = model as FrameworkElement;

            if (fe != null)
            {
                _name = fe.Name;
            }

            this.DescendantCount = model.GetDescendants().Count();

            if (this.DescendantCount == 0)
            {
                this.Children.Clear();
            }
        }

        private void UpdateAscendantChildCounts()
        {
            var parent = this;

            while (parent != null)
            {
                parent.DescendantCount =
                    parent.Children.OfType<DependencyObjectViewModel>().Count() +
                    parent.Children.OfType<DependencyObjectViewModel>().Sum(dovm => dovm.DescendantCount);
                parent = parent.Parent as DependencyObjectViewModel;
            }
        }

        public override string ToString()
        {
            var typeName = Model.GetType().Name;

            if (_name == null)
            {
                return typeName;
            }

            return
                string.Format(
                    "{0}{1}{2}",
                    typeName,
                    !string.IsNullOrEmpty(_name) ? string.Format(" ({0})", _name) : string.Empty,
                    _descendantCount != 0 ? string.Format(" [{0}]", _descendantCount) : string.Empty);
        }

        internal override async Task LoadProperties()
        {
            var type = this.Model.GetType();

            var dependencyProperties =
                (await DependencyPropertyCache.GetDependencyProperties(type))
                    .Select(dpi => new DependencyPropertyViewModel(this, dpi)).ToList();

            var plainProperties =
                type.GetRuntimeProperties()
                    .Where(pi => !pi.GetMethod.IsStatic)
                    .OrderBy(pi => pi.Name)
                    .Select(pi => new PropertyViewModel(this, pi))
                    .Except(
                        dependencyProperties,
                        new DelegateEqualityComparer<BasePropertyViewModel, string>(p => p.Name));

            var properties = dependencyProperties.Concat(plainProperties);

            _allProperties = properties.OrderBy(p => p.Name).ToList();
// ReSharper disable ExplicitCallerInfoArgument
            OnPropertyChanged("Properties");
// ReSharper restore ExplicitCallerInfoArgument
            this.Details.Clear();
            this.Details.Add(new DetailViewModel("Type", GetTypeInheritanceInfo()));
            this.Details.Add(new DetailViewModel("Child element count", VisualTreeHelper.GetChildrenCount(this.Model).ToString()));

            //if (TreeModel.IsPreviewShown)
            //{
            //    await this.LoadPreview();
            //}
        }

        private string GetTypeInheritanceInfo()
        {
            var sb = new StringBuilder();
            var type = this.Model.GetType();
            sb.AppendFormat("    {0}\r\n", type.AssemblyQualifiedName);

            sb.AppendLine("\r\nBased on");

            do
            {
                type = type.GetTypeInfo().BaseType;
                sb.AppendFormat("    {0}\r\n", type.AssemblyQualifiedName);
            } while (type != typeof(object));

            return sb.ToString();            
        }

#pragma warning disable 1998
        internal override async Task LoadChildren()
#pragma warning restore 1998
        {
            this.Children =
                new ObservableCollection<TreeItemViewModel>(
                    from childElement in Model.GetChildren().Cast<UIElement>()
                    select new DependencyObjectViewModel(this.TreeModel, this, childElement));

            UpdateAscendantChildCounts();
        }

        internal override async Task Refresh()
        {
            await base.Refresh();
            await LoadChildren();
            await LoadProperties();
        }

//        public async Task LoadPreview()
//        {
//            var fe = this.Model as FrameworkElement;
//            if (fe == null)
//            {
//                return;
//            }

//            try
//            {
//                var wb = await WriteableBitmapRenderExtensions.Render(fe);

//                PreviewImageSource = wb;
//            }
//// ReSharper disable EmptyGeneralCatchClause
//            catch
//// ReSharper restore EmptyGeneralCatchClause
//            {
//            }
//        }
    }

    public class DetailViewModel
    {
        public string Label { get; private set; }
        public string Detail { get; private set; }

        public DetailViewModel(string label, string detail)
        {
            Label = label;
            Detail = detail;
        }
    }

    public class DelegateEqualityComparer<T, TCompare> : IEqualityComparer<T>
    {
        private readonly Func<T, TCompare> _func;

        public DelegateEqualityComparer(Func<T, TCompare> func)
        {
            _func = func;
        }

        public bool Equals(T x, T y)
        {
            return _func(x).Equals(_func(y));
        }

        public int GetHashCode(T obj)
        {
            return _func(obj).GetHashCode();
        }
    }
}