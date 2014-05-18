using System;
using Windows.UI.Xaml;
using WinRTXamlToolkit.Tools;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class BackgroundTimerTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        private BackgroundTimer _bt; // AdjustedDelay=false
        private BackgroundTimer _abt; // AdjustedDelay=true
        private DispatcherTimer _dt;
        private DateTime _btStartTime;
        private DateTime _abtStartTime;
        private DateTime _dtStartTime;
        private int _btTicks;
        private int _abtTicks;
        private int _dtTicks;

        public BackgroundTimerTestPage()
        {
            this.InitializeComponent();
            _bt = new BackgroundTimer { Interval = TimeSpan.FromSeconds(.1), AdjustDelays = false };
            _bt.Tick += _bt_Tick;
            _abt = new BackgroundTimer { Interval = TimeSpan.FromSeconds(.1) };
            _abt.Tick += _abt_Tick;
            _dt = new DispatcherTimer { Interval = TimeSpan.FromSeconds(.1)};
            _dt.Tick += _dt_Tick;
        }

        private void _bt_Tick(object sender, object e)
        {
            _btTicks++;
            var now = DateTime.Now;
            var interval = (now - _btStartTime).TotalMilliseconds / _btTicks;
            var line = string.Format(
                "{0} - Tick, Avg Tick Interval: {1:F3}ms",
                now.ToString("HH:mm:ss.ffffff"),
                interval);

#pragma warning disable 4014
            Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.High,
                () => BackgroundTimerEventLog.Items.Add(line));
#pragma warning restore 4014

            //if (_btTicks == 10)
            //{
            //    _btTicks = 0;
            //    _btStartTime = now;
            //}
        }

        private void _abt_Tick(object sender, object e)
        {
            _abtTicks++;
            var now = DateTime.Now;
            var interval = (now - _abtStartTime).TotalMilliseconds / _abtTicks;
            var line = string.Format(
                "{0} - Tick, Avg Tick Interval: {1:F3}ms",
                now.ToString("HH:mm:ss.ffffff"),
                interval);

#pragma warning disable 4014
            Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.High,
                () => AdjustedBackgroundTimerEventLog.Items.Add(line));
#pragma warning restore 4014

            //if (_abtTicks == 10)
            //{
            //    _abtTicks = 0;
            //    _abtStartTime = now;
            //}
        }

        private void _dt_Tick(object sender, object e)
        {
            _dtTicks++;
            var now = DateTime.Now;
            var interval = (now - _dtStartTime).TotalMilliseconds / _btTicks;
            var line = string.Format(
                "{0} - Tick, Avg Tick Interval: {1:F3}ms",
                now.ToString("HH:mm:ss.ffffff"),
                interval);

#pragma warning disable 4014
            Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.High,
                () => DispatcherTimerEventLog.Items.Add(line));
#pragma warning restore 4014

            //if (_dtTicks == 10)
            //{
            //    _dtTicks = 0;
            //    _dtStartTime = now;
            //}
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            _bt.Stop();
            _abt.Stop();
            _dt.Stop();
            this.Frame.GoBack();
        }

        private void OnStartClick(object sender, RoutedEventArgs e)
        {
            _btTicks = _abtTicks = _dtTicks = 0;
            _btStartTime = _abtStartTime = _dtStartTime = DateTime.Now;
            _bt.Start();
            _abt.Start();
            _dt.Start();
        }

        private void OnStopClick(object sender, RoutedEventArgs e)
        {
            _bt.Stop();
            _abt.Stop();
            _dt.Stop();
        }

        private void OnClearClick(object sender, RoutedEventArgs e)
        {
            BackgroundTimerEventLog.Items.Clear();
            AdjustedBackgroundTimerEventLog.Items.Clear();
            DispatcherTimerEventLog.Items.Clear();
        }
    }
}
