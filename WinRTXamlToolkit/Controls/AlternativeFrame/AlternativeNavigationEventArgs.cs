using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace WinRTXamlToolkit.Controls
{
    /// <summary>
    /// EventHandler delegate for AlternativeFrame navigation events.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The <see cref="AlternativeNavigationEventArgs" /> instance containing the event data.</param>
    /// <returns></returns>
    public delegate Task AlternativeNavigationEventHandler(
        object sender,
        AlternativeNavigationEventArgs args);

    /// <summary>
    /// Provides data for navigation methods and event handlers that cannot cancel the navigation request.
    /// </summary>
    public class AlternativeNavigationEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the root node of the target page's content.
        /// </summary>
        /// <value>
        /// The root node of the target page's content.
        /// </value>
        public object Content { get; private set; }

        /// <summary>
        /// Gets a value that indicates the direction of page navigation.
        /// </summary>
        /// <value>
        /// A value of the enumeration.
        /// </value>
        public NavigationMode NavigationMode { get; private set; }

        /// <summary>
        /// Gets any Parameter object passed to the target page for the navigation.
        /// </summary>
        /// <value>
        /// An object that potentially passes parameters to the navigation target. May be null.
        /// </value>
        public object Parameter { get; private set; }

        /// <summary>
        /// Gets the data type of the target page.
        /// </summary>
        /// <value>
        /// The data type of the target page, represented as namespace.type or simply type.
        /// </value>
        public Type SourcePageType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlternativeNavigationEventArgs" /> class.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="navigationMode">The navigation mode.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="sourcePageType">Type of the source page.</param>
        public AlternativeNavigationEventArgs(object content, NavigationMode navigationMode, object parameter, Type sourcePageType)
        {
            this.Content = content;
            this.NavigationMode = navigationMode;
            this.Parameter = parameter;
            this.SourcePageType = sourcePageType;
        }
    }
}