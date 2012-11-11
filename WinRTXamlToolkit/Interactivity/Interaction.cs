// Credits to Joost van Schaik who created the original WinRT Behaviors.
// This namespace is based on his work.
// It was however highly modified and is based on both his library
// and the API of Silverlight's Interactivity namespace.
using System.ComponentModel;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Interactivity
{
    /// <summary>
    /// Static class that owns the Behaviors attached properties. Handles
    /// propagation of AssociatedObject change notifications.
    /// </summary>
    public static class Interaction
    {
        #region Behaviors
        /// <summary>
        /// Behaviors Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty BehaviorsProperty =
            DependencyProperty.RegisterAttached(
                "Behaviors",
                typeof(BehaviorCollection),
                typeof(Interaction),
                new PropertyMetadata(
                    DesignMode.DesignModeEnabled ?
                        new BehaviorCollection() :
                        null,
                    BehaviorsChanged));

        /// <summary>
        /// Gets the <see cref="BehaviorCollection"/> associated with
        /// a specified object.
        /// </summary>
        /// <param name="obj">
        /// The object from which to retrieve the <see cref="BehaviorCollection"/>.
        /// </param>
        /// <returns>
        /// A <see cref="BehaviorCollection"/> containing the behaviors associated with the specified object.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Always)]
        public static BehaviorCollection GetBehaviors(DependencyObject obj)
        {
            var behaviors = obj.GetValue(BehaviorsProperty) as BehaviorCollection;

            if (behaviors == null)
            {
                behaviors = new BehaviorCollection();
                SetBehaviors(obj, behaviors);
            }

            return behaviors;
        }

        /// <summary>
        /// Called when Property is retrieved
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        private static void SetBehaviors(DependencyObject obj, BehaviorCollection value)
        {
            obj.SetValue(BehaviorsProperty, value);
        }

        /// <summary>
        /// Called when the property changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void BehaviorsChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var associatedObject = sender as DependencyObject;

            if (associatedObject != null)
            {
                var oldList = args.OldValue as BehaviorCollection;

                if (oldList != null)
                {
                    oldList.Detach();
                }

                var newList = args.NewValue as BehaviorCollection;

                if (newList != null)
                {
                    newList.Attach(associatedObject);
                }
            }
        } 
        #endregion
    }
}
