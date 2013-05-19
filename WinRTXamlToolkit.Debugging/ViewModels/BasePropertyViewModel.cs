using System;
using System.ComponentModel;
using WinRTXamlToolkit.Debugging.Commands;

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

        public abstract object Value { get; set; }

        public abstract Type PropertyType { get; }

        public abstract bool IsDefault { get; }

        public abstract bool IsReadOnly { get; }

        public abstract bool CanResetValue { get; }

        public abstract void ResetValue();

        public RelayCommand ResetValueCommand { get; private set; }

        public BasePropertyViewModel()
        {
            this.ResetValueCommand = new RelayCommand(
                this.ResetValue,
                () => this.CanResetValue);
            this.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "CanResetValue")
            {
                this.ResetValueCommand.RaiseCanExecuteChanged();
            }
        }

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