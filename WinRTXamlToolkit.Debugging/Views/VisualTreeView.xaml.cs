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
    }
}
