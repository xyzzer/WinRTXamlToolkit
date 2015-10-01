using WinRTXamlToolkit.Sample.ViewModels.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using WinRTXamlToolkit.Controls;
using System;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class TreeViewTestView : UserControl
    {
        private TreeView tvDataBound;
        private TreeViewPageViewModel vm;

        public TreeViewTestView()
        {
            this.InitializeComponent();
            this.DataContext = this.vm = new TreeViewPageViewModel();
            this.NewTreeView();
            this.vm.BuildTree();
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var toggleButton = (ToggleButton)sender;
            var styleName = (string)toggleButton.Content;
            var style = this.Resources[styleName] as Style;

            if (style != null &&
                tvDataBound != null)
            {
                tvDataBound.Style = style;

                if (toggleButton == MouseThemeRadioButton)
                {
                    tvDataBound.RequestedTheme = ElementTheme.Light;
                }
                else
                {
                    tvDataBound.RequestedTheme = ElementTheme.Default;
                }
            }
        }

        private void NewTreeView()
        {
            this.ContainerGrid.Children.Clear();
            vm.TreeItems?.Clear();
            this.ContainerGrid.Children.Add(tvDataBound = (TreeView)this.TreeViewTemplate.LoadContent());
        }

        private void OnNewTreeViewButtonClick(object sender, RoutedEventArgs e)
        {
            this.NewTreeView();
        }

        private void OnBuildTreeButtonClick(object sender, RoutedEventArgs e)
        {
            this.vm.BuildTree();
        }
    }
}
