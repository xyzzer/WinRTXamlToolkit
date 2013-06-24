using WinRTXamlToolkit.Debugging.ViewModels;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Debugging.Views
{
    public sealed partial class PropertiesView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertiesView"/> class.
        /// </summary>
        public PropertiesView()
        {
            this.InitializeComponent();
        }

        private void OnSearchBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var vm = (DependencyObjectViewModel)this.DataContext;
            vm.PropertyNameFilter = this.SearchBox.Text;
        }
    }
}
