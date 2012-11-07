using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// The transition where the new page gets revealed with a moving rectangular clip rectangle.
    /// </summary>
    public class WipeTransition : PageTransition
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
                typeof(WipeTransition),
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
            var target = (WipeTransition)d;
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
                ((WipeAnimation)this.ForwardInAnimation).Direction = newForwardDirection;
            }

            if (this.ForwardOutAnimation != null)
            {
                ((WipeAnimation)this.ForwardOutAnimation).Direction = newForwardDirection;
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
                typeof(WipeTransition),
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
            var target = (WipeTransition)d;
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
                ((WipeAnimation)this.BackwardInAnimation).Direction = newBackwardDirection;
            }

            if (this.BackwardOutAnimation != null)
            {
                ((WipeAnimation)this.BackwardOutAnimation).Direction = newBackwardDirection;
            }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="WipeTransition" /> class.
        /// </summary>
        public WipeTransition()
        {
            this.ForwardOutAnimation = null;
                //new WipeAnimation
                //{
                //    Direction = ForwardDirection,
                //    Mode = AnimationMode.Out
                //};
            this.ForwardInAnimation =
                new WipeAnimation
                {
                    Direction = ForwardDirection,
                    Mode = AnimationMode.In
                };
            this.BackwardOutAnimation = null;
                //new WipeAnimation
                //{
                //    Direction = BackwardDirection,
                //    Mode = AnimationMode.Out
                //};
            this.BackwardInAnimation =
                new WipeAnimation
                {
                    Direction = BackwardDirection,
                    Mode = AnimationMode.In
                };
        }

        protected override void PrepareForwardAnimations(DependencyObject previousPage, DependencyObject newPage)
        {
            base.PrepareForwardAnimations(previousPage, newPage);

            Canvas.SetZIndex((UIElement)newPage, 1);

            if (this.ForwardDirection == DirectionOfMotion.Random)
            {
                var randomDirection = (DirectionOfMotion)_random.Next(4);

                //if (this.ForwardOutAnimation is WipeAnimation)
                //{
                //    ((WipeAnimation)this.ForwardOutAnimation).Direction = randomDirection;
                //}

                if (this.ForwardInAnimation is WipeAnimation)
                {
                    ((WipeAnimation)this.ForwardInAnimation).Direction = randomDirection;
                }
            }
        }

        protected override void PrepareBackwardAnimations(DependencyObject previousPage, DependencyObject newPage)
        {
            base.PrepareBackwardAnimations(previousPage, newPage);

            Canvas.SetZIndex((UIElement)newPage, 1);

            if (this.BackwardDirection == DirectionOfMotion.Random)
            {
                var randomDirection = (DirectionOfMotion)_random.Next(4);

                //if (this.BackwardOutAnimation is WipeAnimation)
                //{
                //    ((WipeAnimation)this.BackwardOutAnimation).Direction = randomDirection;
                //}

                if (this.BackwardInAnimation is WipeAnimation)
                {
                    ((WipeAnimation)this.BackwardInAnimation).Direction = randomDirection;
                }
            }
        }

        protected override void CleanupBackwardAnimations(DependencyObject previousPage, DependencyObject newPage)
        {
            newPage.ClearValue(Canvas.ZIndexProperty);
            base.CleanupBackwardAnimations(previousPage, newPage);
            ((FrameworkElement)newPage).Clip = null;
        }

        protected override void CleanupForwardAnimations(DependencyObject previousPage, DependencyObject newPage)
        {
            newPage.ClearValue(Canvas.ZIndexProperty);
            base.CleanupForwardAnimations(previousPage, newPage);
            ((FrameworkElement)newPage).Clip = null;
        }
    }
}
