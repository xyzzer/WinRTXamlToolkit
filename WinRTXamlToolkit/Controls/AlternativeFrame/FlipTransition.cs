using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// 3D flip transition
    /// </summary>
    public class FlipTransition : PageTransition
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
                typeof(FlipTransition),
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
            var target = (FlipTransition)d;
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
                ((FlipAnimation)this.ForwardInAnimation).Direction = newForwardDirection;
            }

            if (this.ForwardOutAnimation != null)
            {
                ((FlipAnimation)this.ForwardOutAnimation).Direction = newForwardDirection;
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
                typeof(FlipTransition),
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
            var target = (FlipTransition)d;
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
                ((FlipAnimation)this.BackwardInAnimation).Direction = newBackwardDirection;
            }

            if (this.BackwardOutAnimation != null)
            {
                ((FlipAnimation)this.BackwardOutAnimation).Direction = newBackwardDirection;
            }
        }
        #endregion

        #region AxisOfFlip
        /// <summary>
        /// AxisOfFlip Dependency Property
        /// </summary>
        public static readonly DependencyProperty AxisOfFlipProperty =
            DependencyProperty.Register(
                "AxisOfFlip",
                typeof(AxisOfFlip),
                typeof(FlipTransition),
                new PropertyMetadata(AxisOfFlip.LeftOrTop, OnAxisOfFlipChanged));

        /// <summary>
        /// Gets or sets the AxisOfFlip property. This dependency property 
        /// indicates the position of the axis to flip around.
        /// </summary>
        public AxisOfFlip AxisOfFlip
        {
            get { return (AxisOfFlip)GetValue(AxisOfFlipProperty); }
            set { SetValue(AxisOfFlipProperty, value); }
        }

        /// <summary>
        /// Handles changes to the AxisOfFlip property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnAxisOfFlipChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (FlipTransition)d;
            AxisOfFlip oldAxisOfFlip = (AxisOfFlip)e.OldValue;
            AxisOfFlip newAxisOfFlip = target.AxisOfFlip;
            target.OnAxisOfFlipChanged(oldAxisOfFlip, newAxisOfFlip);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the AxisOfFlip property.
        /// </summary>
        /// <param name="oldAxisOfFlip">The old AxisOfFlip value</param>
        /// <param name="newAxisOfFlip">The new AxisOfFlip value</param>
        private void OnAxisOfFlipChanged(
            AxisOfFlip oldAxisOfFlip, AxisOfFlip newAxisOfFlip)
        {
            if (this.ForwardInAnimation != null)
            {
                ((FlipAnimation)this.ForwardInAnimation).AxisOfFlip = newAxisOfFlip;
            }

            if (this.ForwardOutAnimation != null)
            {
                ((FlipAnimation)this.ForwardOutAnimation).AxisOfFlip = newAxisOfFlip;
            }

            if (this.BackwardInAnimation != null)
            {
                ((FlipAnimation)this.BackwardInAnimation).AxisOfFlip = newAxisOfFlip;
            }

            if (this.BackwardOutAnimation != null)
            {
                ((FlipAnimation)this.BackwardOutAnimation).AxisOfFlip = newAxisOfFlip;
            }
        }
        #endregion

        public FlipTransition()
        {
            this.ForwardOutAnimation =
                new FlipAnimation
                {
                    Direction = this.ForwardDirection,
                    Mode = AnimationMode.ForwardOut,
                    AxisOfFlip = this.AxisOfFlip
                };
            this.ForwardInAnimation =
                new FlipAnimation
                {
                    Direction = this.ForwardDirection,
                    Mode = AnimationMode.ForwardIn,
                    AxisOfFlip = this.AxisOfFlip
                };
            this.BackwardOutAnimation =
                new FlipAnimation
                {
                    Direction = this.BackwardDirection,
                    Mode = AnimationMode.BackwardOut,
                    AxisOfFlip = this.AxisOfFlip
                };
            this.BackwardInAnimation =
                new FlipAnimation
                {
                    Direction = this.BackwardDirection,
                    Mode = AnimationMode.BackwardIn,
                    AxisOfFlip = this.AxisOfFlip
                };
        }

        protected override void PrepareForwardAnimations(DependencyObject previousPage, DependencyObject newPage)
        {
            base.PrepareForwardAnimations(previousPage, newPage);

            Canvas.SetZIndex((UIElement)newPage, -1);

            if (this.ForwardDirection == DirectionOfMotion.Random)
            {
                var randomDirection = (DirectionOfMotion)_random.Next(4);

                if (this.ForwardOutAnimation is FlipAnimation)
                {
                    ((FlipAnimation)this.ForwardOutAnimation).Direction = randomDirection;
                }

                if (this.ForwardInAnimation is FlipAnimation)
                {
                    ((FlipAnimation)this.ForwardInAnimation).Direction = randomDirection;
                }
            }

            if (this.AxisOfFlip == AxisOfFlip.Random)
            {
                var randomAxis = (AxisOfFlip)_random.Next(3);

                if (this.ForwardOutAnimation is FlipAnimation)
                {
                    ((FlipAnimation)this.ForwardOutAnimation).AxisOfFlip = randomAxis;
                }

                if (this.ForwardInAnimation is FlipAnimation)
                {
                    ((FlipAnimation)this.ForwardInAnimation).AxisOfFlip = randomAxis;
                }
            }
        }

        protected override void CleanupForwardAnimations(DependencyObject previousPage, DependencyObject newPage)
        {
            base.CleanupForwardAnimations(previousPage, newPage);
            newPage.ClearValue(Canvas.ZIndexProperty);
        }

        protected override void PrepareBackwardAnimations(DependencyObject previousPage, DependencyObject newPage)
        {
            base.PrepareBackwardAnimations(previousPage, newPage);

            if (this.BackwardDirection == DirectionOfMotion.Random)
            {
                var randomDirection = (DirectionOfMotion)_random.Next(4);

                if (this.BackwardOutAnimation is FlipAnimation)
                {
                    ((FlipAnimation)this.BackwardOutAnimation).Direction = randomDirection;
                }

                if (this.BackwardInAnimation is FlipAnimation)
                {
                    ((FlipAnimation)this.BackwardInAnimation).Direction = randomDirection;
                }
            }

            if (this.AxisOfFlip == AxisOfFlip.Random)
            {
                var randomAxis = (AxisOfFlip)_random.Next(3);

                if (this.BackwardOutAnimation is FlipAnimation)
                {
                    ((FlipAnimation)this.BackwardOutAnimation).AxisOfFlip = randomAxis;
                }

                if (this.BackwardInAnimation is FlipAnimation)
                {
                    ((FlipAnimation)this.BackwardInAnimation).AxisOfFlip = randomAxis;
                }
            }
        }
    }
}
