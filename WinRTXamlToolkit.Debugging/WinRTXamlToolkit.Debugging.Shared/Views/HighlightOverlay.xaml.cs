using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            //this.UpdateHighlightTextPosition();
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
            //this.UpdateHighlightTextPosition();
        }
        #endregion

        public HighlightOverlay()
        {
            this.InitializeComponent();
            this.Loaded += this.OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var result = base.MeasureOverride(availableSize);
            ElementDescriptionBorder.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            return result;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            UpdateHighlightTextPosition();
            return base.ArrangeOverride(finalSize);
        }

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
    }
}
