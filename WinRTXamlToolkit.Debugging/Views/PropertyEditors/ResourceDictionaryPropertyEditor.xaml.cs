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
            this.DataContextChanged += ResourceDictionaryPropertyEditor_DataContextChanged;
        }

        private void ResourceDictionaryPropertyEditor_DataContextChanged(FrameworkElement sender,
            DataContextChangedEventArgs args)
        {
            var rd = ((PropertyViewModel)this.DataContext).Value as ResourceDictionary;

            if (rd == null)
            {
                this.ResourceBrowserButton.IsEnabled = false;
                this.InfoTextBlock.Text = "<null>";
            }
            else
            {
                this.InfoTextBlock.Text = $"Resources: {rd.Count}\r\nMergedDictionaries: {rd.MergedDictionaries.Count}\r\nThemeDictionaries: {rd.ThemeDictionaries.Count}";
                this.ResourceBrowserButton.IsEnabled =
                    rd.Count > 0 ||
                    rd.MergedDictionaries.Count > 0 ||
                    rd.ThemeDictionaries.Count > 0;
            }
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
