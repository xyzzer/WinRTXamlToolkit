using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Interactivity
{
    /// <summary>
    /// Represents a collection of behaviors with a shared AssociatedObject and provides
    /// change notifications to its contents when that AssociatedObject changes.
    /// </summary>
    public class BehaviorCollection : ObservableCollection<Behavior>
    {
        #region AssociatedObject
        /// <summary>
        /// The object on which the collection is hosted.
        /// </summary>
        protected DependencyObject AssociatedObject { get; private set; } 
        #endregion

        #region Attach()
        /// <summary>
        /// Attaches to the specified object.
        /// </summary>
        /// <param name="dependencyObject">
        /// The object to attach to.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// The BehaviorCollection is already attached to a different object.
        /// </exception>
        public void Attach(DependencyObject dependencyObject)
        {
            if (this.AssociatedObject != null)
            {
                throw new InvalidOperationException("The BehaviorCollection is already attached to a different object.");
            }

            this.AssociatedObject = dependencyObject;

            foreach (var behavior in this)
            {
                behavior.Attach(dependencyObject);
            }

            this.CollectionChanged += OnCollectionChanged;
            this.OnAttached();
        } 
        #endregion

        #region Detach()
        /// <summary>
        /// Detaches this instance from its associated object.
        /// </summary>
        public void Detach()
        {
            this.OnDetaching();

            foreach (var behavior in this)
            {
                behavior.Detach();
            }

            this.CollectionChanged -= OnCollectionChanged;
            this.AssociatedObject = null;
        } 
        #endregion

        #region OnAttached()
        /// <summary>
        /// Called immediately after the collection is attached to an AssociatedObject.
        /// </summary>
        protected virtual void OnAttached()
        {
        } 
        #endregion

        #region OnDetaching()
        /// <summary>
        /// Called when the collection is being detached from its AssociatedObject, but
        /// before it has actually occurred.
        /// </summary>
        protected virtual void OnDetaching()
        {
        } 
        #endregion

        #region OnCollectionChanged()
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    foreach (Behavior behavior in e.NewItems)
                    {
                        behavior.Attach(this.AssociatedObject);
                    }

                    break;
                }
                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Remove:
                {
                    foreach (Behavior behavior in e.OldItems)
                    {
                        behavior.Detach();
                    }

                    break;
                }
            }
        }
        #endregion
    }
}
