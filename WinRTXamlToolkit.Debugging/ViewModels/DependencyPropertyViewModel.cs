using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class DependencyPropertyViewModel : BasePropertyViewModel
    {
        private readonly DependencyObjectViewModel _elementModel;
        private readonly DependencyProperty _dependencyProperty;

        #region CTOR
        public DependencyPropertyViewModel(
            DependencyObjectViewModel elementModel,
            DependencyProperty dependencyProperty,
            string name)
        {
            _elementModel = elementModel;
            _dependencyProperty = dependencyProperty;
            this.Name = name;
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
    }
}