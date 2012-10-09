using System;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Controls
{
    public class FadeAnimation : PageTransitionAnimation
    {
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
    }
}
