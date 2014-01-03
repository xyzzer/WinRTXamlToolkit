using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls.AutoCompleteTextBoxControl.Algorithm;
using WinRTXamlToolkit.Sample.ViewModels.Controls;



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WinRTXamlToolkit.Sample.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AutoCompleteTextBoxTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public AutoCompleteTextBoxTestPage()
        {
            this.InitializeComponent();
            this.DataContext = new AutoCompleteTextBoxTestsViewModel();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }


        private void FlipView_SelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            if (settingsGrid == null || controlGrid == null)
                return;

            var flipView = sender as FlipView;
            
            if ( flipView.SelectedIndex == 0 )
            {
                settingsGrid.Visibility = Visibility.Visible;
                controlGrid.Visibility = Visibility.Collapsed;
            }
            else if ( flipView.SelectedIndex == 1)
            {
                settingsGrid.Visibility = Visibility.Collapsed;
                controlGrid.Visibility = Visibility.Visible;
            }
        }

        private void AddWordToDictionaryButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(addWordTextBox.Text))
                return;

            GetViewModel().AddWordToDictionaryCommand.Execute(addWordTextBox.Text);
        }
        private async void RemoveWordFromDictionaryButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedWord = this.wordDictionaryListBox.SelectedItem as string;
            if ( selectedWord == null)
            {
                Windows.UI.Popups.MessageDialog msgDialog = new Windows.UI.Popups.MessageDialog("You have to select word from dictionary to remove it.", "Invalid operation;-(");
                await msgDialog.ShowAsync();
                return;
            }

            GetViewModel().RemoveWordFromDictionaryCommand.Execute(selectedWord);
        }

        private AutoCompleteTextBoxTestsViewModel GetViewModel()
        {
            return this.DataContext as AutoCompleteTextBoxTestsViewModel;
        }

        private void AlgorithmListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.DataContext == null)
                return;

            var listBox = sender as ListBox;
            var selectedIndex = listBox.SelectedIndex;

            if (selectedIndex == 0)
                GetViewModel().SetNewAutoCompleteAlgorithmCommand.Execute(new DamerauLevenstheinDistance());
            else if (selectedIndex == 1)
                GetViewModel().SetNewAutoCompleteAlgorithmCommand.Execute(new CommonSubstringSuggestion());
            else if (selectedIndex == 2)
                GetViewModel().SetNewAutoCompleteAlgorithmCommand.Execute(new PrefixSuggestion());
        }

        

    }
}
