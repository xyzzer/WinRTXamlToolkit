using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using WinRTXamlToolkit.Controls.Common;
using WinRTXamlToolkit.Tools;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// Determins whether the value bar behind the value text should be visible
    /// </summary>
    public enum NumericUpDownValueBarVisibility
    {
        /// <summary>
        /// Visible
        /// </summary>
        Visible,
        /// <summary>
        /// Collapsed
        /// </summary>
        Collapsed
    }

    /// <summary>
    /// NumericUpDown control - for representing values that can be
    /// entered with keyboard,
    /// using increment/decrement buttons
    /// as well as swiping over the control.
    /// </summary>
    [TemplatePart(Name = ValueTextBoxName, Type = typeof(TextBox))]
    [TemplatePart(Name = ValueBarName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = DragOverlayName, Type = typeof(UIElement))]
    [TemplatePart(Name = DecrementButtonName, Type = typeof(RepeatButton))]
    [TemplatePart(Name = IncrementButtonName, Type = typeof(RepeatButton))]
    [TemplateVisualState(GroupName = "IncrementalButtonStates", Name = "IncrementEnabled")]
    [TemplateVisualState(GroupName = "IncrementalButtonStates", Name = "IncrementDisabled")]
    [TemplateVisualState(GroupName = "DecrementalButtonStates", Name = "DecrementEnabled")]
    [TemplateVisualState(GroupName = "DecrementalButtonStates", Name = "DecrementDisabled")]
    public sealed class NumericUpDown : RangeBase
    {
        private const string DecrementButtonName = "PART_DecrementButton";
        private const string IncrementButtonName = "PART_IncrementButton";
        private const string DragOverlayName = "PART_DragOverlay";
        private const string ValueTextBoxName = "PART_ValueTextBox";
        private const string ValueBarName = "PART_ValueBar";
        private UIElement _dragOverlay;
        private UpDownTextBox _valueTextBox;
        private RepeatButton _decrementButton;
        private RepeatButton _incrementButton;
        private FrameworkElement _valueBar;
        private bool _isDragUpdated;
        private bool _isChangingTextWithCode;
        private bool _isChangingValueWithCode;
        private double _unusedManipulationDelta;

        private bool _isDraggingWithMouse;
        private MouseDevice _mouseDevice;
        private const double MinMouseDragDelta = 2;
        private double _totalDeltaX;
        private double _totalDeltaY;

        #region ValueFormat
        /// <summary>
        /// ValueFormat Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValueFormatProperty =
            DependencyProperty.Register(
                "ValueFormat",
                typeof(string),
                typeof(NumericUpDown),
                new PropertyMetadata("F2", OnValueFormatChanged));

        /// <summary>
        /// Gets or sets the ValueFormat property. This dependency property 
        /// indicates the format of the value string.
        /// </summary>
        public string ValueFormat
        {
            get { return (string)GetValue(ValueFormatProperty); }
            set { SetValue(ValueFormatProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ValueFormat property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnValueFormatChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (NumericUpDown)d;
            string oldValueFormat = (string)e.OldValue;
            string newValueFormat = target.ValueFormat;
            target.OnValueFormatChanged(oldValueFormat, newValueFormat);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the ValueFormat property.
        /// </summary>
        /// <param name="oldValueFormat">The old ValueFormat value</param>
        /// <param name="newValueFormat">The new ValueFormat value</param>
        private void OnValueFormatChanged(
            string oldValueFormat, string newValueFormat)
        {
            UpdateValueText();
        }
        #endregion

        #region ValueBarVisibility
        /// <summary>
        /// ValueBarVisibility Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValueBarVisibilityProperty =
            DependencyProperty.Register(
                "ValueBarVisibility",
                typeof(NumericUpDownValueBarVisibility),
                typeof(NumericUpDown),
                new PropertyMetadata(NumericUpDownValueBarVisibility.Visible, OnValueBarVisibilityChanged));

        /// <summary>
        /// Gets or sets the ValueBarVisibility property. This dependency property 
        /// indicates whether the value bar should be shown.
        /// </summary>
        public NumericUpDownValueBarVisibility ValueBarVisibility
        {
            get { return (NumericUpDownValueBarVisibility)GetValue(ValueBarVisibilityProperty); }
            set { SetValue(ValueBarVisibilityProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ValueBarVisibility property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnValueBarVisibilityChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (NumericUpDown)d;
            NumericUpDownValueBarVisibility oldValueBarVisibility = (NumericUpDownValueBarVisibility)e.OldValue;
            NumericUpDownValueBarVisibility newValueBarVisibility = target.ValueBarVisibility;
            target.OnValueBarVisibilityChanged(oldValueBarVisibility, newValueBarVisibility);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the ValueBarVisibility property.
        /// </summary>
        /// <param name="oldValueBarVisibility">The old ValueBarVisibility value</param>
        /// <param name="newValueBarVisibility">The new ValueBarVisibility value</param>
        private void OnValueBarVisibilityChanged(
            NumericUpDownValueBarVisibility oldValueBarVisibility, NumericUpDownValueBarVisibility newValueBarVisibility)
        {
            UpdateValueBar();
        }
        #endregion

        #region IsReadOnly
        /// <summary>
        /// IsReadOnly Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(
                "IsReadOnly",
                typeof(bool),
                typeof(NumericUpDown),
                new PropertyMetadata(false, OnIsReadOnlyChanged));

        /// <summary>
        /// Gets or sets the IsReadOnly property. This dependency property 
        /// indicates whether the box should only allow to read the values by copying and pasting them.
        /// </summary>
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IsReadOnly property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnIsReadOnlyChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (NumericUpDown)d;
            bool oldIsReadOnly = (bool)e.OldValue;
            bool newIsReadOnly = target.IsReadOnly;
            target.OnIsReadOnlyChanged(oldIsReadOnly, newIsReadOnly);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the IsReadOnly property.
        /// </summary>
        /// <param name="oldIsReadOnly">The old IsReadOnly value</param>
        /// <param name="newIsReadOnly">The new IsReadOnly value</param>
        private void OnIsReadOnlyChanged(
            bool oldIsReadOnly, bool newIsReadOnly)
        {
            UpdateIsReadOnlyDependants();
        }
        #endregion

        #region DragSpeed
        /// <summary>
        /// DragSpeed Dependency Property
        /// </summary>
        public static readonly DependencyProperty DragSpeedProperty =
            DependencyProperty.Register(
                "DragSpeed",
                typeof(double),
                typeof(NumericUpDown),
                new PropertyMetadata(double.NaN));

        /// <summary>
        /// Gets or sets the DragSpeed property. This dependency property 
        /// indicates the speed with which the value changes when manipulated with dragging.
        /// The default value of double.NaN indicates that the value will change by (Maximum - Minimum),
        /// when the control is dragged a full screen length.
        /// </summary>
        public double DragSpeed
        {
            get { return (double)GetValue(DragSpeedProperty); }
            set { SetValue(DragSpeedProperty, value); }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericUpDown" /> class.
        /// </summary>
        public NumericUpDown()
        {
            this.DefaultStyleKey = typeof(NumericUpDown);
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_dragOverlay != null)
            {
                Window.Current.CoreWindow.PointerReleased += CoreWindowOnPointerReleased;
                Window.Current.CoreWindow.VisibilityChanged += OnCoreWindowVisibilityChanged;
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerReleased -= CoreWindowOnPointerReleased;
            Window.Current.CoreWindow.VisibilityChanged -= OnCoreWindowVisibilityChanged;
        }

        #region OnApplyTemplate()
        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            this.GotFocus += OnGotFocus;
            this.LostFocus += OnLostFocus;
            this.PointerWheelChanged += OnPointerWheelChanged;
            _valueTextBox = GetTemplateChild(ValueTextBoxName) as UpDownTextBox;
            _dragOverlay = GetTemplateChild(DragOverlayName) as UIElement;
            _decrementButton = GetTemplateChild(DecrementButtonName) as RepeatButton;
            _incrementButton = GetTemplateChild(IncrementButtonName) as RepeatButton;
            _valueBar = GetTemplateChild(ValueBarName) as FrameworkElement;
            
            if (_valueTextBox != null)
            {
                _valueTextBox.LostFocus += OnValueTextBoxLostFocus;
                _valueTextBox.GotFocus += OnValueTextBoxGotFocus;
                _valueTextBox.Text = Value.ToString();
                _valueTextBox.TextChanged += OnValueTextBoxTextChanged;
                _valueTextBox.KeyDown += OnValueTextBoxKeyDown;
                _valueTextBox.UpPressed += (s, e) => this.Increment();
                _valueTextBox.DownPressed += (s, e) => this.Decrement();
                _valueTextBox.PointerExited += OnValueTextBoxPointerExited;
            }

            if (_dragOverlay != null)
            {
                _dragOverlay.Tapped += OnDragOverlayTapped;
                _dragOverlay.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
                _dragOverlay.PointerPressed += OnDragOverlayPointerPressed;
                _dragOverlay.PointerReleased += OnDragOverlayPointerReleased;
                _dragOverlay.PointerCaptureLost += OnDragOverlayPointerCaptureLost;
            }

            if (_decrementButton != null)
            {
                _decrementButton.Click += OnDecrementButtonClick;
                var pcc =
                    new PropertyChangeEventSource<bool>
                        (_decrementButton, "IsPressed");
                pcc.ValueChanged += OnDecrementButtonIsPressedChanged;
            }

            if (_incrementButton != null)
            {
                _incrementButton.Click += OnIncrementButtonClick;
                var pcc =
                    new PropertyChangeEventSource<bool>
                        (_incrementButton, "IsPressed");
                pcc.ValueChanged += OnIncrementButtonIsPressedChanged;
            }

            if (_valueBar != null)
            {
                _valueBar.SizeChanged += OnValueBarSizeChanged;

                UpdateValueBar();
            }

            UpdateIsReadOnlyDependants();
            SetValidIncrementDirection();
        }

        private void OnPointerWheelChanged(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            if (!_hasFocus)
            {
                return;
            }

            var delta = pointerRoutedEventArgs.GetCurrentPoint(this).Properties.MouseWheelDelta;

            if (delta < 0)
            {
                this.Decrement();
            }
            else
            {
                this.Increment();
            }

            pointerRoutedEventArgs.Handled = true;
        }

        private bool _hasFocus;

        private void OnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            _hasFocus = false;
        }

        private void OnGotFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            _hasFocus = true;
        }

        private void OnValueTextBoxTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            UpdateValueFromText();
        }

        private void OnValueTextBoxKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                if (UpdateValueFromText())
                {
                    UpdateValueText();
                    _valueTextBox.SelectAll();
                    e.Handled = true;
                }
            }
        }

        private bool UpdateValueFromText()
        {
            if (_isChangingTextWithCode)
            {
                return false;
            }

            double val;

            if (double.TryParse(_valueTextBox.Text, NumberStyles.Any, CultureInfo.CurrentUICulture, out val) ||
                Calculator.TryCalculate(_valueTextBox.Text, out val))
            {
                _isChangingValueWithCode = true;
                SetValueAndUpdateValidDirections(val);
                _isChangingValueWithCode = false;

                return true;
            }

            return false;
        }

        #endregion

        #region Button event handlers
        private void OnDecrementButtonIsPressedChanged(object decrementButton, bool isPressed)
        {
            // TODO: The thinking was to handle speed and acceleration of value changes manually on a regular Button when it is pressed.
            // Currently just using RepeatButtons
        }

        private void OnIncrementButtonIsPressedChanged(object incrementButton, bool isPressed)
        {
        }

        private void OnDecrementButtonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            this.Decrement();
        }

        private void OnIncrementButtonClick(object sender, RoutedEventArgs routedEventArgs)
        {
            this.Increment();
        }
        #endregion

        private bool Decrement()
        {
            return SetValueAndUpdateValidDirections(this.Value - this.SmallChange);
        }

        private bool Increment()
        {
            return SetValueAndUpdateValidDirections(this.Value + this.SmallChange);
        }

        private void OnValueTextBoxGotFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_dragOverlay != null)
            {
                _dragOverlay.IsHitTestVisible = false;
            }

            _valueTextBox.SelectAll();
        }

        private void OnValueTextBoxLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_dragOverlay != null)
            {
                _dragOverlay.IsHitTestVisible = true;
                UpdateValueText();
            }
        }

        private void OnDragOverlayPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            _dragOverlay.CapturePointer(e.Pointer);

            _totalDeltaX = 0;
            _totalDeltaY = 0;
            
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
            {
                _isDraggingWithMouse = true;
                _mouseDevice = MouseDevice.GetForCurrentView();
                _mouseDevice.MouseMoved += OnMouseDragged;
#if WINDOWS_APP
                Window.Current.CoreWindow.PointerCursor = null;
#endif
            }
            else
            {
                _dragOverlay.ManipulationDelta += OnDragOverlayManipulationDelta;
            }
        }

        private async void CoreWindowOnPointerReleased(CoreWindow sender, PointerEventArgs args)
        {
            if (_isDragUpdated)
            {
                args.Handled = true;
                await Task.Delay(100);

                if (_valueTextBox != null)
                {
                    _valueTextBox.IsTabStop = true;
                }
            }
        }

        private async void OnDragOverlayPointerReleased(object sender, PointerRoutedEventArgs args)
        {
            await EndDragging(args);
        }

        private async void OnDragOverlayPointerCaptureLost(object sender, PointerRoutedEventArgs args)
        {
            await EndDragging(args);
        }

        private async Task EndDragging(PointerRoutedEventArgs args)
        {
            if (_isDraggingWithMouse)
            {
                _isDraggingWithMouse = false;
                _mouseDevice.MouseMoved -= OnMouseDragged;
#if WINDOWS_APP
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.SizeAll, 1);
#endif
                _mouseDevice = null;
            }
            else if (_dragOverlay != null)
            {
                _dragOverlay.ManipulationDelta -= OnDragOverlayManipulationDelta;
            }

            if (_isDragUpdated)
            {
                if (args != null)
                {
                    args.Handled = true;
                }

                await Task.Delay(100);

                if (_valueTextBox != null)
                {
                    _valueTextBox.IsTabStop = true;
                }
            }
        }

        private void OnCoreWindowVisibilityChanged(CoreWindow sender, VisibilityChangedEventArgs args)
        {
            // There are cases where pointer isn't getting released - this should hopefully end dragging too.
            if (!args.Visible)
            {
#pragma warning disable 4014
                this.EndDragging(null);
#pragma warning restore 4014
            }
        }

        private void OnMouseDragged(MouseDevice sender, MouseEventArgs args)
        {
            var dx = args.MouseDelta.X;
            var dy = args.MouseDelta.Y;

            if (dx > 200 || dx < -200 || dy > 200 || dy < -200)
            {
                return;
            }

            _totalDeltaX += dx;
            _totalDeltaY += dy;

            if (_totalDeltaX > MinMouseDragDelta ||
                _totalDeltaX < -MinMouseDragDelta ||
                _totalDeltaY > MinMouseDragDelta ||
                _totalDeltaY < -MinMouseDragDelta)
            {
                UpdateByDragging(_totalDeltaX, _totalDeltaY);
                _totalDeltaX = 0;
                _totalDeltaY = 0;
            }
        }

        private void OnDragOverlayManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs manipulationDeltaRoutedEventArgs)
        {
            var dx = manipulationDeltaRoutedEventArgs.Delta.Translation.X;
            var dy = manipulationDeltaRoutedEventArgs.Delta.Translation.Y;

            if (UpdateByDragging(dx, dy))
                return;

            manipulationDeltaRoutedEventArgs.Handled = true;
        }

        private bool UpdateByDragging(double dx, double dy)
        {
            if (!this.IsEnabled ||
                this.IsReadOnly ||
// ReSharper disable CompareOfFloatsByEqualityOperator
                dx == 0 && dy == 0)
// ReSharper restore CompareOfFloatsByEqualityOperator
            {
                return false;
            }

            double delta;

            if (Math.Abs(dx) > Math.Abs(dy))
            {
                delta = dx;
            }
            else
            {
                delta = -dy;
            }

            ApplyManipulationDelta(delta);

            if (_valueTextBox != null)
            {
                _valueTextBox.IsTabStop = false;
            }

            _isDragUpdated = true;

            return true;
        }

        private void ApplyManipulationDelta(double delta)
        {
            if (Math.Sign(delta) == Math.Sign(_unusedManipulationDelta))
                _unusedManipulationDelta += delta;
            else
                _unusedManipulationDelta = delta;

            if (_unusedManipulationDelta <= 0 && this.Value == this.Minimum)
            {
                _unusedManipulationDelta = 0;
                return;
            }

            if (_unusedManipulationDelta >= 0 && this.Value == this.Maximum)
            {
                _unusedManipulationDelta = 0;
                return;
            }

            double smallerScreenDimension;

            if (Window.Current != null)
            {
                smallerScreenDimension = Math.Min(Window.Current.Bounds.Width, Window.Current.Bounds.Height);
            }
            else
            {
                smallerScreenDimension = 768;
            }

            var speed = this.DragSpeed;

            if (double.IsNaN(speed) ||
                double.IsInfinity(speed))
            {
                speed = this.Maximum - this.Minimum;
            }

            if (double.IsNaN(speed) ||
                double.IsInfinity(speed))
            {
                speed = double.MaxValue;
            }

            var screenAdjustedDelta = speed * _unusedManipulationDelta / smallerScreenDimension;
            SetValueAndUpdateValidDirections(this.Value + screenAdjustedDelta);
            _unusedManipulationDelta = 0;
        }

        private void OnDragOverlayTapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            if (this.IsEnabled &&
                _valueTextBox != null &&
                _valueTextBox.IsTabStop)
            {
                _valueTextBox.Focus(FocusState.Programmatic);
#if WINDOWS_APP
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.IBeam, 0);
#endif
            }
        }
        private void OnValueTextBoxPointerExited(object sender, PointerRoutedEventArgs e)
        {
#if WINDOWS_APP
            if (Window.Current.CoreWindow.PointerCursor.Type == CoreCursorType.IBeam)
            {
                Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
            }
#endif
        }

        /// <summary>
        /// Fires the ValueChanged routed event.
        /// </summary>
        /// <param name="oldValue">Old value of the Value property.</param>
        /// <param name="newValue">New value of the Value property.</param>
        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);

            UpdateValueBar();

            if (!_isChangingValueWithCode)
            {
                UpdateValueText();
            }
        }

        private void UpdateValueBar()
        {
            if (_valueBar == null)
                return;

            var effectiveValueBarVisibility = ValueBarVisibility;

            if (effectiveValueBarVisibility == NumericUpDownValueBarVisibility.Collapsed)
            {
                _valueBar.Visibility = Visibility.Collapsed;

                return;
            }

            _valueBar.Clip =
                new RectangleGeometry
                {
                    Rect = new Rect
                    {
                        X = 0,
                        Y = 0,
                        Height = _valueBar.ActualHeight,
                        Width = _valueBar.ActualWidth * (Value - Minimum) / (Maximum - Minimum)
                    }
                };

            //_valueBar.Width =
            //    _valueTextBox.ActualWidth * (Value - Minimum) / (Maximum - Minimum);
        }

        private void OnValueBarSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            UpdateValueBar();
        }

        private void UpdateValueText()
        {
            if (_valueTextBox != null)
            {
                _isChangingTextWithCode = true;
                _valueTextBox.Text = Value.ToString(ValueFormat);
                _isChangingTextWithCode = false;
            }
        }

        private void UpdateIsReadOnlyDependants()
        {
            if (_decrementButton != null)
            {
                _decrementButton.Visibility = IsReadOnly ? Visibility.Collapsed : Visibility.Visible;
            }

            if (_incrementButton != null)
            {
                _incrementButton.Visibility = IsReadOnly ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private bool SetValueAndUpdateValidDirections(double value)
        {
            // Range coercion is handled by base class.
            var oldValue = this.Value;
            this.Value = value;
            this.SetValidIncrementDirection();

            return this.Value != oldValue;
        }

        private void SetValidIncrementDirection()
        {
            if (this.Value < this.Maximum)
            {
                VisualStateManager.GoToState(this, "IncrementEnabled", true);
            }
            if (this.Value > this.Minimum)
            {
                VisualStateManager.GoToState(this, "DecrementEnabled", true);
            }
            if (this.Value == this.Maximum)
            {
                VisualStateManager.GoToState(this, "IncrementDisabled", true);
            }

            if (this.Value == this.Minimum)
            {
                VisualStateManager.GoToState(this, "DecrementDisabled", true);
            }
        }
    }

    //public sealed class NumericUpDownAccelerationCollection : List<NumericUpDownAcceleration>
    //{
    //    //TODO: Consider adding this
    //}

    //public sealed class NumericUpDownAcceleration : DependencyObject
    //{
    //    #region Seconds
    //    /// <summary>
    //    /// Seconds Dependency Property
    //    /// </summary>
    //    public static readonly DependencyProperty SecondsProperty =
    //        DependencyProperty.Register(
    //            "Seconds",
    //            typeof(int),
    //            typeof(NumericUpDownAcceleration),
    //            new PropertyMetadata(0));

    //    /// <summary>
    //    /// Gets or sets the Seconds property. This dependency property 
    //    /// indicates the number of seconds before the acceleration takes place.
    //    /// </summary>
    //    public int Seconds
    //    {
    //        get { return (int)GetValue(SecondsProperty); }
    //        set { SetValue(SecondsProperty, value); }
    //    }
    //    #endregion

    //    #region Increment
    //    /// <summary>
    //    /// Increment Dependency Property
    //    /// </summary>
    //    public static readonly DependencyProperty IncrementProperty =
    //        DependencyProperty.Register(
    //            "Increment",
    //            typeof(double),
    //            typeof(NumericUpDownAcceleration),
    //            new PropertyMetadata(0));

    //    /// <summary>
    //    /// Gets or sets the Increment property. This dependency property 
    //    /// indicates the increment that takes place every button click
    //    /// after the button has been pressed for the duration specified
    //    /// by the Seconds property.
    //    /// </summary>
    //    public double Increment
    //    {
    //        get { return (double)GetValue(IncrementProperty); }
    //        set { SetValue(IncrementProperty, value); }
    //    }
    //    #endregion
    //}
}
