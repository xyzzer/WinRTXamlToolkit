using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// ListBoxItem implementation for use in SimpleThemingListBox.
    /// Provides properties to set brushes for hover states.
    /// </summary>
    public class SimpleThemingListBoxItem : ListBoxItem
    {
        #region PointerOverItemBackgroundBrush
        /// <summary>
        /// PointerOverItemBackgroundBrush Dependency Property
        /// </summary>
        private static readonly DependencyProperty _PointerOverItemBackgroundBrushProperty =
            DependencyProperty.Register(
                "PointerOverItemBackgroundBrush",
                typeof(Brush),
                typeof(SimpleThemingListBoxItem),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the PointerOverItemBackgroundBrush dependency property.
        /// </summary>
        public static DependencyProperty PointerOverItemBackgroundBrushProperty { get { return _PointerOverItemBackgroundBrushProperty; } }

        /// <summary>
        /// Gets or sets the PointerOverItemBackgroundBrush property. This dependency property 
        /// indicates the background brush on hover.
        /// </summary>
        public Brush PointerOverItemBackgroundBrush
        {
            get { return (Brush)GetValue(PointerOverItemBackgroundBrushProperty); }
            set { this.SetValue(PointerOverItemBackgroundBrushProperty, value); }
        }
        #endregion

        #region PointerOverItemForegroundBrush
        /// <summary>
        /// PointerOverItemForegroundBrush Dependency Property
        /// </summary>
        private static readonly DependencyProperty _PointerOverItemForegroundBrushProperty =
            DependencyProperty.Register(
                "PointerOverItemForegroundBrush",
                typeof(Brush),
                typeof(SimpleThemingListBoxItem),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the PointerOverItemForegroundBrush dependency property.
        /// </summary>
        public static DependencyProperty PointerOverItemForegroundBrushProperty { get { return _PointerOverItemForegroundBrushProperty; } }

        /// <summary>
        /// Gets or sets the PointerOverItemForegroundBrush property. This dependency property 
        /// indicates the brush to use for the foreground on hover.
        /// </summary>
        public Brush PointerOverItemForegroundBrush
        {
            get { return (Brush)GetValue(PointerOverItemForegroundBrushProperty); }
            set { this.SetValue(PointerOverItemForegroundBrushProperty, value); }
        }
        #endregion

        /// <summary>
        /// Gets or sets the original foreground brush before hover state.
        /// </summary>
        /// <value>
        /// The original foreground brush.
        /// </value>
        protected Brush OriginalForegroundBrush { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleThemingListBoxItem"/> class.
        /// </summary>
        public SimpleThemingListBoxItem()
        {
            this.Loaded += this.OnLoaded;
            this.Unloaded += this.OnUnloaded;
        }

        // using events instead of VisualStateManager because VSM's aren't working properly (states aren't changed when popup is closed)
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.OriginalForegroundBrush = this.Foreground;

            this.PointerEntered += this.OnPointerEntered;
            this.PointerExited += this.OnPointerExited;
            this.PointerCaptureLost += this.OnPointerCaptureLost;
            this.LostFocus += this.OnLostFocus;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.PointerEntered -= this.OnPointerEntered;
            this.PointerExited -= this.OnPointerExited;
            this.PointerCaptureLost -= this.OnPointerCaptureLost;
            this.LostFocus -= this.OnLostFocus;
            this.Loaded -= this.OnLoaded;
            this.Unloaded -= this.OnUnloaded;
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            this.OnPointerLeftItem();
        }

        private void OnPointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            this.OnPointerLeftItem();
        }

        private void OnPointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            this.OnPointerOverItem();
        }

        /// <summary>
        /// Handles scrolling by touch.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">PointerArgs</param>
        private void OnPointerCaptureLost(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            this.OnPointerLeftItem();
        }

        private void OnPointerLeftItem()
        {
            this.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
            this.Foreground = this.OriginalForegroundBrush;
        }

        private void OnPointerOverItem()
        {
            this.Background = this.PointerOverItemBackgroundBrush;
            this.Foreground = this.PointerOverItemForegroundBrush;
        }
    }
}
