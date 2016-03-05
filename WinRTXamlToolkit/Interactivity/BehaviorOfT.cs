using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Interactivity
{
    /// <summary>
    /// Encapsulates state information and zero or more ICommands into an attachable object.
    /// </summary>
    /// <typeparam name="T">
    /// The type the <see cref="Behavior&lt;T&gt;"/> can be attached to.
    /// </typeparam>
    /// <remarks>
    /// Behavior is the base class for providing attachable state and commands to
    /// an object. The types the Behavior can be attached to can be controlled by
    /// the generic parameter. Override OnAttached() and OnDetaching() methods to
    /// hook and unhook any necessary handlers from the AssociatedObject.
    /// </remarks>
    public abstract class Behavior<T> : Behavior where T : DependencyObject
    {
        #region Behavior() - CTOR
        /// <summary>
        /// Initializes a new instance of the <see cref="Behavior&lt;T&gt;"/> class.
        /// </summary>
        protected Behavior()
        {
            _associatedType = typeof(T);
        } 
        #endregion

        #region AssociatedObject
        /// <summary>
        /// Gets the object to which this <see cref="Behavior&lt;T&gt;" /> is attached.
        /// </summary>
        public new T AssociatedObject
        {
            get
            {
                return (T)_associatedObject;
            }
            internal set
            {
                _associatedObject = value;
            }
        } 
        #endregion
    }
}
