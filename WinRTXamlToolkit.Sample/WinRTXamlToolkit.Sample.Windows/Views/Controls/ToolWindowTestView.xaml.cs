using Windows.UI.Xaml.Input;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Sample.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ToolWindowTestView : UserControl
    {
        public ToolWindowTestView()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= OnLoaded;

            foreach (var tw in this.GetDescendantsOfType<ToolWindow>())
            {
                //ToolWindow tw1 = tw;
                //tw.Closing += (s, e2) => { e2.Cancel = true; tw1.SnapToEdge(); };
                tw.Closed += (s, e2) => this.OuterGrid.Children.Add((ToolWindow)s);
                //tw.WindowEdgeSnapBehavior = WindowEdgeSnapBehavior.None;
            }
        }

        private void UIElement_OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            e.Handled = true;
            e.Complete();
        }

        private void UIElement_OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
