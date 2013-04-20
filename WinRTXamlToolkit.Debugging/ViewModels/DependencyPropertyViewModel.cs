using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Debugging.ViewModels
{
    public class DependencyPropertyViewModel : BasePropertyViewModel
    {
        private readonly DependencyProperty _dependencyProperty;
        public bool IsDefault { get; private set; }

        public DependencyPropertyViewModel(
            DependencyObjectViewModel elementModel,
            DependencyProperty dependencyProperty,
            string name)
        {
            _dependencyProperty = dependencyProperty;
            this.Name = name;
            this.ValueString = (elementModel.Model.GetValue(dependencyProperty) ?? "<null>").ToString();
            var localValue = elementModel.Model.ReadLocalValue(dependencyProperty);
            this.IsDefault = localValue == DependencyProperty.UnsetValue;
            //elementModel.Model.GetAnimationBaseValue()
        }
    }
}