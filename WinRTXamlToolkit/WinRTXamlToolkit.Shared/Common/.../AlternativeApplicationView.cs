#if !WIN81
using Windows.UI.ViewManagement;
#endif
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Common
{
    /// <summary>
    /// A helper class to simplify ApplicationView checks between different versions of Windows.
    /// </summary>
    public static class AlternativeApplicationView
    {
        /// <summary>
        /// Gets or sets the value that determines the width of the app window at which to recognize the app as snapped.
        /// </summary>
        public static double SnappedViewWidth { get; set; }

        /// <summary>
        /// Gets a value indicating whether the app should act as if it were snapped.
        /// </summary>
        public static bool IsSnapped
        {
            get
            {
#if WIN81
                return Window.Current.Bounds.Width <= SnappedViewWidth;
#else
                return ApplicationView.Value == ApplicationViewState.Snapped;
#endif
            }
        }

        /// <summary>
        /// Initializes static members of the <see cref="AlternativeApplicationView"/> class.
        /// </summary>
        static AlternativeApplicationView()
        {
            SnappedViewWidth = 320;
        }
    }
}
