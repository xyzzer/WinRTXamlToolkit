using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using WinRTXamlToolkit.Controls.AutoCompleteTextBoxControl;
using WinRTXamlToolkit.Controls.AutoCompleteTextBoxControl.Algorithm;

namespace WinRTXamlToolkit.Controls
{
    [TemplatePart(Name = TextBoxPropertyName, Type = typeof(TextBox))]
    [TemplatePart(Name = AutocompletePresenterPropertyName, Type = typeof(Popup))]
    [TemplatePart(Name = AutocompleteItemsContainerPropertyName, Type = typeof(ItemsControl))]
    [TemplateVisualState(Name=StateDisabled, GroupName=GroupCommon)]
    [TemplateVisualState(Name=StateNormal, GroupName=GroupCommon)]
    [TemplateVisualState(Name=StatePointerOver, GroupName=GroupCommon)]
    public sealed class AutoCompleteTextBox : Control
    {
        #region Constants
        private const string TextBoxPropertyName = "TextBox";
        private const string AutocompletePresenterPropertyName = "AutocompletePresenter";
        private const string AutocompleteItemsContainerPropertyName = "AutocompleteItemsContainer";
       
        private const string GroupCommon = "CommonStates";
        private const string StateDisabled = "Disabled";
        private const string StateNormal = "Normal";
        private const string StatePointerOver = "PointerOver";
        #endregion

        #region Template controls
        private TextBox textBox;
        private Popup suggestionPopupPresenter;
        private CustomListBox suggestionsItemsContainer;
        #endregion

        #region settings
        SuggestionPopupDisplaySide suggestionPopupDisplaySide;
        public enum SuggestionPopupDisplaySide
        {
            Top = 0,
            Bottom = 1
        }

        public enum VisualControlState
        {
            Disabled = 0,
            Normal = 1,
            PointerOver = 2
        }


        #endregion settings 

        #region Dependency Properties

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Brush ScrollBarBackground
        {
            get { return (Brush)GetValue(ScrollBarBackgroundProperty); }
            set { SetValue(ScrollBarBackgroundProperty, value); }
        }

        public Brush ScrollBarBorderBrush
        {
            get { return (Brush)GetValue(ScrollBarBorderBrushProperty); }
            set { SetValue(ScrollBarBorderBrushProperty, value); }
        }

        public Thickness ScrollBarBorderThickness
        {
            get { return (Thickness)GetValue(ScrollBarBorderThicknessProperty); }
            set { SetValue(ScrollBarBorderThicknessProperty, value); }
        }

        public Brush ItemPointerOverBackgroundThemeBrush
        {
            get { return (Brush)GetValue(ItemPointerOverBackgroundThemeBrushProperty); }
            set { SetValue(ItemPointerOverBackgroundThemeBrushProperty, value); }
        }

        public Brush ItemPointerOverForegroundThemeBrush
        {
            get { return (Brush)GetValue(ItemPointerOverForegroundThemeBrushProperty); }
            set { SetValue(ItemPointerOverForegroundThemeBrushProperty, value); }
        }

        /// <summary>
        /// Controls algorithm of getting suggestions. Default is Levensthein Distance algorithm.
        /// object instead of IAutoCompletable because of:
        /// http://stackoverflow.com/questions/20906222/interface-as-dependency-property-converter-failed
        /// </summary>
        public object AutoCompleteService
        {
            get { return (IAutoCompletable)GetValue(AutoCompleteServiceProperty); }
            set { SetValue(AutoCompleteServiceProperty, value); }
        }

        /// <summary>
        /// This property controls maximum count of displayed suggestions without need to scroll.
        /// </summary>
        public int MaximumVisibleSuggestions
        {
            get { return (int)GetValue(MaximumVisibleSuggestionsProperty); }
            set { SetValue(MaximumVisibleSuggestionsProperty, value); }
        }
        
        // object instead of ICollection<string> because of same reason as im AutoCompleteService
        public object WordDictionary
        {
            get { return (ICollection<string>)GetValue(WordDictionaryProperty); }
            set { SetValue(WordDictionaryProperty, value); }
        }     

        public static readonly DependencyProperty TextProperty = 
            DependencyProperty.Register("Text", typeof(string), typeof(AutoCompleteTextBox), new PropertyMetadata(string.Empty, TextPropertyChanged));

        public static readonly DependencyProperty ScrollBarBackgroundProperty =
            DependencyProperty.Register("ScrollBarBackground", typeof(Brush), typeof(AutoCompleteTextBox), new PropertyMetadata(null));

        public static readonly DependencyProperty ScrollBarBorderBrushProperty =
            DependencyProperty.Register("ScrollBarBorderBrush", typeof(Brush), typeof(AutoCompleteTextBox), new PropertyMetadata(null));

        public static readonly DependencyProperty ScrollBarBorderThicknessProperty =
            DependencyProperty.Register("ScrollBarBorderThickness", typeof(Thickness), typeof(AutoCompleteTextBox), new PropertyMetadata(0));

        public static readonly DependencyProperty ItemPointerOverBackgroundThemeBrushProperty =
              DependencyProperty.Register("ItemPointerOverBackgroundThemeBrush", typeof(Brush), typeof(AutoCompleteTextBox), new PropertyMetadata(null));

        public static readonly DependencyProperty ItemPointerOverForegroundThemeBrushProperty =
            DependencyProperty.Register("ItemPointerOverForegroundThemeBrush", typeof(Brush), typeof(AutoCompleteTextBox), new PropertyMetadata(null));

        public static readonly DependencyProperty AutoCompleteServiceProperty =
            DependencyProperty.Register("AutoCompleteService", typeof(object), typeof(AutoCompleteTextBox),
            new PropertyMetadata(new DamerauLevenstheinDistance()));
        
        public static readonly DependencyProperty MaximumVisibleSuggestionsProperty =
            DependencyProperty.Register("MaximumVisibleSuggestions", typeof(int), typeof(AutoCompleteTextBox), new PropertyMetadata(3));

        public static readonly DependencyProperty WordDictionaryProperty =
    DependencyProperty.Register("WordDictionary", typeof(object), typeof(AutoCompleteTextBox), new PropertyMetadata(null));

        private static void TextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as AutoCompleteTextBox;
            if (sender == null)
                return;

            var newValue = e.NewValue as string;
            var oldValue = e.OldValue as string;
            if (!newValue.Equals(oldValue))
                sender.Text = newValue;
        }
        #endregion

        // this property informs how was text changed raised (by user typing or not)
        // for instance: if TextBox.Text is changed by selecting suggestion isRaisedByUser will be false and next suggestion popup won't
        // be launched.
        bool isRaisedByUserTyping = true;

        public AutoCompleteTextBox()
        {
            this.DefaultStyleKey = typeof(AutoCompleteTextBox);
        }

        protected override void OnApplyTemplate()
        {
            this.textBox = this.GetTemplateChild(TextBoxPropertyName) as TextBox;
            this.suggestionPopupPresenter = this.GetTemplateChild(AutocompletePresenterPropertyName) as Popup;
            this.suggestionsItemsContainer = this.GetTemplateChild(AutocompleteItemsContainerPropertyName) as CustomListBox;

            SetBorderThickness();

            InitializeEvents();
            this.Unloaded += AutoCompleteTextBox_Unloaded;

            base.OnApplyTemplate();
        }

 
        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            UpdateState(VisualControlState.PointerOver);
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);
            UpdateState(VisualControlState.Normal);
        }

        private void InitializeEvents()
        {
            this.textBox.TextChanged += AutoCompleteTexBox_TextChanged;
            this.suggestionPopupPresenter.Closed += SuggestionPopup_Closed;
            this.suggestionsItemsContainer.SelectionChanged += SuggestionsItemsContainer_ItemSelectionChanged;
            this.LostFocus += AutoCompleteTextBox_LostFocus;
        }

        void AutoCompleteTextBox_Unloaded(object sender, RoutedEventArgs e)
        {
            this.textBox.TextChanged -= AutoCompleteTexBox_TextChanged;
            this.suggestionPopupPresenter.Closed -= SuggestionPopup_Closed;
            this.suggestionsItemsContainer.SelectionChanged -= SuggestionsItemsContainer_ItemSelectionChanged;
            this.LostFocus -= AutoCompleteTextBox_LostFocus;
            this.Unloaded -= AutoCompleteTextBox_Unloaded;
        }
        private void AutoCompleteTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            HideAutoCompleteSuggestions();
        }

        private void SuggestionPopup_Closed(object sender, object e)
        {
            SetBorderThickness();
        }

        private void SuggestionsItemsContainer_ItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSuggestion = suggestionsItemsContainer.SelectedItem as string;
            if ( selectedSuggestion != null)
            {
                isRaisedByUserTyping = false; // text change raised by selecting suggestion
                this.Text = selectedSuggestion;
                textBox.SelectionStart = selectedSuggestion.Length;
                this.textBox.Focus(FocusState.Programmatic);
            }
        }

        private void AutoCompleteTexBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();

            if (isRaisedByUserTyping)
            {
                this.TryOpenAutoComplete(this.Text);
            }
            else
            {
                isRaisedByUserTyping = true;
            }
        }

        public void TryOpenAutoComplete(string textToComplete)
        {
            if (AutoCompleteService as IAutoCompletable != null && WordDictionary as ICollection<string> != null)
            {
                var suggestedWords = ((IAutoCompletable)AutoCompleteService).GetSuggestedWords(textToComplete, (ICollection<string>)WordDictionary);

                if (suggestedWords.Any())
                    ShowAutoCompletePopup(suggestedWords);
                else
                    HideAutoCompleteSuggestions();
            }
 
        }
        private void ShowAutoCompletePopup(IList<string> suggestedWords)
        {
            this.suggestionsItemsContainer.ItemsSource = suggestedWords;
            this.suggestionPopupPresenter.IsOpen = true; // IsOpen should be called first - to get view adjusting working
            AdjustSuggestionItemContainerSize(suggestedWords.Count);
            SetCompletionDisplaySide();
            SetBorderThickness();
        }

        public void HideAutoCompleteSuggestions()
        {
            this.suggestionPopupPresenter.IsOpen = false;
            this.suggestionsItemsContainer.ItemsSource = null;
            SetBorderThickness();
        }

        private void AdjustSuggestionItemContainerSize(int suggestedWordsCount)
        {
            int displayedWithoutScrollingItemsCount = (suggestedWordsCount > MaximumVisibleSuggestions) ? MaximumVisibleSuggestions : suggestedWordsCount;
            this.suggestionsItemsContainer.Height = displayedWithoutScrollingItemsCount * suggestionsItemsContainer.GetItemHeight() + 5;
            // 5 is added to height to prevent ScrollViewer to show up even if there is no need to scroll
        }
        private void SetCompletionDisplaySide()
        {
            Thickness popupMargin = new Thickness(0);
            suggestionPopupDisplaySide = GetDisplaySide();

            switch (suggestionPopupDisplaySide)
            {
                case SuggestionPopupDisplaySide.Top:
                    {
                        var autoCompleteHeight = CalculateAutoCompleteListBoxHeight();
                        popupMargin.Top = - autoCompleteHeight;
                        break;
                    }
                case SuggestionPopupDisplaySide.Bottom:
                    {
                        popupMargin.Top = this.textBox.ActualHeight;
                        break;
                    }
            }

            this.suggestionPopupPresenter.Margin = popupMargin;
        }

        private SuggestionPopupDisplaySide GetDisplaySide()
        {
            double textBoxBottomBound = GetSuggestionPopupVerticalEndCoordinate();
            double windowBottomBound = Window.Current.Bounds.Bottom;

            return (textBoxBottomBound > windowBottomBound) ? SuggestionPopupDisplaySide.Top : SuggestionPopupDisplaySide.Bottom;
        }

        private double GetSuggestionPopupVerticalEndCoordinate()
        {
            var textBoxTransform = this.textBox.TransformToVisual(Window.Current.Content);
            Point textBoxPosition = textBoxTransform.TransformPoint(new Point(0, 0));
            double autoCompletePresenterHeight = CalculateAutoCompleteListBoxHeight();
            double textBoxBottomBound = textBoxPosition.Y + textBox.ActualHeight + autoCompletePresenterHeight;

            return textBoxBottomBound;
        }

        private double CalculateAutoCompleteListBoxHeight()
        {
            var autoCompleteItems = suggestionsItemsContainer.ItemsSource as IList<string>;
            if (autoCompleteItems == null || !autoCompleteItems.Any())
                return 0.0d;

            double singleItemHeight = suggestionsItemsContainer.GetItemHeight();
            double suggestionCount = (autoCompleteItems.Count > MaximumVisibleSuggestions) ? MaximumVisibleSuggestions : autoCompleteItems.Count;

            return singleItemHeight * suggestionCount;
        }
  

        private void SetBorderThickness()
        {
            Thickness textBoxThickenss, suggestionsItemsContainerThickness;

            if ( suggestionPopupPresenter.IsOpen )
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
            this.suggestionsItemsContainer.BorderThickness = suggestionsItemsContainerThickness;
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
