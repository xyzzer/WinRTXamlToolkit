using System.Collections.Generic;

namespace WinRTXamlToolkit.Controls
{
    public sealed partial class AutoCompleteTextBox
    {
        /// <summary>
        /// Provides a common implementation for all autocomplete distance scoring and suggestion providing algorithms.
        /// </summary>
        public abstract class AutoCompletable : IAutoCompletable
        {
            #region MaximumSuggestionCount
            private int maximumSuggestionCount = 25;

            /// <summary>
            /// Gets or sets the maximum suggestion count.
            /// </summary>
            /// <value>
            /// The maximum suggestion count.
            /// </value>
            public int MaximumSuggestionCount
            {
                get { return this.maximumSuggestionCount; }
                set { this.maximumSuggestionCount = value; }
            } 
            #endregion

            /// <summary>
            /// Gets a list of suggested word completions for the specified word, given a dictionary of words.
            /// </summary>
            /// <param name="wordToSuggest">Word/string to get suggestions for.</param>
            /// <param name="suggestionDictionary">Dictionary of words to select suggestions from.</param>
            /// <returns>A list of suggestions.</returns>
            public abstract IList<string> GetSuggestedWords(string wordToSuggest, ICollection<string> suggestionDictionary);
        }
    }
}
