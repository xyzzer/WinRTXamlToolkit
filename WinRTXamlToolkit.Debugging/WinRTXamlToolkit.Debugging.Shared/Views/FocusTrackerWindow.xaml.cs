using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Debugging.ViewModels;

namespace WinRTXamlToolkit.Debugging.Shared.Views
{
    public sealed partial class FocusTrackerWindow : UserControl
    {
        private FocusTrackerToolWindowViewModel vm;

        public FocusTrackerWindow()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.vm = (FocusTrackerToolWindowViewModel)this.DataContext;
            vm.FocusTracker = this.FocusVisualizer.FocusTracker;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            vm.FocusTracker = null;
        }

        private void Window_OnClosing(object sender, CancelEventArgs e)
        {
            var vm = (FocusTrackerToolWindowViewModel)this.DataContext;
            vm.Remove();
            //((ToolWindow)sender).Hide();
            //e.Cancel = true;
        }
    }
}
