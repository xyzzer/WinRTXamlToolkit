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
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// The Value is the 0..360deg range hue.
    /// </summary>
    [TemplatePart(Name = ContainerGridName, Type = typeof(Grid))]
    [TemplatePart(Name = HueRingImageName, Type = typeof(Image))]
    [TemplatePart(Name = RingThumbName, Type = typeof(RingSlice))]
    public class HueRingPicker : RangeBase
    {
        private const string ContainerGridName = "PART_ContainerGrid";
        private const string HueRingImageName = "PART_HueRingImage";
        private const string RingThumbName = "PART_RingThumb";

        private Grid _containerGrid;
        private Image _hueRingImage;
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

        public HueRingPicker()
        {
            this.DefaultStyleKey = typeof (HueRingPicker);
            InitializeMinMaxCoercion();
            this.SizeChanged += OnSizeChanged;
            DelayedUpdateWorkaround();

            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;
            _bitmapUpdateRequired.Set();
            RunBitmapUpdaterAsync();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = false;
            _bitmapUpdateRequired.Set();
        }

        private async void RunBitmapUpdaterAsync()
        {
            do
            {
                await _bitmapUpdateRequired.WaitAsync();

                if (_isLoaded)
                {
                    if (_hueRingImage == null)
                    {
                        continue;
                    }

                    if (_containerGrid.ActualHeight <= 2 * (RingThickness + ThumbBorderThickness) ||
                        _containerGrid.ActualWidth <= 2 * (RingThickness + ThumbBorderThickness))
                    {
                        continue;
                    }

                    var hueRingSize = (int)Math.Min(
                        _containerGrid.ActualWidth - 2 * ThumbBorderThickness,
                        _containerGrid.ActualHeight - 2 * ThumbBorderThickness);

                    var wb = new WriteableBitmap(hueRingSize, hueRingSize);
                    await wb.RenderColorPickerHueRingAsync(hueRingSize / 2 - (int)RingThickness);
                    _hueRingImage.Source = wb;
                }
            } while (_isLoaded);
        }

        private async void DelayedUpdateWorkaround()
        {
            // Not sure why the thumb starts at the wrong position without this code.
            // Perhaps something with path positioning overall, but I don't want to dig into this now.
            await Task.Delay(10);
            UpdateRingThumb();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            UpdateVisuals();
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            UpdateRingThumb();
        }

        protected override async void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _containerGrid = (Grid)GetTemplateChild(ContainerGridName);
            _hueRingImage = (Image)GetTemplateChild(HueRingImageName);
            _ringThumb = (RingSlice)GetTemplateChild(RingThumbName);
            _hueRingImage.PointerPressed += OnPointerPressed;
            _hueRingImage.PointerMoved += OnPointerMoved;
            await this.WaitForLoadedAsync();
            UpdateVisuals();
        }

        private void OnPointerMoved(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            if (!pointerRoutedEventArgs.Pointer.IsInContact)
            {
                return;
            }

            var point = pointerRoutedEventArgs.GetCurrentPoint(_hueRingImage).Position;
            UpdateValueForPoint(point);
        }

        private void OnPointerPressed(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            _hueRingImage.CapturePointer(pointerRoutedEventArgs.Pointer);
            var point = pointerRoutedEventArgs.GetCurrentPoint(_hueRingImage).Position;
            UpdateValueForPoint(point);
        }

        private void UpdateValueForPoint(Point point)
        {
            var pc = new Point(
                _hueRingImage.ActualWidth / 2, _hueRingImage.ActualHeight / 2);
            var atan2 = Math.Atan2(point.Y - pc.Y, point.X - pc.X) * 180 / Math.PI;
            this.Value = (atan2 + 360 + 90) % 360;
        }

        private void UpdateVisuals()
        {
            UpdateHueRingImage();
            UpdateRingThumb();
        }

        private void UpdateRingThumb()
        {
            if (_ringThumb == null)
            {
                return;
            }

            if (_containerGrid.ActualHeight <= 2 * (RingThickness + ThumbBorderThickness) ||
                _containerGrid.ActualWidth <= 2 * (RingThickness + ThumbBorderThickness))
            {
                return;
            }

            //VisualTreeDebugger2.DebugVisualTree(_ringThumb, true);

            // Hue ring needs to be smaller than available container space so the thumb fits
            var hueRingSize = Math.Min(
                _containerGrid.ActualWidth - 2 * ThumbBorderThickness,
                _containerGrid.ActualHeight - 2 * ThumbBorderThickness);

            //if (_ringThumb.Width != hueRingSize + 2 * ThumbBorderThickness)
            {
                _ringThumb.Width = hueRingSize + 2 * ThumbBorderThickness;
                _ringThumb.Height = hueRingSize + 2 * ThumbBorderThickness;

                _ringThumb.BeginUpdate();

                // Half of the thumb border stroke goes outside the radius and the other half goes inside,
                // so radius needs to be 0.5 border thickness larger than the hue ring's outer radius.
                // Less 1 to make sure there is an overlap.
                _ringThumb.Center =
                    new Point(
                        hueRingSize / 2 + ThumbBorderThickness,
                        hueRingSize / 2 + ThumbBorderThickness);
                _ringThumb.Radius = (hueRingSize + ThumbBorderThickness) / 2 - 1;
                _ringThumb.InnerRadius = _ringThumb.Radius - RingThickness - ThumbBorderThickness + 2;
                _ringThumb.StartAngle = this.Value - ThumbArcAngle / 2;
                _ringThumb.EndAngle = this.Value + ThumbArcAngle / 2;
                //_ringThumb.StartAngle = - ThumbArcAngle / 2;
                //_ringThumb.EndAngle = ThumbArcAngle / 2;
                _ringThumb.EndUpdate();
            }

            _ringThumb.Stroke =
                new SolidColorBrush(
                    ColorExtensions.FromHsv(this.Value, 0.5, 1.0));
        }

        #region UpdateHueRingImage()
        private void UpdateHueRingImage()
        {
            _bitmapUpdateRequired.Set();
        } 
        #endregion

        #region Min/Max coercion methods
        /// <summary>
        /// Initializes event handling to coerce Minimum and Maximum properties to the 0..360 range.
        /// </summary>
        private void InitializeMinMaxCoercion()
        {
            this.Minimum = 0.0;
            this.Maximum = 360.0;
            //var minimumChangedEventSource = new PropertyChangeEventSource<double>(
            //    this, "Minimum");
            //minimumChangedEventSource.ValueChanged += OnMinimumChanged;
            //var maximumChangedEventSource = new PropertyChangeEventSource<double>(
            //    this, "Maximum");
            //maximumChangedEventSource.ValueChanged += OnMaximumChanged;
        }

        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            base.OnMinimumChanged(oldMinimum, newMinimum);

            if (newMinimum < 0)
            {
                this.Minimum = 0;
            }
        }

        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            base.OnMaximumChanged(oldMaximum, newMaximum);

            if (newMaximum > 360)
            {
                this.Maximum = 360;
            }
        } 
        #endregion
    }

    //public class VisualTreeDebugger2
    //{
    //    #region BreakOnLoaded
    //    /// <summary>
    //    /// BreakOnLoaded BreakOnLoaded Dependency Property
    //    /// </summary>
    //    public static readonly DependencyProperty BreakOnLoadedProperty =
    //        DependencyProperty.RegisterAttached(
    //            "BreakOnLoaded",
    //            typeof(bool),
    //            typeof(VisualTreeDebugger2),
    //            new PropertyMetadata(false, OnBreakOnLoadedChanged));

    //    /// <summary>
    //    /// Gets the BreakOnLoaded property. This dependency property
    //    /// indicates whether the debugger should BreakOnLoaded when control is loaded.
    //    /// </summary>
    //    /// <param name="d">
    //    /// The DependencyObject.
    //    /// </param>
    //    /// <returns>
    //    /// the boolean value
    //    /// </returns>
    //    public static bool GetBreakOnLoaded(DependencyObject d)
    //    {
    //        return (bool)d.GetValue(BreakOnLoadedProperty);
    //    }

    //    /// <summary>
    //    /// Sets the BreakOnLoaded property. This dependency property
    //    /// indicates whether the debugger should BreakOnLoaded when control is loaded.
    //    /// </summary>
    //    /// <param name="d">
    //    /// The DependencyObject.
    //    /// </param>
    //    /// <param name="value">
    //    /// If set to <c>true</c> the attached debugger will break when the control
    //    /// loads.
    //    /// </param>
    //    public static void SetBreakOnLoaded(DependencyObject d, bool value)
    //    {
    //        d.SetValue(BreakOnLoadedProperty, value);
    //    }

    //    /// <summary>
    //    /// Called when [break on loaded changed].
    //    /// </summary>
    //    /// <param name="d">
    //    /// The DependencyObject.
    //    /// </param>
    //    /// <param name="e">
    //    /// The <see cref="Windows.UI.Xaml.DependencyPropertyChangedEventArgs"/>
    //    /// instance containing the event data.</param>
    //    private static void OnBreakOnLoadedChanged(
    //        DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {
    //        if ((bool)e.NewValue)
    //        {
    //            ((FrameworkElement)d).Loaded += BreakOnControlLoaded;
    //        }
    //        else
    //        {
    //            ((FrameworkElement)d).Loaded -= BreakOnControlLoaded;
    //        }
    //    }
    //    #endregion

    //    #region BreakOnTap
    //    /// <summary>
    //    /// BreakOnTap Attached Dependency Property
    //    /// </summary>
    //    public static readonly DependencyProperty BreakOnTapProperty =
    //        DependencyProperty.RegisterAttached(
    //            "BreakOnTap",
    //            typeof(bool),
    //            typeof(VisualTreeDebugger2),
    //            new PropertyMetadata(false, OnBreakOnTapChanged));

    //    /// <summary>
    //    /// Gets the BreakOnTap property. This dependency property
    //    /// indicates whether the attached debugger should break when
    //    /// the FrameworkElement on which this property is set is tapped.
    //    /// </summary>
    //    /// <param name="d">
    //    /// The DependencyObject.
    //    /// </param>
    //    /// <returns>
    //    /// The boolean value
    //    /// </returns>
    //    public static bool GetBreakOnTap(DependencyObject d)
    //    {
    //        return (bool)d.GetValue(BreakOnTapProperty);
    //    }

    //    /// <summary>
    //    /// Sets the BreakOnTap property. This dependency property
    //    /// indicates whether the attached debugger should break when
    //    /// the FrameworkElement on which this property is set is tapped.
    //    /// </summary>
    //    /// <param name="d">
    //    /// The DependencyObject.
    //    /// </param>
    //    /// <param name="value">if set to <c>true</c>
    //    ///  [value].
    //    /// </param>
    //    public static void SetBreakOnTap(DependencyObject d, bool value)
    //    {
    //        d.SetValue(BreakOnTapProperty, value);
    //    }

    //    /// <summary>
    //    /// Handles changes to the BreakOnTap property.
    //    /// </summary>
    //    /// <param name="d">
    //    /// The DependencyObject.
    //    /// </param>
    //    /// <param name="e">
    //    /// The <see cref="Windows.UI.Xaml.DependencyPropertyChangedEventArgs"/>
    //    /// instance containing the event data.</param>
    //    private static void OnBreakOnTapChanged(
    //        DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {
    //        var frameworkElement = d as FrameworkElement;

    //        Debug.Assert(
    //            frameworkElement != null,
    //            "BreakOnTapProperty should only be set on FrameworkElements.");

    //        if ((bool)e.NewValue)
    //        {
    //            ((FrameworkElement)d).Tapped += BreakOnControlTapped;
    //        }
    //        else
    //        {
    //            ((FrameworkElement)d).Tapped -= BreakOnControlTapped;
    //        }
    //    }

    //    /// <summary>
    //    /// Called when the control gets tapped or clicked.
    //    /// </summary>
    //    /// <param name="sender">
    //    /// The sender.
    //    /// </param>
    //    /// <param name="e">
    //    /// The <see cref="Windows.UI.Xaml.Input.TappedRoutedEventArgs"/> instance
    //    /// containing the event data.
    //    /// </param>
    //    private static async void BreakOnControlTapped(object sender, TappedRoutedEventArgs e)
    //    {
    //        var startElement = (DependencyObject)sender;
    //        var delay = GetBreakDelay(startElement);

    //        if (delay > 0)
    //        {
    //            await Task.Delay((int)(delay * 1000));
    //        }

    //        DebugVisualTree(startElement);
    //    }
    //    #endregion

    //    #region BreakOnLayoutUpdated
    //    /// <summary>
    //    /// BreakOnLayoutUpdated Attached Dependency Property
    //    /// </summary>
    //    public static readonly DependencyProperty BreakOnLayoutUpdatedProperty =
    //        DependencyProperty.RegisterAttached(
    //            "BreakOnLayoutUpdated",
    //            typeof(bool),
    //            typeof(VisualTreeDebugger2),
    //            new PropertyMetadata(false, OnBreakOnLayoutUpdatedChanged));

    //    /// <summary>
    //    /// Gets the BreakOnLayoutUpdated property. This dependency property
    //    /// indicates whether the attached debugger should break when
    //    /// the FrameworkElement on which this property is set has its layout updated.
    //    /// </summary>
    //    /// <param name="d">
    //    /// The DependencyObject.
    //    /// </param>
    //    /// <returns>
    //    /// The boolean value
    //    /// </returns>
    //    public static bool GetBreakOnLayoutUpdated(DependencyObject d)
    //    {
    //        return (bool)d.GetValue(BreakOnLayoutUpdatedProperty);
    //    }

    //    /// <summary>
    //    /// Sets the BreakOnLayoutUpdated property. This dependency property
    //    /// indicates whether the attached debugger should break when
    //    /// the FrameworkElement on which this property is set has its layout updated.
    //    /// </summary>
    //    /// <param name="d">
    //    /// The DependencyObject.
    //    /// </param>
    //    /// <param name="value">if set to <c>true</c>
    //    ///  [value].
    //    /// </param>
    //    public static void SetBreakOnLayoutUpdated(DependencyObject d, bool value)
    //    {
    //        d.SetValue(BreakOnLayoutUpdatedProperty, value);
    //    }

    //    /// <summary>
    //    /// Handles changes to the BreakOnLayoutUpdated property.
    //    /// </summary>
    //    /// <param name="d">
    //    /// The DependencyObject.
    //    /// </param>
    //    /// <param name="e">
    //    /// The <see cref="Windows.UI.Xaml.DependencyPropertyChangedEventArgs"/>
    //    /// instance containing the event data.</param>
    //    private static void OnBreakOnLayoutUpdatedChanged(
    //        DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {
    //        var frameworkElement = (FrameworkElement)d;

    //        frameworkElement.LayoutUpdated += async (s, o) =>
    //        {
    //            var delay = GetBreakDelay(frameworkElement);

    //            if (delay > 0)
    //            {
    //                await Task.Delay((int)(delay * 1000));
    //            }

    //            DebugVisualTree(frameworkElement);
    //        };
    //    }
    //    #endregion

    //    #region BreakDelay
    //    /// <summary>
    //    /// BreakDelay Attached Dependency Property
    //    /// </summary>
    //    public static readonly DependencyProperty BreakDelayProperty =
    //        DependencyProperty.RegisterAttached(
    //            "BreakDelay",
    //            typeof(double),
    //            typeof(VisualTreeDebugger2),
    //            new PropertyMetadata(0d));

    //    /// <summary>
    //    /// Gets the BreakDelay property. This dependency property 
    //    /// indicates the delay to wait after the trigger is fired before breaking in debugger.
    //    /// </summary>
    //    public static double GetBreakDelay(DependencyObject d)
    //    {
    //        return (double)d.GetValue(BreakDelayProperty);
    //    }

    //    /// <summary>
    //    /// Sets the BreakDelay property. This dependency property 
    //    /// indicates the delay to wait after the trigger is fired before breaking in debugger.
    //    /// </summary>
    //    public static void SetBreakDelay(DependencyObject d, double value)
    //    {
    //        d.SetValue(BreakDelayProperty, value);
    //    }
    //    #endregion

    //    #region BreakOnControlLoaded()
    //    /// <summary>
    //    /// Called when the control gets loaded.
    //    /// </summary>
    //    /// <param name="sender">
    //    /// The sender.
    //    /// </param>
    //    /// <param name="e">
    //    /// The <see cref="RoutedEventArgs"/> instance containing
    //    /// the event data.</param>
    //    private static async void BreakOnControlLoaded(object sender, RoutedEventArgs e)
    //    {
    //        var startElement = (DependencyObject)sender;
    //        var delay = GetBreakDelay(startElement);

    //        if (delay > 0)
    //        {
    //            await Task.Delay((int)(delay * 1000));
    //        }

    //        DebugVisualTree(startElement);
    //    }
    //    #endregion

    //    #region TraceOnLoaded
    //    /// <summary>
    //    /// TraceOnLoaded TraceOnLoaded Dependency Property
    //    /// </summary>
    //    public static readonly DependencyProperty TraceOnLoadedProperty =
    //        DependencyProperty.RegisterAttached(
    //            "TraceOnLoaded",
    //            typeof(bool),
    //            typeof(VisualTreeDebugger2),
    //            new PropertyMetadata(false, OnTraceOnLoadedChanged));

    //    /// <summary>
    //    /// Gets the TraceOnLoaded property. This dependency property
    //    /// indicates whether the debugger should TraceOnLoaded when control is loaded.
    //    /// </summary>
    //    /// <param name="d">
    //    /// The DependencyObject.
    //    /// </param>
    //    /// <returns>
    //    /// the boolean value
    //    /// </returns>
    //    public static bool GetTraceOnLoaded(DependencyObject d)
    //    {
    //        return (bool)d.GetValue(TraceOnLoadedProperty);
    //    }

    //    /// <summary>
    //    /// Sets the TraceOnLoaded property. This dependency property
    //    /// indicates whether the debugger should TraceOnLoaded when control is loaded.
    //    /// </summary>
    //    /// <param name="d">
    //    /// The DependencyObject.
    //    /// </param>
    //    /// <param name="value">
    //    /// If set to <c>true</c> the attached debugger will Trace when the control
    //    /// loads.
    //    /// </param>
    //    public static void SetTraceOnLoaded(DependencyObject d, bool value)
    //    {
    //        d.SetValue(TraceOnLoadedProperty, value);
    //    }

    //    /// <summary>
    //    /// Called when [Trace on loaded changed].
    //    /// </summary>
    //    /// <param name="d">
    //    /// The DependencyObject.
    //    /// </param>
    //    /// <param name="e">
    //    /// The <see cref="Windows.UI.Xaml.DependencyPropertyChangedEventArgs"/>
    //    /// instance containing the event data.</param>
    //    private static void OnTraceOnLoadedChanged(
    //        DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {
    //        if ((bool)e.NewValue)
    //        {
    //            ((FrameworkElement)d).Loaded += TraceOnControlLoaded;
    //        }
    //        else
    //        {
    //            ((FrameworkElement)d).Loaded -= TraceOnControlLoaded;
    //        }
    //    }
    //    #endregion

    //    #region TraceOnTap
    //    /// <summary>
    //    /// TraceOnTap Attached Dependency Property
    //    /// </summary>
    //    public static readonly DependencyProperty TraceOnTapProperty =
    //        DependencyProperty.RegisterAttached(
    //            "TraceOnTap",
    //            typeof(bool),
    //            typeof(VisualTreeDebugger2),
    //            new PropertyMetadata(false, OnTraceOnTapChanged));

    //    /// <summary>
    //    /// Gets the TraceOnTap property. This dependency property
    //    /// indicates whether the attached debugger should Trace when
    //    /// the FrameworkElement on which this property is set is tapped.
    //    /// </summary>
    //    /// <param name="d">
    //    /// The DependencyObject.
    //    /// </param>
    //    /// <returns>
    //    /// The boolean value
    //    /// </returns>
    //    public static bool GetTraceOnTap(DependencyObject d)
    //    {
    //        return (bool)d.GetValue(TraceOnTapProperty);
    //    }

    //    /// <summary>
    //    /// Sets the TraceOnTap property. This dependency property
    //    /// indicates whether the attached debugger should Trace when
    //    /// the FrameworkElement on which this property is set is tapped.
    //    /// </summary>
    //    /// <param name="d">
    //    /// The DependencyObject.
    //    /// </param>
    //    /// <param name="value">if set to <c>true</c>
    //    ///  [value].
    //    /// </param>
    //    public static void SetTraceOnTap(DependencyObject d, bool value)
    //    {
    //        d.SetValue(TraceOnTapProperty, value);
    //    }

    //    /// <summary>
    //    /// Handles changes to the TraceOnTap property.
    //    /// </summary>
    //    /// <param name="d">
    //    /// The DependencyObject.
    //    /// </param>
    //    /// <param name="e">
    //    /// The <see cref="Windows.UI.Xaml.DependencyPropertyChangedEventArgs"/>
    //    /// instance containing the event data.</param>
    //    private static void OnTraceOnTapChanged(
    //        DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {
    //        var frameworkElement = d as FrameworkElement;

    //        Debug.Assert(
    //            frameworkElement != null,
    //            "TraceOnTapProperty should only be set on FrameworkElements.");

    //        if ((bool)e.NewValue)
    //        {
    //            ((FrameworkElement)d).Tapped += TraceOnControlTapped;
    //        }
    //        else
    //        {
    //            ((FrameworkElement)d).Tapped -= TraceOnControlTapped;
    //        }
    //    }
    //    #endregion

    //    #region TraceOnLayoutUpdated
    //    /// <summary>
    //    /// TraceOnLayoutUpdated Attached Dependency Property
    //    /// </summary>
    //    public static readonly DependencyProperty TraceOnLayoutUpdatedProperty =
    //        DependencyProperty.RegisterAttached(
    //            "TraceOnLayoutUpdated",
    //            typeof(bool),
    //            typeof(VisualTreeDebugger2),
    //            new PropertyMetadata(false, OnTraceOnLayoutUpdatedChanged));

    //    /// <summary>
    //    /// Gets the TraceOnLayoutUpdated property. This dependency property
    //    /// indicates whether the attached debugger should Trace when
    //    /// the FrameworkElement on which this property is set has its layout updated.
    //    /// </summary>
    //    /// <param name="d">
    //    /// The DependencyObject.
    //    /// </param>
    //    /// <returns>
    //    /// The boolean value
    //    /// </returns>
    //    public static bool GetTraceOnLayoutUpdated(DependencyObject d)
    //    {
    //        return (bool)d.GetValue(TraceOnLayoutUpdatedProperty);
    //    }

    //    /// <summary>
    //    /// Sets the TraceOnLayoutUpdated property. This dependency property
    //    /// indicates whether the attached debugger should Trace when
    //    /// the FrameworkElement on which this property is set has its layout updated.
    //    /// </summary>
    //    /// <param name="d">
    //    /// The DependencyObject.
    //    /// </param>
    //    /// <param name="value">if set to <c>true</c>
    //    ///  [value].
    //    /// </param>
    //    public static void SetTraceOnLayoutUpdated(DependencyObject d, bool value)
    //    {
    //        d.SetValue(TraceOnLayoutUpdatedProperty, value);
    //    }

    //    /// <summary>
    //    /// Handles changes to the TraceOnLayoutUpdated property.
    //    /// </summary>
    //    /// <param name="d">
    //    /// The DependencyObject.
    //    /// </param>
    //    /// <param name="e">
    //    /// The <see cref="Windows.UI.Xaml.DependencyPropertyChangedEventArgs"/>
    //    /// instance containing the event data.</param>
    //    private static void OnTraceOnLayoutUpdatedChanged(
    //        DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {
    //        var frameworkElement = (FrameworkElement)d;

    //        frameworkElement.LayoutUpdated += (s, o) => DebugVisualTree(frameworkElement, false);
    //    }
    //    #endregion

    //    #region TraceOnControlLoaded()
    //    /// <summary>
    //    /// Called when the control gets loaded.
    //    /// </summary>
    //    /// <param name="sender">
    //    /// The sender.
    //    /// </param>
    //    /// <param name="e">
    //    /// The <see cref="Windows.UI.Xaml.RoutedEventArgs"/> instance containing
    //    /// the event data.</param>
    //    private static void TraceOnControlLoaded(object sender, RoutedEventArgs e)
    //    {
    //        var startElement = (DependencyObject)sender;
    //        DebugVisualTree(startElement, false);
    //    }
    //    #endregion

    //    #region TraceOnControlTapped()
    //    /// <summary>
    //    /// Called when the control gets tapped or clicked.
    //    /// </summary>
    //    /// <param name="sender">
    //    /// The sender.
    //    /// </param>
    //    /// <param name="e">
    //    /// The <see cref="Windows.UI.Xaml.Input.TappedRoutedEventArgs"/> instance
    //    /// containing the event data.</param>
    //    private static void TraceOnControlTapped(object sender, TappedRoutedEventArgs e)
    //    {
    //        var startElement = (DependencyObject)sender;
    //        DebugVisualTree(startElement, false);
    //    }
    //    #endregion

    //    #region DebugVisualTree()
    //    /// <summary>
    //    /// Debugs the visual tree.
    //    /// </summary>
    //    /// <param name="startElement">
    //    /// The start element.
    //    /// </param>
    //    /// <param name="breakInDebugger">If true and debugger is attached - it will break execution once done tracing the visual tree properties.</param>
    //    public static void DebugVisualTree(DependencyObject startElement, bool breakInDebugger = true)
    //    {
    //        var path = new List<DependencyObject>();
    //        var dob = startElement;

    //        while (dob != null)
    //        {
    //            path.Add(dob);
    //            dob = VisualTreeHelper.GetParent(dob);
    //        }

    //        for (int i = path.Count - 1; i >= 0; i--)
    //        {
    //            TraceDependencyObject(path[i], i);
    //        }

    //        // Put breakpoint here
    //        Debug.WriteLine(
    //            string.Format(
    //                "Watch path[0] to path[{0}]",
    //                path.Count - 1));

    //        if (breakInDebugger && Debugger.IsAttached)
    //        {
    //            Debugger.Break();
    //        }
    //    }
    //    #endregion

    //    #region TraceDependencyObject()
    //    /// <summary>
    //    /// Traces the dependency object.
    //    /// </summary>
    //    /// <param name="dob">
    //    /// The dependancy object.
    //    /// </param>
    //    /// <param name="i">
    //    /// The object index.
    //    /// </param>
    //    private static void TraceDependencyObject(DependencyObject dob, int i)
    //    {
    //        var frameworkElement = dob as FrameworkElement;

    //        if (frameworkElement == null)
    //        {
    //            Debug.WriteLine(
    //                "path[{0}] is Dependency Object: {1}",
    //                i,
    //                dob.GetType());
    //        }
    //        else
    //        {
    //            var c = frameworkElement as Control;
    //            var cc = frameworkElement as ContentControl;
    //            var panel = frameworkElement as Panel;
    //            var parentGrid = frameworkElement.Parent as Grid;
    //            var image = frameworkElement as Image;
    //            var scrollViewer = frameworkElement as ScrollViewer;

    //            Debug.WriteLine(
    //                "path[{0}] is Control: {1}({2}):",
    //                i,
    //                frameworkElement.GetType(),
    //                frameworkElement.Name ?? "<unnamed>");

    //            // Actual layout information
    //            Debug.WriteLine(
    //                "\tActualWidth={0}\r\n\tActualHeight={1}",
    //                frameworkElement.ActualWidth,
    //                frameworkElement.ActualHeight);
    //            var pos =
    //                frameworkElement
    //                    .TransformToVisual(Window.Current.Content)
    //                    .TransformPoint(new Point());
    //            var pos2 =
    //                frameworkElement
    //                    .TransformToVisual(Window.Current.Content)
    //                    .TransformPoint(
    //                        new Point(
    //                            frameworkElement.ActualWidth,
    //                            frameworkElement.ActualHeight));

    //            Debug.WriteLine(
    //                "\tPosition – X={0}, Y={1}, Right={2}, Bottom={3}",
    //                pos.X,
    //                pos.Y,
    //                pos2.X,
    //                pos2.Y);

    //            if (frameworkElement.Opacity < 1.0)
    //            {
    //                Debug.WriteLine("\tOpacity={0}", frameworkElement.Opacity);
    //            }

    //            // DataContext often turns out to be a surprise
    //            Debug.WriteLine(
    //                "\tDataContext: {0} {1}",
    //                frameworkElement.DataContext,
    //                frameworkElement.DataContext != null
    //                    ? "HashCode: " + frameworkElement.DataContext.GetHashCode()
    //                    : "");

    //            // List common layout properties
    //            if (!double.IsNaN(frameworkElement.Width) ||
    //                !double.IsNaN(frameworkElement.Height))
    //            {
    //                Debug.WriteLine(
    //                    "\tWidth={0}\r\n\tHeight={1}",
    //                    frameworkElement.Width,
    //                    frameworkElement.Height);
    //            }

    //            if (scrollViewer != null)
    //            {
    //                Debug.WriteLine(
    //                    "\tScrollViewer.HorizontalOffset={0}\r\n\tScrollViewer.ViewportWidth={1}\r\n\tScrollViewer.ExtentWidth={2}\r\n\tScrollViewer.VerticalOffset={3}\r\n\tScrollViewer.ViewportHeight={4}\r\n\tScrollViewer.ExtentHeight={5}",
    //                    scrollViewer.HorizontalOffset,
    //                    scrollViewer.ViewportWidth,
    //                    scrollViewer.ExtentWidth,
    //                    scrollViewer.VerticalOffset,
    //                    scrollViewer.ViewportHeight,
    //                    scrollViewer.ExtentHeight
    //                    );
    //            }

    //            if (frameworkElement.MinWidth > 0 ||
    //                frameworkElement.MinHeight > 0 ||
    //                !double.IsInfinity(frameworkElement.MaxWidth) ||
    //                !double.IsInfinity(frameworkElement.MaxHeight))
    //            {
    //                Debug.WriteLine(
    //                    "\tMinWidth={0}\r\n\tMaxWidth={1}\r\n\tMinHeight={2}\r\n\tMaxHeight={3}",
    //                    frameworkElement.MinWidth,
    //                    frameworkElement.MaxWidth,
    //                    frameworkElement.MinHeight,
    //                    frameworkElement.MaxHeight);
    //            }

    //            Debug.WriteLine(
    //                "\tHorizontalAlignment={0}\r\n\tVerticalAlignment={1}",
    //                frameworkElement.HorizontalAlignment,
    //                frameworkElement.VerticalAlignment);

    //            if (cc != null)
    //            {
    //                Debug.WriteLine(
    //                    "\tHorizontalContentAlignment={0}\r\n\tVerticalContentAlignment={1}\r\n\tContent={2}",
    //                    cc.HorizontalContentAlignment,
    //                    cc.VerticalContentAlignment,
    //                    cc.Content ?? "<null>");
    //            }

    //            Debug.WriteLine(
    //                "\tMargins={0},{1},{2},{3}",
    //                frameworkElement.Margin.Left,
    //                frameworkElement.Margin.Top,
    //                frameworkElement.Margin.Right,
    //                frameworkElement.Margin.Bottom);

    //            if (cc != null)
    //            {
    //                Debug.WriteLine("\tPadding={0}", cc.Padding);
    //            }

    //            if (panel != null)
    //            {
    //                Debug.WriteLine("\tBackground={0}", BrushToString(panel.Background));
    //            }
    //            else if (c != null)
    //            {
    //                Debug.WriteLine("\tBackground={0}", BrushToString(c.Background));
    //                Debug.WriteLine("\tForeground={0}", BrushToString(c.Foreground));
    //            }

    //            if (parentGrid != null)
    //            {
    //                var col = Grid.GetColumn(frameworkElement);
    //                var row = Grid.GetRow(frameworkElement);

    //                if (parentGrid.ColumnDefinitions.Count != 0 || col != 0)
    //                {
    //                    Debug.Assert(
    //                        col < parentGrid.ColumnDefinitions.Count,
    //                        string.Format(
    //                            "Column {0} not defined on the parent Grid!", col));
    //                    col = Math.Min(col, parentGrid.ColumnDefinitions.Count - 1);
    //                    Debug.WriteLine(
    //                        "\tColumn: {0} ({1})",
    //                        col,
    //                        parentGrid.ColumnDefinitions[col].Width);
    //                }

    //                if (parentGrid.RowDefinitions.Count != 0 || row != 0)
    //                {
    //                    Debug.Assert(
    //                        row < parentGrid.RowDefinitions.Count,
    //                        string.Format(
    //                            "Row {0} not defined on the parent Grid!", row));
    //                    row = Math.Min(row, parentGrid.RowDefinitions.Count - 1);
    //                    Debug.WriteLine(
    //                        "\tRow: {0} ({1})",
    //                        row,
    //                        parentGrid.RowDefinitions[row].Height);
    //                }
    //            }

    //            if (c != null)
    //            {
    //                Debug.WriteLine("\tFontFamily: {0}", c.FontFamily.Source);
    //            }

    //            if (image != null)
    //            {
    //                Debug.WriteLine("\tImage\n  Source: {0}", image.Source);

    //                var bs = image.Source as BitmapSource;
    //                var bi = image.Source as BitmapImage;

    //                if (bi != null)
    //                {
    //                    Debug.WriteLine("\t\tSource.UriSource: {0}", bi.UriSource.OriginalString);
    //                }

    //                if (bs != null)
    //                {
    //                    Debug.WriteLine("\t\tPixelWidth: {0}", bs.PixelWidth);
    //                    Debug.WriteLine("\t\tPixelHeight: {0}", bs.PixelHeight);
    //                }
    //            }

    //            if (frameworkElement.Parent is Canvas)
    //            {
    //                var x = Canvas.GetLeft(frameworkElement);
    //                var y = Canvas.GetTop(frameworkElement);
    //                var zIndex = Canvas.GetZIndex(frameworkElement);

    //                Debug.WriteLine(
    //                    "\tCanvas – X={0}, Y={1}, ZIndex={2}", x, y, zIndex);
    //            }
    //        }
    //    }
    //    #endregion

    //    #region BrushToString()
    //    /// <summary>
    //    /// Brushes to string.
    //    /// </summary>
    //    /// <param name="brush">
    //    /// The brush.
    //    /// </param>
    //    /// <returns>
    //    /// brush type
    //    /// </returns>
    //    private static string BrushToString(Brush brush)
    //    {
    //        if (brush == null)
    //        {
    //            return "";
    //        }

    //        var solidColorBrush = brush as SolidColorBrush;

    //        if (solidColorBrush != null)
    //        {
    //            return string.Format("SolidColorBrush: {0}", solidColorBrush.Color);
    //        }

    //        var imageBrush = brush as ImageBrush;

    //        if (imageBrush != null)
    //        {
    //            var bi = imageBrush.ImageSource as BitmapImage;

    //            if (bi != null)
    //            {
    //                return string.Format(
    //                    "ImageBrush: {0}, UriSource: {1}",
    //                    bi,
    //                    bi.UriSource);
    //            }

    //            return string.Format("ImageBrush: {0}", imageBrush.ImageSource);
    //        }

    //        return brush.ToString();
    //    }
    //    #endregion
    //}
}
