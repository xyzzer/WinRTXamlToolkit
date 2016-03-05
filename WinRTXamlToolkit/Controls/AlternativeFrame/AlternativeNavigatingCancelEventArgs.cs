using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// EventHandler delegate for AlternativeFrame cancellable navigation events.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="AlternativeNavigationEventArgs" /> instance containing the event data.</param>
    /// <returns></returns>
    public delegate Task AlternativeNavigatingCancelEventHandler(
        object sender,
        AlternativeNavigatingCancelEventArgs args);

    /// <summary>
    /// Provides event data for the OnNavigatingFrom callback that can be used
    /// to cancel a navigation request from origination.
    /// </summary>
    public class AlternativeNavigatingCancelEventArgs
    {
        /// <summary>
        /// Specifies whether a pending navigation should be canceled.
        /// </summary>
        /// <value>
        ///   <c>true</c> to cancel the pending cancelable navigation; <c>false</c> to continue with navigation.
        /// </value>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets the value of the mode parameter from the originating Navigate call.
        /// </summary>
        /// <value>
        /// The value of the mode parameter from the originating Navigate call.
        /// </value>
        public NavigationMode NavigationMode { get; private set; }

        /// <summary>
        /// Gets the value of the SourcePageType parameter from the originating Navigate call.
        /// </summary>
        /// <value>
        /// The value of the SourcePageType parameter from the originating Navigate call.
        /// </value>
        public Type SourcePageType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlternativeNavigatingCancelEventArgs" /> class.
        /// </summary>
        /// <param name="navigationMode">The navigation mode.</param>
        /// <param name="sourcePageType">Type of the source page.</param>
        public AlternativeNavigatingCancelEventArgs(NavigationMode navigationMode, Type sourcePageType)
        {
            this.NavigationMode = navigationMode;
            this.SourcePageType = sourcePageType;
        }
    }
}