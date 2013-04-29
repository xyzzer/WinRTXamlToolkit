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
    }
}