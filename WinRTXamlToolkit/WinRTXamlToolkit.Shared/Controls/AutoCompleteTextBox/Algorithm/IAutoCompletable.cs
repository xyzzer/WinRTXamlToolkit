using System.Collections.Generic;

namespace WinRTXamlToolkit.Controls
{
    public sealed partial class AutoCompleteTextBox
    {
        public interface IAutoCompletable
        {
            IList<string> GetSuggestedWords(string wordToSuggest, ICollection<string> suggestionDictionary);
        }
    }
}
