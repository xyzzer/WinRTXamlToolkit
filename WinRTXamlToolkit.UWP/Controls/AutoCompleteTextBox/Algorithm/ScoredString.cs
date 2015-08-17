
namespace WinRTXamlToolkit.Controls
{
    public sealed partial class AutoCompleteTextBox
    {
        /// <summary>
        /// Represents an autocomplete suggestion string with ranker score value.
        /// </summary>
        public class ScoredString
        {
            /// <summary>
            /// Gets or sets the string
            /// </summary>
            public string Text { get; set; }

            /// <summary>
            /// Gets or sets the score of the string
            /// </summary>
            public int Score { get; set; }
        }
    }
}