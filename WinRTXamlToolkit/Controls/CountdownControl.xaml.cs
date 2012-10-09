using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Animation;
using WinRTXamlToolkit.AwaitableUI;

namespace WinRTXamlToolkit.Controls
{
    public sealed partial class CountdownControl
    {
        private bool countingDown;
        public event RoutedEventHandler CountdownComplete;

        #region Seconds
        public static readonly DependencyProperty SecondsProperty =
            DependencyProperty.Register(
                "Seconds",
                typeof(int),
                typeof(CountdownControl),
                new PropertyMetadata(
                    0,
                    new PropertyChangedCallback(OnSecondsChanged)));

        public int Seconds
        {
            get { return (int)GetValue(SecondsProperty); }
            set { SetValue(SecondsProperty, value); }
        }

        private static void OnSecondsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (CountdownControl)sender;
            var oldSeconds = (int)e.OldValue;
            var newSeconds = (int)e.NewValue;
            target.OnSecondsChanged(oldSeconds, newSeconds);
        }

        private void OnSecondsChanged(int oldSeconds, int newSeconds)
        {
            if (!countingDown && newSeconds > 0)
            {
#pragma warning disable 4014
                StartCountdown(newSeconds);
#pragma warning restore 4014
            }
        }
        #endregion

        public CountdownControl()
        {
            InitializeComponent();
        }

        public async Task StartCountdown(int seconds)
        {
            countingDown = true;

            this.Seconds = seconds;

            bool grow = true;

            while (this.Seconds > 0)
            {
                var sb = new Storyboard();

                if (grow)
                {
                    var da = new DoubleAnimation
                    {
                        From = 0d,
                        To = 359.999d,
                        Duration = new Duration(TimeSpan.FromSeconds(1d)),
                        EnableDependentAnimation = true
                    };

                    sb.Children.Add(da);

                    // Workaround for a problem animating custom dependency properties
                    //PART_RingSlice.SetBinding(
                    //   RingSlice.EndAngleProperty,
                    //   new Binding { Source = r1, Path = new PropertyPath("Opacity"), Mode = BindingMode.OneWay });
                    //Storyboard.SetTargetProperty(da, "Opacity");
                    //Storyboard.SetTarget(sb, r1);
                    Storyboard.SetTargetProperty(da, "EndAngle");
                    Storyboard.SetTarget(sb, PART_RingSlice);
                }
                else
                {
                    var da = new DoubleAnimation
                    {
                        From = 0d,
                        To = 359.999d,
                        Duration = new Duration(TimeSpan.FromSeconds(1d)),
                        EnableDependentAnimation = true
                    };

                    sb.Children.Add(da);

                    //PART_RingSlice.SetBinding(
                    //   RingSlice.StartAngleProperty,
                    //   new Binding { Source = r2, Path = new PropertyPath("Opacity"), Mode = BindingMode.OneWay });
                    //Storyboard.SetTargetProperty(da, "Opacity");
                    //Storyboard.SetTarget(sb, r2);
                    Storyboard.SetTargetProperty(da, "StartAngle");
                    Storyboard.SetTarget(sb, PART_RingSlice);
                }

                PART_Label.Text = this.Seconds.ToString();
                await sb.BeginAsync();

                if (grow)
                {
                    PART_RingSlice.StartAngle = 0d;
                    PART_RingSlice.EndAngle = 359.999d;
                }
                else
                {
                    PART_RingSlice.StartAngle = 0d;
                    PART_RingSlice.EndAngle = 0d;
                }

                grow = !grow;
                this.Seconds--;
            }

            PART_Label.Text = this.Seconds.ToString();

            if (this.CountdownComplete != null)
            {
                CountdownComplete(this, new RoutedEventArgs());
            }

            countingDown = false;
        }
    }
}
