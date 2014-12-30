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
        public class DamerauLevenshteinDistance : AutoCompletable
        {
            private ScoredString CalculateDistance(string first, string second)
            {
                int replaceCost = 0;
                int gapCost = 1;

                int[,] subproblems = new int[first.Length + 1, second.Length + 1];

                for (int i = 0; i < first.Length + 1; ++i)
                    subproblems[i, 0] = i;

                for (int i = 0; i < second.Length + 1; ++i)
                    subproblems[0, i] = i;

                for (int i = 1; i < first.Length + 1; ++i)
                {
                    for (int j = 1; j < second.Length + 1; ++j)
                    {
                        char firstWordChar = char.ToLower(first[i - 1]);
                        char secondWordChar = char.ToLower(second[j - 1]);

                        if (firstWordChar.Equals(secondWordChar))
                            replaceCost = 0;
                        else
                            replaceCost = 1;

                        subproblems[i, j] = Math.Min(subproblems[i - 1, j] + gapCost,
                            Math.Min(subproblems[i, j - 1] + gapCost, subproblems[i - 1, j - 1] + replaceCost));

                        if (i > 1 && j > 1 && firstWordChar == second[j - 2] && first[i - 2] == secondWordChar)
                            subproblems[i, j] = Math.Min(subproblems[i, j], subproblems[i - 2, j - 2] + replaceCost);
                        // handles "Damerau" - case where characters are transposed
                    }
                }

                return new ScoredString {Text = second, Score = subproblems[first.Length, second.Length]};
            }

            /// <summary>
            /// Gets a list of suggested word completions for the specified word, given a dictionary of words.
            /// </summary>
            /// <param name="wordToSuggest">Word/string to get suggestions for.</param>
            /// <param name="suggestionDictionary">Dictionary of words to select suggestions from.</param>
            /// <returns>A list of suggestions.</returns>
            public override IList<string> GetSuggestedWords(string wordToSuggest, ICollection<string> suggestionDictionary)
            {
                var scoredSuggestions =
                    suggestionDictionary.Select(suggestion => CalculateDistance(wordToSuggest, suggestion));
                int maximalScore = scoredSuggestions.Min(scoredString => scoredString.Score); // less value means better

                IComparer<int> scoreComparer =
                    Comparer<int>.Create((firstScore, secondScore) => firstScore.CompareTo(secondScore));
                return
                    scoredSuggestions
                        .Where(scoredString =>
                            scoredString.Score < wordToSuggest.Length &&
                            Math.Abs(maximalScore - scoredString.Score) <= 1)
                        .OrderBy(scoredString => scoredString.Score, scoreComparer)
                        .Take(this.MaximumSuggestionCount)
                        .Select(scoredString => scoredString.Text)
                        .ToList();
            }
        }
    }
}
