using System;
using System.Threading.Tasks;
using WinRTXamlToolkit.AwaitableUI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Controls.Extensions
{
    /// <summary>
    /// Extension methods and attached properties for UIElement class.
    /// </summary>
    public static class UIElementAnimationExtensions
    {
        #region AttachedFadeStoryboard
        /// <summary>
        /// AttachedFadeStoryboard Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty AttachedFadeStoryboardProperty =
            DependencyProperty.RegisterAttached(
                "AttachedFadeStoryboard",
                typeof(Storyboard),
                typeof(UIElementAnimationExtensions),
                new PropertyMetadata(null, OnAttachedFadeStoryboardChanged));

        /// <summary>
        /// Gets the AttachedFadeStoryboard property. This dependency property 
        /// indicates the currently running custom fade in/out storyboard.
        /// </summary>
        private static Storyboard GetAttachedFadeStoryboard(DependencyObject d)
        {
            return (Storyboard)d.GetValue(AttachedFadeStoryboardProperty);
        }

        /// <summary>
        /// Sets the AttachedFadeStoryboard property. This dependency property 
        /// indicates the currently running custom fade in/out storyboard.
        /// </summary>
        private static void SetAttachedFadeStoryboard(DependencyObject d, Storyboard value)
        {
            d.SetValue(AttachedFadeStoryboardProperty, value);
        }

        /// <summary>
        /// Handles changes to the AttachedFadeStoryboard property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnAttachedFadeStoryboardChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Storyboard oldAttachedFadeStoryboard = (Storyboard)e.OldValue;
            Storyboard newAttachedFadeStoryboard = (Storyboard)d.GetValue(AttachedFadeStoryboardProperty);
        }
        #endregion

        #region FadeIn()
        /// <summary>
        /// Fades the element in using the FadeInThemeAnimation.
        /// </summary>
        /// <remarks>
        /// Opacity property of the element is not affected.<br/>
        /// The duration of the visible animation itself is not affected by the duration parameter. It merely indicates how long the Storyboard will run.<br/>
        /// If FadeOutThemeAnimation was not used on the element before - nothing will happen.<br/>
        /// </remarks>
        /// <param name="element"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static async Task FadeIn(this UIElement element, TimeSpan? duration = null)
        {
            ((FrameworkElement)element).Visibility = Visibility.Visible;
            var fadeInStoryboard = new Storyboard();
            var fadeInAnimation = new FadeInThemeAnimation();

            if (duration != null)
            {
                fadeInAnimation.Duration = duration.Value;
            }

            Storyboard.SetTarget(fadeInAnimation, element);
            fadeInStoryboard.Children.Add(fadeInAnimation);
            await fadeInStoryboard.BeginAsync();
        } 
        #endregion

        #region FadeOut()
        /// <summary>
        /// Fades the element out using the FadeOutThemeAnimation.
        /// </summary>
        /// <remarks>
        /// Opacity property of the element is not affected.<br/>
        /// The duration of the visible animation itself is not affected by the duration parameter. It merely indicates how long the Storyboard will run.<br/>
        /// If FadeOutThemeAnimation was already run before and FadeInThemeAnimation was not run after that - nothing will happen.<br/>
        /// </remarks>
        /// <param name="element"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static async Task FadeOut(this UIElement element, TimeSpan? duration = null)
        {
            var fadeOutStoryboard = new Storyboard();
            var fadeOutAnimation = new FadeOutThemeAnimation();

            if (duration != null)
            {
                fadeOutAnimation.Duration = duration.Value;
            }

            Storyboard.SetTarget(fadeOutAnimation, element);
            fadeOutStoryboard.Children.Add(fadeOutAnimation);
            await fadeOutStoryboard.BeginAsync();
        } 
        #endregion

        #region FadeInCustom()
        /// <summary>
        /// Fades the element in using a custom DoubleAnimation of the Opacity property.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="duration"></param>
        /// <param name="easingFunction"> </param>
        /// <returns></returns>
        public static async Task FadeInCustom(this UIElement element, TimeSpan? duration = null, EasingFunctionBase easingFunction = null, double targetOpacity = 1.0)
        {
            CleanUpPreviousFadeStoryboard(element);

            var fadeInStoryboard = new Storyboard();
            var fadeInAnimation = new DoubleAnimation();

            if (duration == null)
                duration = TimeSpan.FromSeconds(0.4);

            fadeInAnimation.Duration = duration.Value;
            fadeInAnimation.To = targetOpacity;
            fadeInAnimation.EasingFunction = easingFunction;

            Storyboard.SetTarget(fadeInAnimation, element);
            Storyboard.SetTargetProperty(fadeInAnimation, "Opacity");
            fadeInStoryboard.Children.Add(fadeInAnimation);
            SetAttachedFadeStoryboard(element, fadeInStoryboard);
            await fadeInStoryboard.BeginAsync();
            element.Opacity = targetOpacity;
            fadeInStoryboard.Stop();
        }
        #endregion

        #region FadeOutCustom()
        /// <summary>
        /// Fades the element out using a custom DoubleAnimation of the Opacity property.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="duration"></param>
        /// <param name="easingFunction"> </param>
        /// <returns></returns>
        public static async Task FadeOutCustom(this UIElement element, TimeSpan? duration = null, EasingFunctionBase easingFunction = null)
        {
            CleanUpPreviousFadeStoryboard(element); 
            
            var fadeOutStoryboard = new Storyboard();
            var fadeOutAnimation = new DoubleAnimation();

            if (duration == null)
                duration = TimeSpan.FromSeconds(0.4);

            fadeOutAnimation.Duration = duration.Value;
            fadeOutAnimation.To = 0.0;
            fadeOutAnimation.EasingFunction = easingFunction;

            Storyboard.SetTarget(fadeOutAnimation, element);
            Storyboard.SetTargetProperty(fadeOutAnimation, "Opacity");
            fadeOutStoryboard.Children.Add(fadeOutAnimation);
            SetAttachedFadeStoryboard(element, fadeOutStoryboard);
            await fadeOutStoryboard.BeginAsync();
            element.Opacity = 0.0;
            fadeOutStoryboard.Stop();
        } 
        #endregion

        #region CleanUpPreviousFadeStoryboard()
        public static void CleanUpPreviousFadeStoryboard(this UIElement element)
        {
            var attachedFadeStoryboard = GetAttachedFadeStoryboard(element);

            if (attachedFadeStoryboard != null)
            {
                attachedFadeStoryboard.Stop();
            }
        }
        #endregion
    }
}
