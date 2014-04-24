using WinRTXamlToolkit.Debugging.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Debugging.Views
{
    public sealed partial class VisualTreeView : UserControl
    {
        public VisualTreeView()
        {
            this.InitializeComponent();
            this.DataContext = new VisualTreeViewModel();
        }

        private void OnRefreshButtonClick(object sender, RoutedEventArgs e)
        {
            var vm = (VisualTreeViewModel)this.DataContext;
#pragma warning disable 4014
            vm.Refresh();
#pragma warning restore 4014
        }

        private void OnSelectFocusedClick(object sender, RoutedEventArgs e)
        {
            var vm = (VisualTreeViewModel)this.DataContext;
#pragma warning disable 4014
            vm.SelectFocused();
#pragma warning restore 4014
        }

        private void OnFocusSelectedClick(object sender, RoutedEventArgs e)
        {
            var dob = this.treeView.SelectedItem as DependencyObjectViewModel;

            if (dob == null)
                return;

            var control = dob.Model as Control;

            if (control != null)
            {
                control.Focus(FocusState.Programmatic);
            }
        }
    }
}
