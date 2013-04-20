namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class BasePropertyViewModel : BindableBase
    {
        #region Name
        private string _name;
        public string Name
        {
            get { return _name; }
            protected set { this.SetProperty(ref _name, value); }
        }
        #endregion

        #region ValueString
        private string _valueString;
        public virtual string ValueString
        {
            get { return _valueString; }
            set { this.SetProperty(ref _valueString, value); }
        }
        #endregion
    }
}