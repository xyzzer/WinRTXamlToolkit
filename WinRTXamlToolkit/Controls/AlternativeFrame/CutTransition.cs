using System;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Controls
{
    public class CutTransition : PageTransition
    {
        protected override PageTransitionMode Mode
        {
            get
            {
                return PageTransitionMode.Sequential;
            }
        }
    }
}
