using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Sample.ViewModels;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class TestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        private bool keyDownWasHandled;

        public TestPage()
        {
            this.InitializeComponent();
        }

        private async void GoBack(object sender, RoutedEventArgs e)
        {
            // TODO: Hook it up. Seems unused nowadays.
            await this.Frame.GoBackAsync();
        }

        protected override Task OnNavigatedToAsync(AlternativeNavigationEventArgs e)
        {
            var title = e.Parameter as string;
            WindowTitleBar.SetText("WinRT XAML Toolkit - " + title, true);

            //this.TitleTextBlock.Text = title;
            var sampleButton = MainPageViewModel.Instance.UngroupedSamples.First(s => s.Caption == title);
            var content = (FrameworkElement)Activator.CreateInstance(sampleButton.ViewType);
            this.ContentGrid.Children.Add(content);
            AddHandler(Control.KeyDownEvent, new KeyEventHandler(TestPage_OnKeyDown), true);
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            return base.OnNavigatedToAsync(e);
        }

        protected override Task OnNavigatedFromAsync(AlternativeNavigationEventArgs e)
        {
            RemoveHandler(Control.KeyDownEvent, new KeyEventHandler(TestPage_OnKeyDown));
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
            return base.OnNavigatedFromAsync(e);
        }

        private async void TestPage_OnKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (!e.Handled &&
                (e.Key == VirtualKey.Escape ||
                (e.Key == VirtualKey.Left && ((Window.Current.CoreWindow.GetKeyState(VirtualKey.Menu) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down))))
            {
                await this.Frame.GoBackAsync();
                e.Handled = true;
            }

            this.keyDownWasHandled = e.Handled;
        }

        private async void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs e)
        {
            this.keyDownWasHandled = false;
            await Task.Delay(1);

            if (!this.keyDownWasHandled &&
                //!e.Handled && // TODO: Why is this handled? Happens when focus is in Visual Tree Debugger's search box
                (e.VirtualKey == VirtualKey.Escape ||
                (e.VirtualKey == VirtualKey.Left && ((Window.Current.CoreWindow.GetKeyState(VirtualKey.Menu) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down))))
            {
                await this.Frame.GoBackAsync();
                e.Handled = true;
            }
        }
    }
}
