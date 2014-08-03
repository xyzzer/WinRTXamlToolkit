using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <remarks>
    /// Note: ListViewExtensions can be used for GridViews as well, since the extensions actually work on ListViewBase
    /// which both ListView and GridView derive from.
    /// </remarks>
    public static class GridViewExtensions
    {
        #region BindableSelection
        /// <summary>
        /// BindableSelection Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty BindableSelectionProperty =
            DependencyProperty.RegisterAttached(
                "BindableSelection",
                typeof (object),
                typeof (GridViewExtensions),
                new PropertyMetadata(null, OnBindableSelectionChanged));

        /// <summary>
        /// Gets the BindableSelection property. This dependency property 
        /// indicates the list of selected items that is synchronized
        /// with the items selected in the GridView.
        /// </summary>
        public static ObservableCollection<object> GetBindableSelection(DependencyObject d)
        {
            return (ObservableCollection<object>)d.GetValue(BindableSelectionProperty);
        }

        /// <summary>
        /// Sets the BindableSelection property. This dependency property 
        /// indicates the list of selected items that is synchronized
        /// with the items selected in the GridView.
        /// </summary>
        public static void SetBindableSelection(
            DependencyObject d,
            ObservableCollection<object> value)
        {
            d.SetValue(BindableSelectionProperty, value);
        }

        /// <summary>
        /// Handles changes to the BindableSelection property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnBindableSelectionChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            dynamic oldBindableSelection = e.OldValue;
            dynamic newBindableSelection = d.GetValue(BindableSelectionProperty);

            if (oldBindableSelection != null)
            {
                var handler = GetBindableSelectionHandler(d);
                SetBindableSelectionHandler(d, null);
                handler.Detach();
            }

            if (newBindableSelection != null)
            {
                var handler = new GridViewBindableSelectionHandler(
                    (GridView)d, newBindableSelection);
                SetBindableSelectionHandler(d, handler);
            }
        }
        #endregion

        #region BindableSelectionHandler
        /// <summary>
        /// BindableSelectionHandler Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty BindableSelectionHandlerProperty =
            DependencyProperty.RegisterAttached(
                "BindableSelectionHandler",
                typeof (GridViewBindableSelectionHandler),
                typeof (GridViewExtensions),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the BindableSelectionHandler property. This dependency property 
        /// indicates BindableSelectionHandler for a GridView - used
        /// to manage synchronization of BindableSelection and SelectedItems.
        /// </summary>
        public static GridViewBindableSelectionHandler GetBindableSelectionHandler(
            DependencyObject d)
        {
            return
                (GridViewBindableSelectionHandler)
                d.GetValue(BindableSelectionHandlerProperty);
        }

        /// <summary>
        /// Sets the BindableSelectionHandler property. This dependency property 
        /// indicates BindableSelectionHandler for a GridView - used to manage synchronization of BindableSelection and SelectedItems.
        /// </summary>
        public static void SetBindableSelectionHandler(
            DependencyObject d,
            GridViewBindableSelectionHandler value)
        {
            d.SetValue(BindableSelectionHandlerProperty, value);
        }
        #endregion

        #region ItemToBringIntoView
        /// <summary>
        /// ItemToBringIntoView Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty ItemToBringIntoViewProperty =
            DependencyProperty.RegisterAttached(
                "ItemToBringIntoView",
                typeof (object),
                typeof (GridViewExtensions),
                new PropertyMetadata(null, OnItemToBringIntoViewChanged));

        /// <summary>
        /// Gets the ItemToBringIntoView property. This dependency property 
        /// indicates the item that should be brought into view.
        /// </summary>
        public static object GetItemToBringIntoView(DependencyObject d)
        {
            return (object)d.GetValue(ItemToBringIntoViewProperty);
        }

        /// <summary>
        /// Sets the ItemToBringIntoView property. This dependency property 
        /// indicates the item that should be brought into view when first set.
        /// </summary>
        public static void SetItemToBringIntoView(DependencyObject d, object value)
        {
            d.SetValue(ItemToBringIntoViewProperty, value);
        }

        /// <summary>
        /// Handles changes to the ItemToBringIntoView property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnItemToBringIntoViewChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            object newItemToBringIntoView =
                (object)d.GetValue(ItemToBringIntoViewProperty);

            if (newItemToBringIntoView != null)
            {
                var GridView = (GridView)d;
                GridView.ScrollIntoView(newItemToBringIntoView);
            }
        }
        #endregion

        /// <summary>
        /// Scrolls a vertical GridView to the bottom.
        /// </summary>
        /// <param name="GridView"></param>
        public static void ScrollToBottom(this GridView GridView)
        {
            var scrollViewer = GridView.GetFirstDescendantOfType<ScrollViewer>();
            scrollViewer.ChangeView(null, scrollViewer.ScrollableHeight, null);
        }
    }

    /// <summary>
    /// Handles synchronization of GridViewExtensions.BindableSelection to a GridView.
    /// </summary>
    public class GridViewBindableSelectionHandler
    {
        private GridView _gridView;
        private dynamic _boundSelection;
        private readonly NotifyCollectionChangedEventHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="GridViewBindableSelectionHandler"/> class.
        /// </summary>
        /// <param name="GridView">The GridView.</param>
        /// <param name="boundSelection">The bound selection.</param>
        public GridViewBindableSelectionHandler(
            GridView GridView, dynamic boundSelection)
        {
            _handler = OnBoundSelectionChanged;
            Attach(GridView, boundSelection);
        }

        private void Attach(GridView gridView, dynamic boundSelection)
        {
            _gridView = gridView;
            _gridView.SelectionChanged += OnGridViewSelectionChanged;
            _boundSelection = boundSelection;
            _gridView.SelectedItems.Clear();

            foreach (object item in _boundSelection)
            {
                if (!_gridView.SelectedItems.Contains(item))
                {
                    _gridView.SelectedItems.Add(item);
                }
            }

            var eventInfo =
                _boundSelection.GetType().GetDeclaredEvent("CollectionChanged");
            eventInfo.AddEventHandler(_boundSelection, _handler);
            //_boundSelection.CollectionChanged += OnBoundSelectionChanged;
        }

        private void OnGridViewSelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            foreach (dynamic item in e.RemovedItems)
            {
                if (_boundSelection.Contains(item))
                {
                    _boundSelection.Remove(item);
                }
            }
            foreach (dynamic item in e.AddedItems)
            {
                if (!_boundSelection.Contains(item))
                {
                    _boundSelection.Add(item);
                }
            }
        }

        private void OnBoundSelectionChanged(
            object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action ==
                NotifyCollectionChangedAction.Reset)
            {
                _gridView.SelectedItems.Clear();

                foreach (var item in _boundSelection)
                {
                    if (!_gridView.SelectedItems.Contains(item))
                    {
                        _gridView.SelectedItems.Add(item);
                    }
                }

                return;
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    if (_gridView.SelectedItems.Contains(item))
                    {
                        _gridView.SelectedItems.Remove(item);
                    }
                }
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (!_gridView.SelectedItems.Contains(item))
                    {
                        _gridView.SelectedItems.Add(item);
                    }
                }
            }
        }

        internal void Detach()
        {
            _gridView.SelectionChanged -= OnGridViewSelectionChanged;
            _gridView = null;
            var eventInfo =
                _boundSelection.GetType().GetDeclaredEvent("CollectionChanged");
            eventInfo.RemoveEventHandler(_boundSelection, _handler);
            _boundSelection = null;
        }
    }
}