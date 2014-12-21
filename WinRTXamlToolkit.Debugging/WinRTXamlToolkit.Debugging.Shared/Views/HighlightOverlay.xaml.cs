using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace WinRTXamlToolkit.Debugging.Shared.Views
{
    public sealed partial class HighlightOverlay : UserControl
    {
        #region HighlightBounds
        /// <summary>
        /// HighlightBounds Dependency Property
        /// </summary>
        private static readonly DependencyProperty _HighlightBoundsProperty =
            DependencyProperty.Register(
                "HighlightBounds",
                typeof(Thickness),
                typeof(HighlightOverlay),
                new PropertyMetadata(new Thickness(), OnHighlightBoundsChanged));

        /// <summary>
        /// Identifies the HighlightBounds dependency property.
        /// </summary>
        public static DependencyProperty HighlightBoundsProperty { get { return _HighlightBoundsProperty; } }

        /// <summary>
        /// Gets or sets the margins of the highlight rectangle.
        /// </summary>
        public Thickness HighlightBounds
        {
            get { return (Thickness)this.GetValue(HighlightBoundsProperty); }
            set { this.SetValue(HighlightBoundsProperty, value); }
        }

        /// <summary>
        /// Handles changes to the HighlightBounds property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnHighlightBoundsChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (HighlightOverlay)d;
            Thickness oldHighlightBounds = (Thickness)e.OldValue;
            Thickness newHighlightBounds = target.HighlightBounds;
            target.OnHighlightBoundsChanged(oldHighlightBounds, newHighlightBounds);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the HighlightBounds property.
        /// </summary>
        /// <param name="oldHighlightBounds">The old HighlightBounds value</param>
        /// <param name="newHighlightBounds">The new HighlightBounds value</param>
        private void OnHighlightBoundsChanged(
            Thickness oldHighlightBounds, Thickness newHighlightBounds)
        {
            this.HighlightRect.Margin = newHighlightBounds;
        }
        #endregion

        #region HighlightText
        /// <summary>
        /// HighlightText Dependency Property
        /// </summary>
        private static readonly DependencyProperty _HighlightTextProperty =
            DependencyProperty.Register(
                "HighlightText",
                typeof(string),
                typeof(HighlightOverlay),
                new PropertyMetadata(null, OnHighlightTextChanged));

        /// <summary>
        /// Identifies the HighlightText dependency property.
        /// </summary>
        public static DependencyProperty HighlightTextProperty { get { return _HighlightTextProperty; } }

        /// <summary>
        /// Gets or sets the description of the highlighted element.
        /// </summary>
        public string HighlightText
        {
            get { return (string)this.GetValue(HighlightTextProperty); }
            set { this.SetValue(HighlightTextProperty, value); }
        }

        /// <summary>
        /// Handles changes to the HighlightText property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnHighlightTextChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (HighlightOverlay)d;
            string oldHighlightText = (string)e.OldValue;
            string newHighlightText = target.HighlightText;
            target.OnHighlightTextChanged(oldHighlightText, newHighlightText);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the HighlightText property.
        /// </summary>
        /// <param name="oldHighlightText">The old HighlightText value</param>
        /// <param name="newHighlightText">The new HighlightText value</param>
        private void OnHighlightTextChanged(
            string oldHighlightText, string newHighlightText)
        {
            this.HighlightTextBlock.Text = newHighlightText;
        }
        #endregion

        public HighlightOverlay()
        {
            this.InitializeComponent();
            this.Loaded += this.OnLoaded;
        }

        private Line[] _extendedLines;
        private Line _extendedLineLeftUpper;
        private Line _extendedLineLeftLower;
        private Line _extendedLineTopLeft;
        private Line _extendedLineTopRight;
        private Line _extendedLineRightUpper;
        private Line _extendedLineRightLower;
        private Line _extendedLineBottomLeft;
        private Line _extendedLineBottomRight;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.EnsureExtendedLinesInitialized();
        }

        private void EnsureExtendedLinesInitialized()
        {
            if (_extendedLines == null)
            {
                _extendedLines = new[]
                {
                    _extendedLineLeftUpper = (Line)(this.ExtendedLineTemplate.LoadContent()),
                    _extendedLineLeftLower = (Line)(this.ExtendedLineTemplate.LoadContent()),
                    _extendedLineTopLeft = (Line)(this.ExtendedLineTemplate.LoadContent()),
                    _extendedLineTopRight = (Line)(this.ExtendedLineTemplate.LoadContent()),
                    _extendedLineRightUpper = (Line)(this.ExtendedLineTemplate.LoadContent()),
                    _extendedLineRightLower = (Line)(this.ExtendedLineTemplate.LoadContent()),
                    _extendedLineBottomLeft = (Line)(this.ExtendedLineTemplate.LoadContent()),
                    _extendedLineBottomRight = (Line)(this.ExtendedLineTemplate.LoadContent()),
                };

                foreach (var extendedLine in _extendedLines)
                {
                    this.LayoutGrid.Children.Add(extendedLine);
                }
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var result = base.MeasureOverride(availableSize);
            ElementDescriptionBorder.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            return result;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this.UpdateHighlightTextPosition();
            this.UpdateExtendedLinesPositions();
            return base.ArrangeOverride(finalSize);
        }

        private void UpdateExtendedLinesPositions()
        {
            this.EnsureExtendedLinesInitialized();
            var aw = this.ActualWidth;
            var ah = this.ActualHeight;
            var l = this.HighlightBounds.Left;
            var t = this.HighlightBounds.Top;
            var r = aw - this.HighlightBounds.Right;
            var b = ah - this.HighlightBounds.Bottom;
            SetLineCoords(_extendedLineLeftUpper,   -.5, -.5, -.5, -.5, 0, t, l, t);
            SetLineCoords(_extendedLineLeftLower,   -.5,  .5, -.5,  .5, 0, b, l, b);
            SetLineCoords(_extendedLineTopLeft,     -.5, -.5, -.5, -.5, l, 0, l, t);
            SetLineCoords(_extendedLineTopRight,     .5, -.5,  .5, -.5, r, 0, r, t);
            SetLineCoords(_extendedLineRightUpper,   .5, -.5,  .5, -.5, r, t, aw, t);
            SetLineCoords(_extendedLineRightLower,   .5,  .5,  .5,  .5, r, b, aw, b);
            SetLineCoords(_extendedLineBottomLeft,  -.5,  .5, -.5,  .5, l, b, l, ah);
            SetLineCoords(_extendedLineBottomRight,  .5,  .5,  .5,  .5, r, b, r, ah);
        }

        private static void SetLineCoords(Line l, double ax1, double ay1, double ax2, double ay2, double x1, double y1, double x2, double y2)
        {
            l.X1 = Math.Round(x1) + ax1;
            l.Y1 = Math.Round(y1) + ay1;
            l.X2 = Math.Round(x2) + ax2;
            l.Y2 = Math.Round(y2) + ay2;
        }

        #region UpdateHighlightTextPosition()
        private void UpdateHighlightTextPosition()
        {
            //ElementDescriptionBorder.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            ElementDescriptionBorder.Arrange(new Rect(0, 0, this.ElementDescriptionBorder.DesiredSize.Width, this.ElementDescriptionBorder.DesiredSize.Height));
            var w = this.ElementDescriptionBorder.ActualWidth;
            var h = this.ElementDescriptionBorder.ActualHeight;
            var aw = this.ActualWidth;
            var ah = this.ActualHeight;
            var rw = aw - this.HighlightBounds.Left - this.HighlightBounds.Right;
            var rh = ah - this.HighlightBounds.Top - this.HighlightBounds.Bottom;

            if (w < rw &&
                h < rh)
            {
                this.ElementDescriptionBorder.Margin = new Thickness(
                    this.HighlightBounds.Left,
                    this.HighlightBounds.Top,
                    0,
                    0);
            }
            else if (rw > rh)
            {
                if (this.HighlightBounds.Top > h)
                {
                    this.ElementDescriptionBorder.Margin = new Thickness(
                        this.HighlightBounds.Left,
                        this.HighlightBounds.Top - h,
                        0,
                        0);
                }
                else if (this.HighlightBounds.Bottom > h)
                {
                    this.ElementDescriptionBorder.Margin = new Thickness(
                        this.HighlightBounds.Left,
                        ah - this.HighlightBounds.Bottom,
                        0,
                        0);
                }
                else if (this.HighlightBounds.Left > w)
                {
                    this.ElementDescriptionBorder.Margin = new Thickness(
                        this.HighlightBounds.Left - w,
                        this.HighlightBounds.Top,
                        0,
                        0);
                }
                else if (this.HighlightBounds.Right > w)
                {
                    this.ElementDescriptionBorder.Margin = new Thickness(
                        aw - this.HighlightBounds.Right,
                        this.HighlightBounds.Top,
                        0,
                        0);
                }
                else
                    this.ElementDescriptionBorder.Margin = new Thickness(
                        this.HighlightBounds.Left,
                        this.HighlightBounds.Top,
                        0,
                        0);
            }
            else
            {
                if (this.HighlightBounds.Left > w)
                {
                    this.ElementDescriptionBorder.Margin = new Thickness(
                        this.HighlightBounds.Left - w,
                        this.HighlightBounds.Top,
                        0,
                        0);
                }
                else if (this.HighlightBounds.Right > w)
                {
                    this.ElementDescriptionBorder.Margin = new Thickness(
                        aw - this.HighlightBounds.Right,
                        this.HighlightBounds.Top,
                        0,
                        0);
                }
                else if (this.HighlightBounds.Top > h)
                {
                    this.ElementDescriptionBorder.Margin = new Thickness(
                        this.HighlightBounds.Left,
                        this.HighlightBounds.Top - h,
                        0,
                        0);
                }
                else if (this.HighlightBounds.Bottom > h)
                {
                    this.ElementDescriptionBorder.Margin = new Thickness(
                        this.HighlightBounds.Left,
                        ah - this.HighlightBounds.Bottom,
                        0,
                        0);
                }
                else
                    this.ElementDescriptionBorder.Margin = new Thickness(
                        this.HighlightBounds.Left,
                        this.HighlightBounds.Top,
                        0,
                        0);
            }
        } 
        #endregion
    }
}
