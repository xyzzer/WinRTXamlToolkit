namespace WinRTXamlToolkit.Sample.ViewModels.Controls.Extensions
{
    public class TextBoxValidationTestPageViewModel : ViewModel
    {
        #region NumericText
        private string _numericText = "invalid initially bound value";
        public string NumericText
        {
            get { return _numericText; }
            set { this.SetProperty(ref _numericText, value); }
        }
        #endregion
    }
}
