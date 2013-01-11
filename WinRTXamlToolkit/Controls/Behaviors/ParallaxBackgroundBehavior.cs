using System;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Interactivity;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Controls.Behaviors
{
    /// <summary>
    /// Allows to define a parallax background for use with a ScrollViewer.
    /// </summary>
    public class ParallaxBackgroundBehavior : Behavior<FrameworkElement>
    {
        #region BackgroundElementTemplate
        /// <summary>
        /// BackgroundElementTemplate Dependency Property
        /// </summary>
        public static readonly DependencyProperty BackgroundElementTemplateProperty =
            DependencyProperty.Register(
                "BackgroundElementTemplate",
                typeof(DataTemplate),
                typeof(ParallaxBackgroundBehavior),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the BackgroundElementTemplate property. This dependency property 
        /// indicates the template of the element to place in the background of the associated ScrollViewer.
        /// </summary>
        public DataTemplate BackgroundElementTemplate
        {
            get { return (DataTemplate)GetValue(BackgroundElementTemplateProperty); }
            set { SetValue(BackgroundElementTemplateProperty, value); }
        }
        #endregion

        private ScrollViewer _associatedScrollViewer;
        private Grid _scrollViewerRootGrid;
        private Canvas _parallaxCanvas;
        private FrameworkElement _backgroundElement;

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += OnAssociatedObjectLoaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.Loaded -= OnAssociatedObjectLoaded;
        }

        private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            AttachScrollViewer();
            AttachRootGrid();
            CreateBackgroundElement();
            CreateParallaxCanvas();
        }

        private void CreateParallaxCanvas()
        {
            _parallaxCanvas = new Canvas();
            Grid.SetColumnSpan(
                _parallaxCanvas,
                _scrollViewerRootGrid.ColumnDefinitions.Count > 0
                    ? _scrollViewerRootGrid.ColumnDefinitions.Count
                    : 1);
            Grid.SetRowSpan(
                _parallaxCanvas,
                _scrollViewerRootGrid.RowDefinitions.Count > 0
                    ? _scrollViewerRootGrid.RowDefinitions.Count
                    : 1);
            _parallaxCanvas.Children.Add(_backgroundElement);
            _scrollViewerRootGrid.Children.Insert(0, _parallaxCanvas);
            UpdateBackgroundElementPosition();
            _parallaxCanvas.SizeChanged += OnParallaxCanvasSizeChanged;
        }

        private bool _inUpdateBackgroundElementPosition;
        private void UpdateBackgroundElementPosition()
        {
            if (_inUpdateBackgroundElementPosition ||
                _parallaxCanvas.ActualHeight == 0 ||
                _parallaxCanvas.ActualWidth == 0 ||
                _associatedScrollViewer.ActualHeight == 0 ||
                _associatedScrollViewer.ActualWidth == 0)
            {
                return;
            }

            _inUpdateBackgroundElementPosition = true;

            try
            {
                var shouldStretchVertically = _associatedScrollViewer.VerticalScrollMode != ScrollMode.Disabled;
                var shouldStretchHorizontally = _associatedScrollViewer.HorizontalScrollMode != ScrollMode.Disabled;

                var verticalRatio = _associatedScrollViewer.ViewportHeight /
                                    _associatedScrollViewer.ActualHeight;

                if (shouldStretchHorizontally && !shouldStretchVertically)
                {
                    _backgroundElement.Height = _parallaxCanvas.ActualHeight;

                    if (_backgroundElementWidth == MaxLength)
                        _backgroundElement.Width = 0.5 * (_associatedScrollViewer.ExtentWidth + _associatedScrollViewer.ViewportWidth);
                    else
                        _backgroundElement.Width = _backgroundElement.Height * _backgroundElementWidth / _backgroundElementHeight;

                    _backgroundElement.Margin = new Thickness(0);
                    Canvas.SetTop(_backgroundElement, 0);
                    double offsetX;

                    if (_backgroundElement.Width < _parallaxCanvas.ActualWidth)
                    {
                        offsetX = 0;
                    }
                    else
                    {
                        offsetX = -(_associatedScrollViewer.HorizontalOffset /
                                  (_associatedScrollViewer.ExtentWidth -
                                   _associatedScrollViewer.ViewportWidth)) *
                                   (_backgroundElement.Width - _associatedScrollViewer.ActualWidth);
                    }

                    Canvas.SetLeft(_backgroundElement, offsetX);
                }
            }
            finally
            {
                _inUpdateBackgroundElementPosition = false;
            }
        }

        private double _backgroundElementWidth;
        private double _backgroundElementHeight;
        private const double MaxLength = 100000;

        private void CreateBackgroundElement()
        {
            if (BackgroundElementTemplate == null)
            {
                throw new InvalidOperationException(
                    "BackgroundElementTemplate needs to be defined.");
            }

            _backgroundElement = BackgroundElementTemplate.LoadContent() as FrameworkElement;

            _backgroundElement.Measure(new Size(MaxLength, MaxLength));
            _backgroundElement.Arrange(new Rect(0, 0, MaxLength, MaxLength));
            _backgroundElementWidth = _backgroundElement.ActualWidth;
            _backgroundElementHeight = _backgroundElement.ActualHeight;

            if (_backgroundElement == null)
            {
                throw new InvalidOperationException(
                    "BackgroundElementTemplate needs to be defined as a FrameworkElement.");
            }
        }

        private void OnParallaxCanvasSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            _parallaxCanvas.Clip = new RectangleGeometry {Rect = new Rect(0, 0, _parallaxCanvas.ActualWidth, _parallaxCanvas.ActualHeight)};
            UpdateBackgroundElementPosition();
        }

        private void AttachRootGrid()
        {
            _scrollViewerRootGrid = this.AssociatedObject.GetFirstDescendantOfType<Grid>();

            if (_scrollViewerRootGrid == null)
            {
                throw new InvalidOperationException(
                    "The ScrollViewer associated with the ParallaxBackgroundBehavior does not contain a root Grid required for the parallax effect.");
            }
        }

        private void AttachScrollViewer()
        {
            _associatedScrollViewer = this.AssociatedObject as ScrollViewer;

            if (_associatedScrollViewer == null)
            {
                _associatedScrollViewer =
                    this.AssociatedObject.GetFirstDescendantOfType<ScrollViewer>();
            }

            if (_associatedScrollViewer == null)
            {
                throw new InvalidOperationException(
                    "ParallaxBackgroundBehavior can only be attached to ScrollViewers or elements that have a ScrollViewer in their visual tree.");
            }

            _associatedScrollViewer.ViewChanged += OnAssociatedScrollViewerViewChanged;
        }

        private void OnAssociatedScrollViewerViewChanged(object sender, ScrollViewerViewChangedEventArgs scrollViewerViewChangedEventArgs)
        {
            UpdateBackgroundElementPosition();
        }
    }
}
