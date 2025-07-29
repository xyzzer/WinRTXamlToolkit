using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WinRTXamlToolkit.Controls.Extensions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Resources;

namespace WinRTXamlToolkit.StylesBrowser
{
    public sealed partial class BrushesPage : Page
    {
        enum SortOptions
        {
            Key,
            Alpha
        }
        private SortOptions sortOption;
        private string filterText = "";

        public string FilterText 
        {
            get => filterText; 
            set 
            {
                filterText = value;
                FilterList();
            }
        }

        public BrushesPage()
        {
            this.InitializeComponent();
            this.Loaded += this.OnLoaded;
        }

        DateTime lastFilterUpdate = DateTime.MinValue;
        private IEnumerable<KeyValuePair<object, object>> brushes;

        private async void FilterList()
        {
            string initialText = this.filterText;
            await Task.Delay(500);
            if (initialText != this.filterText &&
                (DateTime.Now.Subtract(lastFilterUpdate).TotalMilliseconds < 3000))
            {
                // Still typing?
                return;
            }

            lastFilterUpdate = DateTime.Now;

            var filteredList = 
                brushes.Where(r => string.IsNullOrEmpty(this.filterText.Trim()) || ((string)r.Key).Contains(this.filterText.Trim(), StringComparison.CurrentCultureIgnoreCase));
            var sortedList = 
                sortOption == SortOptions.Key
                ? filteredList.OrderBy(r => r.Key)
                : filteredList.OrderBy(r => (r.Value as SolidColorBrush)?.Color.A ?? 0);
            var brushList = 
                sortedList.Select(
                    r => new BrushResourceViewModel(
                             (Brush)r.Value,
                             r.Key.ToString(),
                             r.Value is SolidColorBrush
                                    ? ((SolidColorBrush)r.Value).Color.ToString() : r.Value is BitmapImage
                                    ? (((BitmapImage)r.Value).UriSource ?? new Uri(string.Empty)).ToString() : string.Empty
                         )).ToList();
            this.BrushesListView.ItemsSource = brushList;
            TextView.Text = string.Join(Environment.NewLine, brushList.Select(r => r.Key));
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.brushes = this.GetBrushResources();
            FilterList();
        }

        private IEnumerable<KeyValuePair<object, object>> GetBrushResources()
        {
            var ancestors = this.GetAncestors();

            foreach (var brushResource in GetBrushResources(this.Resources))
            {
                yield return brushResource;
            }

            foreach (var fe in ancestors.OfType<FrameworkElement>().Where(e => e.Resources != null))
            {
                foreach (var brushResource in GetBrushResources(fe.Resources))
                {
                    yield return brushResource;
                }
            }

            foreach (var brushResource in GetBrushResources(Application.Current.Resources))
            {
                yield return brushResource;
            }
        }

        private static IEnumerable<KeyValuePair<object, object>> GetBrushResources(ResourceDictionary resources)
        {
            Debug.WriteLine(new { resources.Source, resources.Count});
            foreach (var mergedDictionary in resources.MergedDictionaries)
            {
                foreach (var brush in GetBrushResources(mergedDictionary))
                {
                    yield return brush;
                }
            }

            object themeDictionary;

            if (resources
                    .ThemeDictionaries
                    .TryGetValue("Default", out themeDictionary))
            {
                Debug.Assert(themeDictionary is ResourceDictionary);

                foreach (var brush in GetBrushResources((ResourceDictionary)themeDictionary))
                {
                    yield return brush;
                }
            }


            if (resources
                    .ThemeDictionaries
                    .TryGetValue(Application.Current.RequestedTheme.ToString(), out themeDictionary))
            {
                Debug.Assert(themeDictionary is ResourceDictionary);

                foreach (var brush in GetBrushResources((ResourceDictionary)themeDictionary))
                {
                    yield return brush;
                }
            }

            foreach (var resource in resources.Where(r => r.Value is Brush))
            {
                yield return resource;
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void rbKey_Checked(object sender, RoutedEventArgs e)
        {
            sortOption = SortOptions.Key;
            FilterList();
        }

        private void rbAlpha_Checked(object sender, RoutedEventArgs e)
        {
            sortOption = SortOptions.Alpha;
            FilterList();
        }
    }
}
