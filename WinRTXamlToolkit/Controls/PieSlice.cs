using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// A path that represents a pie slice with a given
    /// Radius,
    /// StartAngle and
    /// EndAngle.
    /// </summary>
    public class PieSlice : Path
    {
        private bool _isUpdating;

        #region StartAngle
        /// <summary>
        /// The start angle property
        /// </summary>
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register(
                "StartAngle",
                typeof(double),
                typeof(PieSlice),
                new PropertyMetadata(
                    0d,
                    OnStartAngleChanged));

        /// <summary>
        /// Gets or sets the start angle.
        /// </summary>
        /// <value>
        /// The start angle.
        /// </value>
        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        private static void OnStartAngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (PieSlice)sender;
            var oldStartAngle = (double)e.OldValue;
            var newStartAngle = (double)e.NewValue;
            target.OnStartAngleChanged(oldStartAngle, newStartAngle);
        }

        private void OnStartAngleChanged(double oldStartAngle, double newStartAngle)
        {
            UpdatePath();
        }
        #endregion

        #region EndAngle
        /// <summary>
        /// The end angle property.
        /// </summary>
        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register(
                "EndAngle",
                typeof(double),
                typeof(PieSlice),
                new PropertyMetadata(
                    0d,
                    OnEndAngleChanged));

        /// <summary>
        /// Gets or sets the end angle.
        /// </summary>
        /// <value>
        /// The end angle.
        /// </value>
        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        private static void OnEndAngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (PieSlice)sender;
            var oldEndAngle = (double)e.OldValue;
            var newEndAngle = (double)e.NewValue;
            target.OnEndAngleChanged(oldEndAngle, newEndAngle);
        }

        private void OnEndAngleChanged(double oldEndAngle, double newEndAngle)
        {
            UpdatePath();
        }
        #endregion

        #region Radius
        /// <summary>
        /// The radius property.
        /// </summary>
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(
                "Radius",
                typeof(double),
                typeof(PieSlice),
                new PropertyMetadata(
                    0d,
                    new PropertyChangedCallback(OnRadiusChanged)));

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        private static void OnRadiusChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (PieSlice)sender;
            var oldRadius = (double)e.OldValue;
            var newRadius = (double)e.NewValue;
            target.OnRadiusChanged(oldRadius, newRadius);
        }

        private void OnRadiusChanged(double oldRadius, double newRadius)
        {
            this.Width = this.Height = 2 * Radius;
            UpdatePath();
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PieSlice" /> class.
        /// </summary>
        public PieSlice()
        {
            this.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdatePath();
        }

        /// <summary>
        /// Suspends path updates until EndUpdate is called;
        /// </summary>
        public void BeginUpdate()
        {
            _isUpdating = true;
        }

        /// <summary>
        /// Resumes immediate path updates every time a component property value changes. Updates the path.
        /// </summary>
        public void EndUpdate()
        {
            _isUpdating = false;
            UpdatePath();
        }

        private void UpdatePath()
        {
            if (_isUpdating)
            {
                return;
            }

            var pathGeometry = new PathGeometry();
            var pathFigure = new PathFigure();
            pathFigure.StartPoint = new Point(Radius, Radius);
            pathFigure.IsClosed = true;

            // Starting Point
            var lineSegment = 
                new LineSegment 
                {
                    Point = new Point(
                        Radius + Math.Sin(StartAngle * Math.PI / 180) * Radius,
                        Radius - Math.Cos(StartAngle * Math.PI / 180) * Radius)
                };

            // Arc
            var arcSegment = new ArcSegment();
            arcSegment.IsLargeArc = (EndAngle - StartAngle) >= 180.0;
            arcSegment.Point =
                new Point(
                        Radius + Math.Sin(EndAngle * Math.PI / 180) * Radius,
                        Radius - Math.Cos(EndAngle * Math.PI / 180) * Radius);
            arcSegment.Size = new Size(Radius, Radius);
            arcSegment.SweepDirection = SweepDirection.Clockwise;
            pathFigure.Segments.Add(lineSegment);
            pathFigure.Segments.Add(arcSegment);
            pathGeometry.Figures.Add(pathFigure);
            this.Data = pathGeometry;
            this.InvalidateArrange();
        }
    }
}