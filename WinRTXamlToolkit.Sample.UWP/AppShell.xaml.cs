using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Debugging;
using WinRTXamlToolkit.Sample.Views;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample
{
    public sealed partial class AppShell : UserControl
    {
        public static AlternativeFrame Frame { get; private set; }

        public AppShell(LaunchActivatedEventArgs e)
        {
            this.InitializeComponent();
            Frame = this.RootFrame;
            Frame.Navigate(typeof(MainPage), e);
            //DC.ShowVisualTree();
            //DC.Hide();
        }
        
        public AppShell(FileActivatedEventArgs e)
        {
            this.InitializeComponent();
            Frame = this.RootFrame;
            Frame.Navigate(typeof(MainPage), e);
#pragma warning disable 4014
            this.HandleFileActivationAsync(e);
#pragma warning restore 4014
        }

        internal async Task HandleFileActivationAsync(FileActivatedEventArgs e)
        {
            foreach (StorageFile file in e.Files.OfType<StorageFile>())
            {
                DC.Trace(await FileIO.ReadTextAsync(file));
            }
        }
    }
}
