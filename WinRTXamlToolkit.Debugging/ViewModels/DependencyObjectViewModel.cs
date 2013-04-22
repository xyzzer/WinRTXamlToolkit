using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Debugging.Common;
using Windows.UI.Text;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class DependencyObjectViewModel : TreeItemViewModel
    {
        internal DependencyObject Model { get; private set; }
        private readonly string _description;

        public string Description { get { return _description; } }

        #region Properties
        private ObservableCollection<BasePropertyViewModel> _properties;
        public ObservableCollection<BasePropertyViewModel> Properties
        {
            get { return _properties; }
            set { this.SetProperty(ref _properties, value); }
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

        public DependencyObjectViewModel(
            VisualTreeViewModel treeModel,
            TreeItemViewModel parent,
            DependencyObject model)
            : base (treeModel, parent)
        {
            this.Model = model;

            var sb = new StringBuilder();
            sb.AppendLine("Type");
            var type = model.GetType();
            sb.AppendFormat("    {0}\r\n", type.AssemblyQualifiedName);

            sb.AppendLine("\r\nBased on");

            do
            {
                type = type.GetTypeInfo().BaseType;
                sb.AppendFormat("    {0}\r\n", type.AssemblyQualifiedName);
            } while (type != typeof (object));

            _description = sb.ToString();

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

        public override async Task LoadProperties()
        {
            var type = this.Model.GetType();

            var dependencyProperties =
                (await DependencyPropertyCache.GetDependencyProperties(type))
                    .Select(dpi => new DependencyPropertyViewModel(this, dpi.Property, dpi.DisplayName)).ToList();

            var plainProperties =
                type.GetRuntimeProperties()
                    .Where(pi => !pi.GetMethod.IsStatic)
                    .OrderBy(pi => pi.Name)
                    .Select(pi => new PropertyViewModel(this, pi))
                    .Except(
                        dependencyProperties,
                        new DelegateEqualityComparer<BasePropertyViewModel, string>(p => p.Name));

            var properties = dependencyProperties.Concat(plainProperties);

            this.Properties = new ObservableCollection<BasePropertyViewModel>(properties.OrderBy(p => p.Name));
        }

        public override async Task LoadChildren()
        {
            this.Children.Clear();

            foreach (var childElement in Model.GetChildren())
            {
                var childVM = await VisualTreeViewModelBuilder.Build(this.TreeModel, this, (UIElement)childElement);
                this.Children.Add(childVM);
            }

            UpdateAscendantChildCounts();
        }

        internal override async Task Refresh()
        {
            await base.Refresh();
            await LoadChildren();
            await LoadProperties();
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