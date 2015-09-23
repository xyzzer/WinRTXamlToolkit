using Windows.ApplicationModel.Activation;
using Windows.Phone.UI.Input;
using WinRTXamlToolkit.Debugging;
using WinRTXamlToolkit.Sample.Views;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample
{
    public sealed partial class AppShell : UserControl
    {
        public static Frame Frame { get; private set; }
        //public static AlternativeFrame Frame { get; private set; }

        public AppShell(LaunchActivatedEventArgs e)
        {
            this.InitializeComponent();
            Frame = this.RootFrame;
            //Frame.CacheSize = 1;

            //            if (Frame.Content == null)
            //            {
            //#if WINDOWS_PHONE_APP
            //    // Removes the turnstile navigation for startup.
            //                if (rootFrame.ContentTransitions != null)
            //                {
            //                    this.transitions = new TransitionCollection();
            //                    foreach (var c in rootFrame.ContentTransitions)
            //                    {
            //                        this.transitions.Add(c);
            //                    }
            //                }

            //                rootFrame.ContentTransitions = null;
            //                rootFrame.Navigated += this.RootFrame_FirstNavigated;
            //#endif
            //            }

            Frame.Navigate(typeof(MainPage), e);
            InitializeNavigationHelper();

            // Uncomment to show visual tree debugger on phone
            this.Loaded += (s, e2) =>
            {
                DC.ShowVisualTree();
                DC.Collapse();
            };
        }
        
        public AppShell(FileActivatedEventArgs e)
        {
            this.InitializeComponent();
            Frame = this.RootFrame;
            //Frame.CacheSize = 1;

//            if (Frame.Content == null)
//            {
//#if WINDOWS_PHONE_APP
//    // Removes the turnstile navigation for startup.
//                if (rootFrame.ContentTransitions != null)
//                {
//                    this.transitions = new TransitionCollection();
//                    foreach (var c in rootFrame.ContentTransitions)
//                    {
//                        this.transitions.Add(c);
//                    }
//                }

//                rootFrame.ContentTransitions = null;
//                rootFrame.Navigated += this.RootFrame_FirstNavigated;
//#endif
//            }

            Frame.Navigate(typeof(MainPage), e);
            InitializeNavigationHelper();
        }

        private void InitializeNavigationHelper()
        {
            HardwareButtons.BackPressed += this.OnBackPressed;
        }

        private void OnBackPressed(object sender, BackPressedEventArgs e)
        {
            if (this.RootFrame != null && this.RootFrame.CanGoBack)
            {
                e.Handled = true;
                this.RootFrame.GoBack();
            }
        }
    }
}
