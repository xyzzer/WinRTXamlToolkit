using System;
using System.Collections.Generic;
using System.Linq;

namespace WinRTXamlToolkit.Controls
{
    public sealed partial class AutoCompleteTextBox
    {
        /// <summary>
        /// Algorithm implementation for AutoCompleteTextBox that scores suggestions from the dictionary
        /// based on the Damerau-Levenshtein distance.
        /// </summary>
        public class PrefixSuggestion : AutoCompletable
        {
            private ScoredString GetSuggestionPrefixScore(string wordToSuggest, string suggestion)
            {
                int smallestLength = Math.Min(wordToSuggest.Length, suggestion.Length);
                ScoredString suggestionScore = new ScoredString() {Text = suggestion};
                int score = 0;

                for (int i = 0; i < smallestLength; ++i)
                {
                    if (wordToSuggest[i] != suggestion[i])
                        break;

                    score++;
                }

                suggestionScore.Score = score;

                return suggestionScore;
            }

            /// <summary>
            /// Gets a list of suggested word completions for the specified word, given a dictionary of words.
            /// </summary>
            /// <param name="wordToSuggest">Word/string to get suggestions for.</param>
            /// <param name="suggestionDictionary">Dictionary of words to select suggestions from.</param>
            /// <returns>A list of suggestions.</returns>
            public override IList<string> GetSuggestedWords(string wordToSuggest, ICollection<string> suggestionDictionary)
            {
                var scoredStrings =
                    suggestionDictionary.Select(suggestion => GetSuggestionPrefixScore(wordToSuggest, suggestion));
                int maximalScore = scoredStrings.Max(scoredString => scoredString.Score);

                IComparer<int> scoreComparer =
                    Comparer<int>.Create((firstScore, secondScore) => secondScore.CompareTo(firstScore));
                return
                    scoredStrings.Where(
                        scoredString => scoredString.Score != 0 && maximalScore - scoredString.Score <= 1)
                        .OrderBy(scoredString => scoredString.Score, scoreComparer)
                        .Take(this.MaximumSuggestionCount)
                        .Select(scoredString => scoredString.Text)
                        .ToList();
            }
        }
    }
}