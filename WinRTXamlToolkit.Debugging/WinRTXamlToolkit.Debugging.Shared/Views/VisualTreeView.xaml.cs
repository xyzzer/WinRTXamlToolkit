using System;
using System.Linq;
using WinRTXamlToolkit.Controls;
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
#if WINDOWS_APP
            var treeViewMouseResources =
                new ResourceDictionary
                {
                    Source = new Uri("ms-appx:///WinRTXamlToolkit/Controls/TreeView/TreeViewMouse.xaml")
                };
            this.Resources.MergedDictionaries.Add(treeViewMouseResources);
            this.treeView.Style = (Style)treeViewMouseResources["MouseTreeViewStyle"];
#endif

            this.DataContext = VisualTreeViewModel.Instance;
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

        private void OnFocusTrackerButtonClick(object sender, RoutedEventArgs e)
        {
            this.FocusTrackerButton = (ToolBarToggleButton)sender;
            var show = this.FocusTrackerButton.IsChecked.Value;

            if (show)
            {
                var vm = new FocusTrackerToolWindowViewModel();
                DebugConsoleViewModel.Instance.ToolWindows.Add(vm);
                vm.Removed += this.OnFocusTrackerRemoved;
            }
            else
            {
                var vm = DebugConsoleViewModel.Instance.ToolWindows.OfType<FocusTrackerToolWindowViewModel>().First();
                vm.Remove();
            }
        }

        private void OnFocusTrackerRemoved(object sender, EventArgs e)
        {
            var vm = (FocusTrackerToolWindowViewModel)sender;
            vm.Removed -= this.OnFocusTrackerRemoved;
            this.FocusTrackerButton.IsChecked = false;
        }
    }
}
