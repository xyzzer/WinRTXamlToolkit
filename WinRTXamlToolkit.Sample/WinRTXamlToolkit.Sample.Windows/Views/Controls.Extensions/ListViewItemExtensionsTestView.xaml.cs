using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ListViewItemExtensionsTestView : UserControl
    {
        public ListViewItemExtensionsTestView()
        {
            this.InitializeComponent();

            lv.ItemsSource = new List<Item>(
                new[]
                {
                    new Item {IsEnabled = true, IsSelected = true, Text = "selected"},
                    new Item {IsEnabled = false, IsSelected = true, Text = "disabled, selected"},
                    new Item {IsEnabled = true, IsSelected = false, Text = "default"},
                    new Item {IsEnabled = false, IsSelected = false, Text = "disabled"},
                });
        }

        public class Item
        {
            public bool IsEnabled { get; set; }
            public bool IsSelected { get; set; }
            public string Text { get; set; }
        }
    }
}
