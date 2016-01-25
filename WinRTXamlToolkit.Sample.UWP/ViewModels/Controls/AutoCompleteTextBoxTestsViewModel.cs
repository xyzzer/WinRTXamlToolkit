using System.Collections.ObjectModel;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Sample.Commands;

namespace WinRTXamlToolkit.Sample.ViewModels
{
    /// <summary>
    /// AutoCompleteTextBoxTestView ViewModel
    /// Created to test do bindings works and does control dependency properties reflects on changed properties
    /// </summary>
    public class AutoCompleteTextBoxTestsViewModel : ViewModel
    {
        ObservableCollection<string> wordDictionary;
        int maximalVisibleSuggestionCount = 5;
        AutoCompleteTextBox.IAutoCompletable autoCompleteAlgorithm;
        string firstTextBoxText, secondTextBoxText;
 
        public AutoCompleteTextBoxTestsViewModel()
        {
            Dictionary = new ObservableCollection<string>(FakeWordDictionary.GetFakeWordDictionary());

            AddWordToDictionaryCommand = new RelayCommand<string>(word => Dictionary.Add(word));
            RemoveWordFromDictionaryCommand = new RelayCommand<string>(word => Dictionary.Remove(word));
            // clears initialize new collection instead of calling .Clear to check does control
            // dependency property reflect on property change.
            ClearDictionaryCommand = new RelayCommand(() => Dictionary = new ObservableCollection<string>());

            SetNewAutoCompleteAlgorithmCommand = new RelayCommand<AutoCompleteTextBox.IAutoCompletable>
                ((algorithm) => AutoCompleteAlgorithm = algorithm);

        }

        public ObservableCollection<string> Dictionary
        {
            get { return wordDictionary; }
            set { SetProperty<ObservableCollection<string>>(ref wordDictionary, value); }
        }
        public AutoCompleteTextBox.IAutoCompletable AutoCompleteAlgorithm
        {
            get { return autoCompleteAlgorithm; }
            set { SetProperty<AutoCompleteTextBox.IAutoCompletable>(ref autoCompleteAlgorithm, value); }
        }

        public int MaximalVisibleSuggestionCount
        {
            get { return maximalVisibleSuggestionCount; }
            set { SetProperty<int>(ref maximalVisibleSuggestionCount, value); }
        }

        public string FirstTextBoxText
        {
            get { return firstTextBoxText; }
            set { SetProperty<string>(ref firstTextBoxText, value); }
        }

        public string SecondTextBoxText
        {
            get { return secondTextBoxText; }
            set { SetProperty<string>(ref secondTextBoxText, value); }
        }

        public RelayCommand<string> AddWordToDictionaryCommand { get; set; }
        public RelayCommand<string> RemoveWordFromDictionaryCommand { get; set; }
        public RelayCommand<AutoCompleteTextBox.IAutoCompletable> SetNewAutoCompleteAlgorithmCommand { get; set; }
        public RelayCommand ClearDictionaryCommand { get; set; }
    }
}
