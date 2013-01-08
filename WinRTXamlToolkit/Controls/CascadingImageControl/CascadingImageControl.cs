using System;
using System.Collections.Generic;
using System.Diagnostics;
using WinRTXamlToolkit.Tools;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace WinRTXamlToolkit.Controls
{
    public enum CascadeDirection
    {
        TopLeft = 0,
        //Top,
        TopRight,
        //Right,
        BottomRight,
        //Bottom,
        BottomLeft,
        //Left,
        Random,
        Shuffle
    }

    public enum CascadeSequence
    {
        /// <summary>
        /// The tile cascade animations end at the same time
        /// </summary>
        EndTogether,
        /// <summary>
        /// The tile cascade animations are equal duration
        /// </summary>
        EqualDuration
    }

    [TemplatePart(Name = LayoutGridName, Type = typeof(Grid))]
    public sealed class CascadingImageControl : Control
    {
        private const string LayoutGridName = "PART_LayoutGrid";

        private Grid _layoutGrid;

        private bool _isLoaded;

        private static readonly Random Random = new Random();

        #region Columns
        /// <summary>
        /// Columns Dependency Property
        /// </summary>
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register(
                "Columns",
                typeof(int),
                typeof(CascadingImageControl),
                new PropertyMetadata(3, OnColumnsChanged));

        /// <summary>
        /// Gets or sets the Columns property. This dependency property 
        /// indicates the number of columns.
        /// </summary>
        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Columns property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnColumnsChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (CascadingImageControl)d;
            int oldColumns = (int)e.OldValue;
            int newColumns = target.Columns;
            target.OnColumnsChanged(oldColumns, newColumns);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Columns property.
        /// </summary>
        /// <param name="oldColumns">The old Columns value</param>
        /// <param name="newColumns">The new Columns value</param>
        private void OnColumnsChanged(
            int oldColumns, int newColumns)
        {
            Cascade();
        }
        #endregion

        #region Rows
        /// <summary>
        /// Rows Dependency Property
        /// </summary>
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register(
                "Rows",
                typeof(int),
                typeof(CascadingImageControl),
                new PropertyMetadata(3, OnRowsChanged));

        /// <summary>
        /// Gets or sets the Rows property. This dependency property 
        /// indicates the number of rows.
        /// </summary>
        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Rows property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnRowsChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (CascadingImageControl)d;
            int oldRows = (int)e.OldValue;
            int newRows = target.Rows;
            target.OnRowsChanged(oldRows, newRows);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Rows property.
        /// </summary>
        /// <param name="oldRows">The old Rows value</param>
        /// <param name="newRows">The new Rows value</param>
        private void OnRowsChanged(
            int oldRows, int newRows)
        {
            Cascade();
        }
        #endregion

        #region ImageSource
        /// <summary>
        /// ImageSource Dependency Property
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(
                "ImageSource",
                typeof(ImageSource),
                typeof(CascadingImageControl),
                new PropertyMetadata(null, OnImageSourceChanged));

        /// <summary>
        /// Gets or sets the ImageSource property. This dependency property 
        /// indicates the image to display in a cascade.
        /// </summary>
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ImageSource property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnImageSourceChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (CascadingImageControl)d;
            ImageSource oldImageSource = (ImageSource)e.OldValue;
            ImageSource newImageSource = target.ImageSource;
            target.OnImageSourceChanged(oldImageSource, newImageSource);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the ImageSource property.
        /// </summary>
        /// <param name="oldImageSource">The old ImageSource value</param>
        /// <param name="newImageSource">The new ImageSource value</param>
        private void OnImageSourceChanged(
            ImageSource oldImageSource, ImageSource newImageSource)
        {
            Cascade();
        }
        #endregion

        #region ColumnDelay
        /// <summary>
        /// ColumnDelay Dependency Property
        /// </summary>
        public static readonly DependencyProperty ColumnDelayProperty =
            DependencyProperty.Register(
                "ColumnDelay",
                typeof(TimeSpan),
                typeof(CascadingImageControl),
                new PropertyMetadata(TimeSpan.FromSeconds(0.025), OnColumnDelayChanged));

        /// <summary>
        /// Gets or sets the ColumnDelay property. This dependency property 
        /// indicates the delay of the cascade for each column.
        /// </summary>
        public TimeSpan ColumnDelay
        {
            get { return (TimeSpan)GetValue(ColumnDelayProperty); }
            set { SetValue(ColumnDelayProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ColumnDelay property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnColumnDelayChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (CascadingImageControl)d;
            TimeSpan oldColumnDelay = (TimeSpan)e.OldValue;
            TimeSpan newColumnDelay = target.ColumnDelay;
            target.OnColumnDelayChanged(oldColumnDelay, newColumnDelay);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the ColumnDelay property.
        /// </summary>
        /// <param name="oldColumnDelay">The old ColumnDelay value</param>
        /// <param name="newColumnDelay">The new ColumnDelay value</param>
        private void OnColumnDelayChanged(
            TimeSpan oldColumnDelay, TimeSpan newColumnDelay)
        {
            Cascade();
        }
        #endregion

        #region RowDelay
        /// <summary>
        /// RowDelay Dependency Property
        /// </summary>
        public static readonly DependencyProperty RowDelayProperty =
            DependencyProperty.Register(
                "RowDelay",
                typeof(TimeSpan),
                typeof(CascadingImageControl),
                new PropertyMetadata(TimeSpan.FromSeconds(0.05), OnRowDelayChanged));

        /// <summary>
        /// Gets or sets the RowDelay property. This dependency property 
        /// indicates the delay of the cascade for each row.
        /// </summary>
        public TimeSpan RowDelay
        {
            get { return (TimeSpan)GetValue(RowDelayProperty); }
            set { SetValue(RowDelayProperty, value); }
        }

        /// <summary>
        /// Handles changes to the RowDelay property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnRowDelayChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (CascadingImageControl)d;
            TimeSpan oldRowDelay = (TimeSpan)e.OldValue;
            TimeSpan newRowDelay = target.RowDelay;
            target.OnRowDelayChanged(oldRowDelay, newRowDelay);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the RowDelay property.
        /// </summary>
        /// <param name="oldRowDelay">The old RowDelay value</param>
        /// <param name="newRowDelay">The new RowDelay value</param>
        private void OnRowDelayChanged(
            TimeSpan oldRowDelay, TimeSpan newRowDelay)
        {
            Cascade();
        }
        #endregion

        #region TileDuration
        /// <summary>
        /// TileDuration Dependency Property
        /// </summary>
        public static readonly DependencyProperty TileDurationProperty =
            DependencyProperty.Register(
                "TileDuration",
                typeof(TimeSpan),
                typeof(CascadingImageControl),
                new PropertyMetadata(TimeSpan.FromSeconds(2.0), OnTileDurationChanged));

        /// <summary>
        /// Gets or sets the TileDuration property. This dependency property 
        /// indicates the duration of an item's animation.
        /// </summary>
        public TimeSpan TileDuration
        {
            get { return (TimeSpan)GetValue(TileDurationProperty); }
            set { SetValue(TileDurationProperty, value); }
        }

        /// <summary>
        /// Handles changes to the TileDuration property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnTileDurationChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (CascadingImageControl)d;
            TimeSpan oldTileDuration = (TimeSpan)e.OldValue;
            TimeSpan newTileDuration = target.TileDuration;
            target.OnTileDurationChanged(oldTileDuration, newTileDuration);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the TileDuration property.
        /// </summary>
        /// <param name="oldTileDuration">The old TileDuration value</param>
        /// <param name="newTileDuration">The new TileDuration value</param>
        private void OnTileDurationChanged(
            TimeSpan oldTileDuration, TimeSpan newTileDuration)
        {
        }
        #endregion

        #region CascadeDirection
        /// <summary>
        /// CascadeDirection Dependency Property
        /// </summary>
        public static readonly DependencyProperty CascadeDirectionProperty =
            DependencyProperty.Register(
                "CascadeDirection",
                typeof(CascadeDirection),
                typeof(CascadingImageControl),
                new PropertyMetadata(CascadeDirection.Shuffle));

        /// <summary>
        /// Gets or sets the CascadeDirection property. This dependency property 
        /// indicates the direction of the cascade animation.
        /// </summary>
        public CascadeDirection CascadeDirection
        {
            get { return (CascadeDirection)GetValue(CascadeDirectionProperty); }
            set { SetValue(CascadeDirectionProperty, value); }
        }
        #endregion

        #region CascadeInEasingFunction
        /// <summary>
        /// CascadeInEasingFunction Dependency Property
        /// </summary>
        public static readonly DependencyProperty CascadeInEasingFunctionProperty =
            DependencyProperty.Register(
                "CascadeInEasingFunction",
                typeof(EasingFunctionBase),
                typeof(CascadingImageControl),
                new PropertyMetadata(new ElasticEase { EasingMode = EasingMode.EaseOut, Oscillations = 3, Springiness= 0.0 }));

        /// <summary>
        /// Gets or sets the CascadeInEasingFunction property. This dependency property 
        /// indicates the easing function to use in the cascade in transition.
        /// </summary>
        public EasingFunctionBase CascadeInEasingFunction
        {
            get { return (EasingFunctionBase)GetValue(CascadeInEasingFunctionProperty); }
            set { SetValue(CascadeInEasingFunctionProperty, value); }
        }
        #endregion

        #region CascadeSequence
        /// <summary>
        /// CascadeSequence Dependency Property
        /// </summary>
        public static readonly DependencyProperty CascadeSequenceProperty =
            DependencyProperty.Register(
                "CascadeSequence",
                typeof(CascadeSequence),
                typeof(CascadingImageControl),
                new PropertyMetadata(CascadeSequence.EqualDuration));

        /// <summary>
        /// Gets or sets the CascadeSequence property. This dependency property 
        /// indicates how cascade animations are sequenced.
        /// </summary>
        public CascadeSequence CascadeSequence
        {
            get { return (CascadeSequence)GetValue(CascadeSequenceProperty); }
            set { SetValue(CascadeSequenceProperty, value); }
        }
        #endregion

        public CascadingImageControl()
        {
            this.DefaultStyleKey = typeof(CascadingImageControl);
            this.Loaded += OnLoaded;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _layoutGrid = GetTemplateChild(LayoutGridName) as Grid;

            if (_layoutGrid == null)
            {
                Debug.WriteLine("CascadingImageControl requires a Grid called PART_LayoutGrid in its template.");
            }

            Cascade();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;
            Cascade();
        }

        public void Cascade()
        {
            if (!_isLoaded ||
                _layoutGrid == null)
            {
                return;
            }

            if (Rows < 1)
                Rows = 1;
            if (Columns < 1)
                Columns = 1;

            _layoutGrid.Children.Clear();
            _layoutGrid.RowDefinitions.Clear();
            _layoutGrid.ColumnDefinitions.Clear();

            for (int row = 0; row < Rows; row++)
            {
                _layoutGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int column = 0; column < Columns; column++)
            {
                _layoutGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            var sb = new Storyboard();

            var totalDurationInSeconds = RowDelay.TotalSeconds * (Rows - 1) +
                                         ColumnDelay.TotalSeconds * (Columns - 1) +
                                         TileDuration.TotalSeconds;

            CascadeDirection direction = this.CascadeDirection;

            if (direction == CascadeDirection.Random)
            {
                direction = (CascadeDirection)Random.Next((int)CascadeDirection.Random);
            }

            int startColumn;
            int exclusiveEndColumn;
            int columnIncrement;

            int startRow;
            int exclusiveEndRow;
            int rowIncrement;

            switch (direction)
            {
                case CascadeDirection.Shuffle:
                case CascadeDirection.TopLeft:
                    startColumn = 0;
                    exclusiveEndColumn = Columns;
                    columnIncrement = 1;
                    startRow = 0;
                    exclusiveEndRow = Rows;
                    rowIncrement = 1;
                    break;
                case CascadeDirection.TopRight:
                    startColumn = Columns - 1;
                    exclusiveEndColumn = -1;
                    columnIncrement = -1;
                    startRow = 0;
                    exclusiveEndRow = Rows;
                    rowIncrement = 1;
                    break;
                case CascadeDirection.BottomRight:
                    startColumn = Columns - 1;
                    exclusiveEndColumn = -1;
                    columnIncrement = -1;
                    startRow = Rows - 1;
                    exclusiveEndRow = -1;
                    rowIncrement = -1;
                    break;
                case CascadeDirection.BottomLeft:
                    startColumn = 0;
                    exclusiveEndColumn = Columns;
                    columnIncrement = 1;
                    startRow = Rows - 1;
                    exclusiveEndRow = -1;
                    rowIncrement = -1;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            List<Tuple<int, int>> rectCoords = new List<Tuple<int, int>>(Rows * Columns);
            List<Rectangle> rects = new List<Rectangle>(Rows * Columns);
            List<PlaneProjection> projs = new List<PlaneProjection>(Rows * Columns);

            for (int row = startRow; row != exclusiveEndRow; row = row + rowIncrement)
                for (int column = startColumn; column != exclusiveEndColumn; column = column + columnIncrement)
                {
                    var rect = new Rectangle();
                    rects.Add(rect);

                    Grid.SetRow(rect, row);
                    Grid.SetColumn(rect, column);
                    rectCoords.Add(new Tuple<int, int>(column, row));

                    var brush = new ImageBrush();
                    brush.ImageSource = this.ImageSource;
                    rect.Fill = brush;

                    var transform = new CompositeTransform();
                    transform.TranslateX = -column;
                    transform.ScaleX = Columns;
                    transform.TranslateY = -row;
                    transform.ScaleY = Rows;
                    brush.RelativeTransform = transform;

                    var projection = new PlaneProjection();
                    projection.CenterOfRotationY = 0;
                    rect.Projection = projection;
                    projs.Add(projection);

                    _layoutGrid.Children.Add(rect);
                }

            var indices = new List<int>(Rows * Columns);

            for (int i = 0; i < Rows * Columns; i++)
                indices.Add(i);

            if (direction == CascadeDirection.Shuffle)
            {
                indices = indices.Shuffle();
            }

            for (int ii = 0; ii < indices.Count; ii++)
            {
                var i = indices[ii];
                var projection = projs[i];
                var rect = rects[i];
                var column = rectCoords[ii].Item1;
                var row = rectCoords[ii].Item2;
                //Debug.WriteLine("i: {0}, p: {1}, rect: {2}, c: {3}, r: {4}", i, projection.GetHashCode(), rect.GetHashCode(), column, row);
                var rotationAnimation = new DoubleAnimationUsingKeyFrames();
                Storyboard.SetTarget(rotationAnimation, projection);
                Storyboard.SetTargetProperty(rotationAnimation, "RotationX");

                var endKeyTime =
                    this.CascadeSequence == CascadeSequence.EndTogether
                        ? TimeSpan.FromSeconds(totalDurationInSeconds)
                        : TimeSpan.FromSeconds(
                            (double)row * RowDelay.TotalSeconds +
                            (double)column * ColumnDelay.TotalSeconds +
                            TileDuration.TotalSeconds);

                rotationAnimation.KeyFrames.Add(
                    new DiscreteDoubleKeyFrame
                    {
                        KeyTime = TimeSpan.Zero,
                        Value = 90
                    });
                rotationAnimation.KeyFrames.Add(
                    new DiscreteDoubleKeyFrame
                    {
                        KeyTime = TimeSpan.FromSeconds((double)row * RowDelay.TotalSeconds + (double)column * ColumnDelay.TotalSeconds),
                        Value = 90
                    });
                rotationAnimation.KeyFrames.Add(
                    new EasingDoubleKeyFrame
                    {
                        KeyTime = endKeyTime,
                        EasingFunction = CascadeInEasingFunction,
                        Value = 0
                    });

                sb.Children.Add(rotationAnimation);

                var opacityAnimation = new DoubleAnimationUsingKeyFrames();
                Storyboard.SetTarget(opacityAnimation, rect);
                Storyboard.SetTargetProperty(opacityAnimation, "Opacity");

                opacityAnimation.KeyFrames.Add(
                    new DiscreteDoubleKeyFrame
                    {
                        KeyTime = TimeSpan.Zero,
                        Value = 0
                    });
                opacityAnimation.KeyFrames.Add(
                    new DiscreteDoubleKeyFrame
                    {
                        KeyTime = TimeSpan.FromSeconds((double)row * RowDelay.TotalSeconds + (double)column * ColumnDelay.TotalSeconds),
                        Value = 0
                    });
                opacityAnimation.KeyFrames.Add(
                    new EasingDoubleKeyFrame
                    {
                        KeyTime = endKeyTime,
                        EasingFunction = CascadeInEasingFunction,
                        Value = 1
                    });

                sb.Children.Add(opacityAnimation);
            }

            sb.Begin();
        }
    }
}
