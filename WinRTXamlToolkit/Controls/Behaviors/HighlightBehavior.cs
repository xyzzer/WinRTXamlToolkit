using System;
using WinRTXamlToolkit.Interactivity;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Controls.Behaviors
{
    /// <summary>
    /// An attached behavior used to highlight instances of the SearchString in a TextBlock.
    /// </summary>
    public class HighlightBehavior : Behavior<TextBlock>
    {
        #region SearchString
        /// <summary>
        /// SearchString Dependency Property
        /// </summary>
        public static readonly DependencyProperty SearchStringProperty =
            DependencyProperty.Register(
                "SearchString",
                typeof(string),
                typeof(HighlightBehavior),
                new PropertyMetadata(null, OnSearchStringChanged));

        /// <summary>
        /// Gets or sets the SearchString property. This dependency property 
        /// indicates the search string to highlight in the associated TextBlock.
        /// </summary>
        public string SearchString
        {
            get { return (string)GetValue(SearchStringProperty); }
            set { SetValue(SearchStringProperty, value); }
        }

        /// <summary>
        /// Handles changes to the SearchString property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnSearchStringChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (HighlightBehavior)d;
            string oldSearchString = (string)e.OldValue;
            string newSearchString = target.SearchString;
            target.OnSearchStringChanged(oldSearchString, newSearchString);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the SearchString property.
        /// </summary>
        /// <param name="oldSearchString">The old SearchString value</param>
        /// <param name="newSearchString">The new SearchString value</param>
        private void OnSearchStringChanged(
            string oldSearchString, string newSearchString)
        {
            UpdateHighlight();
        }
        #endregion

        #region IsCaseSensitive
        /// <summary>
        /// IsCaseSensitive Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsCaseSensitiveProperty =
            DependencyProperty.Register(
                "IsCaseSensitive",
                typeof(bool),
                typeof(HighlightBehavior),
                new PropertyMetadata(false, OnIsCaseSensitiveChanged));

        /// <summary>
        /// Gets or sets the IsCaseSensitive property. This dependency property 
        /// indicates whether the highlight behavior is case sensitive.
        /// </summary>
        public bool IsCaseSensitive
        {
            get { return (bool)GetValue(IsCaseSensitiveProperty); }
            set { SetValue(IsCaseSensitiveProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IsCaseSensitive property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnIsCaseSensitiveChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (HighlightBehavior)d;
            bool oldIsCaseSensitive = (bool)e.OldValue;
            bool newIsCaseSensitive = target.IsCaseSensitive;
            target.OnIsCaseSensitiveChanged(oldIsCaseSensitive, newIsCaseSensitive);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the IsCaseSensitive property.
        /// </summary>
        /// <param name="oldIsCaseSensitive">The old IsCaseSensitive value</param>
        /// <param name="newIsCaseSensitive">The new IsCaseSensitive value</param>
        private void OnIsCaseSensitiveChanged(
            bool oldIsCaseSensitive, bool newIsCaseSensitive)
        {
            UpdateHighlight();
        }
        #endregion

        #region HighlightTemplate
        /// <summary>
        /// HighlightTemplate Dependency Property
        /// </summary>
        public static readonly DependencyProperty HighlightTemplateProperty =
            DependencyProperty.Register(
                "HighlightTemplate",
                typeof(DataTemplate),
                typeof(HighlightBehavior),
                new PropertyMetadata(null, OnHighlightTemplateChanged));

        /// <summary>
        /// Gets or sets the HighlightTemplate property. This dependency property 
        /// indicates the template to use to generate the highlight Run inlines.
        /// </summary>
        public DataTemplate HighlightTemplate
        {
            get { return (DataTemplate)GetValue(HighlightTemplateProperty); }
            set { SetValue(HighlightTemplateProperty, value); }
        }

        /// <summary>
        /// Handles changes to the HighlightTemplate property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnHighlightTemplateChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (HighlightBehavior)d;
            DataTemplate oldHighlightTemplate = (DataTemplate)e.OldValue;
            DataTemplate newHighlightTemplate = target.HighlightTemplate;
            target.OnHighlightTemplateChanged(oldHighlightTemplate, newHighlightTemplate);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the HighlightTemplate property.
        /// </summary>
        /// <param name="oldHighlightTemplate">The old HighlightTemplate value</param>
        /// <param name="newHighlightTemplate">The new HighlightTemplate value</param>
        private void OnHighlightTemplateChanged(
            DataTemplate oldHighlightTemplate, DataTemplate newHighlightTemplate)
        {
            UpdateHighlight();
        }
        #endregion

        #region HighlightBrush
        /// <summary>
        /// HighlightBrush Dependency Property
        /// </summary>
        public static readonly DependencyProperty HighlightBrushProperty =
            DependencyProperty.Register(
                "HighlightBrush",
                typeof(Brush),
                typeof(HighlightBehavior),
                new PropertyMetadata(new SolidColorBrush(Colors.Red), OnHighlightBrushChanged));

        /// <summary>
        /// Gets or sets the HighlightBrush property. This dependency property 
        /// indicates the brush to use to highlight the found instances of the search string.
        /// </summary>
        /// <remarks>
        /// Note that the brush is ignored if HighlightTemplate is specified
        /// </remarks>
        public Brush HighlightBrush
        {
            get { return (Brush)GetValue(HighlightBrushProperty); }
            set { SetValue(HighlightBrushProperty, value); }
        }

        /// <summary>
        /// Handles changes to the HighlightBrush property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnHighlightBrushChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (HighlightBehavior)d;
            Brush oldHighlightBrush = (Brush)e.OldValue;
            Brush newHighlightBrush = target.HighlightBrush;
            target.OnHighlightBrushChanged(oldHighlightBrush, newHighlightBrush);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the HighlightBrush property.
        /// </summary>
        /// <param name="oldHighlightBrush">The old HighlightBrush value</param>
        /// <param name="newHighlightBrush">The new HighlightBrush value</param>
        private void OnHighlightBrushChanged(
            Brush oldHighlightBrush, Brush newHighlightBrush)
        {
            UpdateHighlight();
        }
        #endregion

        private PropertyChangeEventSource<string> _textChangeEventSource;

        protected override void OnAttached()
        {
            UpdateHighlight();
            _textChangeEventSource = new PropertyChangeEventSource<string>(this.AssociatedObject, "Text", BindingMode.OneWay);
            _textChangeEventSource.ValueChanged += TextChanged;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            ClearHighlight();
            _textChangeEventSource.ValueChanged -= TextChanged;
            _textChangeEventSource = null;
            base.OnDetaching();
        }

        private void TextChanged(object sender, string s)
        {
            UpdateHighlight();
        }

        public void UpdateHighlight()
        {
            if (this.AssociatedObject == null ||
                string.IsNullOrEmpty(this.AssociatedObject.Text) ||
                string.IsNullOrEmpty(this.SearchString))
            {
                ClearHighlight();
                return;
            }

            var txt = this.AssociatedObject.Text;
            var searchTxt = this.SearchString;
            var processedCharacters = 0;
            this.AssociatedObject.Inlines.Clear();

            int pos;

            while ((pos = txt.IndexOf(
                searchTxt,
                processedCharacters,
                this.IsCaseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase)) >= 0)
            {
                if (pos > processedCharacters)
                {
                    var run = new Run
                    {
                        Text =
                            txt.Substring(
                                processedCharacters, pos - processedCharacters)
                    };

                    this.AssociatedObject.Inlines.Add(run);
                }

                Run highlight;
                var highlightText = txt.Substring(pos, searchTxt.Length);

                if (this.HighlightTemplate == null)
                {
                    highlight =
                        new Run
                        {
                            Text = highlightText,
                            Foreground = this.HighlightBrush
                        };
                }
                else
                {
                    highlight = (Run)this.HighlightTemplate.LoadContent();
                    highlight.Text = highlightText;
                }

                this.AssociatedObject.Inlines.Add(highlight);
                processedCharacters = pos + searchTxt.Length;
            }

            if (processedCharacters < txt.Length)
            {
                var run = new Run
                {
                    Text =
                        txt.Substring(
                            processedCharacters, txt.Length - processedCharacters)
                };

                this.AssociatedObject.Inlines.Add(run);
            }
        }

        public void ClearHighlight()
        {
            if (this.AssociatedObject == null)
            {
                return;
            }

            var text = this.AssociatedObject.Text;
            this.AssociatedObject.Inlines.Clear();
            this.AssociatedObject.Inlines.Add(new Run{Text = text});
        }
    }
}
