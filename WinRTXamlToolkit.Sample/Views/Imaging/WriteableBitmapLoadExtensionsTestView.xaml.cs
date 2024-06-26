﻿using System;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Net;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class WriteableBitmapLoadExtensionsTestView : UserControl
    {
        public WriteableBitmapLoadExtensionsTestView()
        {
            this.InitializeComponent();
            this.Loaded += (sender, args) => this.GetFirstAncestorOfType<AlternativePage>().ShouldWaitForImagesToLoad = false;
            InitializeTest();
        }

        private async void InitializeTest()
        {
            var imageUri = new Uri("https://www.ndigitec.com/wp-content/uploads/2019/05/Untitled-2Artboard_8.jpg");
            var fullSizeBitmap = new BitmapImage(imageUri);
            originalImage.Source = fullSizeBitmap;

            var file = await WebFile.SaveAsync(imageUri);

            resizedImage.Source = await new WriteableBitmap(1, 1).LoadAsync(file, 160, 120);
        }
    }
}
