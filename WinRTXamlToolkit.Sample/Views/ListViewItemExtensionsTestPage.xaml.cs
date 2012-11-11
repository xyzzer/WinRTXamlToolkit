using System.Collections.Generic;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ListViewItemExtensionsTestPage
        : WinRTXamlToolkit.Controls.AlternativePage
    {
        public ListViewItemExtensionsTestPage()
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

        private void GoBack(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        public class Item
        {
            public bool IsEnabled { get; set; }
            public bool IsSelected { get; set; }
            public string Text { get; set; }
        }
    }
}
