using System;
using System.Threading.Tasks;
using WinRTXamlToolkit.Controls.Behaviors;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class BehaviorsTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public BehaviorsTestPage()
        {
            this.InitializeComponent();
            TestGrid.Children.Add((UIElement)TestControlTemplate.LoadContent());
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void OnTestForLeaksButtonClick(object sender, RoutedEventArgs e)
        {
            TestGrid.Children.Clear();
            TestGrid.Children.Add((UIElement)TestControlTemplate.LoadContent());
        }
    }

    public class HeavyFlickBehavior : FlickBehavior
    {
        private bool _isLoaded;

        protected override void OnAttached()
        {
            _isLoaded = true;
        }

        protected override void OnDetaching()
        {
            _isLoaded = false;
        }

        protected override void OnLoaded()
        {
            KeepHeavyWeightAlive();
            base.OnLoaded();
        }

        protected override void OnUnloaded()
        {
            base.OnUnloaded();
            _isLoaded = false;
        }

        private async void KeepHeavyWeightAlive()
        {
            var heavyWeight = new byte[1024 * 1024 * 100];

            while (_isLoaded)
            {
                for (int i = 0; i < heavyWeight.Length; i+= 1024 )
                    heavyWeight[i] = (byte)i;
                await Task.Delay(1000);
            }

            GC.Collect();
        }
    }
}
