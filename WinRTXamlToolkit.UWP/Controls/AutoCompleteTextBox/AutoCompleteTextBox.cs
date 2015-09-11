using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using WinRTXamlToolkit.Controls.Extensions;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// A TextBox-like control with auto-complete suggestions dropdown.
    /// </summary>
    [TemplatePart(Name = TextBoxPropertyName, Type = typeof(TextBox))]
    [TemplatePart(Name = AutoCompletePresenterPropertyName, Type = typeof(Popup))]
    [TemplatePart(Name = AutocompleteItemsContainerPropertyName, Type = typeof(ItemsControl))]
    [TemplateVisualState(Name = StateDisabled, GroupName = GroupCommon)]
    [TemplateVisualState(Name = StateNormal, GroupName = GroupCommon)]
    [TemplateVisualState(Name = StatePointerOver, GroupName = GroupCommon)]
    public sealed partial class AutoCompleteTextBox : Control
    {
        #region Constants
        private const string TextBoxPropertyName = "TextBox";
        private const string AutoCompletePresenterPropertyName = "AutoCompletePresenter";
        private const string AutocompleteItemsContainerPropertyName = "AutocompleteItemsContainer";

        private const string GroupCommon = "CommonStates";
        private const string StateDisabled = "Disabled";
        private const string StateNormal = "Normal";
        private const string StatePointerOver = "PointerOver";
        #endregion

        #region Template controls
        private UpDownTextBox textBox;
        private Popup autoCompletePresenter;
        private SimpleThemingListBox autocompleteItemsContainer;
        #endregion

        #region Settings
        private SuggestionPopupDisplaySide suggestionPopupDisplaySide;

        /// <summary>
        /// Specifies options for where the suggestion popup is showing up.
        /// </summary>
        public enum SuggestionPopupDisplaySide
        {
            /// <summary>
            /// The suggestions popup shows up above the TextBox.
            /// </summary>
            Top = 0,
            /// <summary>
            /// The suggestions popup shows up below the TextBox.
            /// </summary>
            Bottom = 1
        }

        /// <summary>
        /// Specifies visual states for AutoCompleteTextBox.
        /// </summary>
        public enum VisualControlState
        {
            /// <summary>
            /// The disabled state
            /// </summary>
            Disabled = 0,
            /// <summary>
            /// The normal state.
            /// </summary>
            Normal = 1,
            /// <summary>
            /// The pointer over state.
            /// </summary>
            PointerOver = 2
        }
        #endregion settings

        #region Dependency Properties
        #region ScrollBarBackground
        /// <summary>
        /// ScrollBarBackground Dependency Property
        /// </summary>
        private static readonly DependencyProperty _ScrollBarBackgroundProperty =
            DependencyProperty.Register(
                "ScrollBarBackground",
                typeof(Brush),
                typeof(AutoCompleteTextBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the ScrollBarBackground dependency property.
        /// </summary>
        public static DependencyProperty ScrollBarBackgroundProperty { get { return _ScrollBarBackgroundProperty; } }

        /// <summary>
        /// Gets or sets the ScrollBarBackground property. This dependency property 
        /// indicates the background of the ScrollBar.
        /// </summary>
        public Brush ScrollBarBackground
        {
            get { return (Brush)GetValue(ScrollBarBackgroundProperty); }
            set { this.SetValue(ScrollBarBackgroundProperty, value); }
        }
        #endregion

        #region ScrollBarBorderBrush
        /// <summary>
        /// ScrollBarBorderBrush Dependency Property
        /// </summary>
        private static readonly DependencyProperty _ScrollBarBorderBrushProperty =
            DependencyProperty.Register(
                "ScrollBarBorderBrush",
                typeof(Brush),
                typeof(AutoCompleteTextBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the ScrollBarBorderBrush dependency property.
        /// </summary>
        public static DependencyProperty ScrollBarBorderBrushProperty { get { return _ScrollBarBorderBrushProperty; } }

        /// <summary>
        /// Gets or sets the ScrollBarBorderBrush property. This dependency property 
        /// indicates the brush used for the border of the ScrollBar.
        /// </summary>
        public Brush ScrollBarBorderBrush
        {
            get { return (Brush)GetValue(ScrollBarBorderBrushProperty); }
            set { this.SetValue(ScrollBarBorderBrushProperty, value); }
        }
        #endregion

        #region ScrollBarBorderThickness
        /// <summary>
        /// ScrollBarBorderThickness Dependency Property
        /// </summary>
        private static readonly DependencyProperty _ScrollBarBorderThicknessProperty =
            DependencyProperty.Register(
                "ScrollBarBorderThickness",
                typeof(Thickness),
                typeof(AutoCompleteTextBox),
                new PropertyMetadata(new Thickness(0)));

        /// <summary>
        /// Identifies the ScrollBarBorderThickness dependency property.
        /// </summary>
        public static DependencyProperty ScrollBarBorderThicknessProperty { get { return _ScrollBarBorderThicknessProperty; } }

        /// <summary>
        /// Gets or sets the ScrollBarBorderThickness property. This dependency property 
        /// indicates the thickness of the ScrollBarBorder.
        /// </summary>
        public Thickness ScrollBarBorderThickness
        {
            get { return (Thickness)GetValue(ScrollBarBorderThicknessProperty); }
            set { this.SetValue(ScrollBarBorderThicknessProperty, value); }
        }
        #endregion

        #region ItemPointerOverBackgroundThemeBrush
        /// <summary>
        /// ItemPointerOverBackgroundThemeBrush Dependency Property
        /// </summary>
        private static readonly DependencyProperty _ItemPointerOverBackgroundThemeBrushProperty =
            DependencyProperty.Register(
                "ItemPointerOverBackgroundThemeBrush",
                typeof(Brush),
                typeof(AutoCompleteTextBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the ItemPointerOverBackgroundThemeBrush dependency property.
        /// </summary>
        public static DependencyProperty ItemPointerOverBackgroundThemeBrushProperty { get { return _ItemPointerOverBackgroundThemeBrushProperty; } }

        /// <summary>
        /// Gets or sets the ItemPointerOverBackgroundThemeBrush property. This dependency property 
        /// indicates the brush used for the background of a hovered item.
        /// </summary>
        public Brush ItemPointerOverBackgroundThemeBrush
        {
            get { return (Brush)GetValue(ItemPointerOverBackgroundThemeBrushProperty); }
            set { this.SetValue(ItemPointerOverBackgroundThemeBrushProperty, value); }
        }
        #endregion

        #region ItemPointerOverForegroundThemeBrush
        /// <summary>
        /// ItemPointerOverForegroundThemeBrush Dependency Property
        /// </summary>
        private static readonly DependencyProperty _ItemPointerOverForegroundThemeBrushProperty =
            DependencyProperty.Register(
                "ItemPointerOverForegroundThemeBrush",
                typeof(Brush),
                typeof(AutoCompleteTextBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the ItemPointerOverForegroundThemeBrush dependency property.
        /// </summary>
        public static DependencyProperty ItemPointerOverForegroundThemeBrushProperty { get { return _ItemPointerOverForegroundThemeBrushProperty; } }

        /// <summary>
        /// Gets or sets the ItemPointerOverForegroundThemeBrush property. This dependency property 
        /// indicates the brush to use for the foreground of a hovered item.
        /// </summary>
        public Brush ItemPointerOverForegroundThemeBrush
        {
            get { return (Brush)GetValue(ItemPointerOverForegroundThemeBrushProperty); }
            set { this.SetValue(ItemPointerOverForegroundThemeBrushProperty, value); }
        }
        #endregion

        #region MaximumVisibleSuggestions
        /// <summary>
        /// MaximumVisibleSuggestions Dependency Property
        /// </summary>
        private static readonly DependencyProperty _MaximumVisibleSuggestionsProperty =
            DependencyProperty.Register(
                "MaximumVisibleSuggestions",
                typeof(int),
                typeof(AutoCompleteTextBox),
                new PropertyMetadata(3));

        /// <summary>
        /// Identifies the MaximumVisibleSuggestions dependency property.
        /// </summary>
        public static DependencyProperty MaximumVisibleSuggestionsProperty { get { return _MaximumVisibleSuggestionsProperty; } }

        /// <summary>
        /// Gets or sets the MaximumVisibleSuggestions property. This dependency property 
        /// indicates the maximum number of suggestions visible without scrolling.
        /// </summary>
        public int MaximumVisibleSuggestions
        {
            get { return (int)GetValue(MaximumVisibleSuggestionsProperty); }
            set { this.SetValue(MaximumVisibleSuggestionsProperty, value); }
        }
        #endregion

        #region AutoCompleteService
        /// <summary>
        /// AutoCompleteService Dependency Property
        /// </summary>
        private static readonly DependencyProperty _AutoCompleteServiceProperty =
            DependencyProperty.Register(
                "AutoCompleteService",
                typeof(object),
                typeof(AutoCompleteTextBox),
                new PropertyMetadata(new DamerauLevenshteinDistance()));

        /// <summary>
        /// Identifies the AutoCompleteService dependency property.
        /// </summary>
        public static DependencyProperty AutoCompleteServiceProperty { get { return _AutoCompleteServiceProperty; } }

        /// <summary>
        /// Gets or sets the AutoCompleteService property. This dependency property 
        /// indicates the service that handles the auto complete requests.
        /// The service controls the algorithm of getting suggestions.
        /// The default is Levensthein Distance algorithm.
        /// object instead of IAutoCompletable because of:
        /// http://stackoverflow.com/questions/20906222/interface-as-dependency-property-converter-failed
        /// </summary>
        public object AutoCompleteService
        {
            get { return (object)GetValue(AutoCompleteServiceProperty); }
            set { this.SetValue(AutoCompleteServiceProperty, value); }
        }
        #endregion

        #region WordDictionary
        /// <summary>
        /// WordDictionary Dependency Property
        /// </summary>
        private static readonly DependencyProperty _WordDictionaryProperty =
            DependencyProperty.Register(
                "WordDictionary",
                typeof(object), // Note: object instead of ICollection<string> because of same reason as im AutoCompleteService
                typeof(AutoCompleteTextBox),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the WordDictionary dependency property.
        /// </summary>
        public static DependencyProperty WordDictionaryProperty { get { return _WordDictionaryProperty; } }

        /// <summary>
        /// Gets or sets the WordDictionary property. This dependency property 
        /// indicates the word dictionary to use for suggestions.
        /// </summary>
        public object WordDictionary
        {
            get { return (object)GetValue(WordDictionaryProperty); }
            set { this.SetValue(WordDictionaryProperty, value); }
        }
        #endregion

        #region Text
        /// <summary>
        /// Text Dependency Property
        /// </summary>
        private static readonly DependencyProperty _TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(AutoCompleteTextBox),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// Identifies the Text dependency property.
        /// </summary>
        public static DependencyProperty TextProperty { get { return _TextProperty; } }

        /// <summary>
        /// Gets or sets a value indicating the text displayed in the control.
        /// </summary>
        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }
        #endregion
        #endregion

        // this property informs how was text changed raised (by user typing or not)
        // for instance: if TextBox.Text is changed by selecting suggestion isRaisedByUser will be false and next suggestion popup won't
        // be launched.
        private bool isRaisedByUserTyping = true;

        private bool isLoaded = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoCompleteTextBox"/> class.
        /// </summary>
        public AutoCompleteTextBox()
        {
            this.DefaultStyleKey = typeof(AutoCompleteTextBox);
            this.SizeChanged += this.OnSizeChanged;
            this.Loaded += this.OnLoaded;
            this.Unloaded += this.OnUnloaded;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            if (this.autoCompletePresenter != null)
            {
                this.autoCompletePresenter.Width = this.ActualWidth;
            }

            if (this.autocompleteItemsContainer != null)
            {
                this.autocompleteItemsContainer.Width = this.ActualWidth;
            }
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            this.UnsubscribeEvents();
            this.textBox = this.GetTemplateChild(TextBoxPropertyName) as UpDownTextBox;
            this.autoCompletePresenter = this.GetTemplateChild(AutoCompletePresenterPropertyName) as Popup;
            this.autoCompletePresenter.Width = this.ActualWidth;
            this.autocompleteItemsContainer = this.GetTemplateChild(AutocompleteItemsContainerPropertyName) as SimpleThemingListBox;
            this.autocompleteItemsContainer.Width = this.ActualWidth;
            this.SetBorderThickness();
            base.OnApplyTemplate();
            this.SubscribeEvents();
        }

        /// <summary>
        /// Called before the PointerEntered event occurs.
        /// </summary>
        /// <param name="e">Event data for the event.</param>
        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            this.UpdateState(VisualControlState.PointerOver);
        }

        /// <summary>
        /// Called before the PointerExited event occurs.
        /// </summary>
        /// <param name="e">Event data for the event.</param>
        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            this.UpdateState(VisualControlState.Normal);
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.isLoaded = true;
            this.SubscribeEvents();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.isLoaded = false;
            this.UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            if (!this.isLoaded)
            {
                return;
            }

            if (this.textBox != null)
            {
                this.textBox.TextChanged += this.OnTextChanged;
                this.textBox.KeyDown += this.OnTextBoxKeyDown;
                this.textBox.UpPressed += this.OnTextBoxUpPressed;
                this.textBox.DownPressed += this.OnTextBoxDownPressed;
            }

            if (this.autoCompletePresenter != null)
            {
                this.autoCompletePresenter.Closed += this.OnAutoCompleteClosed;
            }

            if (this.autocompleteItemsContainer != null)
            {
                this.autocompleteItemsContainer.SelectionChanged += this.OnAutocompleteItemsContainerItemSelectionChanged;
            }

            this.LostFocus += this.OnLostFocus;
        }

        private void UnsubscribeEvents()
        {
            if (this.textBox != null)
            {
                this.textBox.TextChanged -= this.OnTextChanged;
                this.textBox.KeyDown -= this.OnTextBoxKeyDown;
                this.textBox.UpPressed -= this.OnTextBoxUpPressed;
                this.textBox.DownPressed -= this.OnTextBoxDownPressed;
            }

            if (this.autoCompletePresenter != null)
            {
                this.autoCompletePresenter.Closed -= this.OnAutoCompleteClosed;
            }

            if (this.autocompleteItemsContainer != null)
            {
                this.autocompleteItemsContainer.SelectionChanged -= this.OnAutocompleteItemsContainerItemSelectionChanged;
            }

            this.LostFocus -= this.OnLostFocus;
        }

        private void OnTextBoxKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Escape)
            {
                this.HideAutoCompleteSuggestions();
            }
            else if (e.Key == VirtualKey.F2)
            {
                this.TryOpenAutoComplete(this.Text);
            }
            else if (e.Key == VirtualKey.Enter &&
                     this.autoCompletePresenter != null &&
                     this.autoCompletePresenter.IsOpen)
            {
                this.HideAutoCompleteSuggestions();
            }
        }

        private void OnTextBoxDownPressed(object sender, EventArgs e)
        {
            if (this.autoCompletePresenter != null &&
                this.autocompleteItemsContainer != null)
            {
                if (!this.autoCompletePresenter.IsOpen)
                {
                    this.TryOpenAutoComplete(this.Text);

                    var suggestions = this.autocompleteItemsContainer.ItemsSource as ICollection<string>;

                    if (suggestions != null &&
                        suggestions.Count > 0)
                    {
                        this.autocompleteItemsContainer.SelectedIndex = 0;
                    }
                }
                else
                {
                    var suggestions = this.autocompleteItemsContainer.ItemsSource as ICollection<string>;

                    if (suggestions != null &&
                        suggestions.Count > this.autocompleteItemsContainer.SelectedIndex + 1)
                    {
                        this.autocompleteItemsContainer.SelectedIndex += 1;
                        //TODO: Scroll into view here and in other SelectedIndex manipulation cases
                    }
                }
            }
        }

        private void OnTextBoxUpPressed(object sender, EventArgs e)
        {
            if (this.autoCompletePresenter != null &&
                this.autocompleteItemsContainer != null)
            {
                if (!this.autoCompletePresenter.IsOpen)
                {
                    this.TryOpenAutoComplete(this.Text);

                    var suggestions = this.autocompleteItemsContainer.ItemsSource as ICollection<string>;

                    if (suggestions != null &&
                        suggestions.Count > 0)
                    {
                        this.autocompleteItemsContainer.SelectedIndex = suggestions.Count - 1;
                    }
                }
                else
                {
                    var suggestions = this.autocompleteItemsContainer.ItemsSource as ICollection<string>;

                    if (suggestions != null &&
                        suggestions.Count > 0)
                    {
                        if (this.autocompleteItemsContainer.SelectedIndex > 0)
                        {
                            this.autocompleteItemsContainer.SelectedIndex -= 1;
                        }
                        else if (this.autocompleteItemsContainer.SelectedIndex == -1)
                        {
                            this.autocompleteItemsContainer.SelectedIndex = suggestions.Count - 1;
                        }
                    }
                }
            }
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            var fe = FocusManager.GetFocusedElement() as DependencyObject;

            if (fe != null &&
                this.autocompleteItemsContainer != null &&
                fe.GetAncestors().Contains(this.autocompleteItemsContainer))
            {
                return;
            }

            this.HideAutoCompleteSuggestions();
        }

        private void OnAutoCompleteClosed(object sender, object e)
        {
            this.SetBorderThickness();
        }

        private void OnAutocompleteItemsContainerItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSuggestion = autocompleteItemsContainer.SelectedItem as string;

            if (selectedSuggestion != null)
            {
                this.isRaisedByUserTyping = false; // text change raised by selecting suggestion
                this.Text = selectedSuggestion;
                this.textBox.SelectionStart = 0;
                this.textBox.SelectionLength = selectedSuggestion.Length;
                this.textBox.Focus(FocusState.Programmatic);
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            this.textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            if (this.isRaisedByUserTyping)
            {
                this.TryOpenAutoComplete(this.Text);
            }
            else
            {
                this.isRaisedByUserTyping = true;
            }
        }

        /// <summary>
        /// Tries to open auto-completion suggestions.
        /// </summary>
        /// <param name="textToComplete">The text to complete.</param>
        public void TryOpenAutoComplete(string textToComplete)
        {
            var autoCompletableService = AutoCompleteService as IAutoCompletable;
            var wordDictionary = this.WordDictionary as ICollection<string>;

            if (autoCompletableService != null &&
                wordDictionary != null)
            {
                var suggestedWords =
                    autoCompletableService.GetSuggestedWords(
                        textToComplete,
                        wordDictionary);

                if (suggestedWords.Any())
                {
                    this.ShowAutoCompletePopup(suggestedWords);
                }
                else
                {
                    this.HideAutoCompleteSuggestions();
                }
            }
        }

        private void ShowAutoCompletePopup(ICollection<string> suggestedWords)
        {
            this.autocompleteItemsContainer.ItemsSource = suggestedWords;
            this.autoCompletePresenter.IsOpen = true; // IsOpen should be called first - to get view adjusting working
            this.AdjustSuggestionItemContainerSize(suggestedWords.Count);
            this.SetCompletionDisplaySide();
            this.SetBorderThickness();
        }

        /// <summary>
        /// Hides the auto-complete suggestions.
        /// </summary>
        public void HideAutoCompleteSuggestions()
        {
            this.autoCompletePresenter.IsOpen = false;
            this.autocompleteItemsContainer.ItemsSource = null;
            this.SetBorderThickness();
        }

        private void AdjustSuggestionItemContainerSize(int suggestedWordsCount)
        {
            int displayedWithoutScrollingItemsCount = (suggestedWordsCount > MaximumVisibleSuggestions) ? MaximumVisibleSuggestions : suggestedWordsCount;
            this.autocompleteItemsContainer.Height = displayedWithoutScrollingItemsCount * autocompleteItemsContainer.GetItemHeight() + 5;
            // 5 is added to height to prevent ScrollViewer to show up even if there is no need to scroll
        }

        private void SetCompletionDisplaySide()
        {
            var popupMargin = new Thickness(0);
            suggestionPopupDisplaySide = this.GetDisplaySide();

            switch (suggestionPopupDisplaySide)
            {
                case SuggestionPopupDisplaySide.Top:
                    var autoCompleteHeight = this.CalculateAutoCompleteListBoxHeight();
                    popupMargin.Top = -autoCompleteHeight;
                    break;
                case SuggestionPopupDisplaySide.Bottom:
                    popupMargin.Top = this.textBox.ActualHeight;
                    break;
            }

            this.autoCompletePresenter.Margin = popupMargin;
        }

        private SuggestionPopupDisplaySide GetDisplaySide()
        {
            double textBoxBottomBound = this.GetSuggestionPopupVerticalEndCoordinate();
            double windowBottomBound = Window.Current.Bounds.Bottom;

            if (textBoxBottomBound > windowBottomBound)
            {
                return SuggestionPopupDisplaySide.Top;
            }
            else
            {
                return SuggestionPopupDisplaySide.Bottom;
            }
        }

        private double GetSuggestionPopupVerticalEndCoordinate()
        {
            var textBoxTransform = this.textBox.TransformToVisual(Window.Current.Content);
            Point textBoxPosition = textBoxTransform.TransformPoint(new Point(0, 0));
            double autoCompletePresenterHeight = this.CalculateAutoCompleteListBoxHeight();
            double textBoxBottomBound = textBoxPosition.Y + textBox.ActualHeight + autoCompletePresenterHeight;

            return textBoxBottomBound;
        }

        private double CalculateAutoCompleteListBoxHeight()
        {
            var autoCompleteItems = autocompleteItemsContainer.ItemsSource as ICollection<string>;

            if (autoCompleteItems == null || !autoCompleteItems.Any())
            {
                return 0.0d;
            }

            double singleItemHeight = autocompleteItemsContainer.GetItemHeight();
            double suggestionCount =
                (autoCompleteItems.Count > MaximumVisibleSuggestions)
                ? MaximumVisibleSuggestions
                : autoCompleteItems.Count;

            return singleItemHeight * suggestionCount;
        }

        private void SetBorderThickness()
        {
            Thickness textBoxThickenss, suggestionsItemsContainerThickness;

            if (autoCompletePresenter.IsOpen)
            {
                if (suggestionPopupDisplaySide == SuggestionPopupDisplaySide.Bottom)
                {
                    textBoxThickenss = new Thickness(1, 1, 1, 0);
                    suggestionsItemsContainerThickness = new Thickness(1, 0, 1, 1);
                }
                else if (suggestionPopupDisplaySide == SuggestionPopupDisplaySide.Top)
                {
                    textBoxThickenss = new Thickness(1, 0, 1, 1);
                    suggestionsItemsContainerThickness = new Thickness(1, 1, 1, 0);
                }
            }
            else
            {
                textBoxThickenss = new Thickness(1);
                suggestionsItemsContainerThickness = new Thickness(0);
            }

            this.textBox.BorderThickness = textBoxThickenss;
            this.autocompleteItemsContainer.BorderThickness = suggestionsItemsContainerThickness;
        }

        private void UpdateState(VisualControlState newState)
        {
            switch (newState)
            {
                case VisualControlState.Disabled:
                    VisualStateManager.GoToState(this, StateDisabled, false);
                    break;
                case VisualControlState.Normal:
                    VisualStateManager.GoToState(this, StateNormal, false);
                    break;
                case VisualControlState.PointerOver:
                    VisualStateManager.GoToState(this, StatePointerOver, false);
                    break;
            }
        }
    }
}
