using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Debugging.ViewModels;

namespace WinRTXamlToolkit.Debugging.Views
{
    public sealed partial class ElementListToolWindow : UserControl
    {
        public ElementListToolWindow()
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
