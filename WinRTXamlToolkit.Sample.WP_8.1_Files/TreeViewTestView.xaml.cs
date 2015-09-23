using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Sample.ViewModels.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class TreeViewTestView : UserControl
    {
        public TreeViewTestView()
        {
            this.InitializeComponent();
            this.DataContext = new TreeViewPageViewModel();
        }
    }
}
