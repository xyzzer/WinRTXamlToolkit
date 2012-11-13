using System.Text;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Debugging
{
    public sealed class DebugConsole : Control
    {
        private readonly StringBuilder _unFlushedLines = new StringBuilder();
        private TextBox _debugTextBox;

        public DebugConsole()
        {
            this.DefaultStyleKey = typeof(DebugConsole);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _debugTextBox = GetTemplateChild("DebugTextBox") as TextBox;

            if (_debugTextBox != null)
            {
                _debugTextBox.Text = _unFlushedLines.ToString();
                _unFlushedLines.Length = 0;
            }
        }

        internal void Clear()
        {
            _unFlushedLines.Length = 0;

            if (_debugTextBox == null)
            {
                return;
            }

            _debugTextBox.Text = "";
        }

        internal void Append(string line)
        {
            if (_debugTextBox == null)
            {
                _unFlushedLines.Append(line);
                return;
            }

            if (!_debugTextBox.Dispatcher.HasThreadAccess)
            {
#pragma warning disable 4014
                _debugTextBox.Dispatcher.RunAsync(
                                    CoreDispatcherPriority.High,
                                    () => Append(line));
#pragma warning restore 4014
                return;
            }

            _debugTextBox.Text += line;
            var sv = _debugTextBox.GetFirstDescendantOfType<ScrollViewer>();
            sv.ScrollToVerticalOffset(10000000);
        }
    }
}
