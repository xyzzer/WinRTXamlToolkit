using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.AwaitableUI;
using WinRTXamlToolkit.Tools;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class FxContentControlTestView : UserControl
    {
        public FxContentControlTestView()
        {
            this.InitializeComponent();
#pragma warning disable 4014
            this.RunAsync();
#pragma warning restore 4014
        }

        private async Task RunAsync()
        {
            var isLoaded = false;
            await this.WaitForLoadedAsync();
            isLoaded = true;
            this.Unloaded += (s, e) => isLoaded = false;
            var r = new Random();

            while (isLoaded)
            {
                var start = this.Gauge1.Value;
                double target = r.Next(0, 100);
                var startTime = DateTime.Now;
                Double progress;

                do
                {
                    progress = (DateTime.Now - startTime).TotalSeconds.Min(1d);
                    var value = MathEx.Lerp(start, target, progress);
                    this.Gauge1.Value = value;
                    this.Gauge2.Value = value;
                    var sw = new Stopwatch();
                    sw.Start();
                    await this.Gauge2FxContentControl.UpdateFxAsync();
                    sw.Stop();
                    this.InfoTextBlock.Text = string.Format("{0}ms per frame", sw.ElapsedMilliseconds);
                } while (progress < 1);

                await Task.Delay(1000);
            }
        }
    }
}
