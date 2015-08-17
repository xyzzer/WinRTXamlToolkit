using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class LayoutTransformControlTestView : UserControl
    {
        private bool _isOn;
        public LayoutTransformControlTestView()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _isOn = true;
            //RotationStoryboard.Begin();
            PlayAnimation();
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _isOn = false;
            //RotationStoryboard.Stop();
        }

        private async void PlayAnimation()
        {
            double angle = 0;

            while (_isOn)
            {
                angle += 3;
                await Task.Delay(30);
                rotateTransform.Angle = angle;
            }
        }
    }
}
