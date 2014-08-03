using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Text;
using WinRTXamlToolkit.Debugging.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Debugging.Views.PropertyEditors
{
    public class NamedFontWeight
    {
        public string Name { get; private set; }
        public FontWeight FontWeight { get; private set; }

        public NamedFontWeight(string name, FontWeight fontWeight)
        {
            Name = name;
            FontWeight = fontWeight;
        }
    }

    public sealed partial class FontWeightPropertyEditor : UserControl
    {
        private List<NamedFontWeight> fontWeights;

        public FontWeightPropertyEditor()
        {
            this.InitializeComponent();
            this.fontWeights = PopulateFontWeights();
            this.Loaded += OnLoaded;
            this.DataContextChanged += FontWeightPropertyEditor_DataContextChanged;
        }

        private void FontWeightPropertyEditor_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var vm = (BasePropertyViewModel)this.DataContext;
            this.combo.SelectedItem = this.fontWeights.First(fw => fw.FontWeight.Weight == ((FontWeight)vm.Value).Weight);
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
#if !WIN81
            var vm = (BasePropertyViewModel)this.DataContext;
            this.combo.SelectedItem = this.fontWeights.First(fw => fw.FontWeight.Weight == ((FontWeight)vm.Value).Weight);
#endif
        }

        private List<NamedFontWeight> PopulateFontWeights()
        {
            var fontWeightsProperties = typeof (FontWeights).GetTypeInfo().DeclaredProperties;

            // Get defined colors
            var fontWeights = fontWeightsProperties
                .Select(
                    pi =>
                    {
                        var fontWeight = (FontWeight)pi.GetValue(null);
                        return new NamedFontWeight(pi.Name, (FontWeight)pi.GetValue(null));
                    })
                .OrderByDescending(nfw => nfw.FontWeight.Weight)
                .ToList();
            this.combo.ItemsSource = fontWeights;
            this.combo.DisplayMemberPath = "Name";

            return fontWeights;
        }

        private void OnComboSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.combo.SelectedItem != null)
            {
                var nfw = (NamedFontWeight)this.combo.SelectedItem;

                var vm = (BasePropertyViewModel)this.DataContext;
                vm.Value = nfw.FontWeight;
            }
        }
    }
}
