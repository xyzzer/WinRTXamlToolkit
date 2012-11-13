using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// Triangle shaped picker control.
    /// </summary>
    [TemplatePart(Name = SelectionCanvasName, Type = typeof(Canvas))]
    [TemplatePart(Name = TouchTargetTriangleName, Type = typeof(Path))]
    [TemplatePart(Name = ThumbName, Type = typeof(Ellipse))]
    public class TrianglePicker : Control
    {
        /// <summary>
        /// Occurs when the value changes.
        /// </summary>
        public event EventHandler ValueChanged;

        #region X
        /// <summary>
        /// X Dependency Property
        /// </summary>
        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register(
                "X",
                typeof(double),
                typeof(TrianglePicker),
                new PropertyMetadata(0d, OnXChanged));

        /// <summary>
        /// Gets or sets the X property. This dependency property 
        /// indicates the X coordinate of the selected point in the triangle.
        /// </summary>
        public double X
        {
            get { return (double)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        /// <summary>
        /// Handles changes to the X property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnXChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (TrianglePicker)d;
            double oldX = (double)e.OldValue;
            double newX = target.X;
            target.OnXChanged(oldX, newX);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the X property.
        /// </summary>
        /// <param name="oldX">The old X value</param>
        /// <param name="newX">The new X value</param>
        private void OnXChanged(
            double oldX, double newX)
        {
            UpdateThumbPosition();
        }
        #endregion

        #region Y
        /// <summary>
        /// Y Dependency Property
        /// </summary>
        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register(
                "Y",
                typeof(double),
                typeof(TrianglePicker),
                new PropertyMetadata(0d, OnYChanged));

        /// <summary>
        /// Gets or sets the Y property. This dependency property 
        /// indicates the Y coordinate of the selected point in the triangle.
        /// </summary>
        public double Y
        {
            get { return (double)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Y property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnYChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (TrianglePicker)d;
            double oldY = (double)e.OldValue;
            double newY = target.Y;
            target.OnYChanged(oldY, newY);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Y property.
        /// </summary>
        /// <param name="oldY">The old Y value</param>
        /// <param name="newY">The new Y value</param>
        private void OnYChanged(
            double oldY, double newY)
        {
            UpdateThumbPosition();
        }
        #endregion

        private const string SelectionCanvasName = "PART_SelectionCanvas";
        private const string TouchTargetTriangleName = "PART_TouchTargetTriangle";
        private const string ThumbName = "PART_Thumb";
        private Canvas _selectionCanvas;
        private Path _touchTargetTriangle;
        private Ellipse _thumb;

        public TrianglePicker()
        {
            this.DefaultStyleKey = typeof (TrianglePicker);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _selectionCanvas = (Canvas)GetTemplateChild(SelectionCanvasName);
            _touchTargetTriangle = (Path)GetTemplateChild(TouchTargetTriangleName);
            _thumb = (Ellipse)GetTemplateChild(ThumbName);
            _selectionCanvas.SizeChanged += OnSomeSizeChanged;

            _thumb.SizeChanged += OnSomeSizeChanged;
            _touchTargetTriangle.PointerPressed += OnTouchTargetPointerPressed;
            _touchTargetTriangle.PointerMoved += OnTouchTargetPointerMoved;

            UpdateThumbPosition();
        }

        private void OnTouchTargetPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = true;
            _touchTargetTriangle.CapturePointer(e.Pointer);
            UpdateSelection(e.GetCurrentPoint(_touchTargetTriangle).Position);
        }

        private void OnTouchTargetPointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!e.Pointer.IsInContact)
            {
                return;
            }

            e.Handled = true;
            UpdateSelection(e.GetCurrentPoint(_touchTargetTriangle).Position);
        }

        private void UpdateSelection(Point position)
        {
            var tw = _touchTargetTriangle.ActualWidth;
            var th = _touchTargetTriangle.ActualHeight;

            // 0..1, 0..1 relative coordinates
            var rx = position.X / tw;
            var ry = 1 - position.Y / th;

            // 0..tw, 0..th triangle's coordinate space values
            var x = position.X;
            var y = th - position.Y;

            if (ry > 0 &&
                ry < 2 * rx &&
                ry < 2 - 2 * rx)
            {
                X = rx;
                Y = ry;
                //Debug.WriteLine("In triangle (x: {0}, y: {1})", rx, ry);
            }
            else if (
                x < 0 &&
                y < -x * 0.5 / Math.Sqrt(0.75))
            {
                X = 0;
                Y = 0;
                //Debug.WriteLine("Left of bottom left vertice (x: {0}, y: {1})", rx, ry);
            }
            else if (
                x > tw &&
                y < (x - tw) * 0.5 / Math.Sqrt(0.75))
            {
                X = 1;
                Y = 0;
                //Debug.WriteLine("Right of bottom right vertice (x: {0}, y: {1})", rx, ry);
            }
            else if (
                y - th > -(x - 0.5 * tw) * 0.5 / Math.Sqrt(0.75) &&
                y - th > (x - 0.5 * tw) * 0.5 / Math.Sqrt(0.75))
            {
                X = 0.5;
                Y = 1;
                //Debug.WriteLine("Above the top vertice (x: {0}, y: {1})", rx, ry);
            }
            else if (ry < 0)
            {
                X = rx;
                Y = 0;
                //Debug.WriteLine("Below the triangle (x: {0}, y: {1})", rx, ry);
            }
            else if (rx < 0.5)
            {
                var a = 2 * Math.Sqrt(0.75);
                var b = 0.5 / Math.Sqrt(0.75);
                var c = y + x * 0.5 / Math.Sqrt(0.75);
                X = Math.Min(0.5, Math.Max(0, (c / (a + b)) / tw));
                Y = X * a * tw / th; 
                
                //Debug.WriteLine("Left of triangle (x: {0}, y: {1})", rx, ry);
            }
            else
            {
                var a = 2 * Math.Sqrt(0.75);
                var b = 0.5 / Math.Sqrt(0.75);
                var c = y - (x - tw) * 0.5 / Math.Sqrt(0.75);
                var x2 = Math.Min(0.5, Math.Max(0, (c / (a + b)) / tw));
                X = 1 - x2;
                Y = x2 * a * tw / th;
                //Debug.WriteLine("Right of triangle (x: {0}, y: {1})", rx, ry);
            }

            var handler = ValueChanged;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void OnSomeSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            UpdateThumbPosition();
        }

        private void UpdateThumbPosition()
        {
            Canvas.SetLeft(_thumb, - _thumb.ActualWidth * 0.5 + X * _selectionCanvas.ActualWidth);
            Canvas.SetTop(_thumb, - _thumb.ActualHeight * 0.5 + (1 - Y) * _selectionCanvas.ActualHeight);
        }
    }
}
