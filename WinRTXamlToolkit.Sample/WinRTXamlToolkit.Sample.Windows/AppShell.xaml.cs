using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using WinRTXamlToolkit.Controls;
#if WINDOWS_APP
using WinRTXamlToolkit.Debugging;
#endif
using WinRTXamlToolkit.Sample.Views;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample
{
    public sealed partial class AppShell : UserControl
    {
        public static AlternativeFrame Frame { get; private set; }

        public AppShell(FileActivatedEventArgs e)
        {
            this.InitializeComponent();
            this.RootFrame = new AlternativeFrame();
            this.AppLayoutGrid.Children.Add(this.RootFrame);
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

            HandleFileActivation(e);
        }

        public AlternativeFrame RootFrame { get; set; }

        public async Task HandleFileActivation(FileActivatedEventArgs e)
        {
            foreach (StorageFile file in e.Files.OfType<StorageFile>())
            {
#if WINDOWS_APP
                DC.Trace(await FileIO.ReadTextAsync(file));
#endif
            }
        }

        public AppShell(LaunchActivatedEventArgs e)
        {
            this.InitializeComponent();
            this.RootFrame = new AlternativeFrame();
            this.AppLayoutGrid.Children.Add(this.RootFrame);
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

            //Frame.Navigate(typeof(MainPage), e);
        }
    }
}
