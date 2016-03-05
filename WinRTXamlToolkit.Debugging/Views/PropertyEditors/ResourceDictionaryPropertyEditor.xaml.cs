using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Debugging.ViewModels;

namespace WinRTXamlToolkit.Debugging.Views.PropertyEditors
{
    public sealed partial class ResourceDictionaryPropertyEditor : UserControl
    {
        public ResourceDictionaryPropertyEditor()
        {
            this.InitializeComponent();
        }

        private void OnResourceBrowserButtonClick(object sender, RoutedEventArgs e)
        {
            var propertyViewModel = (BasePropertyViewModel)this.DataContext;
            var resourceDictionary = (ResourceDictionary)propertyViewModel.Value;
            var vm = new ResourceBrowserToolWindowViewModel(resourceDictionary);
            DebugConsoleViewModel.Instance.ToolWindows.Add(vm);
        }
    }
}
