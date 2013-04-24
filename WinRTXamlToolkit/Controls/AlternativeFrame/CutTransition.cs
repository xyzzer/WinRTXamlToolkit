namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// A simple cut page transition where the previous page is immediately replaced with the new page.
    /// </summary>
    public class CutTransition : PageTransition
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
                return PageTransitionMode.Sequential;
            }
        }
    }
}
