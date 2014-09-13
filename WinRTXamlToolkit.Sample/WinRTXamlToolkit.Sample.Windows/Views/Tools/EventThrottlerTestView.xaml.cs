using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using WinRTXamlToolkit.AwaitableUI;
using WinRTXamlToolkit.Tools;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class EventThrottlerTestView : UserControl
    {
        private readonly EventThrottlerMultiThreaded throttler = new EventThrottlerMultiThreaded();
        private readonly Random rand = new Random();

        public EventThrottlerTestView()
        {
            this.InitializeComponent();
            
        }

        private void OnAnimateButtonClick(object sender, RoutedEventArgs e)
        {
            throttler.Run(Animate);
        }

        private async Task Animate()
        {
            var sb = new Storyboard();
            var daX = new DoubleAnimation();
            var daY = new DoubleAnimation();
            daX.Duration = TimeSpan.FromSeconds(0.8);
            daY.Duration = TimeSpan.FromSeconds(0.8);
            Storyboard.SetTarget(daX, AnimatedTransform);
            Storyboard.SetTarget(daY, AnimatedTransform);
            Storyboard.SetTargetProperty(daX, "X");
            Storyboard.SetTargetProperty(daY, "Y");
            daX.To = rand.Next(0, (int)AnimationPanel.ActualWidth - 100);
            daY.To = rand.Next(0, (int)AnimationPanel.ActualHeight - 100);
            sb.Children.Add(daX);
            sb.Children.Add(daY);
            await sb.BeginAsync();
        }
    }
}
