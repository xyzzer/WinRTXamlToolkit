using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRTXamlToolkit.Controls.AutoCompleteTextBoxControl.Algorithm
{
    public interface IAutoCompletable
    {
        IList<string> GetSuggestedWords(string wordToSuggest, ICollection<string> suggestionDictionary);
    }
}
