using System;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Attached properties and extension methods for RichTextBlock class.
    /// </summary>
    public static class RichTextBlockExtensions
    {
        #region PlainText
        /// <summary>
        /// PlainText Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty PlainTextProperty =
            DependencyProperty.RegisterAttached(
                "PlainText",
                typeof(string),
                typeof(RichTextBlockExtensions),
                new PropertyMetadata("", OnPlainTextChanged));

        /// <summary>
        /// Gets the PlainText property. This dependency property 
        /// indicates the plain text to assign to the RichTextBlock.
        /// </summary>
        public static string GetPlainText(DependencyObject d)
        {
            return (string)d.GetValue(PlainTextProperty);
        }

        /// <summary>
        /// Sets the PlainText property. This dependency property 
        /// indicates the plain text to assign to the RichTextBlock.
        /// </summary>
        public static void SetPlainText(DependencyObject d, string value)
        {
            d.SetValue(PlainTextProperty, value);
        }

        /// <summary>
        /// Handles changes to the PlainText property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnPlainTextChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string oldPlainText = (string)e.OldValue;
            string newPlainText = (string)d.GetValue(PlainTextProperty);
            ((RichTextBlock)d).Blocks.Clear();
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run { Text = newPlainText });
            ((RichTextBlock)d).Blocks.Add(paragraph);
        }
        #endregion

        #region LinkedHtmlFragment
        /// <summary>
        /// LinkedHtmlFragment Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty LinkedHtmlFragmentProperty =
            DependencyProperty.RegisterAttached(
                "LinkedHtmlFragment",
                typeof(string),
                typeof(RichTextBlockExtensions),
                new PropertyMetadata(null, OnLinkedHtmlFragmentChanged));

        /// <summary>
        /// Gets the LinkedHtmlFragment property. This dependency property 
        /// indicates the linked text that can be bound to the RichTextBlock to automatically generate inline links.
        /// </summary>
        /// <remarks>
        /// Note that only simple html text with opening and closing anchor tags and href attribute with double-quotes is supported.
        /// No escapes or other tags will be parsed.
        /// </remarks>
        public static string GetLinkedHtmlFragment(DependencyObject d)
        {
            return (string)d.GetValue(LinkedHtmlFragmentProperty);
        }

        /// <summary>
        /// Sets the LinkedHtmlFragment property. This dependency property 
        /// indicates the linked text that can be bound to the RichTextBlock to automatically generate inline links.
        /// </summary>
        /// <remarks>
        /// Note that only simple html text with opening and closing anchor tags and href attribute with double-quotes is supported.
        /// No escapes or other tags will be parsed.
        /// </remarks>
        public static void SetLinkedHtmlFragment(DependencyObject d, string value)
        {
            d.SetValue(LinkedHtmlFragmentProperty, value);
        }

        /// <summary>
        /// Handles changes to the LinkedHtmlFragment property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnLinkedHtmlFragmentChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string oldLinkedHtmlFragment = (string)e.OldValue;
            string newLinkedHtmlFragment = (string)d.GetValue(LinkedHtmlFragmentProperty);

            ((RichTextBlock)d).SetLinkedHtmlFragment(newLinkedHtmlFragment);
        }
        #endregion

        /// <summary>
        /// Sets the linked HTML fragment.
        /// </summary>
        /// <remarks>
        /// Note that only simple html text with opening and closing anchor tags and href attribute with double-quotes is supported.
        /// No escapes or other tags will be parsed.
        /// </remarks>
        /// <param name="richTextBlock">The rich text block.</param>
        /// <param name="htmlFragment">The HTML fragment.</param>
        public static void SetLinkedHtmlFragment(this RichTextBlock richTextBlock, string htmlFragment)
        {
            richTextBlock.Blocks.Clear();

            if (string.IsNullOrEmpty(htmlFragment))
            {
                return;
            }

            var regEx = new Regex(
                @"\<a\s(href\=""|[^\>]+?\shref\="")(?<link>[^""]+)"".*?\>(?<text>.*?)(\<\/a\>|$)",
                RegexOptions.IgnoreCase | RegexOptions.Multiline);

            int nextOffset = 0;

            foreach (Match match in regEx.Matches(htmlFragment))
            {
                if (match.Index > nextOffset)
                {
                    richTextBlock.AppendText(htmlFragment.Substring(nextOffset, match.Index - nextOffset));
                    nextOffset = match.Index + match.Length;
                    richTextBlock.AppendLink(match.Groups["text"].Value, new Uri(match.Groups["link"].Value));
                }

                //Debug.WriteLine(match.Groups["text"] + ":" + match.Groups["link"]);
            }

            if (nextOffset < htmlFragment.Length)
            {
                richTextBlock.AppendText(htmlFragment.Substring(nextOffset));
            }
        }

        /// <summary>
        /// Appends a paragraph of plain text to the RichTextBlock.
        /// </summary>
        /// <param name="richTextBlock">The rich text block.</param>
        /// <param name="text">The text.</param>
        public static void AppendText(this RichTextBlock richTextBlock, string text)
        {
            Paragraph paragraph;

            if (richTextBlock.Blocks.Count == 0 ||
                (paragraph = richTextBlock.Blocks[richTextBlock.Blocks.Count - 1] as Paragraph) == null)
            {
                paragraph = new Paragraph();
                richTextBlock.Blocks.Add(paragraph);
            }

            paragraph.Inlines.Add(new Run { Text = text });
        }

        /// <summary>
        /// Appends a HyperlinkButton with
        /// the given text and navigate uri to the given RichTextBlock.
        /// </summary>
        /// <param name="richTextBlock">The rich text block.</param>
        /// <param name="text">The text.</param>
        /// <param name="uri">The URI.</param>
        public static void AppendLink(this RichTextBlock richTextBlock, string text, Uri uri)
        {
            Paragraph paragraph;

            if (richTextBlock.Blocks.Count == 0 ||
                (paragraph = richTextBlock.Blocks[richTextBlock.Blocks.Count - 1] as Paragraph) == null)
            {
                paragraph = new Paragraph();
                richTextBlock.Blocks.Add(paragraph);
            }

            var link =
                new HyperlinkButton
                {
                    Content = text,
                    NavigateUri = uri
                };

            paragraph.Inlines.Add(new InlineUIContainer {Child = link});
        }
    }
}
