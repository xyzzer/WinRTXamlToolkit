using WinRTXamlToolkit.Debugging.ViewModels;
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
    }
}
