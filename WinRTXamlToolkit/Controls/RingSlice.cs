using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace WinRTXamlToolkit.Controls
{
    public class RingSlice : Path
    {
        private bool _isUpdating;

        #region StartAngle
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register(
                "StartAngle",
                typeof(double),
                typeof(RingSlice),
                new PropertyMetadata(
                    0d,
                    new PropertyChangedCallback(OnStartAngleChanged)));

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        private static void OnStartAngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (RingSlice)sender;
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
        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register(
                "EndAngle",
                typeof(double),
                typeof(RingSlice),
                new PropertyMetadata(
                    0d,
                    new PropertyChangedCallback(OnEndAngleChanged)));

        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        private static void OnEndAngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (RingSlice)sender;
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
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(
                "Radius",
                typeof(double),
                typeof(RingSlice),
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
            var target = (RingSlice)sender;
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

        #region InnerRadius
        public static readonly DependencyProperty InnerRadiusProperty =
            DependencyProperty.Register(
                "InnerRadius",
                typeof(double),
                typeof(RingSlice),
                new PropertyMetadata(
                    0d,
                    new PropertyChangedCallback(OnInnerRadiusChanged)));

        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        private static void OnInnerRadiusChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (RingSlice)sender;
            var oldInnerRadius = (double)e.OldValue;
            var newInnerRadius = (double)e.NewValue;
            target.OnInnerRadiusChanged(oldInnerRadius, newInnerRadius);
        }

        private void OnInnerRadiusChanged(double oldInnerRadius, double newInnerRadius)
        {
            UpdatePath();
        }
        #endregion

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
            pathFigure.IsClosed = true;

            // Starting Point
            pathFigure.StartPoint =
                new Point(
                        Radius + Math.Sin(StartAngle * Math.PI / 180) * InnerRadius,
                        Radius - Math.Cos(StartAngle * Math.PI / 180) * InnerRadius);

            // Inner Arc
            var innerArcSegment = new ArcSegment();
            innerArcSegment.IsLargeArc = (EndAngle - StartAngle) >= 180.0;
            innerArcSegment.Point =
                new Point(
                        Radius + Math.Sin(EndAngle * Math.PI / 180) * InnerRadius,
                        Radius - Math.Cos(EndAngle * Math.PI / 180) * InnerRadius);
            innerArcSegment.Size = new Size(InnerRadius, InnerRadius);
            innerArcSegment.SweepDirection = SweepDirection.Clockwise;

            var lineSegment =
                new LineSegment
                {
                    Point = new Point(
                        Radius + Math.Sin(EndAngle * Math.PI / 180) * Radius,
                        Radius - Math.Cos(EndAngle * Math.PI / 180) * Radius)
                };

            // Outer Arc
            var outerArcSegment = new ArcSegment();
            outerArcSegment.IsLargeArc = (EndAngle - StartAngle) >= 180.0;
            outerArcSegment.Point =
                new Point(
                        Radius + Math.Sin(StartAngle * Math.PI / 180) * Radius,
                        Radius - Math.Cos(StartAngle * Math.PI / 180) * Radius);
            outerArcSegment.Size = new Size(Radius, Radius);
            outerArcSegment.SweepDirection = SweepDirection.Counterclockwise;

            pathFigure.Segments.Add(innerArcSegment);
            pathFigure.Segments.Add(lineSegment);
            pathFigure.Segments.Add(outerArcSegment);
            pathGeometry.Figures.Add(pathFigure);
            this.InvalidateArrange();
            this.Data = pathGeometry;
        }
    }
}