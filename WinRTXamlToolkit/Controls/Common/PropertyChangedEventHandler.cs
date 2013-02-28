using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Controls
{
    //public delegate void PropertyChangedEventHandler(object sender, object OldValue, object NewValue);
    //public delegate void RoutedPropertyChangedEventHandler<T>(object sender, T OldValue, T NewValue);
    // Summary:
    //     Represents methods that will handle various routed events that track property
    //     value changes.
    //
    // Parameters:
    //   sender:
    //     The object where the event handler is attached.
    //
    //   e:
    //     The event data. Specific event definitions will constrain System.Windows.RoutedPropertyChangedEventArgs<T>
    //     to a type, with the type parameter of the constraint matching the type parameter
    //     constraint of a delegate implementation.
    //
    // Type parameters:
    //   T:
    //     The type of the property value where changes in value are reported.
    public delegate void RoutedPropertyChangedEventHandler<T>(object sender, RoutedPropertyChangedEventArgs<T> e);

    // Summary:
    //     Provides data about a change in value to a dependency property as reported
    //     by particular routed events, including the previous and current value of
    //     the property that changed.
    //
    // Type parameters:
    //   T:
    //     The type of the dependency property that has changed.
    public class RoutedPropertyChangedEventArgs<T> : RoutedEventArgs
    {
        // Summary:
        //     Initializes a new instance of the System.Windows.RoutedPropertyChangedEventArgs<T>
        //     class, with provided old and new values.
        //
        // Parameters:
        //   oldValue:
        //     The previous value of the property, before the event was raised.
        //
        //   newValue:
        //     The current value of the property at the time of the event.
        public RoutedPropertyChangedEventArgs(T oldValue, T newValue)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        // Summary:
        //     Gets the new value of a property as reported by a property-changed event.
        //
        // Returns:
        //     The generic value. In a practical implementation of the System.Windows.RoutedPropertyChangedEventArgs<T>,
        //     the generic type of this property is replaced with the constrained type of
        //     the implementation.
        public T NewValue { get; private set; }
        //
        // Summary:
        //     Gets the previous value of the property as reported by a property-changed
        //     event.
        //
        // Returns:
        //     The generic value. In a practical implementation of the System.Windows.RoutedPropertyChangedEventArgs<T>,
        //     the generic type of this property is replaced with the constrained type of
        //     the implementation.
        public T OldValue { get; private set; }
    }

    //public class RoutedPropertyChangedEventArgs : RoutedEventArgs
    //{
    //    public object OldValue { get; private set; }
    //    public object NewValue { get; private set; }

    //    public RoutedPropertyChangedEventArgs(object oldValue, object newValue)
    //    {
    //        this.OldValue = oldValue;
    //        this.NewValue = newValue;
    //    }
    //}
}
