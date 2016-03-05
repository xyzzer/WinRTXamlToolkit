using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// Page transition animation used to fade the target page in or out.
    /// </summary>
    public class FadeAnimation : PageTransitionAnimation
    {
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
                var sb = new Storyboard();
                var da = new DoubleAnimation();
                Storyboard.SetTargetProperty(da, "Opacity");
                da.EasingFunction = this.EasingFunction;
                da.Duration = this.Duration;
                da.From = Mode == AnimationMode.In ? 0 : 1;
                da.To = Mode == AnimationMode.In ? 1 : 0;
                sb.Children.Add(da);

                return sb;
            }
        }
    }
}
