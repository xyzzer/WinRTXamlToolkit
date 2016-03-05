using System;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Interactivity
{
    /// <summary>
    /// Encapsulates state information and zero or more ICommands into an attachable object.
    /// </summary>
    /// <remarks>
    /// This is an infrastructure class. Behavior authors should derive
    /// from <see cref="Behavior&lt;T&gt;"/> instead of from this class.
    /// </remarks>
    public abstract class Behavior : FrameworkElement
    {
        #region AssociatedObject
        /// <summary>
        /// The backing field for AssociatedObject.
        /// </summary>
        protected internal DependencyObject _associatedObject;

        /// <summary>
        /// Gets the object to which this behavior is attached.
        /// </summary>
        protected DependencyObject AssociatedObject
        {
            get
            {
                return _associatedObject;
            }
        } 
        #endregion

        #region AssociatedType
        /// <summary>
        /// The backing field for AssociatedType property.
        /// </summary>
        protected internal Type _associatedType = typeof(object);

        /// <summary>
        /// The type to which this behavior can be attached.
        /// </summary>
        protected Type AssociatedType
        {
            get
            {
                return _associatedType;
            }
        } 
        #endregion

        #region Attach()
        /// <summary>
        /// Attaches to the specified object.
        /// </summary>
        /// <param name="dependencyObject">
        /// The object to attach to.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// The Behavior is already hosted on a different element.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// dependencyObject does not satisfy the Behavior type constraint.
        /// </exception>
        public void Attach(DependencyObject dependencyObject)
        {
            if (this.AssociatedObject != null)
            {
                throw new InvalidOperationException("The Behavior is already hosted on a different element.");
            }

            _associatedObject = dependencyObject;

            if (dependencyObject != null)
            {
                if (!this.AssociatedType.GetTypeInfo().IsAssignableFrom(dependencyObject.GetType().GetTypeInfo()))
                {
                    throw new InvalidOperationException("dependencyObject does not satisfy the Behavior type constraint.");
                }

                var frameworkElement = this.AssociatedObject as FrameworkElement;

                if (frameworkElement != null)
                {
                    frameworkElement.Loaded += AssociatedFrameworkElementLoaded;
                    frameworkElement.Unloaded += AssociatedFrameworkElementUnloaded;
                }

                OnAttached();
            }
        } 
        #endregion

        #region Detach()
        /// <summary>
        /// Detaches this instance from its associated object.
        /// </summary>
        public void Detach()
        {
            if (this.AssociatedObject != null)
            {
                OnDetaching();

                var frameworkElement = this.AssociatedObject as FrameworkElement;

                if (frameworkElement != null)
                {
                    frameworkElement.Loaded -= AssociatedFrameworkElementLoaded;
                    frameworkElement.Unloaded -= AssociatedFrameworkElementUnloaded;
                }

                _associatedObject = null;
            }
        } 
        #endregion

        #region OnAttached()
        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected virtual void OnAttached()
        {
        } 
        #endregion

        #region OnDetaching()
        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but
        /// before it has actually occurred.
        /// </summary>
        /// <remarks>
        /// Override this to unhook functionality from the AssociatedObject.
        /// </remarks>
        protected virtual void OnDetaching()
        {
        } 
        #endregion

        #region OnLoaded()
        /// <summary>
        /// Called after the AssociatedObject is loaded (added to visual tree).
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected virtual void OnLoaded()
        {
        } 
        #endregion

        #region OnUnloaded()
        /// <summary>
        /// Called after the AssociatedObject is unloaded (removed from visual tree).
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the AssociatedObject.
        /// </remarks>
        protected virtual void OnUnloaded()
        {
        } 
        #endregion

        #region AssociatedFrameworkElementLoaded()
        private void AssociatedFrameworkElementLoaded(object sender, RoutedEventArgs e)
        {
            this.SetBinding(
                DataContextProperty,
                new Binding
                {
                    Path = new PropertyPath("DataContext"),
                    Source = _associatedObject
                });
            OnLoaded();
        } 
        #endregion

        #region AssociatedFrameworkElementUnloaded()
        private void AssociatedFrameworkElementUnloaded(object sender, RoutedEventArgs e)
        {
            OnUnloaded();
            this.ClearValue(DataContextProperty);
            DataContext = null;
        } 
        #endregion
    }
}
