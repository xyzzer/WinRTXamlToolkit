
using System;
using WinRTXamlToolkit.Async;
using WinRTXamlToolkit.AwaitableUI;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Imaging;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// The Value is the 0..360deg range hue.
    /// </summary>
    [TemplatePart(Name = ContainerGridName, Type = typeof(Grid))]
    [TemplatePart(Name = HueRingName, Type = typeof(Ellipse))]
    [TemplatePart(Name = RingThumbName, Type = typeof(RingSlice))]
    public class HueRingPicker : RangeBase
    {
        private const string ContainerGridName = "PART_ContainerGrid";
        private const string HueRingName = "PART_HueRing";
        private const string RingThumbName = "PART_RingThumb";

        private Grid _containerGrid;
        private Ellipse _hueRing;
        private RingSlice _ringThumb;

        private bool _isLoaded;
        private AsyncAutoResetEvent _bitmapUpdateRequired = new AsyncAutoResetEvent();

        #region RingThickness
        /// <summary>
        /// RingThickness Dependency Property
        /// </summary>
        public static readonly DependencyProperty RingThicknessProperty =
            DependencyProperty.Register(
                "RingThickness",
                typeof(double),
                typeof(HueRingPicker),
                new PropertyMetadata(40.0, OnRingThicknessChanged));

        /// <summary>
        /// Gets or sets the RingThickness property. This dependency property 
        /// indicates the thickness of the hue ring.
        /// </summary>
        public double RingThickness
        {
            get { return (double)GetValue(RingThicknessProperty); }
            set { SetValue(RingThicknessProperty, value); }
        }

        /// <summary>
        /// Handles changes to the RingThickness property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnRingThicknessChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (HueRingPicker)d;
            double oldRingThickness = (double)e.OldValue;
            double newRingThickness = target.RingThickness;
            target.OnRingThicknessChanged(oldRingThickness, newRingThickness);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the RingThickness property.
        /// </summary>
        /// <param name="oldRingThickness">The old RingThickness value</param>
        /// <param name="newRingThickness">The new RingThickness value</param>
        private void OnRingThicknessChanged(
            double oldRingThickness, double newRingThickness)
        {
            UpdateVisuals();
        }
        #endregion

        #region ThumbArcAngle
        /// <summary>
        /// ThumbArcAngle Dependency Property
        /// </summary>
        public static readonly DependencyProperty ThumbArcAngleProperty =
            DependencyProperty.Register(
                "ThumbArcAngle",
                typeof(double),
                typeof(HueRingPicker),
                new PropertyMetadata(10.0, OnThumbArcAngleChanged));

        /// <summary>
        /// Gets or sets the ThumbArcAngle property. This dependency property 
        /// indicates the angle between the ring thumb ring slice start and end angles.
        /// </summary>
        /// <remarks>
        /// Depending on the size if the control - smaller or larger angles might work better.
        /// </remarks>
        public double ThumbArcAngle
        {
            get { return (double)GetValue(ThumbArcAngleProperty); }
            set { SetValue(ThumbArcAngleProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ThumbArcAngle property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnThumbArcAngleChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (HueRingPicker)d;
            double oldThumbArcAngle = (double)e.OldValue;
            double newThumbArcAngle = target.ThumbArcAngle;
            target.OnThumbArcAngleChanged(oldThumbArcAngle, newThumbArcAngle);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the ThumbArcAngle property.
        /// </summary>
        /// <param name="oldThumbArcAngle">The old ThumbArcAngle value</param>
        /// <param name="newThumbArcAngle">The new ThumbArcAngle value</param>
        private void OnThumbArcAngleChanged(
            double oldThumbArcAngle, double newThumbArcAngle)
        {
            if (newThumbArcAngle < 0 || newThumbArcAngle > 180)
            {
                throw new ArgumentException("ThumbArcAngle only supports values in the 0..180 range.");
            }

            UpdateRingThumb();
        }
        #endregion

        #region ThumbBorderBrush
        /// <summary>
        /// ThumbBorderBrush Dependency Property
        /// </summary>
        public static readonly DependencyProperty ThumbBorderBrushProperty =
            DependencyProperty.Register(
                "ThumbBorderBrush",
                typeof(Brush),
                typeof(HueRingPicker),
                new PropertyMetadata(null, OnThumbBorderBrushChanged));

        /// <summary>
        /// Gets or sets the ThumbBorderBrush property. This dependency property 
        /// indicates the brush used to render the ring thumb's border.
        /// </summary>
        public Brush ThumbBorderBrush
        {
            get { return (Brush)GetValue(ThumbBorderBrushProperty); }
            set { SetValue(ThumbBorderBrushProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ThumbBorderBrush property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnThumbBorderBrushChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (HueRingPicker)d;
            Brush oldThumbBorderBrush = (Brush)e.OldValue;
            Brush newThumbBorderBrush = target.ThumbBorderBrush;
            target.OnThumbBorderBrushChanged(oldThumbBorderBrush, newThumbBorderBrush);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the ThumbBorderBrush property.
        /// </summary>
        /// <param name="oldThumbBorderBrush">The old ThumbBorderBrush value</param>
        /// <param name="newThumbBorderBrush">The new ThumbBorderBrush value</param>
        private void OnThumbBorderBrushChanged(
            Brush oldThumbBorderBrush, Brush newThumbBorderBrush)
        {
        }
        #endregion

        #region ThumbBorderThickness
        /// <summary>
        /// ThumbBorderThickness Dependency Property
        /// </summary>
        public static readonly DependencyProperty ThumbBorderThicknessProperty =
            DependencyProperty.Register(
                "ThumbBorderThickness",
                typeof(double),
                typeof(HueRingPicker),
                new PropertyMetadata(2.0, OnThumbBorderThicknessChanged));

        /// <summary>
        /// Gets or sets the ThumbBorderThickness property. This dependency property 
        /// indicates the thickness of the thumb border.
        /// </summary>
        public double ThumbBorderThickness
        {
            get { return (double)GetValue(ThumbBorderThicknessProperty); }
            set { SetValue(ThumbBorderThicknessProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ThumbBorderThickness property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnThumbBorderThicknessChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (HueRingPicker)d;
            double oldThumbBorderThickness = (double)e.OldValue;
            double newThumbBorderThickness = target.ThumbBorderThickness;
            target.OnThumbBorderThicknessChanged(oldThumbBorderThickness, newThumbBorderThickness);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the ThumbBorderThickness property.
        /// </summary>
        /// <param name="oldThumbBorderThickness">The old ThumbBorderThickness value</param>
        /// <param name="newThumbBorderThickness">The new ThumbBorderThickness value</param>
        private void OnThumbBorderThicknessChanged(
            double oldThumbBorderThickness, double newThumbBorderThickness)
        {
            UpdateRingThumb();
        }
        #endregion

        #region ThumbBackground
        /// <summary>
        /// ThumbBackground Dependency Property
        /// </summary>
        public static readonly DependencyProperty ThumbBackgroundProperty =
            DependencyProperty.Register(
                "ThumbBackground",
                typeof(Brush),
                typeof(HueRingPicker),
                new PropertyMetadata(null, OnThumbBackgroundChanged));

        /// <summary>
        /// Gets or sets the ThumbBackground property. This dependency property 
        /// indicates the brush used to fill the ring thumb.
        /// </summary>
        public Brush ThumbBackground
        {
            get { return (Brush)GetValue(ThumbBackgroundProperty); }
            set { SetValue(ThumbBackgroundProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ThumbBackground property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnThumbBackgroundChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (HueRingPicker)d;
            Brush oldThumbBackground = (Brush)e.OldValue;
            Brush newThumbBackground = target.ThumbBackground;
            target.OnThumbBackgroundChanged(oldThumbBackground, newThumbBackground);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the ThumbBackground property.
        /// </summary>
        /// <param name="oldThumbBackground">The old ThumbBackground value</param>
        /// <param name="newThumbBackground">The new ThumbBackground value</param>
        private void OnThumbBackgroundChanged(
            Brush oldThumbBackground, Brush newThumbBackground)
        {
        }
        #endregion

        #region HueRingPicker()
        /// <summary>
        /// Initializes a new instance of the <see cref="HueRingPicker"/> class.
        /// </summary>
        public HueRingPicker()
        {
            this.DefaultStyleKey = typeof (HueRingPicker);
            this.InitializeMinMaxCoercion();
            this.DelayedUpdateWorkaround();

            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }
        #endregion

        #region OnLoaded()
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;
            _bitmapUpdateRequired.Set();
            RunBitmapUpdaterAsync();
        }
        #endregion

        #region OnUnloaded()
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = false;
            _bitmapUpdateRequired.Set();
        }
        #endregion

        #region RunBitmapUpdaterAsync()
        private async void RunBitmapUpdaterAsync()
        {
            do
            {
                await _bitmapUpdateRequired.WaitAsync();

                if (_isLoaded)
                {
                    if (_hueRing == null)
                    {
                        continue;
                    }

                    if (_containerGrid.ActualHeight <= 2*(this.RingThickness + this.ThumbBorderThickness) ||
                        _containerGrid.ActualWidth <= 2*(this.RingThickness + this.ThumbBorderThickness))
                    {
                        continue;
                    }

                    _hueRing.StrokeThickness = this.RingThickness;
                    var hueRingSize = (int)Math.Min(
                        _containerGrid.ActualWidth - 2*this.ThumbBorderThickness,
                        _containerGrid.ActualHeight - 2*this.ThumbBorderThickness);
                    _hueRing.Width = hueRingSize;
                    _hueRing.Height = hueRingSize;

                    // Rendering a hue ring one pixel thicker than the Ellipse that uses it as a stroke brush.
                    var wb = new WriteableBitmap(hueRingSize + 1, hueRingSize + 1);
                    await wb.RenderColorPickerHueRingAsync(
                        innerRingRadius: hueRingSize/2 - (int)this.RingThickness - 1);

                    _hueRing.Stroke =
                        new ImageBrush
                        {
                            ImageSource = wb,
                            AlignmentX = AlignmentX.Center,
                            AlignmentY = AlignmentY.Center,
                            Stretch = Stretch.None
                        };
                }
            } while (_isLoaded);
        }
        #endregion

        #region DelayedUpdateWorkaround()
        private async void DelayedUpdateWorkaround()
        {
            // Not sure why the thumb starts at the wrong position without this code.
            // Perhaps something with path positioning overall, but I don't want to dig into this now.
            await Task.Delay(10);
            UpdateRingThumb();
        }
        #endregion

        #region ArrangeOverride()
        protected override Size ArrangeOverride(Size finalSize)
        {
            this.UpdateVisuals();
            return base.ArrangeOverride(finalSize);
        }
        #endregion

        #region OnValueChanged()
        /// <summary>
        /// Fires the ValueChanged routed event.
        /// </summary>
        /// <param name="oldValue">Old value of the Value property.</param>
        /// <param name="newValue">New value of the Value property.</param>
        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            UpdateRingThumb();
        }
        #endregion

        #region OnApplyTemplate()
        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override async void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _containerGrid = (Grid)GetTemplateChild(ContainerGridName);
            _hueRing = (Ellipse)GetTemplateChild(HueRingName);
            _ringThumb = (RingSlice)GetTemplateChild(RingThumbName);
            _hueRing.PointerPressed += this.OnHueRingPointerPressed;
            _hueRing.PointerMoved += this.OnHueRingPointerMoved;
            await this.WaitForLoadedAsync();
            this.UpdateVisuals();
        }
        #endregion

        #region OnHueRingPointerMoved()
        private void OnHueRingPointerMoved(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            if (!pointerRoutedEventArgs.Pointer.IsInContact)
            {
                return;
            }

            var point = pointerRoutedEventArgs.GetCurrentPoint(_hueRing).Position;
            UpdateValueForPoint(point);
        }
        #endregion

        private void OnHueRingPointerPressed(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            _hueRing.CapturePointer(pointerRoutedEventArgs.Pointer);
            var point = pointerRoutedEventArgs.GetCurrentPoint(_hueRing).Position;
            UpdateValueForPoint(point);
        }

        private void UpdateValueForPoint(Point point)
        {
            var pc = new Point(
                _hueRing.ActualWidth / 2, _hueRing.ActualHeight / 2);
            var atan2 = Math.Atan2(point.Y - pc.Y, point.X - pc.X) * 180 / Math.PI;
            this.Value = (atan2 + 360 + 90) % 360;
        }

        private void UpdateVisuals()
        {
            this.UpdateHueRingImage();
            this.UpdateRingThumb();
        }

        #region UpdateRingThumb()
        private void UpdateRingThumb()
        {
            if (_ringThumb == null)
            {
                return;
            }

            if (_containerGrid.ActualHeight <= 2*(RingThickness + ThumbBorderThickness) ||
                _containerGrid.ActualWidth <= 2*(RingThickness + ThumbBorderThickness))
            {
                return;
            }

            // Hue ring needs to be smaller than available container space so the thumb fits
            var hueRingSize = Math.Min(
                _containerGrid.ActualWidth - 2*ThumbBorderThickness,
                _containerGrid.ActualHeight - 2*ThumbBorderThickness);

            _ringThumb.Width = hueRingSize + 2*ThumbBorderThickness;
            _ringThumb.Height = hueRingSize + 2*ThumbBorderThickness;

            _ringThumb.BeginUpdate();

            // Half of the thumb border stroke goes outside the radius and the other half goes inside,
            // so radius needs to be 0.5 border thickness larger than the hue ring's outer radius.
            // Less 1 to make sure there is an overlap.
            _ringThumb.Center =
                new Point(
                    hueRingSize/2 + ThumbBorderThickness,
                    hueRingSize/2 + ThumbBorderThickness);
            _ringThumb.Radius = (hueRingSize + ThumbBorderThickness)/2 - 1;
            _ringThumb.InnerRadius = _ringThumb.Radius - RingThickness - ThumbBorderThickness + 2;
            _ringThumb.StartAngle = this.Value - ThumbArcAngle/2;
            _ringThumb.EndAngle = this.Value + ThumbArcAngle/2;
            _ringThumb.EndUpdate();
            _ringThumb.Stroke =
                new SolidColorBrush(
                    ColorExtensions.FromHsv(this.Value, 0.5, 1.0));
        }
        #endregion

        #region UpdateHueRingImage()
        private void UpdateHueRingImage()
        {
            _bitmapUpdateRequired.Set();
        }
        #endregion

        #region Min/Max coercion methods
        #region InitializeMinMaxCoercion()
        /// <summary>
        /// Initializes event handling to coerce Minimum and Maximum properties to the 0..360 range.
        /// </summary>
        private void InitializeMinMaxCoercion()
        {
            this.Minimum = 0.0;
            this.Maximum = 360.0;
        }
        #endregion

        #region OnMinimumChanged()
        /// <summary>
        /// Called when the Minimum property changes.
        /// </summary>
        /// <param name="oldMinimum">Old value of the Minimum property.</param>
        /// <param name="newMinimum">New value of the Minimum property.</param>
        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            base.OnMinimumChanged(oldMinimum, newMinimum);

            if (newMinimum < 0)
            {
                this.Minimum = 0;
            }
        }
        #endregion

        #region OnMaximumChanged()
        /// <summary>
        /// Called when the Maximum property changes.
        /// </summary>
        /// <param name="oldMaximum">Old value of the Maximum property.</param>
        /// <param name="newMaximum">New value of the Maximum property.</param>
        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            base.OnMaximumChanged(oldMaximum, newMaximum);

            if (newMaximum > 360)
            {
                this.Maximum = 360;
            }
        }
        #endregion
        #endregion
    }
}
