using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace WinRTXamlToolkit.Controls
{
    public delegate Task AlternativeNavigationEventHandler(
        object sender,
        AlternativeNavigationEventArgs args);

    // Summary:
    //     Provides data for navigation methods and event handlers that cannot cancel
    //     the navigation request.
    public class AlternativeNavigationEventArgs
    {
        // Summary:
        //     Gets the root node of the target page's content.
        //
        // Returns:
        //     The root node of the target page's content.
        public object Content { get; private set; }
        //
        // Summary:
        //     Gets a value that indicates the direction of movement during navigation
        //
        // Returns:
        //     A value of the enumeration.
        public NavigationMode NavigationMode { get; private set; }
        //
        // Summary:
        //     Gets any Parameter object passed to the target page for the navigation.
        //
        // Returns:
        //     An object that potentially passes parameters to the navigation target. May
        //     be null.
        public object Parameter { get; private set; }
        //
        // Summary:
        //     Gets the data type of the target page.
        //
        // Returns:
        //     The data type of the target page, represented as namespace.type or simply
        //     type.
        public Type SourcePageType { get; private set; }

        public AlternativeNavigationEventArgs(object content, NavigationMode navigationMode, object parameter, Type sourcePageType)
        {
            this.Content = content;
            this.NavigationMode = navigationMode;
            this.Parameter = parameter;
            this.SourcePageType = sourcePageType;
        }
    }
}