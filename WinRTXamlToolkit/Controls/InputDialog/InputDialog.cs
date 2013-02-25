using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using WinRTXamlToolkit.AwaitableUI;
using WinRTXamlToolkit.Controls.Extensions;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// A dialog control for collecting user's text input.
    /// </summary>
    [TemplatePart(Name=LayoutRootPanelName, Type=typeof(Panel))]
    [TemplatePart(Name=ContentBorderName, Type=typeof(Border))]
    [TemplatePart(Name=InputTextBoxName, Type=typeof(TextBox))]
    [TemplatePart(Name=TitleTextBlockName, Type=typeof(TextBlock))]
    [TemplatePart(Name=TextTextBlockName, Type=typeof(TextBlock))]
    [TemplatePart(Name=ButtonsPanelName, Type=typeof(Panel))]
    [TemplateVisualState(GroupName=PopupStatesGroupName, Name=OpenPopupStateName)]
    [TemplateVisualState(GroupName=PopupStatesGroupName, Name=ClosedPopupStateName)]
    [StyleTypedProperty(Property = "ButtonStyle", StyleTargetType = typeof(ButtonBase))]
    [StyleTypedProperty(Property = "TitleStyle", StyleTargetType = typeof(TextBlock))]
    [StyleTypedProperty(Property = "TextStyle", StyleTargetType = typeof(TextBlock))]
    [StyleTypedProperty(Property = "InputTextStyle", StyleTargetType = typeof(TextBox))]
    public class InputDialog : Control
    {
        #region Template Part Names
        private const string PopupStatesGroupName = "PopupStates";
        private const string OpenPopupStateName = "OpenPopupState";
        private const string ClosedPopupStateName = "ClosedPopupState";

        private const string LayoutRootPanelName = "LayoutRoot";
        private const string ContentBorderName = "ContentBorder";
        private const string InputTextBoxName = "InputTextBox";
        private const string TitleTextBlockName = "TitleTextBlock";
        private const string TextTextBlockName = "TextTextBlock";
        private const string ButtonsPanelName = "ButtonsPanel"; 
        #endregion

        #region Template Part Fields
        private Panel _layoutRoot;
        private Border _contentBorder;
        private TextBox _inputTextBox;
        private TextBlock _titleTextBlock;
        private TextBlock _textTextBlock;
        private Panel _buttonsPanel; 
        #endregion

        /// <summary>
        /// Flag for preventing reentrancy in the Show() method.
        /// </summary>
        private bool _shown;
        private TaskCompletionSource<string> _dismissTaskSource;
        private List<ButtonBase> _buttons;

        private Popup _dialogPopup;
        private Panel _parentPanel;
        private Panel _temporaryParentPanel;
        private ContentControl _parentContentControl;

        #region ButtonStyle
        /// <summary>
        /// ButtonStyle Dependency Property
        /// </summary>
        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register(
                "ButtonStyle",
                typeof(Style),
                typeof(InputDialog),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the ButtonStyle property. This dependency property 
        /// indicates the Style to use for the buttons.
        /// </summary>
        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }
        #endregion

        #region InputText
        /// <summary>
        /// InputText Dependency Property
        /// </summary>
        public static readonly DependencyProperty InputTextProperty =
            DependencyProperty.Register(
                "InputText",
                typeof(string),
                typeof(InputDialog),
                new PropertyMetadata("", OnInputTextChanged));

        /// <summary>
        /// Gets or sets the InputText property. This dependency property 
        /// indicates the text in the input box.
        /// </summary>
        public string InputText
        {
            get { return (string)GetValue(InputTextProperty); }
            set { SetValue(InputTextProperty, value); }
        }

        /// <summary>
        /// Handles changes to the InputText property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnInputTextChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (InputDialog)d;
            string oldInputText = (string)e.OldValue;
            string newInputText = target.InputText;
            target.OnInputTextChanged(oldInputText, newInputText);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the InputText property.
        /// </summary>
        /// <param name="oldInputText">The old InputText value</param>
        /// <param name="newInputText">The new InputText value</param>
        protected virtual void OnInputTextChanged(
            string oldInputText, string newInputText)
        {
            if (_inputTextBox != null)
            {
                _inputTextBox.Text = newInputText;
            }
        }
        #endregion

        #region AcceptButton
        /// <summary>
        /// AcceptButton Dependency Property
        /// </summary>
        public static readonly DependencyProperty AcceptButtonProperty =
            DependencyProperty.Register(
                "AcceptButton",
                typeof(string),
                typeof(InputDialog),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the AcceptButton property. This dependency property 
        /// indicates the result to return when user hits the Enter key while the input text box has focus.
        /// </summary>
        public string AcceptButton
        {
            get { return (string)GetValue(AcceptButtonProperty); }
            set { SetValue(AcceptButtonProperty, value); }
        }
        #endregion

        #region CancelButton
        /// <summary>
        /// CancelButton Dependency Property
        /// </summary>
        public static readonly DependencyProperty CancelButtonProperty =
            DependencyProperty.Register(
                "CancelButton",
                typeof(string),
                typeof(InputDialog),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the CancelButton property. This dependency property 
        /// indicates the result to return when user hits the Escape key or taps out of the dialog.
        /// </summary>
        public string CancelButton
        {
            get { return (string)GetValue(CancelButtonProperty); }
            set { SetValue(CancelButtonProperty, value); }
        }
        #endregion

        #region BackgroundScreenBrush
        /// <summary>
        /// BackgroundScreenBrush Dependency Property
        /// </summary>
        public static readonly DependencyProperty BackgroundScreenBrushProperty =
            DependencyProperty.Register(
                "BackgroundScreenBrush",
                typeof(Brush),
                typeof(InputDialog),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the BackgroundScreenBrush property. This dependency property 
        /// indicates the brush to use for the screen behind the dialog window.
        /// </summary>
        public Brush BackgroundScreenBrush
        {
            get { return (Brush)GetValue(BackgroundScreenBrushProperty); }
            set { SetValue(BackgroundScreenBrushProperty, value); }
        }
        #endregion

        #region BackgroundStripeBrush
        /// <summary>
        /// BackgroundStripeBrush Dependency Property
        /// </summary>
        public static readonly DependencyProperty BackgroundStripeBrushProperty =
            DependencyProperty.Register(
                "BackgroundStripeBrush",
                typeof(Brush),
                typeof(InputDialog),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the BackgroundStripeBrush property. This dependency property 
        /// indicates the brush to use for the stripe behind the dialog window.
        /// </summary>
        public Brush BackgroundStripeBrush
        {
            get { return (Brush)GetValue(BackgroundStripeBrushProperty); }
            set { SetValue(BackgroundStripeBrushProperty, value); }
        }
        #endregion

        #region TitleStyle
        /// <summary>
        /// TitleStyle Dependency Property
        /// </summary>
        public static readonly DependencyProperty TitleStyleProperty =
            DependencyProperty.Register(
                "TitleStyle",
                typeof(Style),
                typeof(InputDialog),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the TitleStyle property. This dependency property 
        /// indicates the style to use for the title TextBlock.
        /// </summary>
        public Style TitleStyle
        {
            get { return (Style)GetValue(TitleStyleProperty); }
            set { SetValue(TitleStyleProperty, value); }
        }
        #endregion

        #region TextStyle
        /// <summary>
        /// TextStyle Dependency Property
        /// </summary>
        public static readonly DependencyProperty TextStyleProperty =
            DependencyProperty.Register(
                "TextStyle",
                typeof(Style),
                typeof(InputDialog),
                new PropertyMetadata(null, OnTextStyleChanged));

        /// <summary>
        /// Gets or sets the TextStyle property. This dependency property 
        /// indicates the style to use for the text section TextBlock.
        /// </summary>
        public Style TextStyle
        {
            get { return (Style)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        /// <summary>
        /// Handles changes to the TextStyle property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnTextStyleChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (InputDialog)d;
            Style oldTextStyle = (Style)e.OldValue;
            Style newTextStyle = target.TextStyle;
            target.OnTextStyleChanged(oldTextStyle, newTextStyle);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the TextStyle property.
        /// </summary>
        /// <param name="oldTextStyle">The old TextStyle value</param>
        /// <param name="newTextStyle">The new TextStyle value</param>
        protected virtual void OnTextStyleChanged(
            Style oldTextStyle, Style newTextStyle)
        {
        }
        #endregion

        #region InputTextStyle
        /// <summary>
        /// InputTextStyle Dependency Property
        /// </summary>
        public static readonly DependencyProperty InputTextStyleProperty =
            DependencyProperty.Register(
                "InputTextStyle",
                typeof(Style),
                typeof(InputDialog),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the InputTextStyle property. This dependency property 
        /// indicates the style of the input TextBox.
        /// </summary>
        public Style InputTextStyle
        {
            get { return (Style)GetValue(InputTextStyleProperty); }
            set { SetValue(InputTextStyleProperty, value); }
        }
        #endregion

        #region IsLightDismissEnabled
        /// <summary>
        /// IsLightDismissEnabled Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsLightDismissEnabledProperty =
            DependencyProperty.Register(
                "IsLightDismissEnabled",
                typeof(bool),
                typeof(InputDialog),
                new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets the IsLightDismissEnabled property. This dependency property 
        /// indicates whether the dialog can be dismissed by tapping outside of the main dialog border.
        /// </summary>
        public bool IsLightDismissEnabled
        {
            get { return (bool)GetValue(IsLightDismissEnabledProperty); }
            set { SetValue(IsLightDismissEnabledProperty, value); }
        }
        #endregion

        #region AwaitsCloseTransition
        /// <summary>
        /// AwaitsCloseTransition Dependency Property
        /// </summary>
        public static readonly DependencyProperty AwaitsCloseTransitionProperty =
            DependencyProperty.Register(
                "AwaitsCloseTransition",
                typeof(bool),
                typeof(InputDialog),
                new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets the AwaitsCloseTransition property. This dependency property 
        /// indicates whether awaiting ShowAsync() also awaits the closing transition..
        /// </summary>
        public bool AwaitsCloseTransition
        {
            get { return (bool)GetValue(AwaitsCloseTransitionProperty); }
            set { SetValue(AwaitsCloseTransitionProperty, value); }
        }
        #endregion

        #region ButtonsPanelOrientation
        /// <summary>
        /// ButtonsPanelOrientation Dependency Property
        /// </summary>
        public static readonly DependencyProperty ButtonsPanelOrientationProperty =
            DependencyProperty.Register(
                "ButtonsPanelOrientation",
                typeof(Orientation),
                typeof(InputDialog),
                new PropertyMetadata(Orientation.Horizontal));

        /// <summary>
        /// Gets or sets the ButtonsPanelOrientation property. This dependency property 
        /// indicates the orientation of the buttons panel.
        /// </summary>
        public Orientation ButtonsPanelOrientation
        {
            get { return (Orientation)GetValue(ButtonsPanelOrientationProperty); }
            set { SetValue(ButtonsPanelOrientationProperty, value); }
        }
        #endregion

        public InputDialog()
        {
            this.DefaultStyleKey = typeof(InputDialog);

            // The dialog can now be hosted in a Panel or ContentControl, but should not be visible until first shown and moved into a Popup
            this.Visibility = Visibility.Collapsed;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _layoutRoot = GetTemplateChild(LayoutRootPanelName) as Panel;
            _contentBorder = GetTemplateChild(ContentBorderName) as Border;
            _inputTextBox = GetTemplateChild(InputTextBoxName) as TextBox;
            _titleTextBlock = GetTemplateChild(TitleTextBlockName) as TextBlock;
            _textTextBlock = GetTemplateChild(TextTextBlockName) as TextBlock;
            _buttonsPanel = GetTemplateChild(ButtonsPanelName) as Panel;

            //_inputTextBox.SetBinding(
            //    TextBox.TextProperty,
            //    new Binding
            //    {
            //        Path = new PropertyPath("InputText"),
            //        Source = this,
            //        Mode = BindingMode.TwoWay
            //    });

            _layoutRoot.Tapped += OnLayoutRootTapped;
            _inputTextBox.Text = this.InputText;
            _inputTextBox.TextChanged += OnInputTextBoxTextChanged;
            _inputTextBox.KeyUp += OnInputTextBoxKeyUp;
            _contentBorder.Tapped += OnContentBorderTapped;
        }

        private void OnGlobalKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Escape)
            {
                DismissDialog();
                e.Handled = true;
            }
        }

        private void OnLayoutRootTapped(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource == sender &&
                this.IsLightDismissEnabled)
            {
                _dismissTaskSource.TrySetResult(CancelButton);
                e.Handled = true;
            }
        }

        private void OnContentBorderTapped(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource == sender)
            {
                FocusOnButton(AcceptButton);
                e.Handled = true;
            }
        }

        private void FocusOnButton(string buttonContent)
        {
            ButtonBase button;

            if (AcceptButton != null &&
                (button = _buttons.FirstOrDefault(b => string.Equals(b.Content, buttonContent))) != null)
            {
                button.Focus(Windows.UI.Xaml.FocusState.Programmatic);
                return;
            }

            if (_buttons.Count > 0)
            {
                button = (Button)_buttons[0];
                button.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            }
        }

        private void OnInputTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            this.InputText = _inputTextBox.Text;
        }

        public async Task<string> ShowAsync(string title, string text, params string[] buttonTexts)
        {
            if (_shown)
            {
                throw new InvalidOperationException("The dialog is already shown.");
            }

            this.Visibility = Visibility.Visible;
            _shown = true;
            //this.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            Window.Current.Content.KeyUp += OnGlobalKeyUp;
            _dismissTaskSource = new TaskCompletionSource<string>();

            _parentPanel = this.Parent as Panel;
            _parentContentControl = this.Parent as ContentControl;

            if (_parentPanel != null)
            {
                _parentPanel.Children.Remove(this);
            }

            if (_parentContentControl != null)
            {
                _parentContentControl.Content = null;
            }

            _dialogPopup = new Popup { Child = this };

            if (_parentPanel != null)
            {
                _parentPanel.Children.Add(_dialogPopup);
                _parentPanel.SizeChanged += OnParentSizeChanged;
            }
            else if (_parentContentControl != null)
            {
                _parentContentControl.Content = _dialogPopup;
                _parentContentControl.SizeChanged += OnParentSizeChanged;
            }
            else
            {
                _temporaryParentPanel = Window.Current.Content.GetFirstDescendantOfType<Panel>();

                if (_temporaryParentPanel != null)
                {
                    _temporaryParentPanel.Children.Add(_dialogPopup);
                    _temporaryParentPanel.SizeChanged += OnParentSizeChanged;
                }
            }

            _dialogPopup.IsOpen = true;
            await this.WaitForLayoutUpdateAsync();
            _titleTextBlock.Text = title;
            _textTextBlock.Text = text;

            _buttons = new List<ButtonBase>();

            foreach (var buttonText in buttonTexts)
            {
                var button = new Button();

                if (this.ButtonStyle != null)
                {
                    button.Style = this.ButtonStyle;
                }

                button.Content = buttonText;
                button.Click += OnButtonClick;
                button.KeyUp += OnGlobalKeyUp;
                _buttons.Add(button);
                _buttonsPanel.Children.Add(button);
            }

            _inputTextBox.Focus(Windows.UI.Xaml.FocusState.Programmatic);

            ResizeLayoutRoot();

            // Show dialog
            await this.GoToVisualStateAsync(_layoutRoot, PopupStatesGroupName, OpenPopupStateName);

            // Wait for button click
            var result = await _dismissTaskSource.Task;

            // Hide dialog
            if (this.AwaitsCloseTransition)
            {
                await this.CloseAsync();
            }
            else
            {
#pragma warning disable 4014
                this.CloseAsync();
#pragma warning restore 4014
            }

            Window.Current.Content.KeyUp -= OnGlobalKeyUp;

            return result;
        }

        private void ResizeLayoutRoot()
        {
            FrameworkElement root =
                _parentPanel ??
                _parentContentControl ??
                _temporaryParentPanel as FrameworkElement;
            _layoutRoot.Width = root.ActualWidth;
            _layoutRoot.Height = root.ActualHeight;
        }

        private void OnParentSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            ResizeLayoutRoot();
        }

        private async Task CloseAsync()
        {
            if (!_shown)
            {
                throw new InvalidOperationException("The dialog isn't shown, so it can't be closed.");
            }

            await this.GoToVisualStateAsync(_layoutRoot, PopupStatesGroupName, ClosedPopupStateName);
            _dialogPopup.IsOpen = false;
            _buttonsPanel.Children.Clear();

            foreach (var button in _buttons)
            {
                button.Click -= OnButtonClick;
                button.KeyUp -= OnGlobalKeyUp;
            }

            _buttons.Clear();

            _dialogPopup.Child = null;

            if (_parentPanel != null)
            {
                _parentPanel.Children.Remove(_dialogPopup);
                _parentPanel.Children.Add(this);
                _parentPanel.SizeChanged -= OnParentSizeChanged;
                _parentPanel = null;
            }

            if (_parentContentControl != null)
            {
                _parentContentControl.Content = this;
                _parentContentControl.SizeChanged -= OnParentSizeChanged;
                _parentContentControl = null;
            }

            if (_temporaryParentPanel != null)
            {
                _temporaryParentPanel.Children.Remove(_dialogPopup);
                _temporaryParentPanel.SizeChanged -= OnParentSizeChanged;
                _temporaryParentPanel = null;
            }

            _dialogPopup = null;
            this.Visibility = Visibility.Collapsed;
            _shown = false;
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            var clickedButton = (ButtonBase)sender;
            _dismissTaskSource.TrySetResult((string)clickedButton.Content);
        }

        private void OnInputTextBoxKeyUp(object sender, KeyRoutedEventArgs e)
        {
            this.InputText = _inputTextBox.Text;

            if (e.Key == VirtualKey.Enter)
            {
                FocusOnButton(AcceptButton);
                e.Handled = true;
                return;
            }

            if (e.Key == VirtualKey.Escape)
            {
                FocusOnButton(CancelButton);
                e.Handled = true;
            }
        }

        private void DismissDialog()
        {
            if (CancelButton != null)
            {
                _dismissTaskSource.TrySetResult(CancelButton);
            }

            if (_buttons.Count > 0)
            {
                var button = (Button)_buttons[0];
                button.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            }
        }
    }
}
