using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// Allows raise an event when the value of a dependency property changes when a view model is otherwise not necessary.
    /// </summary>
    /// <typeparam name="TPropertyType"></typeparam>
    public class PropertyChangeEventSource<TPropertyType>
        : FrameworkElement
    {
        /// <summary>
        /// Occurs when the value changes.
        /// </summary>
        public event EventHandler<TPropertyType> ValueChanged;
        private readonly DependencyObject _source;

        #region Value
        /// <summary>
        /// Value Dependency Property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value",
                typeof(TPropertyType),
                typeof(PropertyChangeEventSource<TPropertyType>),
                new PropertyMetadata(default(TPropertyType), OnValueChanged));

        /// <summary>
        /// Gets or sets the Value property. This dependency property 
        /// indicates the value.
        /// </summary>
        public TPropertyType Value
        {
            get { return (TPropertyType)GetValue(ValueProperty); }
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
            var target = (PropertyChangeEventSource<TPropertyType>)d;
            TPropertyType oldValue = (TPropertyType)e.OldValue;
            TPropertyType newValue = target.Value;
            target.OnValueChanged(oldValue, newValue);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Value property.
        /// </summary>
        /// <param name="oldValue">The old Value value</param>
        /// <param name="newValue">The new Value value</param>
        private void OnValueChanged(
            TPropertyType oldValue, TPropertyType newValue)
        {
            var handler = ValueChanged;

            if (handler != null)
            {
                handler(_source, newValue);
            }
        }
        #endregion

        #region CTOR
        public PropertyChangeEventSource(
            DependencyObject source,
            string propertyName,
            BindingMode bindingMode = BindingMode.TwoWay)
        {
            //var panel =
            //    ((DependencyObject)Window.Current.Content).GetFirstDescendantOfType<Panel>();
            //panel.Children.Add(this);
            _source = source;

            // Bind to the property to be able to get its changes relayed as events throug the ValueChanged event.
            var binding =
                new Binding
                {
                    Source = source,
                    Path = new PropertyPath(propertyName),
                    Mode = bindingMode
                };

            this.SetBinding(
                ValueProperty,
                binding);
        } 
        #endregion
    }
}
