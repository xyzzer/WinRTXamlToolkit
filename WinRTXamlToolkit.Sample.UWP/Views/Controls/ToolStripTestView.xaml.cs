using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Debugging;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ToolStripTestView : UserControl
    {
        public ToolStripTestView()
        {
            this.InitializeComponent();
            DC.ShowLog();
        }

        private void OnFileClick(object sender, RoutedEventArgs e)
        {
            DC.TraceLocalized();
        }

        private void OnOptionsClick(object sender, RoutedEventArgs e)
        {
            DC.TraceLocalized();
        }

        private void OnPreferencesClick(object sender, RoutedEventArgs e)
        {
            DC.TraceLocalized();
        }

        private void OnBobbleClick(object sender, RoutedEventArgs e)
        {
            DC.TraceLocalized();
        }

        private void OnViewsClick(object sender, RoutedEventArgs e)
        {
            DC.TraceLocalized();
        }
    }
}
