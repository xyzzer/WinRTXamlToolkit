using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Debugging.Commands;
using WinRTXamlToolkit.Debugging.Common;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class DependencyObjectViewModel : VisualTreeItemViewModel
    {
        private bool _skipUpdatingProperties;

        internal object Model { get; }

        public event EventHandler ModelPropertyChanged;

        public string Description => null;

        #region Properties property
        private List<BasePropertyViewModel> _allProperties;
        public IEnumerable<BindableBase> Properties => this.ShowPropertiesGrouped
            ? (IEnumerable<BindableBase>)this.GroupedProperties
            : (IEnumerable<BindableBase>)this.FilteredProperties;
        #endregion

        #region GroupedProperties
        private ObservableCollection<BindableBase> _groupedProperties;
        private ObservableCollection<BindableBase> GroupedProperties
        {
            get
            {
                if (_groupedProperties == null)
                {
                    if (this.FilteredProperties == null)
                    {
                        return new ObservableCollection<BindableBase>();
                    }

                    _groupedProperties = new ObservableCollection<BindableBase>(
                        this.FilteredProperties
                            .OrderBy(p => p.Name)
                            .GroupBy(p => p.Category)
                            .Select(g => new PropertyGroupViewModel(
                                g.Key,
                                g))
                            .OrderBy(g => g.Category));

                    foreach (var sampleButtonViewModel in _groupedProperties.Cast<PropertyGroupViewModel>().ToList())
                    {
                        sampleButtonViewModel.ParentList = _groupedProperties;

                        if (!string.IsNullOrEmpty(this.PropertyNameFilter))
                        {
                            sampleButtonViewModel.IsExpanded = true;
                        }
                    }
                }

                return _groupedProperties;
            }
        } 
        #endregion

        #region FilteredProperties
        private ObservableCollection<BasePropertyViewModel> FilteredProperties
        {
            get
            {
                if (!string.IsNullOrEmpty(this.PropertyNameFilter))
                {
                    return new ObservableCollection<BasePropertyViewModel>(this.UngroupedProperties
                            .Where(p => p.Name.ToLower().Contains(this.PropertyNameFilter.ToLower())));
                }

                return this.UngroupedProperties;
            }
        } 
        #endregion

        #region UngroupedProperties
        private ObservableCollection<BasePropertyViewModel> UngroupedProperties
        {
            get
            {
                if (_allProperties == null)
                {
                    return new ObservableCollection<BasePropertyViewModel>();
                }

                var isPropertyNameFiltered = !string.IsNullOrEmpty(this.PropertyNameFilter);

                // If there is an active name filter - display filtered properties
                if (this.CurrentPropertyList != null &&
                    this.CurrentPropertyList.PropertyNames.Count > 0)
                {
                    return
                        new ObservableCollection<BasePropertyViewModel>(_allProperties.Where(
                            p =>
                                (isPropertyNameFiltered || this.ShowDefaultedProperties || !p.IsDefault) &&
                                (isPropertyNameFiltered || this.ShowReadOnlyProperties || !p.IsReadOnly) &&
                                this.CurrentPropertyList.PropertyNames.Contains(p.Name)));
                }

                // If no checkbox filters are set - simply return all properties
                if (this.ShowDefaultedProperties && this.ShowReadOnlyProperties)
                {
                    return new ObservableCollection<BasePropertyViewModel>(_allProperties);
                }

                // If default/readonly filters are set - return flag-filtered properties
                return
                    new ObservableCollection<BasePropertyViewModel>(_allProperties.Where(
                        p =>
                        (isPropertyNameFiltered || this.ShowDefaultedProperties || !p.IsDefault) &&
                        (isPropertyNameFiltered || this.ShowReadOnlyProperties || !p.IsReadOnly)));
            }
        } 
        #endregion

        #region ShowPropertiesGrouped
        /// <summary>
        /// Gets or sets the property that indicates whether the properties should be shown grouped.
        /// </summary>
        public bool ShowPropertiesGrouped
        {
            get { return this.VisualTreeViewModel.ShowPropertiesGrouped; }
            set
            {
                if (this.VisualTreeViewModel.ShowPropertiesGrouped == value)
                {
                    return;
                }

                this.VisualTreeViewModel.ShowPropertiesGrouped = value;
                this.OnPropertyChanged();

                if (!_skipUpdatingProperties)
                {
                    // ReSharper disable once ExplicitCallerInfoArgument
                    this.OnPropertyChanged(nameof(this.Properties));
                }
            }
        }
        #endregion

        #region ShowDefaultedProperties
        public bool ShowDefaultedProperties
        {
            get { return this.VisualTreeViewModel.ShowDefaultedProperties; }
            set
            {
                if (this.VisualTreeViewModel.ShowDefaultedProperties == value)
                {
                    return;
                }

                this.VisualTreeViewModel.ShowDefaultedProperties = value;
                this.OnPropertyChanged();

                if (!_skipUpdatingProperties)
                {
                    _groupedProperties = null;
                    // ReSharper disable once ExplicitCallerInfoArgument
                    this.OnPropertyChanged(nameof(this.Properties));
                }
            }
        }
        #endregion

        #region ShowReadOnlyProperties
        public bool ShowReadOnlyProperties
        {
            get { return this.VisualTreeViewModel.ShowReadOnlyProperties; }
            set
            {
                if (this.VisualTreeViewModel.ShowReadOnlyProperties == value)
                {
                    return;
                }

                this.VisualTreeViewModel.ShowReadOnlyProperties = value;
                this.OnPropertyChanged();

                if (!_skipUpdatingProperties)
                {
                    _groupedProperties = null;
                    // ReSharper disable once ExplicitCallerInfoArgument
                    this.OnPropertyChanged(nameof(this.Properties));
                }
            }
        }
        #endregion

        #region PropertyNameFilter
        /// <summary>
        /// Gets or sets the string that filters the property list.
        /// </summary>
        public string PropertyNameFilter
        {
            get { return this.VisualTreeViewModel.PropertyNameFilter; }
            set
            {
                if (this.VisualTreeViewModel.PropertyNameFilter == value)
                {
                    return;
                }

                this.VisualTreeViewModel.PropertyNameFilter = value;

                this.OnPropertyChanged();

                if (!_skipUpdatingProperties)
                {
                    _groupedProperties = null;
                    // ReSharper disable once ExplicitCallerInfoArgument
                    this.OnPropertyChanged(nameof(this.Properties));
                }
            }
        }
        #endregion

        #region CurrentPropertyList
        /// <summary>
        /// Gets or sets the currently active property list.
        /// The list is a comma-separated list of properties to display.
        /// </summary>
        public PropertyList CurrentPropertyList
        {
            get { return this.VisualTreeViewModel.CurrentPropertyList; }
            set
            {
                if (this.VisualTreeViewModel.CurrentPropertyList == value)
                {
                    return;
                }

                var isOldPropertyListFiltered = this.VisualTreeViewModel.CurrentPropertyList != null &&
                                                this.VisualTreeViewModel.CurrentPropertyList
                                                    .PropertyNames.Count > 0;
                var isNewPropertyListFiltered = value != null &&
                                                value.PropertyNames.Count > 0;

                this.VisualTreeViewModel.CurrentPropertyList = value;
                this.OnPropertyChanged();

                if (!isOldPropertyListFiltered &&
                    isNewPropertyListFiltered)
                {
                    _skipUpdatingProperties = true;
                    this.ShowDefaultedProperties = true;
                    this.ShowReadOnlyProperties = true;
                    _skipUpdatingProperties = false;
                }

                _groupedProperties = null;
                // ReSharper disable once ExplicitCallerInfoArgument
                this.OnPropertyChanged(nameof(this.Properties));
            }
        }
        #endregion

        #region PropertyLists
        /// <summary>
        /// Gets the list of property name filters.
        /// The strings contain comma-separated lists of properties to display.
        /// </summary>
        public ObservableCollection<PropertyList> PropertyLists => this.VisualTreeViewModel.PropertyLists;
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
                    // ReSharper disable once ExplicitCallerInfoArgument
                    this.OnPropertyChanged(nameof(this.DisplayName));
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
                    // ReSharper disable once ExplicitCallerInfoArgument
                    this.OnPropertyChanged(nameof(this.DisplayName));
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

        public RelayCommand RefreshCommand { get; private set; }

        #region CTOR
        public DependencyObjectViewModel(
            VisualTreeViewModel treeViewModel,
            VisualTreeItemViewModel parent,
            object model)
            : base(treeViewModel, parent)
        {
            this.Model = model;

            if (!Equals(this.Model.GetType().GetTypeInfo().Assembly,
                    typeof (FrameworkElement).GetTypeInfo().Assembly))
            {
                this.FontWeight = FontWeights.Bold;
            }

            var fe = model as FrameworkElement;

            if (fe != null)
            {
                _name = fe.Name;
            }

            var dob = model as DependencyObject;

            if (dob != null &&
                dob.GetDescendants().Any())
            {
                this.Children.Add(new StubVisualTreeItemViewModel(this.VisualTreeViewModel, this));
            }

#pragma warning disable 4014
            this.RefreshCommand = new RelayCommand(() => this.RefreshAsync());
#pragma warning restore 4014
        } 
        #endregion

        #region UpdateAscendantChildCounts()
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
        #endregion

        #region ToString()
        public override string ToString()
        {
            var typeName = this.Model.GetType().Name;

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
        #endregion

        #region LoadProperties()
        internal override async Task LoadPropertiesAsync()
        {
            var type = this.Model.GetType();

            var dependencyProperties =
                (await DependencyPropertyCache.GetDependencyPropertiesAsync(type))
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

            foreach (var propertyViewModel in _allProperties)
            {
                propertyViewModel.PropertyChanged += this.OnPropertyPropertyChanged;
            }

            _groupedProperties = null;

            // ReSharper disable once ExplicitCallerInfoArgument
            this.OnPropertyChanged(nameof(this.Properties));
            this.Details.Clear();
            this.Details.Add(new DetailViewModel("Type", this.GetTypeInheritanceInfo()));

            var element = this.Model as UIElement;

            if (element != null)
            {
                this.Details.Add(new DetailViewModel("Child element count", VisualTreeHelper.GetChildrenCount(element).ToString()));
            }

            if (this.VisualTreeViewModel.IsPreviewShown)
            {
                await this.LoadPreviewAsync();
            }
        } 
        #endregion

        #region OnPropertyPropertyChanged()
        private void OnPropertyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.ModelPropertyChanged?.Invoke(this, EventArgs.Empty);
        } 
        #endregion

        #region GetTypeInheritanceInfo()
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
        #endregion

#pragma warning disable 1998
        #region LoadChildren()
        internal override async Task LoadChildrenAsync()
#pragma warning restore 1998
        {
            var element = this.Model as UIElement;

            if (element != null)
            {
                this.Children =
                    new ObservableCollection<TreeItemViewModel>(
                        from childElement in element.GetChildren().Cast<UIElement>()
                        select new DependencyObjectViewModel(this.VisualTreeViewModel, this, childElement));
                this.UpdateAscendantChildCounts();
            }
            else
            {
                this.Children = new ObservableCollection<TreeItemViewModel>();
            }
        }
        #endregion

        #region Refresh()
        internal override async Task RefreshAsync()
        {
            await base.RefreshAsync();
            await this.LoadChildrenAsync();
            await this.LoadPropertiesAsync();
        }
        #endregion

        #region LoadPreviewAsync()
        public async Task LoadPreviewAsync()
        {
            var fe = this.Model as FrameworkElement;
            if (fe == null)
            {
                return;
            }

            try
            {
                var rtb = new RenderTargetBitmap();
                await rtb.RenderAsync(fe);

                this.PreviewImageSource = rtb;
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            // ReSharper restore EmptyGeneralCatchClause
            {
            }
        }
        #endregion
    }

    public class DetailViewModel
    {
        public string Label { get; private set; }
        public string Detail { get; private set; }

        public DetailViewModel(string label, string detail)
        {
            this.Label = label;
            this.Detail = detail;
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