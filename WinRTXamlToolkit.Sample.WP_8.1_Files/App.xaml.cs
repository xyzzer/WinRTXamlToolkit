// Uncomment below to enable XAML Spy. XAML Spy needs to be installed before running the app to use it.
//#define USE_XAML_SPY

using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        // This can be used if you have XAML Spy, which you can get from http://xamlspy.com/download
#if USE_XAML_SPY
        private FirstFloor.XamlSpy.XamlSpyService service;
        private const string XamlSpyServicePassword = "13155";
        //"[get your own - http://xamlspy.com/learn/tutorials/connect/winrt]"
#endif

#if WINDOWS_PHONE_APP
        //private TransitionCollection transitions;
#endif

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
#if USE_XAML_SPY
            this.service = new FirstFloor.XamlSpy.XamlSpyService(this) { Password = XamlSpyServicePassword };
#endif

            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        protected override void OnFileActivated(FileActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                //this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: Load state from previously suspended application
            }

            var appShell = Window.Current.Content as AppShell;

            if (appShell == null)
            {
                Window.Current.Content = appShell = new AppShell(e);
            }
            else
            {
                //appShell.HandleFileActivation(e);
            }
            
            Window.Current.Activate();

            base.OnFileActivated(e);
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                //this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            var appShell = Window.Current.Content as AppShell;

            if (appShell == null)
            {
                appShell = new AppShell(e);

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                Window.Current.Content = appShell;
            }

            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}