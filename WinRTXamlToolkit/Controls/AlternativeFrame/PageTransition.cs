using System;
using System.Threading.Tasks;
using WinRTXamlToolkit.AwaitableUI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// Abstract base class for page transitions used with AlternativeFrame control.
    /// </summary>
    public abstract class PageTransition : DependencyObject
    {
        /// <summary>
        /// Gets the page transition mode.
        /// </summary>
        /// <value>
        /// The page transition mode.
        /// </value>
        protected abstract PageTransitionMode Mode { get; }

        #region ForwardOutAnimation
        /// <summary>
        /// ForwardOutAnimation Dependency Property
        /// </summary>
        public static readonly DependencyProperty ForwardOutAnimationProperty =
            DependencyProperty.Register(
                "ForwardOutAnimation",
                typeof(PageTransitionAnimation),
                typeof(PageTransition),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the ForwardOutAnimation property. This dependency property 
        /// indicates the animation to use during forward navigation to remove the previous page from view.
        /// </summary>
        public PageTransitionAnimation ForwardOutAnimation
        {
            get { return (PageTransitionAnimation)GetValue(ForwardOutAnimationProperty); }
            set { SetValue(ForwardOutAnimationProperty, value); }
        }
        #endregion

        #region ForwardInAnimation
        /// <summary>
        /// ForwardInAnimation Dependency Property
        /// </summary>
        public static readonly DependencyProperty ForwardInAnimationProperty =
            DependencyProperty.Register(
                "ForwardInAnimation",
                typeof(PageTransitionAnimation),
                typeof(PageTransition),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the ForwardInAnimation property. This dependency property 
        /// indicates the animation to use during forward navigation to bring the new page into view.
        /// </summary>
        public PageTransitionAnimation ForwardInAnimation
        {
            get { return (PageTransitionAnimation)GetValue(ForwardInAnimationProperty); }
            set { SetValue(ForwardInAnimationProperty, value); }
        }
        #endregion

        #region BackwardOutAnimation
        /// <summary>
        /// BackwardOutAnimation Dependency Property
        /// </summary>
        public static readonly DependencyProperty BackwardOutAnimationProperty =
            DependencyProperty.Register(
                "BackwardOutAnimation",
                typeof(PageTransitionAnimation),
                typeof(PageTransition),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the BackwardOutAnimation property. This dependency property 
        /// indicates the animation to use during backward navigation to remove the previous page from view.
        /// </summary>
        public PageTransitionAnimation BackwardOutAnimation
        {
            get { return (PageTransitionAnimation)GetValue(BackwardOutAnimationProperty); }
            set { SetValue(BackwardOutAnimationProperty, value); }
        }
        #endregion

        #region BackwardInAnimation
        /// <summary>
        /// BackwardInAnimation Dependency Property
        /// </summary>
        public static readonly DependencyProperty BackwardInAnimationProperty =
            DependencyProperty.Register(
                "BackwardInAnimation",
                typeof(PageTransitionAnimation),
                typeof(PageTransition),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the BackwardInAnimation property. This dependency property 
        /// indicates the animation to use during backward navigation to bring the new page into view.
        /// </summary>
        public PageTransitionAnimation BackwardInAnimation
        {
            get { return (PageTransitionAnimation)GetValue(BackwardInAnimationProperty); }
            set { SetValue(BackwardInAnimationProperty, value); }
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
                typeof(PageTransition),
                new PropertyMetadata(new Duration(TimeSpan.FromSeconds(0.4))));

        /// <summary>
        /// Gets or sets the Duration property. This dependency property 
        /// indicates the length of time for which the transition plays.
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
                typeof(PageTransition),
                new PropertyMetadata(new CubicEase { EasingMode = EasingMode.EaseOut }));

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

        #region TransitionForward()
        public async Task TransitionForward(DependencyObject previousPage, DependencyObject newPage)
        {
            if (previousPage == null && newPage == null)
            {
                throw new InvalidOperationException();
            }

            PrepareForwardAnimations(previousPage, newPage);

            UpdateTimelineAttributes();

            if (previousPage == null)
            {
                if (ForwardInAnimation != null)
                {
                    await ForwardInAnimation.Animate(newPage);
                }
            }
            else if (newPage == null)
            {
                if (ForwardOutAnimation != null)
                {
                    await ForwardOutAnimation.Animate(previousPage);
                }
            }
            else if (this.Mode == PageTransitionMode.Parallel)
            {
                var sb = new Storyboard();

                Storyboard outSb = null;
                Storyboard inSb = null;

                if (this.ForwardOutAnimation != null)
                {
                    outSb = this.ForwardOutAnimation.GetAnimation(previousPage);
                    sb.Children.Add(outSb);
                }

                if (this.ForwardInAnimation != null)
                {
                    inSb = this.ForwardInAnimation.GetAnimation(newPage);
                    sb.Children.Add(inSb);
                }

                await sb.BeginAsync();
                sb.Stop();
                sb.Children.Clear();

                if (this.ForwardOutAnimation != null)
                {
                    this.ForwardOutAnimation.CleanupAnimation(previousPage, outSb);
                }

                if (this.ForwardInAnimation != null)
                {
                    this.ForwardInAnimation.CleanupAnimation(newPage, inSb);
                }
                //await Task.WhenAll(
                //    ForwardOutAnimation.Animate(previousPage),
                //    ForwardInAnimation.Animate(newPage));
            }
            else
            {
                if (this.ForwardOutAnimation != null)
                {
                    await this.ForwardOutAnimation.Animate(previousPage);
                }

                if (this.ForwardInAnimation != null)
                {
                    await this.ForwardInAnimation.Animate(newPage);
                }
            }

            CleanupForwardAnimations(previousPage, newPage);
        }
        #endregion

        #region TransitionBackward()
        public async Task TransitionBackward(DependencyObject previousPage, DependencyObject newPage)
        {
            if (previousPage == null && newPage == null)
            {
                throw new InvalidOperationException();
            }

            PrepareBackwardAnimations(previousPage, newPage);

            UpdateTimelineAttributes();

            if (previousPage == null)
            {
                await BackwardInAnimation.Animate(newPage);
            }
            else if (newPage == null)
            {
                await BackwardOutAnimation.Animate(previousPage);
            }
            else if (this.Mode == PageTransitionMode.Parallel)
            {
                var sb = new Storyboard();

                Storyboard outSb = null;
                Storyboard inSb = null;

                if (this.BackwardOutAnimation != null)
                {
                    outSb = this.BackwardOutAnimation.GetAnimation(previousPage);
                    sb.Children.Add(outSb);
                }

                if (this.BackwardInAnimation != null)
                {
                    inSb = this.BackwardInAnimation.GetAnimation(newPage);
                    sb.Children.Add(inSb);
                }
                
                await sb.BeginAsync();
                sb.Stop();
                sb.Children.Clear();

                if (this.BackwardOutAnimation != null)
                {
                    this.BackwardOutAnimation.CleanupAnimation(previousPage, outSb);
                }

                if (this.BackwardInAnimation != null)
                {
                    this.BackwardInAnimation.CleanupAnimation(newPage, inSb);
                }
                //await Task.WhenAll(
                //    BackwardOutAnimation.Animate(previousPage),
                //    BackwardInAnimation.Animate(newPage));
            }
            else
            {
                if (this.BackwardOutAnimation != null)
                {
                    await this.BackwardOutAnimation.Animate(previousPage);
                }

                if (this.BackwardInAnimation != null)
                {
                    await this.BackwardInAnimation.Animate(newPage);
                }
            }

            CleanupBackwardAnimations(previousPage, newPage);
        }
        #endregion

        protected virtual void UpdateTimelineAttributes()
        {
            if (this.Mode == PageTransitionMode.Parallel)
            {
                if (this.ForwardInAnimation != null)
                {
                    this.ForwardInAnimation.Duration = this.Duration;
                    this.ForwardInAnimation.EasingFunction = this.EasingFunction;
                }

                if (this.ForwardOutAnimation != null)
                {
                    this.ForwardOutAnimation.Duration = this.Duration;
                    this.ForwardOutAnimation.EasingFunction = this.EasingFunction;
                }

                if (this.BackwardInAnimation != null)
                {
                    this.BackwardInAnimation.Duration = this.Duration;
                    this.BackwardInAnimation.EasingFunction = this.EasingFunction;
                }

                if (this.BackwardOutAnimation != null)
                {
                    this.BackwardOutAnimation.Duration = this.Duration;
                    this.BackwardOutAnimation.EasingFunction = this.EasingFunction;
                }
            }
            else
            {
                if (this.ForwardInAnimation != null)
                {
                    this.ForwardInAnimation.Duration = TimeSpan.FromSeconds(this.Duration.TimeSpan.TotalSeconds * 0.5);
                    this.ForwardInAnimation.EasingFunction = this.EasingFunction;
                }

                if (this.ForwardOutAnimation != null)
                {
                    this.ForwardOutAnimation.Duration = TimeSpan.FromSeconds(this.Duration.TimeSpan.TotalSeconds * 0.5);
                    this.ForwardOutAnimation.EasingFunction = this.EasingFunction;
                }

                if (this.BackwardInAnimation != null)
                {
                    this.BackwardInAnimation.Duration = TimeSpan.FromSeconds(this.Duration.TimeSpan.TotalSeconds * 0.5);
                    this.BackwardInAnimation.EasingFunction = this.EasingFunction;
                }

                if (this.BackwardOutAnimation != null)
                {
                    this.BackwardOutAnimation.Duration = TimeSpan.FromSeconds(this.Duration.TimeSpan.TotalSeconds * 0.5);
                    this.BackwardOutAnimation.EasingFunction = this.EasingFunction;
                }
            }
        }

        protected virtual void PrepareForwardAnimations(DependencyObject previousPage, DependencyObject newPage)
        {
        }

        protected virtual void PrepareBackwardAnimations(DependencyObject previousPage, DependencyObject newPage)
        {
        }

        protected virtual void CleanupForwardAnimations(DependencyObject previousPage, DependencyObject newPage)
        {
        }

        protected virtual void CleanupBackwardAnimations(DependencyObject previousPage, DependencyObject newPage)
        {
        }
    }
}
