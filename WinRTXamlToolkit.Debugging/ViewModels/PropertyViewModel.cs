using System;
using System.ComponentModel;
using System.Reflection;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class PropertyViewModel : BasePropertyViewModel
    {
        private readonly DependencyObjectViewModel _elementModel;
        private readonly PropertyInfo _propertyInfo;

        public PropertyViewModel(DependencyObjectViewModel elementModel, PropertyInfo propertyInfo)
        {
            _elementModel = elementModel;
            _propertyInfo = propertyInfo;
            this.Name = propertyInfo.Name;
        }

        public override object Value
        {
            get
            {
                return _propertyInfo.GetValue(_elementModel.Model, new object[] { });
            }
            set
            {
                _propertyInfo.SetValue(_elementModel.Model, value);
                _isDefault = null;
                OnPropertyChanged();
                // ReSharper disable ExplicitCallerInfoArgument
                OnPropertyChanged("CanResetValue");
                OnPropertyChanged("IsDefault");
                // ReSharper restore ExplicitCallerInfoArgument
            }
        }

        public override Type PropertyType
        {
            get
            {
                return _propertyInfo.PropertyType;
            }
        }

        public override string Category
        {
            get
            {
                return MiscCategoryName;
            }
        }

        public override bool IsDefault
        {
            get
            {
                if (_isDefault == null)
                {
                    var defaultValueAttribute =
                        _propertyInfo.GetCustomAttribute(typeof (DefaultValueAttribute)) as DefaultValueAttribute;

                    if (defaultValueAttribute != null)
                    {
                        _isDefault = this.Value == defaultValueAttribute.Value;
                    }
                    else
                    {
                        _isDefault = false;
                    }
                }

                return _isDefault.Value;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                var sm = _propertyInfo.SetMethod;

                return sm == null;
            }
        }

        public override bool CanResetValue
        {
            get
            {
                return false;
            }
        }

        public override void ResetValue()
        {
            _isDefault = null;
            OnPropertyChanged();
            // ReSharper disable ExplicitCallerInfoArgument
            OnPropertyChanged("CanResetValue");
            OnPropertyChanged("IsDefault");
            // ReSharper restore ExplicitCallerInfoArgument
        }

        public override bool CanAnalyze
        {
            get
            {
                return false;
            }
        }

        public override void Analyze()
        {
        }
    }
}