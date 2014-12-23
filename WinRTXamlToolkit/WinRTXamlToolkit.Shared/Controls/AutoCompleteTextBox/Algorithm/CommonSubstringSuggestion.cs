using System;
using System.Collections.Generic;
using System.Linq;

namespace WinRTXamlToolkit.Controls
{
    public sealed partial class AutoCompleteTextBox
    {
        public class CommonSubstringSuggestion : IAutoCompletable
        {
            private readonly int maximumSuggestionCount = 25;

            private ScoredString GetScoreByLongestCommonSubstring(string wordToSuggest, string suggestion)
            {
                int[,] subproblems = new int[wordToSuggest.Length + 1, suggestion.Length + 1];

                int score = int.MinValue;

                for (int i = 1; i <= wordToSuggest.Length; ++i)
                    for (int j = 1; j <= suggestion.Length; ++j)
                    {
                        if (wordToSuggest[i - 1] == suggestion[j - 1])
                        {
                            subproblems[i, j] = subproblems[i - 1, j - 1] + 1;
                            score = Math.Max(score, subproblems[i, j]);
                        }
                    }

                return new ScoredString() {Text = suggestion, Score = score};
            }

            public IList<string> GetSuggestedWords(string wordToSuggest, ICollection<string> suggestionDictionary)
            {
                var suggestions =
                    suggestionDictionary.Select(
                        suggestion => GetScoreByLongestCommonSubstring(wordToSuggest, suggestion));
                int maximalScore = suggestions.Max(scoredString => scoredString.Score);

                return
                    suggestions.Where(
                        stringWithScore => stringWithScore.Score > 0 && maximalScore - stringWithScore.Score <= 1)
                        .OrderBy(scoreString => scoreString.Score,
                            Comparer<int>.Create((first, second) => second.CompareTo(first)))
                        .Take(maximumSuggestionCount)
                        .Select(stringWithScore => stringWithScore.Text)
                        .ToList();
            }
        }
    }
}
