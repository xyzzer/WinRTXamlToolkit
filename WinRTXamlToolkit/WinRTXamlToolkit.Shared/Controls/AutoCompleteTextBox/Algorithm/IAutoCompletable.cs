using System.Collections.Generic;

namespace WinRTXamlToolkit.Controls
{
    public sealed partial class AutoCompleteTextBox
    {
        /// <summary>
        /// Provides a shared interface for all autocomplete distance scoring and suggestion providing algorithms.
        /// </summary>
        public interface IAutoCompletable
        {
            /// <summary>
            /// Gets a list of suggested word completions for the specified word, given a dictionary of words.
            /// </summary>
            /// <param name="wordToSuggest">Word/string to get suggestions for.</param>
            /// <param name="suggestionDictionary">Dictionary of words to select suggestions from.</param>
            /// <returns>A list of suggestions.</returns>
            IList<string> GetSuggestedWords(string wordToSuggest, ICollection<string> suggestionDictionary);
        }
    }
}
