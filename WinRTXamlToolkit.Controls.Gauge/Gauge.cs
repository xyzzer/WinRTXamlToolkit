using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// A Modern UI Radial Gauge.
    /// </summary>
    [TemplatePart(Name = NeedlePartName, Type = typeof(Path))]
    [TemplatePart(Name = ScalePartName, Type = typeof(Path))]
    [TemplatePart(Name = TrailPartName, Type = typeof(Path))]
    [TemplatePart(Name = ValueTextPartName, Type = typeof(TextBlock))]
    public class Gauge : Control
    {
        #region Constants
        private const string NeedlePartName = "PART_Needle";
        private const string ScalePartName = "PART_Scale";
        private const string TrailPartName = "PART_Trail";
        private const string ValueTextPartName = "PART_ValueText";
        private const double Degrees2Radians = Math.PI / 180;
        #endregion Constants

        #region Dependency Property Registrations
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                "Minimum",
                typeof (double),
                typeof (Gauge),
                new PropertyMetadata(0.0));

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                "Maximum",
                typeof (double),
                typeof (Gauge),
                new PropertyMetadata(100.0));

        public static readonly DependencyProperty ScaleWidthProperty =
            DependencyProperty.Register(
                "ScaleWidth",
                typeof (Double),
                typeof (Gauge),
                new PropertyMetadata(26.0));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof (double),
                typeof (Gauge),
                new PropertyMetadata(0.0, OnValueChanged));

        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register(
                "Unit",
                typeof (string),
                typeof (Gauge),
                new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty NeedleBrushProperty =
            DependencyProperty.Register(
                "NeedleBrush",
                typeof (Brush),
                typeof (Gauge),
                new PropertyMetadata(new SolidColorBrush(Colors.OrangeRed)));

        public static readonly DependencyProperty ScaleBrushProperty =
            DependencyProperty.Register(
                "ScaleBrush",
                typeof (Brush),
                typeof (Gauge),
                new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));

        public static readonly DependencyProperty TickBrushProperty =
            DependencyProperty.Register(
                "TickBrush",
                typeof (Brush),
                typeof (Gauge),
                new PropertyMetadata(new SolidColorBrush(Colors.DimGray)));

        public static readonly DependencyProperty TrailBrushProperty =
            DependencyProperty.Register(
                "TrailBrush",
                typeof (Brush),
                typeof (Gauge),
                new PropertyMetadata(new SolidColorBrush(Colors.Orange)));

        public static readonly DependencyProperty ValueBrushProperty =
            DependencyProperty.Register(
                "ValueBrush",
                typeof (Brush),
                typeof (Gauge),
                new PropertyMetadata(new SolidColorBrush(Colors.DimGray)));

        public static readonly DependencyProperty ScaleTickBrushProperty =
            DependencyProperty.Register(
                "ScaleTickBrush",
                typeof (Brush),
                typeof (Gauge),
                new PropertyMetadata(new SolidColorBrush(Colors.White)));

        public static readonly DependencyProperty UnitBrushProperty =
            DependencyProperty.Register(
                "UnitBrush",
                typeof (Brush),
                typeof (Gauge),
                new PropertyMetadata(new SolidColorBrush(Colors.DimGray)));

        public static readonly DependencyProperty ValueStringFormatProperty =
            DependencyProperty.Register(
                "ValueStringFormat",
                typeof (string),
                typeof (Gauge),
                new PropertyMetadata("N0"));

        protected static readonly DependencyProperty ValueAngleProperty =
            DependencyProperty.Register(
                "ValueAngle",
                typeof (double),
                typeof (Gauge),
                new PropertyMetadata(null));

        protected static readonly DependencyProperty TicksProperty =
            DependencyProperty.Register(
                "Ticks",
                typeof (IEnumerable<double>),
                typeof (Gauge),
                new PropertyMetadata(null));
        #endregion Dependency Property Registrations

        #region Constructors
        public Gauge()
        {
            this.DefaultStyleKey = typeof(Gauge);
            this.Ticks = this.GetTicks();
        }
        #endregion Constructors

        #region Properties
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public Double ScaleWidth
        {
            get { return (Double)GetValue(ScaleWidthProperty); }
            set { SetValue(ScaleWidthProperty, value); }
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public string Unit
        {
            get { return (string)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }

        public Brush NeedleBrush
        {
            get { return (Brush)GetValue(NeedleBrushProperty); }
            set { SetValue(NeedleBrushProperty, value); }
        }

        public Brush TrailBrush
        {
            get { return (Brush)GetValue(TrailBrushProperty); }
            set { SetValue(TrailBrushProperty, value); }
        }

        public Brush ScaleBrush
        {
            get { return (Brush)GetValue(ScaleBrushProperty); }
            set { SetValue(ScaleBrushProperty, value); }
        }

        public Brush ScaleTickBrush
        {
            get { return (Brush)GetValue(ScaleTickBrushProperty); }
            set { SetValue(ScaleTickBrushProperty, value); }
        }

        public Brush TickBrush
        {
            get { return (Brush)GetValue(TickBrushProperty); }
            set { SetValue(TickBrushProperty, value); }
        }

        public Brush ValueBrush
        {
            get { return (Brush)GetValue(ValueBrushProperty); }
            set { SetValue(ValueBrushProperty, value); }
        }

        public Brush UnitBrush
        {
            get { return (Brush)GetValue(UnitBrushProperty); }
            set { SetValue(UnitBrushProperty, value); }
        }

        public string ValueStringFormat
        {
            get { return (string)GetValue(ValueStringFormatProperty); }
            set { SetValue(ValueStringFormatProperty, value); }
        }

        protected double ValueAngle
        {
            get { return (double)GetValue(ValueAngleProperty); }
            set { SetValue(ValueAngleProperty, value); }
        }

        protected IEnumerable<double> Ticks
        {
            get { return (IEnumerable<double>)GetValue(TicksProperty); }
            set { SetValue(TicksProperty, value); }
        }
        #endregion Properties

        protected override void OnApplyTemplate()
        {
            // Draw Scale
            var scale = this.GetTemplateChild(ScalePartName) as Path;

            if (scale != null)
            {
                var pg = new PathGeometry();
                var pf = new PathFigure();
                pf.IsClosed = false;
                var middleOfScale = 77 - this.ScaleWidth / 2;
                pf.StartPoint = ScalePoint(-150, middleOfScale);
                var seg = new ArcSegment();
                seg.SweepDirection = SweepDirection.Clockwise;
                seg.IsLargeArc = true;
                seg.Size = new Size(middleOfScale, middleOfScale);
                seg.Point = ScalePoint(150, middleOfScale);
                pf.Segments.Add(seg);
                pg.Figures.Add(pf);
                scale.Data = pg;
            }

            OnValueChanged(this, null);
            base.OnApplyTemplate();
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = (Gauge)d;

            if (Double.IsNaN(c.Value))
                return;

            var middleOfScale = 77 - c.ScaleWidth / 2;
            var needle = c.GetTemplateChild(NeedlePartName) as Path;
            var valueText = c.GetTemplateChild(ValueTextPartName) as TextBlock;
            c.ValueAngle = c.ValueToAngle(c.Value);

            // Needle
            if (needle != null)
            {
                needle.RenderTransform = new RotateTransform() { Angle = c.ValueAngle };
            }

            // Trail
            var trail = c.GetTemplateChild(TrailPartName) as Path;

            if (trail != null)
            {
                if (c.ValueAngle > -146)
                {
                    trail.Visibility = Visibility.Visible;
                    var pg = new PathGeometry();
                    var pf = new PathFigure();
                    pf.IsClosed = false;
                    pf.StartPoint = ScalePoint(-150, middleOfScale);
                    var seg = new ArcSegment();
                    seg.SweepDirection = SweepDirection.Clockwise;
                    // We start from -150, so +30 becomes a large arc.
                    seg.IsLargeArc = c.ValueAngle > 30;
                    seg.Size = new Size(middleOfScale, middleOfScale);
                    seg.Point = ScalePoint(c.ValueAngle, middleOfScale);
                    pf.Segments.Add(seg);
                    pg.Figures.Add(pf);
                    trail.Data = pg;
                }
                else
                {
                    trail.Visibility = Visibility.Collapsed;
                }
            }

            // Value Text
            if (valueText != null)
            {
                valueText.Text = c.Value.ToString(c.ValueStringFormat);
            }
        }

        private static Point ScalePoint(double angle, double middleOfScale)
        {
            return new Point(100 + Math.Sin(Degrees2Radians * angle) * middleOfScale, 100 - Math.Cos(Degrees2Radians * angle) * middleOfScale);
        }

        private double ValueToAngle(double value)
        {
            const double minAngle = -150;
            const double maxAngle = 150;

            // Off-scale to the left
            if (value < this.Minimum)
            {
                return minAngle - 7.5;
            }

            // Off-scale to the right
            if (value > this.Maximum)
            {
                return maxAngle + 7.5;
            }

            double angularRange = maxAngle - minAngle;

            return (value - this.Minimum) / (this.Maximum - this.Minimum) * angularRange + minAngle;
        }

        private IEnumerable<double> GetTicks()
        {
            var tickSpacing = (this.Maximum - this.Minimum) / 10;

            for (double tick = this.Minimum; tick <= this.Maximum; tick += tickSpacing)
            {
                yield return ValueToAngle(tick);
            }
        }
    }
}