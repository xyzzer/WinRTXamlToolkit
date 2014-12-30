using System.ComponentModel;
using System.Text;
using WinRTXamlToolkit.AwaitableUI;
using WinRTXamlToolkit.Controls;
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

#if WINDOWS_PHONE_APP
            this.Window.WindowEdgeSnapBehavior = WindowEdgeSnapBehavior.None;
            this.LayoutGrid.Children.Remove(this.Window);
            var vb = new Viewbox();
            var sg = new Grid();
            sg.Children.Add(this.Window);
            vb.Child = sg;
            this.LayoutGrid.Children.Insert(0, vb);

            this.SizeChanged += (s, e) =>
            {
                sg.Width = this.ActualWidth * 2;
                sg.Height = this.ActualHeight * 2;
            };
#endif

            this.DataContext = _viewModel = DebugConsoleViewModel.Instance;
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
                sv.ChangeView(0, 10000000, 1);
            }
        }

        internal void ShowLog()
        {
            LogTabButton.IsChecked = true;
        }

        internal async void ShowVisualTree(UIElement element = null)
        {
            await this.WaitForLoadedAsync();

            if (element != null &&
                _viewModel.VisualTreeView != null)
            {
                _viewModel.VisualTreeView.IsShown = true;
                var fe = element as FrameworkElement;

                if (fe != null)
                {
                    await fe.WaitForLoadedAsync();
                }

#pragma warning disable 4014
                _viewModel.VisualTreeView.SelectItem(element);
#pragma warning restore 4014
            }
        }

        internal void Collapse()
        {
            this.Window.SnapToEdgeAsync();
        }

        internal void Expand()
        {
            this.Window.RestoreAsync();
        }

        private void Window_OnClosing(object sender, CancelEventArgs e)
        {
            ((ToolWindow)sender).Hide();
            e.Cancel = true;
            this._viewModel.VisualTreeView.HighlightVisibility = Visibility.Collapsed;
        }
    }
}
