using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Controls.AutoCompleteTextBoxControl
{
    /// <summary>
    /// The only diffrence between ListBox is that this CustomListBox contains dependency properties to style
    /// ScrollViewer: background, border and thickness
    /// PointerOverItem, PointerOverPressed background etc.
    /// </summary>
    public class CustomListBox : ListBox
    {
        private readonly double ItemHeight = 40;
        public Brush ScrollViewerBackground
        {
            get { return (Brush)GetValue(ScrollViewerBackgroundProperty); }
            set { SetValue(ScrollViewerBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScrollViewerBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollViewerBackgroundProperty =
            DependencyProperty.Register("ScrollViewerBackground", typeof(Brush), typeof(CustomListBox), new PropertyMetadata(null));

        public Brush ScrollViewerBorderBrush
        {
            get { return (Brush)GetValue(ScrollViewerBorderBrushProperty); }
            set { SetValue(ScrollViewerBorderBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScrollViewerBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollViewerBorderBrushProperty =
            DependencyProperty.Register("ScrollViewerBorderBrush", typeof(Brush), typeof(CustomListBox), new PropertyMetadata(null));

        public Thickness ScrollViewerBorderThickness
        {
            get { return (Thickness)GetValue(ScrollViewerBorderThicknessProperty); }
            set { SetValue(ScrollViewerBorderThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScrollViewerBorderThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScrollViewerBorderThicknessProperty =
            DependencyProperty.Register("ScrollViewerBorderThickness", typeof(Thickness), typeof(CustomListBox), new PropertyMetadata(null));

        public Brush PointerOverItemBackground
        {
            get { return (Brush)GetValue(PointerOverItemBackgroundProperty); }
            set { SetValue(PointerOverItemBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PointerOverItemBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointerOverItemBackgroundProperty =
            DependencyProperty.Register("PointerOverItemBackground", typeof(Brush), typeof(CustomListBox), new PropertyMetadata(0));

        public Brush PointerOverItemForeground
        {
            get { return (Brush)GetValue(PointerOverItemForegroundProperty); }
            set { SetValue(PointerOverItemForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PointerOverItemForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointerOverItemForegroundProperty =
            DependencyProperty.Register("PointerOverItemForeground", typeof(Brush), typeof(CustomListBox), new PropertyMetadata(0));

      
        protected override DependencyObject GetContainerForItemOverride()
        {
            // as far as I know there is no way to change Container in .xaml 
            var customListBoxItem = new CustomListBoxItem() { Height = ItemHeight };

            Binding pointerOverItemBackgroundBrushBinding = new Binding()
            {
                Source = this,
                Path = new PropertyPath("PointerOverItemBackground")
            };

            Binding pointerOverItemForegroundBrushBinding = new Binding()
            {
                Source = this,
                Path = new PropertyPath("PointerOverItemForeground")
            };
            customListBoxItem.SetBinding(CustomListBoxItem.PointerOverItemBackgroundBrushProperty, pointerOverItemBackgroundBrushBinding);
            customListBoxItem.SetBinding(CustomListBoxItem.PointerOverItemForegroundBrushProperty, pointerOverItemForegroundBrushBinding);

            return customListBoxItem;
        }

        public double GetItemHeight()
        {
            return this.ItemHeight;
        }

        
    }
}
