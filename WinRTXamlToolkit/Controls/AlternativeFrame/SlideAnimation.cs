using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// The transition where the new page slides on top of the old page.
    /// </summary>
    public class SlideAnimation : PageTransitionAnimation
    {
        #region Direction
        /// <summary>
        /// Direction Dependency Property
        /// </summary>
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register(
                "Direction",
                typeof(DirectionOfMotion),
                typeof(SlideAnimation),
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

        /// <summary>
        /// Gets the generated animation.
        /// </summary>
        /// <value>
        /// The animation.
        /// </value>
        protected override Storyboard Animation
        {
            get
            {
                // NOTE: There seem to be problems with WinRT when reusing same Storyboard for multiple elements, so we need to always get a new storyboard.
                var sb = new Storyboard();
                var da = new DoubleAnimation();
                da.EasingFunction = this.EasingFunction;
                da.Duration = this.Duration;
                sb.Children.Add(da);

                return sb;
            }
        }

        protected override void ApplyTargetProperties(DependencyObject target, Storyboard animation)
        {
            var fe = (FrameworkElement)target;
            TranslateTransform tt = fe.RenderTransform as TranslateTransform;

            if (tt == null)
            {
                fe.RenderTransform = tt = new TranslateTransform();
            }

            var da = (DoubleAnimation)animation.Children[0];

            Storyboard.SetTarget(da, tt);

            if (Direction == DirectionOfMotion.RightToLeft ||
                Direction == DirectionOfMotion.LeftToRight)
            {
                Storyboard.SetTargetProperty(da, "X");

                if (Mode == AnimationMode.In)
                {
                    da.From =
                        Direction == DirectionOfMotion.LeftToRight
                            ? -fe.ActualWidth
                            : fe.ActualWidth;
                    da.To = 0;
                }
                else
                {
                    da.From = 0;
                    da.To =
                        Direction == DirectionOfMotion.LeftToRight
                            ? fe.ActualWidth
                            : -fe.ActualWidth;
                }
            }
            else
            {
                Storyboard.SetTargetProperty(da, "Y");

                if (Mode == AnimationMode.In)
                {
                    da.From =
                        Direction == DirectionOfMotion.TopToBottom
                            ? -fe.ActualHeight
                            : fe.ActualHeight;
                    da.To = 0;
                }
                else
                {
                    da.From = 0;
                    da.To =
                        Direction == DirectionOfMotion.TopToBottom
                            ? fe.ActualHeight
                            : -fe.ActualHeight;
                }
            }
        }
    }
}