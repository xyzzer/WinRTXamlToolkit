using System.Diagnostics;
using WinRTXamlToolkit.Debugging;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class DebugConsoleTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public DebugConsoleTestPage()
        {
            this.InitializeComponent();
            this.CollapseReleaseBuildWarning();
        }

        [Conditional("DEBUG")]
        private void CollapseReleaseBuildWarning()
        {
            releaseBuildWarning.Visibility = Visibility.Collapsed;
            testPanel.Visibility = Visibility.Visible;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
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
    }
}
