using System;
using System.Reflection;
using Windows.UI.Xaml;
using WinRTXamlToolkit.Debugging.Common;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public partial class DependencyPropertyViewModel : BasePropertyViewModel
    {
        private readonly DependencyObjectViewModel _elementModel;
        private readonly DependencyPropertyInfo _dpi;
        private readonly PropertyInfo _propertyInfo;
        private readonly Type _propertyType;

        private IValueCoercionHelper _coercionHelper;
        internal IValueCoercionHelper CoercionHelper
        {
            get
            {
                return _coercionHelper;
            }
            set
            {
                _coercionHelper = value;
            }
        }

        private readonly DependencyProperty _dependencyProperty;
        public DependencyProperty DependencyProperty
        {
            get
            {
                return _dependencyProperty;
            }
        }

        #region CTOR
        public DependencyPropertyViewModel(
            DependencyObjectViewModel elementModel,
            DependencyPropertyInfo dpi)
        {
            _elementModel = elementModel;
            _dpi = dpi;
            _dependencyProperty = dpi.Property;
            this.Name = dpi.DisplayName;
            _propertyInfo = elementModel.Model.GetType().GetRuntimeProperty(dpi.DisplayName);

            if (_propertyInfo != null)
            {
                var accessor = _propertyInfo.GetMethod ?? _propertyInfo.SetMethod;

                _propertyType = accessor.ReturnType;
            }

            if (_propertyType == null)
            {
                var defaultValue =
                    _dpi.Property.GetMetadata(_elementModel.Model.GetType())
                        .DefaultValue;

                if (defaultValue != null)
                {
                    _propertyType = defaultValue.GetType();
                }
                else
                {
                    _propertyType = typeof (object);
                }
            }

            CoercionHelper =
                ValueCoercionHelperFactory.GetValueCoercionHelper(DependencyProperty);
        }
        #endregion

        #region Value
        public override object Value
        {
            get
            {
                return _elementModel.Model.GetValue(DependencyProperty);
            }
            set
            {
                try
                {
                    if (CoercionHelper != null)
                    {
                        CoercionHelper.CoerceValue(ref value);
                    }

                    _elementModel.Model.SetValue(DependencyProperty, value);
                    _isDefault = null;
                    OnPropertyChanged();
// ReSharper disable ExplicitCallerInfoArgument
                    OnPropertyChanged("CanResetValue");
                    OnPropertyChanged("IsDefault");
// ReSharper restore ExplicitCallerInfoArgument
                }
                catch
                {
                }
            }
        }

        public override Type PropertyType
        {
            get
            {
                return _propertyType;
            }
        }
        #endregion

        #region IsDefault
        public override bool IsDefault
        {
            get
            {
                if (_isDefault == null)
                {
                    var localValue = _elementModel.Model.ReadLocalValue(DependencyProperty);
                    _isDefault = localValue == DependencyProperty.UnsetValue;
                }

                return _isDefault.Value;
            }
        }
        #endregion

        #region IsReadOnly
        public override bool IsReadOnly
        {
            get
            {
                if (DependencyPropertyCache.AttachedProperties.Contains(_dpi))
                {
                    return false;
                }

                if (_propertyInfo == null)
                {
                    return true;
                }

                var sm = _propertyInfo.SetMethod;

                return sm == null;
            }
        }
        #endregion

        public override bool CanResetValue
        {
            get
            {
                return !this.IsReadOnly && !this.IsDefault;
            }
        }

        public override void ResetValue()
        {
            _elementModel.Model.ClearValue(_dependencyProperty);
            _isDefault = null;

 // ReSharper disable ExplicitCallerInfoArgument
            OnPropertyChanged("Value");
            OnPropertyChanged("CanResetValue");
            OnPropertyChanged("IsDefault");
// ReSharper restore ExplicitCallerInfoArgument
       }
    }
}