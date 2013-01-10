using System;
using WinRTXamlToolkit.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Net;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class WriteableBitmapLoadExtensionsTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public WriteableBitmapLoadExtensionsTestPage()
        {
            this.InitializeComponent();
            this.ShouldWaitForImagesToLoad = false;
            InitializeTest();
        }

        private async void InitializeTest()
        {
            var imageUri = new Uri("https://dl.dropbox.com/u/1192076/IMG_8839.JPG");
            var fullSizeBitmap = new BitmapImage(imageUri);
            originalImage.Source = fullSizeBitmap;

            var file = await WebFile.SaveAsync(imageUri);

            resizedImage.Source = await new WriteableBitmap(1, 1).LoadAsync(file, 160, 120);
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
