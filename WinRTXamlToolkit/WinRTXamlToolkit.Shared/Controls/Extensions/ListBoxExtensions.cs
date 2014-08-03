using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Extension methods and attached properties for the ListBox class.
    /// </summary>
    public static class ListBoxExtensions
    {
        #region BindableSelection
        /// <summary>
        /// BindableSelection Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty BindableSelectionProperty =
            DependencyProperty.RegisterAttached(
                "BindableSelection",
                typeof(object),
                typeof(ListBoxExtensions),
                new PropertyMetadata(null, OnBindableSelectionChanged));

        /// <summary>
        /// Gets the BindableSelection property. This dependency property 
        /// indicates the list of selected items that is synchronized
        /// with the items selected in the ListBox.
        /// </summary>
        public static ObservableCollection<object> GetBindableSelection(DependencyObject d)
        {
            return (ObservableCollection<object>)d.GetValue(BindableSelectionProperty);
        }

        /// <summary>
        /// Sets the BindableSelection property. This dependency property 
        /// indicates the list of selected items that is synchronized
        /// with the items selected in the ListBox.
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
                var handler = new ListBoxBindableSelectionHandler(
                    (ListBox)d, newBindableSelection);
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
                typeof(ListBoxBindableSelectionHandler),
                typeof(ListBoxExtensions),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the BindableSelectionHandler property. This dependency property 
        /// indicates BindableSelectionHandler for a ListBox - used to manage synchronization of BindableSelection and SelectedItems.
        /// </summary>
        public static ListBoxBindableSelectionHandler GetBindableSelectionHandler(
            DependencyObject d)
        {
            return
                (ListBoxBindableSelectionHandler)
                d.GetValue(BindableSelectionHandlerProperty);
        }

        /// <summary>
        /// Sets the BindableSelectionHandler property. This dependency property 
        /// indicates BindableSelectionHandler for a ListBox - used to manage synchronization of BindableSelection and SelectedItems.
        /// </summary>
        public static void SetBindableSelectionHandler(
            DependencyObject d,
            ListBoxBindableSelectionHandler value)
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
                typeof(object),
                typeof(ListBoxExtensions),
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
                var listBox = (ListBox)d;
                listBox.ScrollIntoView(newItemToBringIntoView);
            }
        }
        #endregion

        /// <summary>
        /// Scrolls a vertical ListBox to the bottom.
        /// </summary>
        /// <param name="listBox"></param>
        public static void ScrollToBottom(this ListBox listBox)
        {
            var scrollViewer = listBox.GetFirstDescendantOfType<ScrollViewer>();
            scrollViewer.ChangeView(null, scrollViewer.ScrollableHeight, null);
       }
    }

    /// <summary>
    /// Handles synchronization of ListBoxExtensions.BindableSelection to a ListBox.
    /// </summary>
    public class ListBoxBindableSelectionHandler
    {
        private ListBox _listBox;
        private dynamic _boundSelection;
        private readonly NotifyCollectionChangedEventHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxBindableSelectionHandler"/> class.
        /// </summary>
        /// <param name="listBox">The ListBox.</param>
        /// <param name="boundSelection">The bound selection.</param>
        public ListBoxBindableSelectionHandler(
            ListBox listBox, dynamic boundSelection)
        {
            _handler = OnBoundSelectionChanged;
            Attach(listBox, boundSelection);
        }

        private void Attach(ListBox listBox, dynamic boundSelection)
        {
            _listBox = listBox;
            _listBox.SelectionChanged += OnListBoxSelectionChanged;
            _boundSelection = boundSelection;
            _listBox.SelectedItems.Clear();

            foreach (object item in _boundSelection)
            {
                if (!_listBox.SelectedItems.Contains(item))
                {
                    _listBox.SelectedItems.Add(item);
                }
            }

            var eventInfo =
                _boundSelection.GetType().GetDeclaredEvent("CollectionChanged");
            eventInfo.AddEventHandler(_boundSelection, _handler);
            //_boundSelection.CollectionChanged += OnBoundSelectionChanged;
        }

        private void OnListBoxSelectionChanged(
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
                _listBox.SelectedItems.Clear();

                foreach (var item in _boundSelection)
                {
                    if (!_listBox.SelectedItems.Contains(item))
                    {
                        _listBox.SelectedItems.Add(item);
                    }
                }

                return;
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    if (_listBox.SelectedItems.Contains(item))
                    {
                        _listBox.SelectedItems.Remove(item);
                    }
                }
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (!_listBox.SelectedItems.Contains(item))
                    {
                        _listBox.SelectedItems.Add(item);
                    }
                }
            }
        }

        internal void Detach()
        {
            _listBox.SelectionChanged -= OnListBoxSelectionChanged;
            _listBox = null;
            var eventInfo =
                _boundSelection.GetType().GetDeclaredEvent("CollectionChanged");
            eventInfo.RemoveEventHandler(_boundSelection, _handler);
            _boundSelection = null;
        }
    }
}