using System;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// Transition in which the old page is pushed off the screen by the new page moving into the screen
    /// </summary>
    public class PushTransition : PageTransition
    {
        private readonly Random _random = new Random();

        /// <summary>
        /// Gets the page transition mode.
        /// </summary>
        /// <value>
        /// The page transition mode.
        /// </value>
        protected override PageTransitionMode Mode
        {
            get
            {
                return PageTransitionMode.Parallel;
            }
        }

        #region ForwardDirection
        /// <summary>
        /// ForwardDirection Dependency Property
        /// </summary>
        public static readonly DependencyProperty ForwardDirectionProperty =
            DependencyProperty.Register(
                "ForwardDirection",
                typeof(DirectionOfMotion),
                typeof(PushTransition),
                new PropertyMetadata(DirectionOfMotion.RightToLeft, OnForwardDirectionChanged));

        /// <summary>
        /// Gets or sets the ForwardDirection property. This dependency property 
        /// indicates the forward transition direction.
        /// </summary>
        public DirectionOfMotion ForwardDirection
        {
            get { return (DirectionOfMotion)GetValue(ForwardDirectionProperty); }
            set { SetValue(ForwardDirectionProperty, value); }
        }

        /// <summary>
        /// Handles changes to the ForwardDirection property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnForwardDirectionChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (PushTransition)d;
            DirectionOfMotion oldForwardDirection = (DirectionOfMotion)e.OldValue;
            DirectionOfMotion newForwardDirection = target.ForwardDirection;
            target.OnForwardDirectionChanged(oldForwardDirection, newForwardDirection);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the ForwardDirection property.
        /// </summary>
        /// <param name="oldForwardDirection">The old ForwardDirection value</param>
        /// <param name="newForwardDirection">The new ForwardDirection value</param>
        protected virtual void OnForwardDirectionChanged(
            DirectionOfMotion oldForwardDirection, DirectionOfMotion newForwardDirection)
        {
            if (this.ForwardInAnimation != null)
            {
                ((SlideAnimation)this.ForwardInAnimation).Direction = newForwardDirection;
            }

            if (this.ForwardOutAnimation != null)
            {
                ((SlideAnimation)this.ForwardOutAnimation).Direction = newForwardDirection;
            }
        }
        #endregion

        #region BackwardDirection
        /// <summary>
        /// BackwardDirection Dependency Property
        /// </summary>
        public static readonly DependencyProperty BackwardDirectionProperty =
            DependencyProperty.Register(
                "BackwardDirection",
                typeof(DirectionOfMotion),
                typeof(PushTransition),
                new PropertyMetadata(DirectionOfMotion.LeftToRight, OnBackwardDirectionChanged));

        /// <summary>
        /// Gets or sets the BackwardDirection property. This dependency property 
        /// indicates the backward transition direction.
        /// </summary>
        public DirectionOfMotion BackwardDirection
        {
            get { return (DirectionOfMotion)GetValue(BackwardDirectionProperty); }
            set { SetValue(BackwardDirectionProperty, value); }
        }

        /// <summary>
        /// Handles changes to the BackwardDirection property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnBackwardDirectionChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (PushTransition)d;
            DirectionOfMotion oldBackwardDirection = (DirectionOfMotion)e.OldValue;
            DirectionOfMotion newBackwardDirection = target.BackwardDirection;
            target.OnBackwardDirectionChanged(oldBackwardDirection, newBackwardDirection);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the BackwardDirection property.
        /// </summary>
        /// <param name="oldBackwardDirection">The old BackwardDirection value</param>
        /// <param name="newBackwardDirection">The new BackwardDirection value</param>
        protected virtual void OnBackwardDirectionChanged(
            DirectionOfMotion oldBackwardDirection, DirectionOfMotion newBackwardDirection)
        {
            if (this.BackwardInAnimation != null)
            {
                ((SlideAnimation)this.BackwardInAnimation).Direction = newBackwardDirection;
            }

            if (this.BackwardOutAnimation != null)
            {
                ((SlideAnimation)this.BackwardOutAnimation).Direction = newBackwardDirection;
            }
        }
        #endregion

        public PushTransition()
        {
            this.ForwardOutAnimation =
                new SlideAnimation
                {
                    Direction = ForwardDirection,
                    Mode = AnimationMode.Out
                };
            this.ForwardInAnimation =
                new SlideAnimation
                {
                    Direction = ForwardDirection,
                    Mode = AnimationMode.In
                };
            this.BackwardOutAnimation =
                new SlideAnimation
                {
                    Direction = BackwardDirection,
                    Mode = AnimationMode.Out
                };
            this.BackwardInAnimation =
                new SlideAnimation
                {
                    Direction = BackwardDirection,
                    Mode = AnimationMode.In
                };
        }

        protected override void PrepareForwardAnimations(DependencyObject previousPage, DependencyObject newPage)
        {
            base.PrepareForwardAnimations(previousPage, newPage);

            if (this.ForwardDirection == DirectionOfMotion.Random)
            {
                var randomDirection = (DirectionOfMotion)_random.Next(4);

                if (this.ForwardOutAnimation is SlideAnimation)
                {
                    ((SlideAnimation)this.ForwardOutAnimation).Direction = randomDirection;
                }

                if (this.ForwardInAnimation is SlideAnimation)
                {
                    ((SlideAnimation)this.ForwardInAnimation).Direction = randomDirection;
                }
            }

            //if (previousPage != null)
            //    FrameworkElementExtensions.SetClipToBounds(previousPage, true);
            //if (newPage != null)
            //    FrameworkElementExtensions.SetClipToBounds(newPage, true);
        }

        protected override void PrepareBackwardAnimations(DependencyObject previousPage, DependencyObject newPage)
        {
            base.PrepareBackwardAnimations(previousPage, newPage);

            if (this.BackwardDirection == DirectionOfMotion.Random)
            {
                var randomDirection = (DirectionOfMotion)_random.Next(4);

                if (this.BackwardOutAnimation is SlideAnimation)
                {
                    ((SlideAnimation)this.BackwardOutAnimation).Direction = randomDirection;
                }

                if (this.BackwardInAnimation is SlideAnimation)
                {
                    ((SlideAnimation)this.BackwardInAnimation).Direction = randomDirection;
                }
            }

            //if (previousPage != null)
            //    FrameworkElementExtensions.SetClipToBounds(previousPage, true);
            //if (newPage != null)
            //    FrameworkElementExtensions.SetClipToBounds(newPage, true);
        }

        protected override void CleanupBackwardAnimations(DependencyObject previousPage, DependencyObject newPage)
        {
            base.CleanupBackwardAnimations(previousPage, newPage);

            //if (previousPage != null)
            //    FrameworkElementExtensions.SetClipToBounds(previousPage, false);
            //if (newPage != null)
            //    FrameworkElementExtensions.SetClipToBounds(newPage, false);
        }

        protected override void CleanupForwardAnimations(DependencyObject previousPage, DependencyObject newPage)
        {
            base.CleanupForwardAnimations(previousPage, newPage);

            //if (previousPage != null)
            //    FrameworkElementExtensions.SetClipToBounds(previousPage, false);
            //if (newPage != null)
            //    FrameworkElementExtensions.SetClipToBounds(newPage, false);
            
            // TODO: Get the transitions container panel and clip that one for push transitions
            // TODO: Consider adding a fade in/out option to this transition
        }
    }
}