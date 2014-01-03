using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Controls.AutoCompleteTextBoxControl
{
    public class CustomListBoxItem : ListBoxItem
    {
        public Brush PointerOverItemBackgroundBrush
        {
            get { return (Brush)GetValue(PointerOverItemBackgroundBrushProperty); }
            set { SetValue(PointerOverItemBackgroundBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PointerOverItemBackgroundBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointerOverItemBackgroundBrushProperty =
            DependencyProperty.Register("PointerOverItemBackgroundBrush", typeof(Brush), typeof(CustomListBoxItem), new PropertyMetadata(null));

        public Brush PointerOverItemForegroundBrush
        {
            get { return (Brush)GetValue(PointerOverItemForegroundBrushProperty); }
            set { SetValue(PointerOverItemForegroundBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PointerOverItemForegroundBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointerOverItemForegroundBrushProperty = 
            DependencyProperty.Register("PointerOverItemForegroundBrush", typeof(Brush), typeof(CustomListBoxItem), new PropertyMetadata(null));

        protected Brush ForegroundBrush { get; set; }

        public CustomListBoxItem()
        {
            this.Loaded += CustomListBoxItem_Loaded;
            this.Unloaded += CustomListBoxItem_Unloaded;
        }

        // using events instead of VisualStateManager because VSM's aren't working properly (states aren't changed when popup is closed)

        void CustomListBoxItem_Loaded(object sender, RoutedEventArgs e)
        {
            this.ForegroundBrush = this.Foreground; 

            this.PointerEntered += CustomListBoxItem_PointerEntered;
            this.PointerExited += CustomListBoxItem_PointerExited;
            this.PointerCaptureLost += CustomListBoxItem_PointerCaptureLost;
            this.LostFocus += CustomListBoxItem_LostFocus;
        }

        void CustomListBoxItem_Unloaded(object sender, RoutedEventArgs e)
        {
            this.PointerEntered -= CustomListBoxItem_PointerEntered;
            this.PointerExited -= CustomListBoxItem_PointerExited;
            this.PointerCaptureLost -= CustomListBoxItem_PointerCaptureLost;
            this.LostFocus -= CustomListBoxItem_LostFocus;
            this.Loaded -= CustomListBoxItem_Loaded;
            this.Unloaded -= CustomListBoxItem_Unloaded;
        }

        void CustomListBoxItem_LostFocus(object sender, RoutedEventArgs e)
        {
            OnPointerLeftItem();
        }

        void CustomListBoxItem_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            OnPointerLeftItem();
        }

        void CustomListBoxItem_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            OnPointerOverItem();
        }

        /// <summary>
        /// Handles scrolling by touch.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">PointerArgs</param>
        void CustomListBoxItem_PointerCaptureLost(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            OnPointerLeftItem();
        }

        private void OnPointerLeftItem()
        {
            this.Background = new SolidColorBrush(Windows.UI.Colors.Transparent);
            this.Foreground = this.ForegroundBrush;
        }

        private void OnPointerOverItem()
        {
            this.Background = PointerOverItemBackgroundBrush;
            this.Foreground = PointerOverItemForegroundBrush;
        }
 

    }
}
