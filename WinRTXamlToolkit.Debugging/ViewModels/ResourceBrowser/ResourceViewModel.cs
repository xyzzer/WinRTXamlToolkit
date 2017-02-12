using System;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Debugging.ViewModels.ResourceBrowser
{
    public class ResourceViewModel : BasePropertyViewModel
    {
        private ResourceDictionary _dictionary;
        private object _value;

        public object Key { get;  }

        public override object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (this.SetProperty(ref _value, value))
                {
                    _dictionary[this.Key] = _value = value;
                    // ReSharper disable once ExplicitCallerInfoArgument
                    this.OnPropertyChanged(nameof(this.ValueString));
                }
            }
        }

        public override Type PropertyType { get; }

        public override string Category
        {
            get
            {
                return "General";
            }
        }

        public override bool IsDefault
        {
            get
            {
                return true;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public override bool CanResetValue
        {
            get
            {
                return false;
            }
        }

        public override bool CanAnalyze
        {
            get
            {
                return false;
            }
        }

        public ResourceViewModel(object key, object resource, ResourceDictionary dictionary) : base(null)
        {
            this.Key = key;
            this.Name = key.ToString();
            _dictionary = dictionary;
            this.Value = resource;
            this.PropertyType = _value.GetType();
        }

        public override void ResetValue()
        {
            throw new NotSupportedException();
        }

        public override void Analyze()
        {
            throw new NotSupportedException();
        }

        public override bool TryGetValue(object model, out object value)
        {
            value = _value;
            return _value != null;
        }
    }
}
