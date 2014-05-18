using System.Linq;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// PasswordBox with a watermark
    /// </summary>
    [TemplateVisualState(GroupName = WatermarkStatesGroupName, Name = WatermarkVisibleStateName)]
    [TemplateVisualState(GroupName = WatermarkStatesGroupName, Name = WatermarkHiddenStateName)]
    [StyleTypedProperty(Property = "WatermarkTextStyle", StyleTargetType = typeof(TextBlock))]
    [TemplatePart(Name = InnerPasswordBoxName, Type = typeof(PasswordBox))]
    public sealed class WatermarkPasswordBox : Control
    {
        #region Consts
        private const string WatermarkStatesGroupName = "WatermarkStates";
        private const string WatermarkVisibleStateName = "WatermarkVisible";
        private const string WatermarkHiddenStateName = "WatermarkHidden";
        private const string InnerPasswordBoxName = "PART_PasswordBox";
        #endregion

        private PasswordBox _innerPasswordBox;

        #region Watermark
        /// <summary>
        /// Watermark Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.RegisterAttached(
                "Watermark",
                typeof(object),
                typeof(WatermarkPasswordBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the Watermark property. This dependency property 
        /// indicates an arbitrary content to use as a Watermark.
        /// </summary>
        public static object GetWatermark(DependencyObject d)
        {
            return (object)d.GetValue(WatermarkProperty);
        }

        /// <summary>
        /// Sets the Watermark property. This dependency property 
        /// indicates an arbitrary content to use as a Watermark.
        /// </summary>
        public static void SetWatermark(DependencyObject d, object value)
        {
            d.SetValue(WatermarkProperty, value);
        }
        
        /////// <summary>
        /////// Gets or sets the Watermark property. This dependency property 
        /////// indicates an arbitrary content to use as a Watermark.
        /////// </summary>
        ////public object Watermark
        ////{
        ////    get { return (object)GetValue(WatermarkProperty); }
        ////    set { SetValue(WatermarkProperty, value); }
        ////}
        #endregion

        #region WatermarkText
        /// <summary>
        /// WatermarkText Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty WatermarkTextProperty =
            DependencyProperty.RegisterAttached(
                "WatermarkText",
                typeof(string),
                typeof(WatermarkPasswordBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the WatermarkText property. This dependency property 
        /// indicates the watermark text to show to the user.
        /// </summary>
        public static string GetWatermarkText(DependencyObject d)
        {
            return (string)d.GetValue(WatermarkTextProperty);
        }

        /// <summary>
        /// Sets the WatermarkText property. This dependency property 
        /// indicates the watermark text to show to the user.
        /// </summary>
        public static void SetWatermarkText(DependencyObject d, string value)
        {
            d.SetValue(WatermarkTextProperty, value);
        }

        /////// <summary>
        /////// Gets or sets the WatermarkText property. This dependency property 
        /////// indicates the watermark text to show to the user.
        /////// </summary>
        ////public string WatermarkText
        ////{
        ////    get { return (string)GetValue(WatermarkTextProperty); }
        ////    set { SetValue(WatermarkTextProperty, value); }
        ////}
        #endregion

        #region WatermarkTextStyle
        /// <summary>
        /// WatermarkTextStyle Dependency Property
        /// </summary>
        public static readonly DependencyProperty WatermarkTextStyleProperty =
            DependencyProperty.Register(
                "WatermarkTextStyle",
                typeof(Style),
                typeof(WatermarkPasswordBox),
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

        #region WatermarkTextStyleRelay
        /// <summary>
        /// WatermarkTextStyleRelay Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty WatermarkTextStyleRelayProperty =
            DependencyProperty.RegisterAttached(
                "WatermarkTextStyleRelay",
                typeof(Style),
                typeof(WatermarkPasswordBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets the WatermarkTextStyleRelay property. This dependency property 
        /// indicates the style of the watermark TextBlock.
        /// </summary>
        public static Style GetWatermarkTextStyleRelay(DependencyObject d)
        {
            return (Style)d.GetValue(WatermarkTextStyleRelayProperty);
        }

        /// <summary>
        /// Sets the WatermarkTextStyleRelay property. This dependency property 
        /// indicates the style of the watermark TextBlock.
        /// </summary>
        public static void SetWatermarkTextStyleRelay(DependencyObject d, Style value)
        {
            d.SetValue(WatermarkTextStyleRelayProperty, value);
        }

        /////// <summary>
        /////// Gets or sets the WatermarkTextStyleRelay property. This dependency property 
        /////// indicates the style of the watermark TextBlock.
        /////// </summary>
        ////public Style WatermarkTextStyleRelay
        ////{
        ////    get { return (Style)GetValue(WatermarkTextStyleRelayProperty); }
        ////    set { SetValue(WatermarkTextStyleRelayProperty, value); }
        ////}
        #endregion

        #region IsPasswordRevealButtonEnabled
        /// <summary>
        /// IsPasswordRevealButtonEnabled Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsPasswordRevealButtonEnabledProperty =
            DependencyProperty.Register(
                "IsPasswordRevealButtonEnabled",
                typeof(bool),
                typeof(WatermarkPasswordBox),
                new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the IsPasswordRevealButtonEnabled property. This dependency property 
        /// indicates whether the visual UI of the PasswordBox
        /// should include a button element that toggles showing or hiding the typed
        /// characters.
        /// </summary>
        /// <returns>
        /// True to show a password reveal button; false to not show a password reveal
        /// button.
        /// </returns>
        public bool IsPasswordRevealButtonEnabled
        {
            get { return (bool)GetValue(IsPasswordRevealButtonEnabledProperty); }
            set { SetValue(IsPasswordRevealButtonEnabledProperty, value); }
        }
        #endregion

        #region MaxLength
        /// <summary>
        /// MaxLength Dependency Property
        /// </summary>
        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register(
                "MaxLength",
                typeof(int),
                typeof(WatermarkPasswordBox),
                new PropertyMetadata(0));

        /// <summary>
        /// Gets or sets the MaxLength property. This dependency property 
        /// indicates the maximum length for passwords to be handled by this PasswordBox.
        /// </summary>
        /// <returns>
        /// An integer that specifies the maximum number of characters for passwords
        /// to be handled by this PasswordBox. A value of zero (0) means no limit. The
        /// default is 0 (no length limit).
        /// </returns>
        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }
        #endregion

        #region Password
        /// <summary>
        /// Password Dependency Property
        /// </summary>
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                "Password",
                typeof(string),
                typeof(WatermarkPasswordBox),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the Password property. This dependency property 
        /// indicates the password currently held by the PasswordBox.
        /// </summary>
        /// <returns>
        /// A string that represents the password currently held by the PasswordBox.
        /// The default is an empty string.
        /// </returns>
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        #endregion

        #region PasswordChar
        /// <summary>
        /// PasswordChar Dependency Property
        /// </summary>
        public static readonly DependencyProperty PasswordCharProperty =
            DependencyProperty.Register(
                "PasswordChar",
                typeof(string),
                typeof(WatermarkPasswordBox),
                new PropertyMetadata("●"));

        /// <summary>
        /// Gets or sets the PasswordChar property. This dependency property 
        /// indicates the masking character for the PasswordBox.
        /// </summary>
        /// <returns>
        /// A masking character to echo when the user enters text into the PasswordBox.
        /// The default value is a bullet character (●).
        /// </returns>
        public string PasswordChar
        {
            get { return (string)GetValue(PasswordCharProperty); }
            set { SetValue(PasswordCharProperty, value); }
        }
        #endregion

        /// <summary>
        /// Occurs when the system processes an interaction that displays a context menu.
        /// </summary>
        public event ContextMenuOpeningEventHandler ContextMenuOpening;

        /// <summary>
        /// Occurs when the value of the Password property changes.
        /// </summary>
        public event RoutedEventHandler PasswordChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatermarkPasswordBox" /> class.
        /// </summary>
        public WatermarkPasswordBox()
        {
            DefaultStyleKey = typeof (WatermarkPasswordBox);
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            UpdateWatermarkVisualState(false, false);
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _innerPasswordBox = (PasswordBox)GetTemplateChild(InnerPasswordBoxName);
            _innerPasswordBox.PasswordChanged += InnerPasswordBoxOnPasswordChanged;
            _innerPasswordBox.KeyUp += InnerPasswordBoxOnKeyUp;
            _innerPasswordBox.GotFocus += InnerPasswordBoxOnGotFocus;
            _innerPasswordBox.LostFocus += InnerPasswordBoxOnLostFocus;
            _innerPasswordBox.ContextMenuOpening += InnerPasswordBoxOnContextMenuOpening;

            _innerPasswordBox.SetBinding(
                PasswordBox.PasswordProperty,
                new Binding
                {
                    Path = new PropertyPath("Password"),
                    Mode = BindingMode.TwoWay,
                    Source = this
                });
            _innerPasswordBox.SetBinding(
                PasswordBox.PasswordCharProperty,
                new Binding
                {
                    Path = new PropertyPath("PasswordChar"),
                    Mode = BindingMode.TwoWay,
                    Source = this
                });
            _innerPasswordBox.SetBinding(
                PasswordBox.IsPasswordRevealButtonEnabledProperty,
                new Binding
                {
                    Path = new PropertyPath("IsPasswordRevealButtonEnabled"),
                    Mode = BindingMode.TwoWay,
                    Source = this
                });
            _innerPasswordBox.SetBinding(
                PasswordBox.MaxLengthProperty,
                new Binding
                {
                    Path = new PropertyPath("MaxLength"),
                    Mode = BindingMode.TwoWay,
                    Source = this
                });

            UpdateWatermarkVisualState(false, false);
        }

        private void InnerPasswordBoxOnContextMenuOpening(object sender, ContextMenuEventArgs contextMenuEventArgs)
        {
            var handler = ContextMenuOpening;

            if (handler != null)
            {
                handler(this, contextMenuEventArgs);
            }
        }

        private void UpdateWatermarkVisualState()
        {
            var focusedElement = FocusManager.GetFocusedElement() as DependencyObject;
            var isFocused =
                (this == focusedElement) ||
                (focusedElement != null && focusedElement.GetAncestors().Contains(this));
            UpdateWatermarkVisualState(isFocused);
        }

        private void UpdateWatermarkVisualState(bool isFocused, bool useTransitions = true)
        {
            if (_innerPasswordBox != null)
            {
                if (!isFocused && string.IsNullOrEmpty(_innerPasswordBox.Password))
                {
                    VisualStateManager.GoToState(_innerPasswordBox, WatermarkVisibleStateName, useTransitions);
                }
                else
                {
                    VisualStateManager.GoToState(_innerPasswordBox, WatermarkHiddenStateName, useTransitions);
                }
            }
        }

        private void InnerPasswordBoxOnGotFocus(object sender, RoutedEventArgs e)
        {
            UpdateWatermarkVisualState(true);
        }

        private void InnerPasswordBoxOnLostFocus(object sender, RoutedEventArgs e)
        {
            UpdateWatermarkVisualState(false);
        }

        private void InnerPasswordBoxOnPasswordChanged(object sender, RoutedEventArgs e)
        {
            UpdateWatermarkVisualState();

            var handler = PasswordChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void InnerPasswordBoxOnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            UpdateWatermarkVisualState();
        }

        /// <summary>
        /// Selects all the character in the PasswordBox.
        /// </summary>
        public void SelectAll()
        {
            _innerPasswordBox.SelectAll();
        }
    }
}
