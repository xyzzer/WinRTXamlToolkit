using System.Text;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Debugging.ViewModels;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Debugging.Views
{
    [TemplateVisualState(GroupName = "ExpansionStates", Name = "Collapsed")]
    [TemplateVisualState(GroupName = "ExpansionStates", Name = "Expanded")]
    public sealed partial class DebugConsoleView : UserControl
    {
        private readonly StringBuilder _unFlushedLines = new StringBuilder();
        private DebugConsoleViewModel _viewModel;

        public DebugConsoleView()
        {
            this.InitializeComponent();
            this.DataContext = _viewModel = new DebugConsoleViewModel();
            VisualStateManager.GoToState(this, "Expanded", false);
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

        internal void ShowLog()
        {
            LogTabButton.IsChecked = true;
        }

        internal void ShowVisualTree(UIElement element = null)
        {
            VisualTreeButton.IsChecked = true;

            if (element != null)
            {
                _viewModel.VisualTreeView.SelectItem(element);
            }
        }

        private void CollapseButton_OnChecked(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Collapsed", true);
        }

        private void CollapseButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Expanded", true);
        }
    }
}
