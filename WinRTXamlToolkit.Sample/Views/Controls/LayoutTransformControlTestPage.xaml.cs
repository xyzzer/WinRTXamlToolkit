using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class LayoutTransformControlTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        private bool _isOn;
        public LayoutTransformControlTestPage()
        {
            this.InitializeComponent();
            _isOn = true;
            //RotationStoryboard.Begin();
            PlayAnimation();
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

        private void GoBack(object sender, RoutedEventArgs e)
        {
            _isOn = false;
            this.Frame.GoBack();
            //RotationStoryboard.Stop();
        }
    }
}
