using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Debugging.ViewModels;

namespace WinRTXamlToolkit.Debugging.Views
{
    public sealed partial class ResourceBrowserToolWindow : UserControl
    {
        private ResourceBrowserToolWindowViewModel vm;

        public ResourceBrowserToolWindow()
        {
            this.InitializeComponent();
        }

        private void Window_OnClosing(object sender, CancelEventArgs e)
        {
            var vm = (ResourceBrowserToolWindowViewModel)this.DataContext;
            vm.Remove();
        }
    }
}
