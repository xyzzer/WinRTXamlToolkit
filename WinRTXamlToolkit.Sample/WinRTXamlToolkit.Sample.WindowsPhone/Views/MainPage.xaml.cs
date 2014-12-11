using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Debugging;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => DC.ShowVisualTree(this);
        }
    }
}
