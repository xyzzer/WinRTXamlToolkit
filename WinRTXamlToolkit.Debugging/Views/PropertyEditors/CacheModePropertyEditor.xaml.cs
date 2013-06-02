using WinRTXamlToolkit.Debugging.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Debugging.Views.PropertyEditors
{
    public sealed partial class CacheModePropertyEditor : UserControl
    {
        public CacheModePropertyEditor()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var vm = (BasePropertyViewModel)this.DataContext;
            cb.IsChecked = vm.Value is BitmapCache;
        }

        private void CheckboxChecked(object sender, RoutedEventArgs routedEventArgs)
        {
            var vm = (BasePropertyViewModel)this.DataContext;

            if (!(vm.Value is BitmapCache))
            {
                vm.Value = new BitmapCache();
            }
        }

        private void CheckboxUnchecked(object sender, RoutedEventArgs routedEventArgs)
        {
            var vm = (BasePropertyViewModel)this.DataContext;

            if (vm.Value != null)
            {
                vm.Value = null;
            }
        }
    }
}
