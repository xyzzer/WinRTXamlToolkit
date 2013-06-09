using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.StylesBrowser
{
    public sealed partial class BrushesPage : Page
    {
        public class BrushResourceViewModel
        {
            public Brush Brush { get; private set; }
            public string Key { get; private set; }
            public string Source { get; private set; }

            public BrushResourceViewModel(Brush brush, string key, string source)
            {
                Brush = brush;
                Key = key;
                Source = source;
            }
        }

        public BrushesPage()
        {
            this.InitializeComponent();
            this.Loaded += this.OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var brushes = this.GetBrushResources().OrderBy(r => r.Key);
            this.BrushesListView.ItemsSource =
                brushes.Select(
                    r => new BrushResourceViewModel(
                             (Brush)r.Value,
                             r.Key.ToString(),
                             r.Value is SolidColorBrush
                                    ? ((SolidColorBrush)r.Value).Color.ToString() : r.Value is BitmapImage
                                    ? (((BitmapImage)r.Value).UriSource ?? new Uri(string.Empty)).ToString() : string.Empty
                         )).ToList();
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
    }
}
