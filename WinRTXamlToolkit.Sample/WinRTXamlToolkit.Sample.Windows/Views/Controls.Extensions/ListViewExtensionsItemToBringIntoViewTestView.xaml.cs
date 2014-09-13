using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ListViewExtensionsItemToBringIntoViewTestView : UserControl
    {
        public ListViewExtensionsItemToBringIntoViewTestView()
        {
            this.InitializeComponent();
            this.DataContext = new ListViewExtensionsTestViewModel();
        }
    }
}
