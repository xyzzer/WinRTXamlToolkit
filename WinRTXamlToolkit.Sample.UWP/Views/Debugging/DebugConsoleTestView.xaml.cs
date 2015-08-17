using System.Diagnostics;
using WinRTXamlToolkit.Debugging;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class DebugConsoleTestView : UserControl
    {
        public DebugConsoleTestView()
        {
            this.InitializeComponent();
            this.CollapseReleaseBuildWarning();
            DC.ShowLog();
            DC.Expand();
        }

        [Conditional("DEBUG")]
        private void CollapseReleaseBuildWarning()
        {
            releaseBuildWarning.Visibility = Visibility.Collapsed;
            testPanel.Visibility = Visibility.Visible;
        }

        private void OnTraceButtonClicked(object sender, RoutedEventArgs e)
        {
            Trace();
        }

        private void OnTraceLineKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                Trace();
            }
        }

        private void Trace()
        {
            DC.Trace(line.Text);
            line.Focus(FocusState.Programmatic);
            line.SelectAll();
        }

        private void Border_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(FlickBorder);
            DC.Trace(point.Position);
        }
    }
}
