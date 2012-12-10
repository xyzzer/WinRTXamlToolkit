using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Imaging;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class UniformGridTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        private int _maxZIndex;

        public UniformGridTestPage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var colorsProperties = typeof (Colors).GetTypeInfo().DeclaredProperties;

            // Get defined colors
            var colors = colorsProperties.Select(pi =>
                {
                    var color = (Color)pi.GetValue(null);
                    return new NamedColor {Color = color, Name = pi.Name};
                }).OrderBy(nc => nc.Color.ToHsv().H)
                .ToList();
            //.Shuffle();
            list.Opacity = 0;

            var ratio = list.ActualWidth / list.ActualHeight;
            var rows = Math.Sqrt((double)colors.Count / ratio);
            var columns = ratio * rows;

            var uniformGrid = list.GetFirstDescendantOfType<UniformGrid>();
            uniformGrid.Columns = (int)Math.Ceiling(columns);
            uniformGrid.Rows = (int)Math.Ceiling(rows);
            list.ItemsSource = colors;

#pragma warning disable 4014
            list.FadeInCustom();
#pragma warning restore 4014
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void UIElement_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var rectangle = (Rectangle)sender;

            // Somehow ZIndex doesn't work in the UniformGrid
            // Perhaps if Jupiter had a method called Panel.GetVisualChild available to override...
            // See: http://blog.pixelingene.com/2007/12/controlling-z-index-of-children-in-custom-controls/
            Canvas.SetZIndex(rectangle, ++_maxZIndex);
            var sb = new Storyboard();

            var a1 = new DoubleAnimation();
            a1.Duration = TimeSpan.FromSeconds(0.2);
            a1.To = 0.9;
            a1.EasingFunction = new PowerEase { Power = 2, EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(a1, rectangle.RenderTransform);
            Storyboard.SetTargetProperty(a1, "ScaleX");
            sb.Children.Add(a1);

            var a2 = new DoubleAnimation();
            a2.Duration = TimeSpan.FromSeconds(0.2);
            a2.To = 0.9;
            a2.EasingFunction = new PowerEase { Power = 2, EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(a2, rectangle.RenderTransform);
            Storyboard.SetTargetProperty(a2, "ScaleY");
            sb.Children.Add(a2);

            sb.Begin();
        }

        private void UIElement_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            var rectangle = (Rectangle)sender;

            var sb = new Storyboard();

            var a1 = new DoubleAnimation();
            a1.Duration = TimeSpan.FromSeconds(0.2);
            a1.To = 1;
            a1.EasingFunction = new PowerEase { Power = 2, EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(a1, rectangle.RenderTransform);
            Storyboard.SetTargetProperty(a1, "ScaleX");
            sb.Children.Add(a1);

            var a2 = new DoubleAnimation();
            a2.Duration = TimeSpan.FromSeconds(0.2);
            a2.To = 1;
            a2.EasingFunction = new PowerEase { Power = 2, EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(a2, rectangle.RenderTransform);
            Storyboard.SetTargetProperty(a2, "ScaleY");
            sb.Children.Add(a2);

            sb.Begin();
        }
    }

    public class NamedColor
    {
        public string Name { get; set; }
        public string DisplayName
        {
            get
            {
                var re = new Regex("[A-Z][^A-Z]*");
                var sb = new StringBuilder();

                foreach (var match in re.Matches(this.Name))
                {
                    if (sb.Length > 0)
                        sb.Append(" ");
                    sb.Append(((Match)match).Value);
                }

                return sb.ToString();
            }
        }

        public Color Color { get; set; }
    }

    public static class ListExtensions
    {
        private static readonly Random _random = new Random();

        public static List<T> Shuffle<T>(this List<T> list)
        {
            var copy = list.ToList();

            var ret = new List<T>(list.Count);

            while (copy.Count > 0)
            {
                var i = _random.Next(copy.Count);
                ret.Add(copy[i]);
                copy.RemoveAt(i);
            }

            return ret;
        }
    }
}
