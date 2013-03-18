//#define USE_XAML_SPY
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        // This can be used if you have XAML Spy, which you can get from http://xamlspy.com/download
#if USE_XAML_SPY
        private FirstFloor.XamlSpy.XamlSpyService service;
        private const string XamlSpyServicePassword = "13155";
        //"[get your own - http://xamlspy.com/learn/tutorials/connect/winrt]"
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
            //DebugSettings.IsOverdrawHeatMapEnabled = true;
            //DebugSettings.EnableFrameRateCounter = true;

            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            base.OnWindowCreated(args);

            // This starts XAML Spy service
#if USE_XAML_SPY
            this.service.StartService();
#endif
        }

        //protected override void OnFileActivated(FileActivatedEventArgs args)
        //{
        //    base.OnFileActivated(args);
        //}

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: Load state from previously suspended application
            }

            Window.Current.Content = new AppShell();
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        void OnSuspending(object sender, SuspendingEventArgs e)
        {
            //TODO: Save application state and stop any background activity
        }
    }
}
