using System;
using System.Collections.Generic;
using System.Linq;

namespace WinRTXamlToolkit.Controls
{
    public sealed partial class AutoCompleteTextBox
    {
        public class PrefixSuggestion : IAutoCompletable
        {
            private readonly int maximumSuggestionCount = 25;

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

            public IList<string> GetSuggestedWords(string wordToSuggest, ICollection<string> suggestionDictionary)
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
                        .Take(maximumSuggestionCount)
                        .Select(scoredString => scoredString.Text)
                        .ToList();
            }
        }
    }
}