using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Debugging.ViewModels;

namespace WinRTXamlToolkit.Debugging.Shared.Views
{
    public sealed partial class ElementListWindow : UserControl
    {
        public ElementListWindow()
        {
            this.InitializeComponent();
        }

        private void Window_OnClosing(object sender, CancelEventArgs e)
        {
            var vm = (ElementListToolWindowViewModel)this.DataContext;
            vm.Remove();
            //((ToolWindow)sender).Hide();
            //e.Cancel = true;
        }
    }
}
