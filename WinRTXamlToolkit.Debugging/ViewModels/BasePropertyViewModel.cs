namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public abstract class BasePropertyViewModel : BindableBase
    {
        protected bool? _isDefault;

        #region Name
        private string _name;
        public string Name
        {
            get { return _name; }
            protected set { this.SetProperty(ref _name, value); }
        }
        #endregion

        public virtual string ValueString
        {
            get { return (this.Value ?? "<null>").ToString(); }
        }

        public abstract object Value { get; }

        public abstract bool IsDefault { get; }

        public abstract bool IsReadOnly { get; }

        public void Refresh()
        {
            // ReSharper disable ExplicitCallerInfoArgument
            OnPropertyChanged("Value");
            OnPropertyChanged("ValueString");
            OnPropertyChanged("IsDefault");
            // ReSharper restore ExplicitCallerInfoArgument
        }
    }
}