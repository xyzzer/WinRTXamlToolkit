using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Debugging.Views
{
    public sealed partial class DependencyObjectDetailsView : UserControl
    {
        public DependencyObjectDetailsView()
        {
            this.InitializeComponent();
        }

//        private void PreviewTabButton_OnChecked(object sender, RoutedEventArgs e)
//        {
//            var vm = this.DataContext as DependencyObjectViewModel;

//            if (vm == null)
//            {
//                return;
//            }

//            //vm.TreeModel.IsPreviewShown = true;
//#pragma warning disable 4014
//            vm.LoadPreview();
//#pragma warning restore 4014
//        }

//        private void PreviewTabButton_OnUnchecked(object sender, RoutedEventArgs e)
//        {
//            var vm = this.DataContext as DependencyObjectViewModel;

//            if (vm == null)
//            {
//                return;
//            }

//            //vm.TreeModel.IsPreviewShown = false;
//        }
    }
}
