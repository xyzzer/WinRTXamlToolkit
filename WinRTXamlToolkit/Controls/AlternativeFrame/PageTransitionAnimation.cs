using System;
using System.Threading.Tasks;
using WinRTXamlToolkit.AwaitableUI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// Base class for page transition animations used with an AlternativeFrame control.
    /// </summary>
    public abstract class PageTransitionAnimation : DependencyObject
    {
        /// <summary>
        /// Gets the generated animation.
        /// </summary>
        /// <value>
        /// The animation.
        /// </value>
        protected abstract Storyboard Animation { get; }

        #region Mode
        /// <summary>
        /// Mode Dependency Property
        /// </summary>
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register(
                "Mode",
                typeof(AnimationMode),
                typeof(PageTransitionAnimation),
                new PropertyMetadata(AnimationMode.Out));

        /// <summary>
        /// Gets or sets the Mode property. This dependency property 
        /// indicates whether this is an animation to slide in or out.
        /// </summary>
        public AnimationMode Mode
        {
            get
            {
                return (AnimationMode)GetValue(ModeProperty);
            }
            set
            {
                SetValue(ModeProperty, value);
            }
        }
        #endregion

        #region Duration
        /// <summary>
        /// Duration Dependency Property
        /// </summary>
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register(
                "Duration",
                typeof(Duration),
                typeof(PageTransitionAnimation),
                new PropertyMetadata(new Duration(TimeSpan.FromSeconds(0.4))));

        /// <summary>
        /// Gets or sets the Duration property. This dependency property 
        /// indicates the length of time for which this timeline plays, not counting repetitions.
        /// </summary>
        public Duration Duration
        {
            get { return (Duration)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }
        #endregion

        #region EasingFunction
        /// <summary>
        /// EasingFunction Dependency Property
        /// </summary>
        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register(
                "EasingFunction",
                typeof(EasingFunctionBase),
                typeof(PageTransitionAnimation),
                new PropertyMetadata(new CubicEase{EasingMode = EasingMode.EaseOut}));

        /// <summary>
        /// Gets or sets the EasingFunction property. This dependency property 
        /// indicates the easing function applied to the transition animation.
        /// </summary>
        public EasingFunctionBase EasingFunction
        {
            get { return (EasingFunctionBase)GetValue(EasingFunctionProperty); }
            set { SetValue(EasingFunctionProperty, value); }
        }
        #endregion

        protected virtual void ApplyTargetProperties(DependencyObject target, Storyboard animation)
        {
        }

        internal virtual void CleanupAnimation(DependencyObject target, Storyboard animation)
        {
        }

        internal Storyboard GetAnimation(DependencyObject target)
        {
            var anim = this.Animation;
            Storyboard.SetTarget(anim, target);
            this.ApplyTargetProperties(target, anim);
            return anim;
        }

        internal async Task Animate(DependencyObject target)
        {
            var anim = this.Animation;
            Storyboard.SetTarget(anim, target);
            this.ApplyTargetProperties(target, anim);
            await anim.BeginAsync();
            anim.Stop();
            this.CleanupAnimation(target, anim);
        }
    }
}