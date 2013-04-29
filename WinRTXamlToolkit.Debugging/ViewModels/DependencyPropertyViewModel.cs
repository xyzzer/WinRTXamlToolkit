using System.Reflection;
using Windows.UI.Xaml;
using WinRTXamlToolkit.Debugging.Common;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class DependencyPropertyViewModel : BasePropertyViewModel
    {
        private readonly DependencyObjectViewModel _elementModel;
        private readonly DependencyPropertyInfo _dpi;
        private readonly DependencyProperty _dependencyProperty;
        private readonly PropertyInfo _propertyInfo;

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
            //elementModel.Model.GetAnimationBaseValue()
        }
        #endregion

        #region Value
        public override object Value
        {
            get { return _elementModel.Model.GetValue(_dependencyProperty); }
        } 
        #endregion

        #region IsDefault
        public override bool IsDefault
        {
            get
            {
                if (_isDefault == null)
                {
                    var localValue = _elementModel.Model.ReadLocalValue(_dependencyProperty);
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
    }
}