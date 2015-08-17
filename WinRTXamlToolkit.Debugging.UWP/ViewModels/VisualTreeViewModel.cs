using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class PropertyList : BindableBase
    {
        #region Name
        private string _name;
        /// <summary>
        /// Gets or sets the name of the property list.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { this.SetProperty(ref _name, value); }
        }
        #endregion

        #region CommaSeparatedPropertyNames
        private string _commaSeparatedPropertyNames;
        /// <summary>
        /// Gets or sets the comma separated property names.
        /// </summary>
        public string CommaSeparatedPropertyNames
        {
            get { return _commaSeparatedPropertyNames; }
            set
            {
                if (this.SetProperty(ref _commaSeparatedPropertyNames, value))
                {
                    this.PropertyNames = new ObservableCollection<string>(_commaSeparatedPropertyNames.Split(',').Select(pn => pn.Trim()).Where(pn => !string.IsNullOrEmpty(pn)));
                }
            }
        }
        #endregion

        public ObservableCollection<string> PropertyNames { get; private set; }
    }

    public class VisualTreeViewModel : BindableBase
    {
        private Point _pointerPosition;
        private bool _isShiftPressed;
        private bool _isCtrlPressed;

        #region Instance (singleton implementation)
        private static VisualTreeViewModel _instance;
        public static VisualTreeViewModel Instance
        {
            get { return _instance ?? (_instance = new VisualTreeViewModel()); }
        }

        private VisualTreeViewModel()
        {
#pragma warning disable 4014
            this.GetPropertyLists();
            this.Build();
#pragma warning restore 4014
            Window.Current.CoreWindow.KeyDown += OnKeyDown;
            Window.Current.CoreWindow.KeyUp += OnKeyUp;
            Window.Current.CoreWindow.PointerMoved += OnPointerMoved;
        }
        #endregion

        #region IsShown
        private bool _isShown;
        public bool IsShown
        {
            get { return _isShown; }
            set
            {
                if (this.SetProperty(ref _isShown, value))
                {
                    this.OnIsShownChanged(value);
                }
            }
        }

        private async void OnIsShownChanged(bool value)
        {
            if (value)
            {
                if (this.RootElements.Count == 0)
                {
                    await this.Refresh();
                }

                this.UpdateHighlight();
            }
            else
            {
                this.HighlightVisibility = Visibility.Collapsed;
            }
        }
        #endregion
        
        #region RootElements
        private readonly ObservableCollection<TreeItemViewModel> _rootElements = new ObservableCollection<TreeItemViewModel>();
        /// <summary>
        /// Gets or sets the root elements in the visual tree.
        /// </summary>
        public ObservableCollection<TreeItemViewModel> RootElements
        {
            get { return _rootElements; }
            //set { this.SetProperty(ref _rootElements, value); }
        }
        #endregion

        #region SelectedItem
        private TreeItemViewModel _selectedItem;
        public TreeItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                var oldSelectedItem = _selectedItem as DependencyObjectViewModel;

                if (this.SetProperty(ref _selectedItem, value))
                {
                    var newSelectedItem = _selectedItem as DependencyObjectViewModel;

                    OnSelectedItemChanged(oldSelectedItem, newSelectedItem);
                }
            }
        }

        private void OnSelectedItemChanged(
            DependencyObjectViewModel oldSelectedItem,
            DependencyObjectViewModel newSelectedItem)
        {
            if (oldSelectedItem != null)
            {
                oldSelectedItem.ModelPropertyChanged -= OnModelPropertyChanged;
            }

            if (newSelectedItem != null)
            {
                newSelectedItem.ModelPropertyChanged += OnModelPropertyChanged;
#pragma warning disable 4014
                newSelectedItem.LoadPropertiesAsync();
#pragma warning restore 4014
            }

            UpdateHighlight();
        }

        private async void OnModelPropertyChanged(object sender, EventArgs eventArgs)
        {
            UpdateHighlight();

            // Wait for pending layout updates
            await Task.Delay(100);

            UpdateHighlight();
        }
        #endregion

        #region HighlightMargin
        private Thickness _highlightMargin;
        public Thickness HighlightMargin
        {
            get { return _highlightMargin; }
            set { this.SetProperty(ref _highlightMargin, value); }
        }
        #endregion

        #region HighlightTextMargin
        private Thickness _highlightTextMargin;
        public Thickness HighlightTextMargin
        {
            get { return _highlightTextMargin; }
            set { this.SetProperty(ref _highlightTextMargin, value); }
        }
        #endregion

        #region HighlightText
        private string _highlightText;
        public string HighlightText
        {
            get { return _highlightText; }
            set { this.SetProperty(ref _highlightText, value); }
        }
        #endregion

        #region HighlightVisibility
        private Visibility _highlightVisibility = Visibility.Collapsed;
        public Visibility HighlightVisibility
        {
            get { return _highlightVisibility; }
            set { this.SetProperty(ref _highlightVisibility, value); }
        }
        #endregion

        #region IsPreviewShown
        //private bool _isPreviewShown;
        //public bool IsPreviewShown
        //{
        //    get { return _isPreviewShown; }
        //    set { this.SetProperty(ref _isPreviewShown, value); }
        //}
        #endregion

        #region ShowPropertiesGrouped
        private bool _showPropertiesGrouped = false;
        /// <summary>
        /// Gets or sets the property that indicates whether the properties should be shown grouped.
        /// </summary>
        public bool ShowPropertiesGrouped
        {
            get { return _showPropertiesGrouped; }
            set { this.SetProperty(ref _showPropertiesGrouped, value); }
        }
        #endregion

        #region ShowDefaultedProperties
        private bool _showDefaultedProperties;
        public bool ShowDefaultedProperties
        {
            get { return _showDefaultedProperties; }
            set { this.SetProperty(ref _showDefaultedProperties, value); }
        }
        #endregion

        #region ShowReadOnlyProperties
        private bool _showReadOnlyProperties;
        public bool ShowReadOnlyProperties
        {
            get { return _showReadOnlyProperties; }
            set { this.SetProperty(ref _showReadOnlyProperties, value); }
        }
        #endregion

        #region PropertyNameFilter
        private string _propertyNameFilter;
        /// <summary>
        /// Gets or sets the string that filters the property list.
        /// </summary>
        public string PropertyNameFilter
        {
            get { return _propertyNameFilter; }
            set { this.SetProperty(ref _propertyNameFilter, value); }
        }
        #endregion

        #region CurrentPropertyList
        private PropertyList _currentPropertyList;
        /// <summary>
        /// Gets or sets the currently active property list.
        /// </summary>
        public PropertyList CurrentPropertyList
        {
            get { return _currentPropertyList; }
            set { this.SetProperty(ref _currentPropertyList, value); }
        }
        #endregion

        #region PropertyLists
        private ObservableCollection<PropertyList> _propertyLists = new ObservableCollection<PropertyList>();
        /// <summary>
        /// Gets or sets the list of property name filters.
        /// </summary>
        public ObservableCollection<PropertyList> PropertyLists
        {
            get { return _propertyLists; }
            private set { this.SetProperty(ref _propertyLists, value); }
        }
        #endregion

        #region OnPointerMoved()
        private void OnPointerMoved(CoreWindow sender, PointerEventArgs args)
        {
            _pointerPosition = args.CurrentPoint.Position;

            if (!_isShiftPressed ||
                !_isCtrlPressed)
            {
                return;
            }

#pragma warning disable 4014
            SelectElementUnderPointer();
            this.IsShown = true;
#pragma warning restore 4014
        } 
        #endregion

        #region SelectElementUnderPointer()
        internal async Task SelectElementUnderPointer(bool showDebugger = true)
        {
            var roots =
                VisualTreeHelper.GetOpenPopups(Window.Current)
                    .Where(p => p.Child != null)
                    .Select(p => p.Child)
                    .Reverse()
                    .ToList();
            roots.Add(VisualTreeHelperExtensions.GetRealWindowRoot());

            UIElement hoveredElement = null;

            foreach (var root in roots)
            {
                var hoveredElements = VisualTreeHelper.FindElementsInHostCoordinates(
                    _pointerPosition,
                    root);

                var hoveredElementCandidate = hoveredElements.FirstOrDefault();

                if (hoveredElementCandidate != null)
                {
                    if (hoveredElement == null)
                    {
                        hoveredElement = hoveredElementCandidate;
                    }

                    // If multiple popups are overlaid and possibly also overlay a non-popup
                    // - how to select the right root?
                    // Consider if the selected element is actually hoverable -
                    //   popups might have a canvas that will show as being in host coordinates
                    //   even though it's not necessarily hit test visible.

                    var control = hoveredElementCandidate as Control;
                    var panel = hoveredElementCandidate as Panel;

                    if (control != null &&
                        control.Background == null ||
                        panel != null &&
                        panel.Background == null)
                    {
                        continue;
                    }

                    hoveredElement = hoveredElementCandidate;
                    break;
                }
            }

            if (hoveredElement != null)
            {
                await SelectItem(hoveredElement, true);
            }

            if (showDebugger)
            {
                DC.Show();
                DC.Expand();
            }
        } 
        #endregion

        #region SelectFocused()
        internal async Task SelectFocused()
        {
            var focusedElement = FocusManager.GetFocusedElement() as UIElement;

            if (focusedElement != null)
            {
                await SelectItem(focusedElement, true);
            }
        } 
        #endregion

        #region SelectItem()
        internal async Task<bool> SelectItem(UIElement element, bool refreshOnFail = false)
        {
            var ancestors = new[] { element }.Concat(element.GetAncestors()).ToList();

            if (this.RootElements == null || this.RootElements.Count == 0)
            {
                await Refresh();
            }

            var vm = this.RootElements[0] as DependencyObjectViewModel;
            var ancestorIndex = ancestors.IndexOf(vm.Model);

            if (ancestorIndex < 0)
            {
                for (int i = 1; i < this.RootElements.Count && ancestorIndex < 0; i++)
                {
                    // Handling popups
                    var popup = this.RootElements[i];

                    if (!popup.IsExpanded)
                    {
                        await popup.LoadChildrenAsync();
                        popup.IsExpanded = true;
                    }

                    if (popup.Children.Count > 0)
                    {
                        vm = popup.Children[0] as DependencyObjectViewModel;
                        ancestorIndex = ancestors.IndexOf(vm.Model);
                    }
                }

                if (ancestorIndex < 0)
                {
                    await Refresh();
                    ancestorIndex = ancestors.IndexOf(vm.Model);
                }
            }

            if (ancestorIndex < 0)
            {
                System.Diagnostics.Debug.WriteLine("Something's wrong, but let's not throw exceptions here.");

                //Debugger.Break();
                if (refreshOnFail)
                {
                    await Refresh();
                    return await SelectItem(element, false);
                }

                return false;
            }

            //Debug.Assert(vm.Model == ancestors[0]);

            for (ancestorIndex = ancestorIndex - 1; ancestorIndex >= 0; ancestorIndex--)
            {
                if (!vm.IsExpanded)
                {
                    await vm.LoadChildrenAsync();
                    vm.IsExpanded = true;
                }
                var child =
                    vm.Children.OfType<DependencyObjectViewModel>()
                        .FirstOrDefault(dovm => dovm.Model == ancestors[ancestorIndex]);
                if (child == null)
                {
                    System.Diagnostics.Debug.WriteLine("Something's wrong, but let's not throw exceptions here.");

                    //Debugger.Break();
                    if (refreshOnFail)
                    {
                        await Refresh();
                        return await SelectItem(element, false);
                    }

                    return false;
                }

                vm = child;
            }

            await Task.Delay(100);
            vm.IsSelected = true;

            return true;
        } 
        #endregion

        #region OnKeyUp()
        private void OnKeyUp(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Shift)
            {
                _isShiftPressed = false;
            }
            else if (args.VirtualKey == VirtualKey.Control)
            {
                _isCtrlPressed = false;
            }
        } 
        #endregion

        #region OnKeyDown()
        private void OnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Shift)
            {
                _isShiftPressed = true;

                if (_isCtrlPressed)
                {
#pragma warning disable 4014
                    SelectElementUnderPointer();
                    this.IsShown = true;
#pragma warning restore 4014
                }
            }
            else if (args.VirtualKey == VirtualKey.Control)
            {
                _isCtrlPressed = true;

                if (_isShiftPressed)
                {
#pragma warning disable 4014
                    SelectElementUnderPointer();
                    this.IsShown = true;
#pragma warning restore 4014
                }
            }
        } 
        #endregion

        #region GetPropertyLists()
        private void GetPropertyLists()
        {
            this.PropertyLists = new ObservableCollection<PropertyList>();
            this.PropertyLists.Add(
                new PropertyList
                {
                    Name = "All",
                    CommaSeparatedPropertyNames = string.Empty
                });
            this.PropertyLists.Add(
                new PropertyList
                {
                    Name = "Layout",
                    CommaSeparatedPropertyNames =
                        "Width,Height,MinWidth,MinHeight,MaxWidth,MaxHeight,Orientation,Clip,ActualWidth,ActualHeight,Margin,Padding,Canvas.Left,Canvas.Top,Canvas.Zindex,ItemHeight,ItemWidth,LineStackingStrategy,LineHeight,Visibility,Opacity,RenderTransform,Projection,StrokeThickness,BorderThickness,Grid.Row,Grid.Column,Grid.RowSpan,Grid.ColumnSpan,VariableSizedWrapGrid.ColumnSpan,VariableSizedWrapGrid.RowSpan,VerticalAlignment,HorizontalAlignment,VerticalContentAlignment,HorizontalContentAlignment"
                });
            this.CurrentPropertyList = this.PropertyLists[0];
        } 
        #endregion

        #region Build()
#pragma warning disable 1998
        private async Task Build()
#pragma warning restore 1998
        {
            this.RootElements.Clear();
            var rootElement = VisualTreeHelperExtensions.GetRealWindowRoot();

            if (rootElement != null)
            {
                var ancestors = rootElement.GetAncestors();

                if (ancestors != null)
                {
                    var newRoot = ancestors.OfType<UIElement>().LastOrDefault();

                    if (newRoot != null)
                    {
                        rootElement = newRoot;
                    }
                }

                this.RootElements.Add(new DependencyObjectViewModel(this, null, rootElement));
            }

            foreach (var popup in VisualTreeHelper.GetOpenPopups(Window.Current))
            {
                this.RootElements.Add(new DependencyObjectViewModel(this, null, popup));
            }
        }

        internal async Task Refresh()
        {
            //if (this.SelectedItem != null)
            //{
            //    await this.SelectedItem.Refresh();
            //}
            //else 
            //if (this.RootElements.Count == 1 &&
            //    this.RootElements[0] is DependencyObjectViewModel &&
            //    ((DependencyObjectViewModel)this.RootElements[0]).Model == Window.Current.Content)
            //{
            //    await this.RootElements[0].RefreshAsync();
            //}
            //else
            //{
                this.RootElements.Clear();
                await this.Build();
            //}
        } 
        #endregion

        #region UpdateHighlight()
        private void UpdateHighlight()
        {
            var dovm = this.SelectedItem as DependencyObjectViewModel;

            if (dovm == null)
            {
                this.HighlightVisibility = Visibility.Collapsed;
                return;
            }

            var fe = dovm.Model as FrameworkElement;

            if (fe == null)
            {
                this.HighlightVisibility = Visibility.Collapsed;
                return;
            }

            try
            {
                var ancestors = fe.GetAncestors().ToList();

                if (!ancestors.Contains(VisualTreeHelperExtensions.GetRealWindowRoot()))
                {
                    var root = ancestors.Count > 0 ? ancestors[ancestors.Count - 1] : fe;
                    Popup popupRoot = null;
                    var popups = VisualTreeHelper.GetOpenPopups(Window.Current);

                    foreach (var openPopup in popups)
                    {
                        if (openPopup == root)
                        {
                            popupRoot = openPopup;
                            break;
                        }

                        if (ancestors.Contains(openPopup.Child))
                        {
                            popupRoot = openPopup;
                            break;
                        }
                    }

                    if (popupRoot == null)
                    {
                        this.HighlightVisibility = Visibility.Collapsed;
                        return;
                    }
                }

                var elementBounds = fe.GetBoundingRect();
                var windowBounds = Window.Current.Bounds;

                this.HighlightMargin = new Thickness(
                    elementBounds.Left,
                    elementBounds.Top,
                    windowBounds.Width - elementBounds.Right,
                    windowBounds.Height - elementBounds.Bottom);
                this.HighlightText = string.Format(
                    "{0}\r\nx: {1:F0}\r\ny: {2:F0}\r\nw: {3:F0}\r\nh: {4:F0}",
                    dovm.DisplayName,
                    elementBounds.Left,
                    elementBounds.Top,
                    elementBounds.Width,
                    elementBounds.Height);
                this.HighlightTextMargin = new Thickness(
                    elementBounds.Left,
                    elementBounds.Bottom,
                    0,
                    0);
            }
#pragma warning disable 168
            catch (Exception ex)
#pragma warning restore 168
            {
                this.HighlightVisibility = Visibility.Collapsed;
            }

            this.HighlightVisibility = Visibility.Visible;
        } 
        #endregion
    }
}
