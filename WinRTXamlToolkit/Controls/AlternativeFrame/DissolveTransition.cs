namespace WinRTXamlToolkit.Controls
{
    public class DissolveTransition : PageTransition
    {
        protected override PageTransitionMode Mode
        {
            get
            {
                return PageTransitionMode.Parallel;
            }
        }

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
