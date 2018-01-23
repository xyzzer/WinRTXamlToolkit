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
using WinRTXamlToolkit.Debugging.Views;

namespace WinRTXamlToolkit.Debugging.ViewModels.Stubs
{
    /// <summary>
    /// A stub type - a hack really so that we could show the app object in the visual tree view.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.Control" />
    internal class Application : Control
    {
        private static void Clone(ResourceDictionary source, ResourceDictionary target)
        {
            foreach (var resourceKeyValue in source)
            {
                if (!target.ContainsKey(resourceKeyValue.Key))
                {
                    target.Add(resourceKeyValue.Key, resourceKeyValue.Value);
                }
            }

            foreach (var dictionary in source.MergedDictionaries)
            {
                var clone = new ResourceDictionary();
                Clone(dictionary, clone);
                target.MergedDictionaries.Add(clone);
            }

            foreach (var dictionaryKeyValue in source.ThemeDictionaries)
            {
                var clone = new ResourceDictionary();
                Clone((ResourceDictionary)dictionaryKeyValue.Value, clone);
                if (!target.ContainsKey(dictionaryKeyValue.Key))
                {
                    target.ThemeDictionaries.Add(dictionaryKeyValue.Key, clone);
                }
            }
        }

        public Application()
        {
            Clone(Windows.UI.Xaml.Application.Current.Resources, this.Resources);
        }
    }
}

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class VisualTreeViewModel : TreeViewModel
    {
        private Point _pointerPosition;
        private bool _isShiftPressed;
        private bool _isCtrlPressed;

        #region Instance (singleton implementation)
        private static VisualTreeViewModel _instance;
        public static VisualTreeViewModel Instance => _instance ?? (_instance = new VisualTreeViewModel());

        private VisualTreeViewModel()
        {
            //this.RootElements.Add(new StubTreeItemViewModel(this, null));
#pragma warning disable 4014
            this.GetPropertyLists();
            this.Build();
#pragma warning restore 4014
            Window.Current.CoreWindow.KeyDown += this.OnKeyDown;
            Window.Current.CoreWindow.KeyUp += this.OnKeyUp;
            Window.Current.CoreWindow.PointerMoved += this.OnPointerMoved;
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
                if (this.RootElements.Count == 0 ||
                    this.RootElements[0] is StubVisualTreeItemViewModel)
                {
                    //await Task.Delay(3000);
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
        /// <summary>
        /// Gets the root elements in the visual tree.
        /// </summary>
        public ObservableCollection<VisualTreeItemViewModel> RootElements { get; } = new ObservableCollection<VisualTreeItemViewModel>();
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
        private bool _isPreviewShown;
        public bool IsPreviewShown
        {
            get { return _isPreviewShown; }
            set { this.SetProperty(ref _isPreviewShown, value); }
        }
        #endregion

        #region ShowPropertiesGrouped
        private bool _showPropertiesGrouped;
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

        public DependencyObjectViewModel AppViewModel { get; private set; }
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
            this.SelectElementUnderPointerAsync();
            this.IsShown = true;
#pragma warning restore 4014
        } 
        #endregion

        #region SelectElementUnderPointerAsync()
        internal async Task SelectElementUnderPointerAsync(bool showDebugger = true)
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

                var hoveredElementCandidate = hoveredElements.FirstOrDefault(element => !element.GetAncestorsOfType<VisualTreeView>().Any());

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
                await this.SelectItemAsync(hoveredElement, true);
            }

            if (showDebugger)
            {
                DC.Show();
                DC.Expand();
            }
        } 
        #endregion

        #region SelectFocusedAsync()
        internal async Task SelectFocusedAsync()
        {
            var focusedElement = FocusManager.GetFocusedElement() as UIElement;

            if (focusedElement != null &&
                !focusedElement.GetAncestorsOfType<VisualTreeView>().Any())
            {
                await this.SelectItemAsync(focusedElement, true);
            }
        } 
        #endregion

        #region SelectItemAsync()
        internal async Task<bool> SelectItemAsync(UIElement element, bool refreshOnFail = false)
        {
            var ancestors = new[] { element }.Concat(element.GetAncestors()).ToList();

            if (this.RootElements == null || this.RootElements.Count == 0)
            {
                await this.Refresh();
            }

            var vm = this.RootElements[0] as DependencyObjectViewModel;
            var ancestorIndex = ancestors.IndexOf(vm?.Model as DependencyObject);

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
                        ancestorIndex = ancestors.IndexOf(vm?.Model as DependencyObject);
                    }
                }

                if (ancestorIndex < 0)
                {
                    await this.Refresh();
                    ancestorIndex = ancestors.IndexOf(vm?.Model as DependencyObject);
                }
            }

            if (ancestorIndex < 0)
            {
                System.Diagnostics.Debug.WriteLine("Something's wrong, but let's not throw exceptions here.");

                //Debugger.Break();
                if (refreshOnFail)
                {
                    await this.Refresh();
                    return await this.SelectItemAsync(element);
                }

                return false;
            }

            System.Diagnostics.Debug.Assert(vm != null);

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
                        vm.Children.Clear();
                        //await vm.LoadPropertiesAsync();
                        await vm.LoadChildrenAsync();
                        //if (vm.Parent == null)
                        //{
                        //    await Refresh();
                        //}
                        //else
                        //{
                        //    // do a partial refresh on the stale subtree
                        //    var i = vm.Parent.Children.IndexOf(vm);
                        //    vm.Parent.Children.RemoveAt(i);
                        //    vm.Parent.Children.Insert(i, new DependencyObjectViewModel(this, vm.Parent, ancestors[ancestorIndex]));
                        //}

                        return await this.SelectItemAsync(element);
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
                    this.SelectElementUnderPointerAsync();
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
                    this.SelectElementUnderPointerAsync();
                    this.IsShown = true;
#pragma warning restore 4014
                }
            }
        } 
        #endregion

        #region GetPropertyLists()
        private void GetPropertyLists()
        {
            this.PropertyLists = new ObservableCollection<PropertyList>
            {
                new PropertyList
                {
                    Name = "All",
                    CommaSeparatedPropertyNames = string.Empty
                },
                new PropertyList
                {
                    Name = "Layout",
                    CommaSeparatedPropertyNames =
                        "Width,Height,MinWidth,MinHeight,MaxWidth,MaxHeight,Orientation,Clip,ActualWidth,ActualHeight,Margin,Padding,Canvas.Left,Canvas.Top,Canvas.Zindex,ItemHeight,ItemWidth,LineStackingStrategy,LineHeight,Visibility,Opacity,RenderTransform,Projection,StrokeThickness,BorderThickness,Grid.Row,Grid.Column,Grid.RowSpan,Grid.ColumnSpan,VariableSizedWrapGrid.ColumnSpan,VariableSizedWrapGrid.RowSpan,VerticalAlignment,HorizontalAlignment,VerticalContentAlignment,HorizontalContentAlignment"
                }
            };
            this.CurrentPropertyList = this.PropertyLists[0];
        } 
        #endregion

        #region Build()
#pragma warning disable 1998
        private async Task Build()
#pragma warning restore 1998
        {
            this.RootElements.Clear();

            var appResourcesElement = new Stubs.Application();
            this.AppViewModel = new DependencyObjectViewModel(this, null, appResourcesElement);
            this.RootElements.Add(this.AppViewModel);

            var rootElement = VisualTreeHelperExtensions.GetRealWindowRoot();

            if (rootElement != null)
            {
                var ancestors = rootElement.GetAncestors();

                var newRoot = ancestors?.OfType<UIElement>().LastOrDefault();

                if (newRoot != null)
                {
                    rootElement = newRoot;
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
                //this.RootElements.Clear(); // this is already done inside of Build()
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

                var debugConsoleView = DebugConsoleOverlay.View;
                var elementBounds = fe.GetBoundingRect(debugConsoleView);

                this.HighlightMargin = new Thickness(
                    elementBounds.Left,
                    elementBounds.Top,
                    debugConsoleView.ActualWidth - elementBounds.Right,
                    debugConsoleView.ActualHeight - elementBounds.Bottom);
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
            catch (Exception)
#pragma warning restore 168
            {
                this.HighlightVisibility = Visibility.Collapsed;
            }

            this.HighlightVisibility = Visibility.Visible;
        }
        #endregion

        protected override void OnSelectedItemChanged(
            TreeItemViewModel oldSelectedItem,
            TreeItemViewModel newSelectedItem)
        {
            var oldItemAsDependencyObjectViewModel = oldSelectedItem as DependencyObjectViewModel;

            if (oldItemAsDependencyObjectViewModel != null)
            {
                oldItemAsDependencyObjectViewModel.ModelPropertyChanged -= this.OnModelPropertyChanged;
            }

            var newItemAsDependencyObjectViewModel = newSelectedItem as DependencyObjectViewModel;

            if (newItemAsDependencyObjectViewModel != null)
            {
                newItemAsDependencyObjectViewModel.ModelPropertyChanged += this.OnModelPropertyChanged;
#pragma warning disable 4014
                newSelectedItem.LoadPropertiesAsync();
#pragma warning restore 4014
            }

            this.UpdateHighlight();
        }

        private async void OnModelPropertyChanged(object sender, EventArgs eventArgs)
        {
            this.UpdateHighlight();

            // Wait for pending layout updates
            await Task.Delay(100);

            this.UpdateHighlight();
        }
    }
}
