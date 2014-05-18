using System.Linq;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// TextBox with a watermark
    /// </summary>
    [TemplateVisualState(GroupName = WatermarkStatesGroupName, Name = WatermarkVisibleStateName)]
    [TemplateVisualState(GroupName = WatermarkStatesGroupName, Name = WatermarkHiddenStateName)]
    [StyleTypedProperty(Property = "WatermarkTextStyle", StyleTargetType = typeof(TextBlock))]
    public class WatermarkTextBox : TextBox
    {
        #region Consts
        private const string WatermarkStatesGroupName = "WatermarkStates";
        private const string WatermarkVisibleStateName = "WatermarkVisible";
        private const string WatermarkHiddenStateName = "WatermarkHidden";
        #endregion

        #region Watermark
        /// <summary>
        /// Watermark Dependency Property
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register(
                "Watermark",
                typeof(object),
                typeof(WatermarkTextBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the Watermark property. This dependency property 
        /// indicates an arbitrary content to use as a Watermark.
        /// </summary>
        public object Watermark
        {
            get { return (object)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }
        #endregion

        #region WatermarkText
        /// <summary>
        /// WatermarkText Dependency Property
        /// </summary>
        public static readonly DependencyProperty WatermarkTextProperty =
            DependencyProperty.Register(
                "WatermarkText",
                typeof(string),
                typeof(WatermarkTextBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the WatermarkText property. This dependency property 
        /// indicates the watermark text to show to the user.
        /// </summary>
        public string WatermarkText
        {
            get { return (string)GetValue(WatermarkTextProperty); }
            set { SetValue(WatermarkTextProperty, value); }
        }
        #endregion

        #region WatermarkTextStyle
        /// <summary>
        /// WatermarkTextStyle Dependency Property
        /// </summary>
        public static readonly DependencyProperty WatermarkTextStyleProperty =
            DependencyProperty.Register(
                "WatermarkTextStyle",
                typeof(Style),
                typeof(WatermarkTextBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the WatermarkTextStyle property. This dependency property 
        /// indicates the style of the watermark TextBlock.
        /// </summary>
        public Style WatermarkTextStyle
        {
            get { return (Style)GetValue(WatermarkTextStyleProperty); }
            set { SetValue(WatermarkTextStyleProperty, value); }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="WatermarkTextBox" /> class.
        /// </summary>
        public WatermarkTextBox()
        {
            DefaultStyleKey = typeof (WatermarkTextBox);
            this.TextChanged += OnTextChanged;
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateWatermarkVisualState();
        }

        private void UpdateWatermarkVisualState()
        {
            var focusedElement = FocusManager.GetFocusedElement() as DependencyObject;
            var isFocused =
                (this == focusedElement) ||
                (focusedElement != null && focusedElement.GetAncestors().Contains(this));
            UpdateWatermarkVisualState(isFocused);
        }

        private void UpdateWatermarkVisualState(bool isFocused)
        {
            if (!isFocused && string.IsNullOrEmpty(this.Text))
            {
                VisualStateManager.GoToState(this, WatermarkVisibleStateName, true);
            }
            else
            {
                VisualStateManager.GoToState(this, WatermarkHiddenStateName, true);
            }
        }

        /// <summary>
        /// Called before the GotFocus event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnGotFocus(Windows.UI.Xaml.RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            UpdateWatermarkVisualState(true);
        }

        /// <summary>
        /// Called before the LostFocus event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnLostFocus(Windows.UI.Xaml.RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            UpdateWatermarkVisualState(false);
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateWatermarkVisualState();
        }
    }
}
