using WinRTXamlToolkit.Controls.Behaviors;
using WinRTXamlToolkit.Interactivity;
using WinRTXamlToolkit.Sample.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class HighlightBehaviorTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public HighlightBehaviorTestPage()
        {
            this.InitializeComponent();
            this.DataContext = new HighlightBehaviorTestViewModel();

            // Somehow the below doesn't get set automatically. Namescoping issues?
            highlightBehavior = (HighlightBehavior)Interaction.GetBehaviors(highlightedTextBlock)[0];
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        public class HighlightBehaviorTestViewModel : BindableBase
        {
            #region SearchString
            private string _searchString;
            /// <summary>
            /// Gets or sets the search string.
            /// </summary>
            public string SearchString
            {
                get { return _searchString; }
                set { this.SetProperty(ref _searchString, value); }
            }
            #endregion
        }

        private void OnSearchStringChanged(object sender, TextChangedEventArgs e)
        {
            // Since textbox text bindings don't update after each key change you can use the TextChanged event instead.
            highlightBehavior.SearchString = searchBox.Text;
            //highlightBehavior.UpdateHighlight();
        }
    }
}
