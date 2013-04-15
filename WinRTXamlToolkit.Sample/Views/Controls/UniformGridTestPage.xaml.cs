using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using WinRTXamlToolkit.AwaitableUI;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Imaging;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class UniformGridTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        //private int _maxZIndex;

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
                }).OrderBy(nc =>  nc.Color.ToHsv().H)//nc.Color.R * 4 + nc.Color.G * 2 + nc.Color.B)// nc.Color.ToHsv().S * nc.Color.ToHsv().V)
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
#pragma warning disable 4014
            this.Frame.GoBack();
#pragma warning restore 4014
        }

        private void UIElement_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var border = (Border)sender;

            // Somehow ZIndex doesn't work in the UniformGrid
            // Perhaps if Jupiter had a method called Panel.GetVisualChild available to override...
            // See: http://blog.pixelingene.com/2007/12/controlling-z-index-of-children-in-custom-controls/
            //Canvas.SetZIndex(border, ++_maxZIndex);
            var sb = new Storyboard();

            var a1 = new DoubleAnimation();
            a1.Duration = TimeSpan.FromSeconds(0.2);
            a1.To = 0.9;
            a1.EasingFunction = new PowerEase { Power = 2, EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(a1, border.RenderTransform);
            Storyboard.SetTargetProperty(a1, "ScaleX");
            sb.Children.Add(a1);

            var a2 = new DoubleAnimation();
            a2.Duration = TimeSpan.FromSeconds(0.2);
            a2.To = 0.9;
            a2.EasingFunction = new PowerEase { Power = 2, EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(a2, border.RenderTransform);
            Storyboard.SetTargetProperty(a2, "ScaleY");
            sb.Children.Add(a2);

            sb.Begin();
        }

        private void UIElement_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            var border = (Border)sender;

            var sb = new Storyboard();

            var a1 = new DoubleAnimation();
            a1.Duration = TimeSpan.FromSeconds(0.2);
            a1.To = 1;
            a1.EasingFunction = new PowerEase { Power = 2, EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(a1, border.RenderTransform);
            Storyboard.SetTargetProperty(a1, "ScaleX");
            sb.Children.Add(a1);

            var a2 = new DoubleAnimation();
            a2.Duration = TimeSpan.FromSeconds(0.2);
            a2.To = 1;
            a2.EasingFunction = new PowerEase { Power = 2, EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(a2, border.RenderTransform);
            Storyboard.SetTargetProperty(a2, "ScaleY");
            sb.Children.Add(a2);

            sb.Begin();
        }

        private bool _isArrangedInGrid = true;
        private int _onBorderTappedCall;
        private bool _inAnimation;

        private async void OnBorderTapped(object sender, TappedRoutedEventArgs e)
        {
            if (_inAnimation)
            {
                return;
            }

            _inAnimation = true;
            _onBorderTappedCall++;
            var currentCall = _onBorderTappedCall;
            //var border = (Border)sender;
            //var grid = border.GetFirstAncestorOfType<UniformGrid>();

            //var sb = new Storyboard();

            //var beginTime = TimeSpan.Zero;

            //if (_isArrangedInGrid)
            //{
            //    var center = new Point(
            //        (grid.ActualWidth - border.ActualWidth) / 2,
            //        (grid.ActualHeight - border.ActualHeight) / 2);

            //    foreach (var child in grid.Children)
            //    {
            //        child.RenderTransform = new CompositeTransform();
            //        var nc = (NamedColor)((FrameworkElement)child).DataContext;
            //        var hsv = nc.Color.ToHsv();
            //        var targetX = center.X + center.X * Math.Sin(hsv.H * Math.PI / 180) * (0.25 + 0.75 * hsv.V);
            //        var targetY = center.Y - center.Y * Math.Cos(hsv.H * Math.PI / 180) * (0.25 + 0.75 * hsv.V);
            //        var actualPosition = child.TransformToVisual(grid).TransformPoint(new Point(0, 0));
            //        //Debug.WriteLine(actualPosition);
            //        var deltaX = targetX - actualPosition.X;
            //        var deltaY = targetY - actualPosition.Y;

            //        var xa = new DoubleAnimation();
            //        xa.BeginTime = beginTime;
            //        xa.Duration = TimeSpan.FromSeconds(1);
            //        xa.To = deltaX;
            //        Storyboard.SetTarget(xa, child.RenderTransform);
            //        Storyboard.SetTargetProperty(xa, "TranslateX");
            //        sb.Children.Add(xa);

            //        var ya = new DoubleAnimation();
            //        ya.BeginTime = beginTime;
            //        ya.Duration = TimeSpan.FromSeconds(1);
            //        ya.To = deltaY;
            //        Storyboard.SetTarget(ya, child.RenderTransform);
            //        Storyboard.SetTargetProperty(ya, "TranslateY");
            //        sb.Children.Add(ya);

            //        var aa = new DoubleAnimation();
            //        aa.BeginTime = beginTime;
            //        aa.Duration = TimeSpan.FromSeconds(1);
            //        aa.To = hsv.H;
            //        Storyboard.SetTarget(aa, child.RenderTransform);
            //        Storyboard.SetTargetProperty(aa, "Rotation");
            //        sb.Children.Add(aa);

            //        beginTime += TimeSpan.FromMilliseconds(5);
            //    }
            //}
            //else
            //{
            //    foreach (var child in grid.Children)
            //    {
            //        var nc = (NamedColor)((FrameworkElement)child).DataContext;

            //        var xa = new DoubleAnimation();
            //        xa.BeginTime = beginTime;
            //        xa.Duration = TimeSpan.FromSeconds(1);
            //        xa.To = 0;
            //        Storyboard.SetTarget(xa, child.RenderTransform);
            //        Storyboard.SetTargetProperty(xa, "TranslateX");
            //        sb.Children.Add(xa);

            //        var ya = new DoubleAnimation();
            //        ya.BeginTime = beginTime;
            //        ya.Duration = TimeSpan.FromSeconds(1);
            //        ya.To = 0;
            //        Storyboard.SetTarget(ya, child.RenderTransform);
            //        Storyboard.SetTargetProperty(ya, "TranslateY");
            //        sb.Children.Add(ya);

            //        var aa = new DoubleAnimation();
            //        aa.BeginTime = beginTime;
            //        aa.Duration = TimeSpan.FromSeconds(1);
            //        aa.To = 0;
            //        Storyboard.SetTarget(aa, child.RenderTransform);
            //        Storyboard.SetTargetProperty(aa, "Rotation");
            //        sb.Children.Add(aa);

            //        beginTime += TimeSpan.FromMilliseconds(5);
            //    }
            //}

            //sb.Begin();
            //_isArrangedInGrid = !_isArrangedInGrid;

            var border = (Border)sender;
            var grid = border.GetFirstAncestorOfType<UniformGrid>();

            if (_isArrangedInGrid)
            {
                var center = new Point(
                    (grid.ActualWidth - border.ActualWidth) / 2,
                    (grid.ActualHeight - border.ActualHeight) / 2);

                {
                    var sb1 = new Storyboard();

                    var beginTime = TimeSpan.Zero;

                    foreach (var child in grid.Children)
                    {
                        child.RenderTransform = new CompositeTransform();
                        child.RenderTransformOrigin = new Point(0.5, 0.5); 
                        var nc = (NamedColor)((FrameworkElement)child).DataContext;
                        var hsv = nc.Color.ToHsl();
                        var targetX = center.X +
                                        center.X * Math.Sin(hsv.H * Math.PI / 180) *
                                        (0.25 + 0.75 * Math.Min(hsv.L, hsv.S));
                        var targetY = center.Y -
                                        center.Y * Math.Cos(hsv.H * Math.PI / 180) *
                                        (0.25 + 0.75 * Math.Min(hsv.L, hsv.S));
                        var actualPosition =
                            child.TransformToVisual(grid)
                                    .TransformPoint(new Point(0, 0));
                        //Debug.WriteLine(actualPosition);
                        var deltaX = targetX - actualPosition.X;
                        var deltaY = targetY - actualPosition.Y;

                        var xa = new DoubleAnimation();
                        xa.BeginTime = beginTime;
                        xa.Duration = TimeSpan.FromSeconds(1);
                        xa.To = deltaX;
                        Storyboard.SetTarget(xa, child.RenderTransform);
                        Storyboard.SetTargetProperty(xa, "TranslateX");
                        sb1.Children.Add(xa);

                        var ya = new DoubleAnimation();
                        ya.BeginTime = beginTime;
                        ya.Duration = TimeSpan.FromSeconds(1);
                        ya.To = deltaY;
                        Storyboard.SetTarget(ya, child.RenderTransform);
                        Storyboard.SetTargetProperty(ya, "TranslateY");
                        sb1.Children.Add(ya);

                        var aa = new DoubleAnimation();
                        aa.BeginTime = beginTime;
                        aa.Duration = TimeSpan.FromSeconds(1);
                        aa.To = hsv.H;
                        Storyboard.SetTarget(aa, child.RenderTransform);
                        Storyboard.SetTargetProperty(aa, "Rotation");
                        sb1.Children.Add(aa);
                    }

                    await sb1.BeginAsync();
                }

                if (currentCall != _onBorderTappedCall)
                {
                    return;
                }

                const double revolutionDurationInS = 30d;

                foreach (var child in grid.Children)
                {
                    child.RenderTransform = new CompositeTransform();
                    child.RenderTransformOrigin = new Point(0.5, 0.5); 
                    var nc = (NamedColor)((FrameworkElement)child).DataContext;
                    var hsv = nc.Color.ToHsl();
                    var actualPosition = child.TransformToVisual(grid).TransformPoint(new Point(0, 0));

                    var sb = new Storyboard();
                    sb.RepeatBehavior = RepeatBehavior.Forever;

                    var minX = center.X - center.X * (0.25 + 0.75 * Math.Min(hsv.L, hsv.S)) - actualPosition.X;
                    var midX = center.X - actualPosition.X;
                    var maxX = center.X + center.X * (0.25 + 0.75 * Math.Min(hsv.L, hsv.S)) - actualPosition.X;
                    var minY = center.Y - center.Y * (0.25 + 0.75 * Math.Min(hsv.L, hsv.S)) - actualPosition.Y;
                    var midY = center.Y - actualPosition.Y;
                    var maxY = center.Y + center.Y * (0.25 + 0.75 * Math.Min(hsv.L, hsv.S)) - actualPosition.Y;

                    var xa = new DoubleAnimationUsingKeyFrames();
                    xa.KeyFrames.Add(new DiscreteDoubleKeyFrame { KeyTime = TimeSpan.Zero, Value = midX});
                    xa.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(revolutionDurationInS * 0.25), Value = maxX, EasingFunction = new SineEase { EasingMode = EasingMode.EaseOut } });
                    xa.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(revolutionDurationInS * 0.50), Value = midX, EasingFunction = new SineEase { EasingMode = EasingMode.EaseIn } });
                    xa.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(revolutionDurationInS * 0.75), Value = minX, EasingFunction = new SineEase { EasingMode = EasingMode.EaseOut } });
                    xa.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(revolutionDurationInS * 1.00), Value = midX, EasingFunction = new SineEase { EasingMode = EasingMode.EaseIn } });
                    Storyboard.SetTarget(xa, child.RenderTransform);
                    Storyboard.SetTargetProperty(xa, "TranslateX");
                    sb.Children.Add(xa);

                    var ya = new DoubleAnimationUsingKeyFrames();
                    ya.KeyFrames.Add(new DiscreteDoubleKeyFrame { KeyTime = TimeSpan.Zero, Value = minY });
                    ya.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(revolutionDurationInS * 0.25), Value = midY, EasingFunction = new SineEase { EasingMode = EasingMode.EaseIn } });
                    ya.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(revolutionDurationInS * 0.50), Value = maxY, EasingFunction = new SineEase { EasingMode = EasingMode.EaseOut } });
                    ya.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(revolutionDurationInS * 0.75), Value = midY, EasingFunction = new SineEase { EasingMode = EasingMode.EaseIn } });
                    ya.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = TimeSpan.FromSeconds(revolutionDurationInS * 1.00), Value = minY, EasingFunction = new SineEase { EasingMode = EasingMode.EaseOut } });
                    Storyboard.SetTarget(ya, child.RenderTransform);
                    Storyboard.SetTargetProperty(ya, "TranslateY");
                    sb.Children.Add(ya);

                    var aa = new DoubleAnimation();
                    aa.Duration = TimeSpan.FromSeconds(revolutionDurationInS);
                    aa.From = 0;
                    aa.To = 360;
                    Storyboard.SetTarget(aa, child.RenderTransform);
                    Storyboard.SetTargetProperty(aa, "Rotation");
                    sb.Children.Add(aa);
                    //sb.BeginTime =
                    //    TimeSpan.FromSeconds(hsv.H * revolutionDurationInS / 360d);
                    sb.Begin();
                    sb.Seek(TimeSpan.FromSeconds(((hsv.H + 360) % 360) * revolutionDurationInS / 360d));
                }
            }
            else
            {
                var sb = new Storyboard();
                var beginTime = TimeSpan.Zero;

                foreach (var child in grid.Children)
                {
                    //var nc = (NamedColor)((FrameworkElement)child).DataContext;

                    var xa = new DoubleAnimation();
                    xa.BeginTime = beginTime;
                    xa.Duration = TimeSpan.FromSeconds(1);
                    xa.To = 0;
                    Storyboard.SetTarget(xa, child.RenderTransform);
                    Storyboard.SetTargetProperty(xa, "TranslateX");
                    sb.Children.Add(xa);

                    var ya = new DoubleAnimation();
                    ya.BeginTime = beginTime;
                    ya.Duration = TimeSpan.FromSeconds(1);
                    ya.To = 0;
                    Storyboard.SetTarget(ya, child.RenderTransform);
                    Storyboard.SetTargetProperty(ya, "TranslateY");
                    sb.Children.Add(ya);

                    var aa = new DoubleAnimation();
                    aa.BeginTime = beginTime;
                    aa.Duration = TimeSpan.FromSeconds(1);
                    aa.To = 0;
                    Storyboard.SetTarget(aa, child.RenderTransform);
                    Storyboard.SetTargetProperty(aa, "Rotation");
                    sb.Children.Add(aa);

                    //beginTime += TimeSpan.FromMilliseconds(5);
                }

                await sb.BeginAsync();
            }

            _isArrangedInGrid = !_isArrangedInGrid;
            _inAnimation = false;
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

                sb.AppendFormat("\n{0}", Color.ToString());

                return sb.ToString();
            }
        }

        public string MultilineName
        {
            get
            {
                var re = new Regex("[A-Z][^A-Z]*");
                var sb = new StringBuilder();

                //if (Color == Colors.Ivory)
                //{
                //    Debug.WriteLine(Color.ToString());
                //    Debug.WriteLine(AccentColor.ToString());
                //    Debug.WriteLine(Colors.Ivory.ToHsl().L);
                //}

                foreach (var match in re.Matches(this.Name))
                {
                    if (sb.Length > 0)
                        sb.Append("\n");
                    sb.Append(((Match)match).Value);
                }

                return sb.ToString();
            }
        }

        public Color Color { get; set; }

        public Color AccentColor
        {
            get
            {
                if ((int)Color.R + Color.G + Color.B > 256 * 1.5)
                {
                    return Colors.Black;
                }

                return Colors.White;

                //var hsl = Color.ToHsl();

                //return ColorExtensions.FromHsv(
                //    (hsv.H + 180) % 360,
                //    hsv.S,
                //    hsv.V,
                //    hsv.A);

                //if (hsl.L < 0.5)
                //{
                //    return ColorExtensions.FromHsl(
                //        hsl.H,
                //        hsl.S + 0.25,
                //        hsl.L,
                //        hsl.A);
                //}

                //return ColorExtensions.FromHsl(
                //    hsl.H,
                //    hsl.S - 0.25,
                //    hsl.L,
                //    hsl.A);
            }
        }
    }
}
