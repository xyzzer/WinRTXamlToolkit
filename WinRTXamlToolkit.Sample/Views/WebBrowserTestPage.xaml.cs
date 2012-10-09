using System.Threading.Tasks;
using WinRTXamlToolkit.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class WebBrowserTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public WebBrowserTestPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override async Task OnNavigatedTo(AlternativeNavigationEventArgs e)
        {
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
}
}
