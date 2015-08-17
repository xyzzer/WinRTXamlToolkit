using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// The only diffrence between ListBox is that this SimpleThemingListBox contains dependency properties to style
    /// ScrollViewer: background, border and thickness
    /// PointerOverItem, PointerOverPressed background etc.
    /// </summary>
    public class SimpleThemingListBox : ListBox
    {
        private readonly double ItemHeight = 40;

        #region ScrollViewerBackground
        /// <summary>
        /// ScrollViewerBackground Dependency Property
        /// </summary>
        private static readonly DependencyProperty _ScrollViewerBackgroundProperty =
            DependencyProperty.Register(
                "ScrollViewerBackground",
                typeof(Brush),
                typeof(SimpleThemingListBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the ScrollViewerBackground dependency property.
        /// </summary>
        public static DependencyProperty ScrollViewerBackgroundProperty { get { return _ScrollViewerBackgroundProperty; } }

        /// <summary>
        /// Gets or sets the ScrollViewerBackground property. This dependency property 
        /// indicates the brush to use for the background of the ScrollViewer.
        /// </summary>
        public Brush ScrollViewerBackground
        {
            get { return (Brush)GetValue(ScrollViewerBackgroundProperty); }
            set { this.SetValue(ScrollViewerBackgroundProperty, value); }
        }
        #endregion

        #region ScrollViewerBorderBrush
        /// <summary>
        /// ScrollViewerBorderBrush Dependency Property
        /// </summary>
        private static readonly DependencyProperty _ScrollViewerBorderBrushProperty =
            DependencyProperty.Register(
                "ScrollViewerBorderBrush",
                typeof(Brush),
                typeof(SimpleThemingListBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the ScrollViewerBorderBrush dependency property.
        /// </summary>
        public static DependencyProperty ScrollViewerBorderBrushProperty { get { return _ScrollViewerBorderBrushProperty; } }

        /// <summary>
        /// Gets or sets the ScrollViewerBorderBrush property. This dependency property 
        /// indicates the brush to use for the border of the ScrollViewer.
        /// </summary>
        public Brush ScrollViewerBorderBrush
        {
            get { return (Brush)GetValue(ScrollViewerBorderBrushProperty); }
            set { this.SetValue(ScrollViewerBorderBrushProperty, value); }
        }
        #endregion

        #region ScrollViewerBorderThickness
        /// <summary>
        /// ScrollViewerBorderThickness Dependency Property
        /// </summary>
        private static readonly DependencyProperty _ScrollViewerBorderThicknessProperty =
            DependencyProperty.Register(
                "ScrollViewerBorderThickness",
                typeof(Thickness),
                typeof(SimpleThemingListBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the ScrollViewerBorderThickness dependency property.
        /// </summary>
        public static DependencyProperty ScrollViewerBorderThicknessProperty { get { return _ScrollViewerBorderThicknessProperty; } }

        /// <summary>
        /// Gets or sets the ScrollViewerBorderThickness property. This dependency property 
        /// indicates the thickness of the ScrollViewer border.
        /// </summary>
        public Thickness ScrollViewerBorderThickness
        {
            get { return (Thickness)GetValue(ScrollViewerBorderThicknessProperty); }
            set { this.SetValue(ScrollViewerBorderThicknessProperty, value); }
        }
        #endregion

        #region PointerOverItemBackground
        /// <summary>
        /// PointerOverItemBackground Dependency Property
        /// </summary>
        private static readonly DependencyProperty _PointerOverItemBackgroundProperty =
            DependencyProperty.Register(
                "PointerOverItemBackground",
                typeof(Brush),
                typeof(SimpleThemingListBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the PointerOverItemBackground dependency property.
        /// </summary>
        public static DependencyProperty PointerOverItemBackgroundProperty { get { return _PointerOverItemBackgroundProperty; } }

        /// <summary>
        /// Gets or sets the PointerOverItemBackground property. This dependency property 
        /// indicates the brush to use for hover state background.
        /// </summary>
        public Brush PointerOverItemBackground
        {
            get { return (Brush)GetValue(PointerOverItemBackgroundProperty); }
            set { this.SetValue(PointerOverItemBackgroundProperty, value); }
        }
        #endregion

        #region PointerOverItemForeground
        /// <summary>
        /// PointerOverItemForeground Dependency Property
        /// </summary>
        private static readonly DependencyProperty _PointerOverItemForegroundProperty =
            DependencyProperty.Register(
                "PointerOverItemForeground",
                typeof(Brush),
                typeof(SimpleThemingListBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the PointerOverItemForeground dependency property.
        /// </summary>
        public static DependencyProperty PointerOverItemForegroundProperty { get { return _PointerOverItemForegroundProperty; } }

        /// <summary>
        /// Gets or sets the PointerOverItemForeground property. This dependency property 
        /// indicates the brush to use for hover state foreground.
        /// </summary>
        public Brush PointerOverItemForeground
        {
            get { return (Brush)GetValue(PointerOverItemForegroundProperty); }
            set { this.SetValue(PointerOverItemForegroundProperty, value); }
        }
        #endregion

        #region GetContainerForItemOverride()
        /// <summary>
        /// Gets the container for item override.
        /// </summary>
        /// <returns></returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            // as far as I know there is no way to change Container in .xaml 
            var customListBoxItem = new SimpleThemingListBoxItem { Height = ItemHeight };

            customListBoxItem.SetBinding(
                SimpleThemingListBoxItem.PointerOverItemBackgroundBrushProperty,
                new Binding
                {
                    Source = this,
                    Path = new PropertyPath("PointerOverItemBackground")
                });
            customListBoxItem.SetBinding(
                SimpleThemingListBoxItem.PointerOverItemForegroundBrushProperty,
                new Binding
                {
                    Source = this,
                    Path = new PropertyPath("PointerOverItemForeground")
                });

            return customListBoxItem;
        }
        #endregion

        /// <summary>
        /// Gets the item height.
        /// </summary>
        /// <returns></returns>
        public double GetItemHeight()
        {
            return this.ItemHeight;
        }
    }
}
