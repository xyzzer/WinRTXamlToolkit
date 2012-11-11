using System;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// Animation used in FlipTransition for a 3D flip of a page element.
    /// </summary>
    public class FlipAnimation : PageTransitionAnimation
    {
        #region Direction
        /// <summary>
        /// Direction Dependency Property
        /// </summary>
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register(
                "Direction",
                typeof(DirectionOfMotion),
                typeof(FlipAnimation),
                new PropertyMetadata(DirectionOfMotion.RightToLeft));

        /// <summary>
        /// Gets or sets the Direction property. This dependency property 
        /// indicates the slide direction.
        /// </summary>
        public DirectionOfMotion Direction
        {
            get { return (DirectionOfMotion)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
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
                typeof(FlipAnimation),
                new PropertyMetadata(AxisOfFlip.LeftOrTop));

        /// <summary>
        /// Gets or sets the AxisOfFlip property. This dependency property 
        /// indicates the position of the axis to flip around.
        /// </summary>
        public AxisOfFlip AxisOfFlip
        {
            get { return (AxisOfFlip)GetValue(AxisOfFlipProperty); }
            set { SetValue(AxisOfFlipProperty, value); }
        }
        #endregion

        protected override Storyboard Animation
        {
            get
            {
                // NOTE: There seem to be problems with WinRT when reusing same Storyboard for multiple elements, so we need to always get a new storyboard.
                var sb = new Storyboard();

                var flipAnimation = new DoubleAnimation();
                flipAnimation.EasingFunction = this.EasingFunction;
                flipAnimation.Duration = this.Duration;
                sb.Children.Add(flipAnimation);

                var visibilityAnimation = new ObjectAnimationUsingKeyFrames();

                if (this.AxisOfFlip == AxisOfFlip.Center)
                {
                    visibilityAnimation.KeyFrames.Add(
                        new DiscreteObjectKeyFrame
                        {
                            KeyTime = KeyTime.FromTimeSpan(new TimeSpan(0)),
                            Value =
                                this.Mode == AnimationMode.ForwardIn || this.Mode == AnimationMode.BackwardIn
                                    ? Visibility.Collapsed
                                    : Visibility.Visible
                        });

                    double normalizedTimeOfMidFlip = 0;
                    while (this.EasingFunction.Ease(normalizedTimeOfMidFlip) < 0.5)
                        normalizedTimeOfMidFlip += 0.001;

                    visibilityAnimation.KeyFrames.Add(
                        new DiscreteObjectKeyFrame
                        {
                            KeyTime =
                                KeyTime.FromTimeSpan(
                                    TimeSpan.FromSeconds(
                                        this.Duration.TimeSpan.TotalSeconds * normalizedTimeOfMidFlip)),
                            Value =
                                this.Mode == AnimationMode.ForwardIn || this.Mode == AnimationMode.BackwardIn
                                    ? Visibility.Visible
                                    : Visibility.Collapsed
                        });
                }
                sb.Children.Add(visibilityAnimation);

                return sb;
            }
        }

        protected override void ApplyTargetProperties(DependencyObject target, Storyboard animation)
        {
            var fe = (FrameworkElement)target;

            var projection = fe.Projection as PlaneProjection;

            if (projection == null)
            {
                fe.Projection = projection = new PlaneProjection();
            }

            var transform = fe.RenderTransform as TranslateTransform;

            if (transform == null)
            {
                fe.RenderTransform = transform = new TranslateTransform();
            }

            if (this.Direction == DirectionOfMotion.TopToBottom || this.Direction == DirectionOfMotion.BottomToTop)
            {
                if (this.AxisOfFlip == AxisOfFlip.LeftOrTop)
                {
                    projection.CenterOfRotationY = 0;
                    projection.GlobalOffsetY = fe.ActualHeight / 2;
                    transform.Y = -fe.ActualHeight / 2;
                }
                else if (this.AxisOfFlip == AxisOfFlip.RightOrBottom)
                {
                    projection.CenterOfRotationY = 1.0;
                    projection.GlobalOffsetY = -fe.ActualHeight / 2;
                    transform.Y = fe.ActualHeight / 2;
                }
                else
                {
                    projection.CenterOfRotationY = 0.5;
                }
            }
            else
            {
                if (this.AxisOfFlip == AxisOfFlip.LeftOrTop)
                {
                    projection.CenterOfRotationX = 0;
                    projection.GlobalOffsetX = fe.ActualWidth / 2;
                    transform.X = -fe.ActualWidth / 2;
                }
                else if (this.AxisOfFlip == AxisOfFlip.RightOrBottom)
                {
                    projection.CenterOfRotationX = 1.0;
                    projection.GlobalOffsetX = -fe.ActualWidth / 2;
                    transform.X = fe.ActualWidth / 2;
                }
                else
                {
                    projection.CenterOfRotationX = 0.5;
                }
            }

            var flipAnimation = (DoubleAnimation)animation.Children[0];
            Storyboard.SetTarget(flipAnimation, projection);
            Storyboard.SetTargetProperty(flipAnimation,
                Direction == DirectionOfMotion.TopToBottom || Direction == DirectionOfMotion.BottomToTop ?
                "RotationX" :
                "RotationY");

            var visibilityAnimation = (ObjectAnimationUsingKeyFrames)animation.Children[1];
            Storyboard.SetTarget(visibilityAnimation, fe);
            Storyboard.SetTargetProperty(visibilityAnimation, "Visibility");

            if (this.AxisOfFlip == AxisOfFlip.Center)
            {
                const double TopAngleFull = -180;
                const double BottomAngleFull = 180;
                const double LeftAngleFull = 180;
                const double RightAngleFull = -180;

                if (this.Mode == AnimationMode.ForwardOut || this.Mode == AnimationMode.BackwardOut)
                {
                    switch (this.Direction)
                    {
                        case DirectionOfMotion.TopToBottom:
                            flipAnimation.From = 0;
                            flipAnimation.To = BottomAngleFull;
                            break;
                        case DirectionOfMotion.BottomToTop:
                            flipAnimation.From = 0;
                            flipAnimation.To = TopAngleFull;
                            break;
                        case DirectionOfMotion.LeftToRight:
                            flipAnimation.From = 0;
                            flipAnimation.To = RightAngleFull;
                            break;
                        case DirectionOfMotion.RightToLeft:
                            flipAnimation.From = 0;
                            flipAnimation.To = LeftAngleFull;
                            break;
                    }
                }
                else //if (this.Mode == AnimationMode.In)
                {
                    switch (this.Direction)
                    {
                        case DirectionOfMotion.TopToBottom:
                            flipAnimation.From = -TopAngleFull;
                            flipAnimation.To = 0;
                            break;
                        case DirectionOfMotion.BottomToTop:
                            flipAnimation.From = BottomAngleFull;
                            flipAnimation.To = 0;
                            break;
                        case DirectionOfMotion.LeftToRight:
                            flipAnimation.From = LeftAngleFull;
                            flipAnimation.To = 0;
                            break;
                        case DirectionOfMotion.RightToLeft:
                            flipAnimation.From = RightAngleFull;
                            flipAnimation.To = 0;
                            break;
                    }
                }
            }
            else
            {
                const double TopAngle = -90;
                const double BottomAngle = 90;
                const double LeftAngle = 90;
                const double RightAngle = -90;

                switch (this.Mode)
                {
                    case AnimationMode.ForwardIn:
                    case AnimationMode.BackwardOut:
                        flipAnimation.From = 0;
                        flipAnimation.To = 0;
                        break;
                    case AnimationMode.BackwardIn:
                        switch (this.Direction)
                        {
                            case DirectionOfMotion.TopToBottom:
                            case DirectionOfMotion.BottomToTop:
                                flipAnimation.From = this.AxisOfFlip == AxisOfFlip.LeftOrTop ? TopAngle : BottomAngle;
                                flipAnimation.To = 0;
                                break;
                            case DirectionOfMotion.LeftToRight:
                            case DirectionOfMotion.RightToLeft:
                                flipAnimation.From = this.AxisOfFlip == AxisOfFlip.LeftOrTop ? LeftAngle : RightAngle;
                                flipAnimation.To = 0;
                                break;
                        }
                        break;
                    case AnimationMode.ForwardOut:
                        switch (this.Direction)
                        {
                            case DirectionOfMotion.TopToBottom:
                            case DirectionOfMotion.BottomToTop:
                                flipAnimation.From = 0;
                                flipAnimation.To = this.AxisOfFlip == AxisOfFlip.LeftOrTop ? TopAngle : BottomAngle;
                                break;
                            case DirectionOfMotion.LeftToRight:
                            case DirectionOfMotion.RightToLeft:
                                flipAnimation.From = 0;
                                flipAnimation.To = this.AxisOfFlip == AxisOfFlip.LeftOrTop ? LeftAngle : RightAngle;
                                break;
                        }
                        break;
                }
            }

            // NOTE: removing the projection animation by uncommenting the line below prevents the bug in WinRT where tapping a button in the transformed visual tree causes an A/V on null pointer in the Jupiter library
            //animation.Children.Remove(flipAnimation);
        }

        internal override void CleanupAnimation(DependencyObject target, Storyboard animation)
        {
            base.CleanupAnimation(target, animation);
            var fe = (FrameworkElement)target;
            fe.Projection = new PlaneProjection();
            fe.RenderTransform = new CompositeTransform();

            foreach (var sv in fe.GetDescendantsOfType<ScrollViewer>())
            {
                sv.ZoomMode = (ZoomMode)(((int)sv.ZoomMode + 1) % 2);
                sv.ZoomMode = (ZoomMode)(((int)sv.ZoomMode + 1) % 2);
            }
        }
    }
}
