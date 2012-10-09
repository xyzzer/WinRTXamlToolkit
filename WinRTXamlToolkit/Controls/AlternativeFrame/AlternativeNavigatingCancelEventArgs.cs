using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace WinRTXamlToolkit.Controls
{
    public delegate Task AlternativeNavigatingCancelEventHandler(
        object sender,
        AlternativeNavigatingCancelEventArgs args);

    // Summary:
    //     Provides event data for the OnNavigatingFrom callback that can be used to
    //     cancel a navigation request from origination.
    public class AlternativeNavigatingCancelEventArgs
    {
        // Summary:
        //     Specifies whether a pending navigation should be canceled.
        //
        // Returns:
        //     True to cancel the pending cancelable navigation; false to continue with
        //     navigation.
        public bool Cancel { get; set; }
        //
        // Summary:
        //     Gets the value of the mode parameter from the originating Navigate call.
        //
        // Returns:
        //     The value of the mode parameter from the originating Navigate call.
        public NavigationMode NavigationMode { get; private set; }
        //
        // Summary:
        //     Gets the value of the SourcePageType parameter from the originating Navigate
        //     call.
        //
        // Returns:
        //     The value of the SourcePageType parameter from the originating Navigate call.
        public Type SourcePageType { get; private set; }

        public AlternativeNavigatingCancelEventArgs(NavigationMode navigationMode, Type sourcePageType)
        {
            this.NavigationMode = navigationMode;
            this.SourcePageType = sourcePageType;
        }
    }
}