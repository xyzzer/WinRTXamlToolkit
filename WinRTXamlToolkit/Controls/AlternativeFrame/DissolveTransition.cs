namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// Transition in which the new page fades in on top of the old page.
    /// </summary>
    public class DissolveTransition : PageTransition
    {
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DissolveTransition" /> class.
        /// </summary>
        public DissolveTransition()
        {
            this.ForwardOutAnimation = null;
            this.ForwardInAnimation =
                new FadeAnimation
                {
                    Mode = AnimationMode.In
                };
            this.BackwardOutAnimation = null;
            this.BackwardInAnimation =
                new FadeAnimation
                {
                    Mode = AnimationMode.In
                };
        }
    }
}
