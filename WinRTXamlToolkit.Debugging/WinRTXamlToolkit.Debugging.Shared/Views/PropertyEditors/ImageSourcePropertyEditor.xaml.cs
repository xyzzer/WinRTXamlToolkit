using System;
using WinRTXamlToolkit.Debugging.ViewModels;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Debugging.Views.PropertyEditors
{
    public sealed partial class ImageSourcePropertyEditor : UserControl
    {
        #region Model
        /// <summary>
        /// Model Dependency Property
        /// </summary>
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register(
                "Model",
                typeof(object),
                typeof(ImageSourcePropertyEditor),
                new PropertyMetadata(null, OnModelChanged));

        /// <summary>
        /// Gets or sets the Model property. This dependency property 
        /// indicates the property view model to update with the values.
        /// </summary>
        public BasePropertyViewModel Model
        {
            get { return (BasePropertyViewModel)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Model property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnModelChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (ImageSourcePropertyEditor)d;
            BasePropertyViewModel oldModel = (BasePropertyViewModel)e.OldValue;
            BasePropertyViewModel newModel = (BasePropertyViewModel)target.Model;
            target.OnModelChanged(oldModel, newModel);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the Model property.
        /// </summary>
        /// <param name="oldModel">The old Model value</param>
        /// <param name="newModel">The new Model value</param>
        private void OnModelChanged(
            BasePropertyViewModel oldModel, BasePropertyViewModel newModel)
        {
            if (newModel == null)
            {
                return;
            }

            var bi = newModel.Value as BitmapImage;

            if (bi != null &&
                bi.UriSource != null &&
                !string.IsNullOrEmpty(bi.UriSource.ToString()))
            {
                this.ValueTextBox.Text = bi.UriSource.ToString();
            }
        }
        #endregion

        public ImageSourcePropertyEditor()
        {
            this.InitializeComponent();
        }

        private async void OpenButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var fop = new FileOpenPicker();
                fop.FileTypeFilter.Add(".jpg");
                fop.FileTypeFilter.Add(".png");
                fop.FileTypeFilter.Add(".gif");
#pragma warning disable 618
                var file = await fop.PickSingleFileAsync();
#pragma warning restore 618

                if (file != null)
                {
                    var bi = new BitmapImage();
                    bi.UriSource = new Uri(file.Path);

                    using (var stream = await file.OpenReadAsync())
                    {
                        await bi.SetSourceAsync(stream);
                        this.Model.Value = bi;
                        this.ValueTextBox.Text = file.Path;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
