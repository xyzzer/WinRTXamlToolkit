using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Debugging.Common;
using Windows.UI.Text;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class DependencyObjectViewModel : TreeItemViewModel
    {
        private static DependencyPropertyCache PropertyCache;
        internal DependencyObject Model { get; private set; }

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
            Model = model;

            if (Model.GetType().GetTypeInfo().Assembly ==
                VisualTreeViewModelBuilder.UserAssembly)
            {
                FontWeight = FontWeights.Bold;
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

        protected override void LoadProperties()
        {
            var type = this.Model.GetType();


            var cache = PropertyCache ??
            (PropertyCache = new DependencyPropertyCache(
                Window.Current.Content.GetType().GetTypeInfo().Assembly));

            var dependencyProperties =
                cache.GetDependencyProperties(type)
                .Select(dpi => new DependencyPropertyViewModel(this, dpi.Property, dpi.DisplayName)).ToList();

            IEnumerable<BasePropertyViewModel> plainProperties =
                type.GetRuntimeProperties()
                    .Where(pi => !pi.GetMethod.IsStatic)
                    .OrderBy(pi => pi.Name)
                    .Select(pi => new PropertyViewModel(this, pi))
                    .Except(
                        dependencyProperties,
                        new DelegateEqualityComparer<BasePropertyViewModel, string>(p => p.Name));
            IEnumerable<BasePropertyViewModel> properties = 
                dependencyProperties
                    .Concat(plainProperties);

            this.Properties = new ObservableCollection<BasePropertyViewModel>(properties.OrderBy(p => p.Name));
        }

        protected override void LoadChildren()
        {
            this.Children.Clear();

            foreach (var childElement in Model.GetChildren())
            {
                var childVM = VisualTreeViewModelBuilder.Build(this.TreeModel, this, (UIElement)childElement);
                this.Children.Add(childVM);
            }

            UpdateAscendantChildCounts();
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