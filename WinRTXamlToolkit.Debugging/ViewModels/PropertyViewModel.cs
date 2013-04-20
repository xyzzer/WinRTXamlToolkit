using System.Reflection;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class PropertyViewModel : BasePropertyViewModel
    {
        private PropertyInfo _propertyInfo;

        public PropertyViewModel(DependencyObjectViewModel elementModel, PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
            this.Name = propertyInfo.Name;
            this.ValueString = (propertyInfo.GetValue(
                elementModel.Model, new object[] {}) ?? "<null>").ToString();
        }
    }
}