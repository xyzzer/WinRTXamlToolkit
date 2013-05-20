using WinRTXamlToolkit.Debugging.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace WinRTXamlToolkit.Debugging.Views.PropertyEditors
{
    public sealed partial class ThicknessPropertyEditor : UserControl
    {
        private bool _readingValue;
        #region Model
        /// <summary>
        /// Model Dependency Property
        /// </summary>
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register(
                "Model",
                typeof(object),
                typeof(ThicknessPropertyEditor),
                new PropertyMetadata(null, OnModelChanged));

        /// <summary>
        /// Gets or sets the Model property. This dependency property 
        /// indicates the property view model to update with the values.
        /// </summary>
        public object Model
        {
            get { return (object)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Model property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnModelChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ThicknessPropertyEditor)d;
            BasePropertyViewModel oldModel = (BasePropertyViewModel)e.OldValue;
            BasePropertyViewModel newModel = (BasePropertyViewModel)target.Model;
            target.OnModelChanged(oldModel, newModel);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Model property.
        /// </summary>
        /// <param name="oldModel">The old Model value</param>
        /// <param name="newModel">The new Model value</param>
        private void OnModelChanged(
            BasePropertyViewModel oldModel, BasePropertyViewModel newModel)
        {
            ReadModelValue();
        }
        #endregion

        #region Value
        /// <summary>
        /// Value Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(object),
                typeof(ThicknessPropertyEditor),
                new PropertyMetadata(false, OnValueChanged));

        /// <summary>
        /// Gets or sets the Value property. This dependency property 
        /// indicates the value of the property.
        /// </summary>
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Value property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnValueChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ThicknessPropertyEditor)d;
            object oldValue = (object)e.OldValue;
            object newValue = target.Value;
            target.OnValueChanged(oldValue, newValue);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Value property.
        /// </summary>
        /// <param name="oldValue">The old Value value</param>
        /// <param name="newValue">The new Value value</param>
        private void OnValueChanged(
            object oldValue, object newValue)
        {
            if (this.Model != null)
            {
                ReadModelValue();
            }
        }
        #endregion

        public ThicknessPropertyEditor()
        {
            this.InitializeComponent();
        }

        private void ReadModelValue()
        {
            var model = (BasePropertyViewModel)this.Model;
            var thickness = (Thickness)model.Value;
            _readingValue = true;
            this.NumericUpDownLeft.Value = thickness.Left;
            this.NumericUpDownTop.Value = thickness.Top;
            this.NumericUpDownRight.Value = thickness.Right;
            this.NumericUpDownBottom.Value = thickness.Bottom;

            var dpvm = model as DependencyPropertyViewModel;

            if (dpvm != null &&
                dpvm.DependencyProperty == FrameworkElement.MarginProperty)
            {
                this.NumericUpDownLeft.Minimum = -1000000;
                this.NumericUpDownTop.Minimum = -1000000;
                this.NumericUpDownRight.Minimum = -1000000;
                this.NumericUpDownBottom.Minimum = -1000000;
            }

            _readingValue = false;
        }

        private void OnNumericUpDownValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (_readingValue)
            {
                return;
            }

            var left = this.NumericUpDownLeft.Value;
            var top = this.NumericUpDownTop.Value;
            var right = this.NumericUpDownRight.Value;
            var bottom = this.NumericUpDownBottom.Value;

            var model = (BasePropertyViewModel)this.Model;
            model.Value = new Thickness(left, top, right, bottom);
        }
    }
}
