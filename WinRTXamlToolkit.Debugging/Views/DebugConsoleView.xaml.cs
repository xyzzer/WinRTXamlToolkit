using System.Text;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Debugging.Views
{
    public sealed partial class DebugConsoleView : UserControl
    {
        private readonly StringBuilder _unFlushedLines = new StringBuilder();

        public DebugConsoleView()
        {
            this.InitializeComponent();
            this.DefaultStyleKey = typeof(DebugConsoleView);
            DebugTextBox.Text = _unFlushedLines.ToString();
            _unFlushedLines.Length = 0;
        }

        internal void Clear()
        {
            _unFlushedLines.Length = 0;

            DebugTextBox.Text = "";
        }

        internal void Append(string line)
        {
            if (!this.Dispatcher.HasThreadAccess)
            {
#pragma warning disable 4014
                this.Dispatcher.RunAsync(CoreDispatcherPriority.High, () => Append(line));
#pragma warning restore 4014

                return;
            }

            DebugTextBox.Text += line;
            var sv = DebugTextBox.GetFirstDescendantOfType<ScrollViewer>();

            if (sv != null &&
                sv.VerticalOffset + sv.ViewportHeight >= sv.ScrollableHeight - 1)
            {
                sv.ScrollToVerticalOffset(10000000);
            }
        }

        private void OnTabButtonChecked(object sender, RoutedEventArgs e)
        {
            if (DebugTextBox == null)
            {
                return;
            }

            DebugTextBox.Visibility = sender == LogTabButton ? Visibility.Visible : Visibility.Collapsed;
            VisualTreeViewControl.Visibility = sender == VisualTreeButton ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
