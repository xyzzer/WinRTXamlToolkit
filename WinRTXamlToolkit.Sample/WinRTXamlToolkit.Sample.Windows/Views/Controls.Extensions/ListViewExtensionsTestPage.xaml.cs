using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;
using WinRTXamlToolkit.Sample.Common;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ListViewExtensionsTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public ListViewExtensionsTestPage()
        {
            this.InitializeComponent();
            this.DataContext = new ListViewExtensionsTestViewModel();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void AddItem(object sender, RoutedEventArgs e)
        {
            var vm = (ListViewExtensionsTestViewModel)this.DataContext;
            vm.MyItems.Add(Guid.NewGuid().ToString());

            // Need to wait for the ListView to update
            await Task.Delay(100);

            SourceListView.ScrollToBottom();
            //SourceListView.ScrollIntoView(SourceListView.Items[SourceListView.Items.Count - 1]);
        }
    }

    public class ListViewExtensionsTestViewModel : BindableBase
    {
        #region MyItems
        private ObservableCollection<string> _myItems;
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public ObservableCollection<string> MyItems
        {
            get { return _myItems; }
            set { this.SetProperty(ref _myItems, value); }
        }
        #endregion

        #region SelectedItems
        private ObservableCollection<object> _selectedItems;
        /// <summary>
        /// Gets or sets the selected items.
        /// </summary>
        public ObservableCollection<object> SelectedItems
        {
            get { return _selectedItems; }
            set { this.SetProperty(ref _selectedItems, value); }
        }
        #endregion

        public ListViewExtensionsTestViewModel()
        {
            MyItems = new ObservableCollection<string>();

            for (int i = 0; i < 100; i++)
            {
                _myItems.Add(string.Format("Item {0}", i));
            }

            SelectedItems = new ObservableCollection<object>();
        }
    }
}
