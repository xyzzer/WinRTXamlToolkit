using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// A simple pair of helper attached dependency properties
    /// that allows to configure a single Storyboard to run on an element.
    /// </summary>
    public static class AnimationHelper
    {
        #region Storyboard
        /// <summary>
        /// Storyboard Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty StoryboardProperty =
            DependencyProperty.RegisterAttached(
                "Storyboard",
                typeof(Storyboard),
                typeof(AnimationHelper),
                new PropertyMetadata(null, OnStoryboardChanged));

        /// <summary>
        /// Gets the Storyboard property. This dependency property 
        /// indicates the storyboard to play.
        /// </summary>
        public static Storyboard GetStoryboard(DependencyObject d)
        {
            return (Storyboard)d.GetValue(StoryboardProperty);
        }

        /// <summary>
        /// Sets the Storyboard property. This dependency property 
        /// indicates the storyboard to play.
        /// </summary>
        public static void SetStoryboard(DependencyObject d, Storyboard value)
        {
            d.SetValue(StoryboardProperty, value);
        }

        /// <summary>
        /// Handles changes to the Storyboard property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnStoryboardChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Storyboard oldStoryboard = (Storyboard)e.OldValue;
            Storyboard newStoryboard = (Storyboard)d.GetValue(StoryboardProperty);

            if (oldStoryboard != null)
                oldStoryboard.Stop();

            if (GetIsPlaying(d) == true &&
                newStoryboard != null)
                newStoryboard.Begin();
        }
        #endregion

        #region IsPlaying
        /// <summary>
        /// IsPlaying Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsPlayingProperty =
            DependencyProperty.RegisterAttached(
                "IsPlaying",
                typeof(bool),
                typeof(AnimationHelper),
                new PropertyMetadata(false, OnIsPlayingChanged));

        /// <summary>
        /// Gets the IsPlaying property. This dependency property 
        /// indicates whether the storyboard is playing.
        /// </summary>
        public static bool GetIsPlaying(DependencyObject d)
        {
            return (bool)d.GetValue(IsPlayingProperty);
        }

        /// <summary>
        /// Sets the IsPlaying property. This dependency property 
        /// indicates whether the storyboard is playing.
        /// </summary>
        public static void SetIsPlaying(DependencyObject d, bool value)
        {
            d.SetValue(IsPlayingProperty, value);
        }

        /// <summary>
        /// Handles changes to the IsPlaying property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnIsPlayingChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool oldIsPlaying = (bool)e.OldValue;
            bool newIsPlaying = (bool)d.GetValue(IsPlayingProperty);

            var storyboard = GetStoryboard(d);
            if (storyboard == null)
                return;

            if (!newIsPlaying)
                storyboard.Stop();

            if (newIsPlaying)
                storyboard.Begin();
        }
        #endregion
    }
}
