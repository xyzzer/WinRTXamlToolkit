using System;
using Windows.Graphics.Display;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
using WinRTXamlToolkit.AwaitableUI;
using WinRTXamlToolkit.Controls.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Controls.Extensions.Forms;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// A control used for visualizing focus changes with an animated rectangle that moves from focused element to focused element.
    /// </summary>
    [TemplatePart(Name = LayoutGridName, Type = typeof(Grid))]
    public class FocusVisualizer : Control
    {
        #region Fields
        private const string LayoutGridName = "LayoutGrid";
        private Grid _layoutGrid;
        private Rectangle _leftRectangle;
        private Rectangle _topRectangle;
        private Rectangle _rightRectangle;
        private Rectangle _bottomRectangle;
        private Rectangle[] _rectangles;
        private CompositeTransform _leftTransform;
        private CompositeTransform _topTransform;
        private CompositeTransform _rightTransform;
        private CompositeTransform _bottomTransform;
        private bool _isLoaded;
        private UIElement _focusedElement; 
        #endregion

        /// <summary>
        /// The <see cref="FocusTracker"/> instance used by this FocusVisualizer control.
        /// </summary>
        public FocusTracker FocusTracker { get; private set; }

        #region CTOR
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageButton" /> class.
        /// </summary>
        public FocusVisualizer()
        {
            DefaultStyleKey = typeof(FocusVisualizer);
            new PropertyChangeEventSource<Thickness>(this, "BorderThickness").ValueChanged += this.OnBorderThicknessChanged;
            new PropertyChangeEventSource<Brush>(this, "BorderBrush").ValueChanged += this.OnBorderBrushChanged;
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
            this.FocusTracker = new FocusTracker();
            this.FocusTracker.FocusChanged += OnFocusChanged;
            _focusedElement = FocusManager.GetFocusedElement() as UIElement;
        } 
        #endregion

        #region Event handlers
        private void OnFocusChanged(object sender, UIElement uiElement)
        {
            var useAnimation = _focusedElement != null && uiElement != null && _focusedElement != uiElement;
            _focusedElement = uiElement;
            this.UpdatePosition(useAnimation);
        }

        private void OnBorderThicknessChanged(object sender, Thickness e)
        {
            this.UpdateThickness();
            this.UpdatePosition(false);
        }

        private void OnBorderBrushChanged(object sender, Brush e)
        {
            this.UpdateBrush();
        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            _isLoaded = true;
            this.UpdateThickness();
            this.UpdatePosition(false);
        }

        private void OnUnloaded(object sender, RoutedEventArgs args)
        {
            _isLoaded = false;
        } 
        #endregion

        #region CreateRectangles()
        private void CreateRectangles()
        {
            _rectangles = new[]
            {
                _leftRectangle = new Rectangle(),
                _topRectangle = new Rectangle(),
                _rightRectangle = new Rectangle(),
                _bottomRectangle = new Rectangle()
            };

            _leftRectangle.RenderTransform = _leftTransform = new CompositeTransform();
            _topRectangle.RenderTransform = _topTransform = new CompositeTransform();
            _rightRectangle.RenderTransform = _rightTransform = new CompositeTransform();
            _bottomRectangle.RenderTransform = _bottomTransform = new CompositeTransform();

            foreach (var rectangle in _rectangles)
            {
                rectangle.Width = 10;
                rectangle.Height = 10;
                rectangle.VerticalAlignment = VerticalAlignment.Top;
                rectangle.HorizontalAlignment = HorizontalAlignment.Left;
                rectangle.IsHitTestVisible = false;
                rectangle.Opacity = 0;
                _layoutGrid.Children.Add(rectangle);
            }
        } 
        #endregion

        #region UpdateBrush()
        private void UpdateBrush()
        {
            if (_leftRectangle == null)
            {
                return;
            }

            _leftRectangle.Fill = this.BorderBrush;
            _topRectangle.Fill = this.BorderBrush;
            _rightRectangle.Fill = this.BorderBrush;
            _bottomRectangle.Fill = this.BorderBrush;
        } 
        #endregion

        #region UpdateThickness()
        private void UpdateThickness()
        {
            if (_leftRectangle == null)
            {
                return;
            }

            _leftRectangle.Width = this.BorderThickness.Left;
            _topRectangle.Height = this.BorderThickness.Top;
            _rightRectangle.Width = this.BorderThickness.Right;
            _bottomRectangle.Height = this.BorderThickness.Bottom;
        } 
        #endregion

        #region OnApplyTemplate()
        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _layoutGrid = (Grid)GetTemplateChild(LayoutGridName);
            _layoutGrid.Children.Clear();
            this.CreateRectangles();
            this.UpdateBrush();
            this.UpdateThickness();
            this.UpdatePosition(false);
        }
        #endregion

        #region UpdatePosition()
        private async void UpdatePosition(bool useAnimation)
        {
            if (_leftTransform == null)
            {
                // template not applied yet
                return;
            }

            if (!_isLoaded || _focusedElement == null)
            {
                foreach (var rectangle in _rectangles)
                {
                    rectangle.Opacity = 0;
                }

                return;
            }

            await ((FrameworkElement)_focusedElement).WaitForLoadedAsync();

            if (!_isLoaded)
            {
                await this.WaitForLoadedAsync();
            }

            var boundingRect = _focusedElement.GetBoundingRect(this);
            var leftRectangleLeft = boundingRect.Left - this.BorderThickness.Left;
            var leftRectangleTop = boundingRect.Top;
            var leftRectangleHeight = boundingRect.Height;
            var topRectangleLeft = boundingRect.Left - this.BorderThickness.Left;
            var topRectangleTop = boundingRect.Top - this.BorderThickness.Top;
            var topRectangleWidth = (boundingRect.Width + this.BorderThickness.Left + this.BorderThickness.Right);
            var rightRectangleLeft = boundingRect.Right;
            var rightRectangleTop = boundingRect.Top;
            var rightRectangleHeight = boundingRect.Height;
            var bottomRectangleLeft = boundingRect.Left - this.BorderThickness.Left;
            var bottomRectangleTop = boundingRect.Bottom;
            var bottomRectangleWidth = (boundingRect.Width + this.BorderThickness.Left + this.BorderThickness.Right);

            if (!useAnimation)
            {
                _leftTransform.TranslateX = leftRectangleLeft;
                _leftTransform.TranslateY = leftRectangleTop;
                _leftTransform.ScaleY = leftRectangleHeight / 10;
                _topTransform.TranslateX = topRectangleLeft;
                _topTransform.TranslateY = topRectangleTop;
                _topTransform.ScaleX = topRectangleWidth / 10;
                _rightTransform.TranslateX = rightRectangleLeft;
                _rightTransform.TranslateY = rightRectangleTop;
                _rightTransform.ScaleY = rightRectangleHeight / 10;
                _bottomTransform.TranslateX = bottomRectangleLeft;
                _bottomTransform.TranslateY = bottomRectangleTop;
                _bottomTransform.ScaleX = bottomRectangleWidth / 10;
                _leftRectangle.Opacity = 1;
                _topRectangle.Opacity = 1;
                _rightRectangle.Opacity = 1;
                _bottomRectangle.Opacity = 1;
            }

            var duration = useAnimation ? .25 : 0;
            var sb = new Storyboard();
            AddAnimation(sb, _leftTransform, "TranslateX", leftRectangleLeft, duration);
            AddAnimation(sb, _leftTransform, "TranslateY", leftRectangleTop, duration);
            AddAnimation(sb, _leftTransform, "ScaleY", leftRectangleHeight / 10, duration);
            AddAnimation(sb, _topTransform, "TranslateX", topRectangleLeft, duration);
            AddAnimation(sb, _topTransform, "TranslateY", topRectangleTop, duration);
            AddAnimation(sb, _topTransform, "ScaleX", topRectangleWidth / 10, duration);
            AddAnimation(sb, _rightTransform, "TranslateX", rightRectangleLeft, duration);
            AddAnimation(sb, _rightTransform, "TranslateY", rightRectangleTop, duration);
            AddAnimation(sb, _rightTransform, "ScaleY", rightRectangleHeight / 10, duration);
            AddAnimation(sb, _bottomTransform, "TranslateX", bottomRectangleLeft, duration);
            AddAnimation(sb, _bottomTransform, "TranslateY", bottomRectangleTop, duration);
            AddAnimation(sb, _bottomTransform, "ScaleX", bottomRectangleWidth / 10, duration);
            AddAnimation(sb, _leftRectangle, "Opacity", 1, duration);
            AddAnimation(sb, _topRectangle, "Opacity", 1, duration);
            AddAnimation(sb, _rightRectangle, "Opacity", 1, duration);
            AddAnimation(sb, _bottomRectangle, "Opacity", 1, duration);
            sb.Begin();
        } 
        #endregion

        #region GetResolutionScale()
        private static double GetResolutionScale()
        {
#if WINDOWS_APP
            var scale = 1.0;

            switch (DisplayInformation.GetForCurrentView().ResolutionScale)
            {
                case ResolutionScale.Scale100Percent:
                    scale = 1.0;
                    break;
                case ResolutionScale.Scale120Percent:
                    scale = 1.2;
                    break;
                case ResolutionScale.Scale140Percent:
                    scale = 1.4;
                    break;
                case ResolutionScale.Scale150Percent:
                    scale = 1.5;
                    break;
                case ResolutionScale.Scale160Percent:
                    scale = 1.6;
                    break;
                case ResolutionScale.Scale180Percent:
                    scale = 1.8;
                    break;
                case ResolutionScale.Scale225Percent:
                    scale = 2.25;
                    break;
            }
#else
            var scale = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
#endif
            return scale;
        } 
        #endregion

        #region AddAnimation()
        private void AddAnimation(Storyboard sb, DependencyObject target, string propertyName, double toValue, double duration)
        {
            var da = new DoubleAnimation();
            Storyboard.SetTarget(da, target);
            Storyboard.SetTargetProperty(da, propertyName);
            da.To = toValue;
            da.Duration = TimeSpan.FromSeconds(duration);
            da.EasingFunction = new ExponentialEase { Exponent = 10, EasingMode = EasingMode.EaseOut };
            sb.Children.Add(da);
        } 
        #endregion
    }
}
