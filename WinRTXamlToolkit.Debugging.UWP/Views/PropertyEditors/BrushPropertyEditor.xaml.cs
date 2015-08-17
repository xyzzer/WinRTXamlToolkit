using System;
using System.Threading.Tasks;
using WinRTXamlToolkit.Debugging.ViewModels;
using WinRTXamlToolkit.Imaging;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Debugging.Views.PropertyEditors
{
    public sealed partial class BrushPropertyEditor : UserControl
    {
        private bool _ignoreTextChange;

        #region Model
        /// <summary>
        /// Model Dependency Property
        /// </summary>
        public static readonly DependencyProperty ModelProperty =
            DependencyProperty.Register(
                "Model",
                typeof(object),
                typeof(BrushPropertyEditor),
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
            var target = (BrushPropertyEditor)d;
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

            var scb = newModel.Value as SolidColorBrush;

            if (scb != null)
            {
                _ignoreTextChange = true;
                this.ValueTextBox.Text = scb.Color.ToString();
                return;
            }

            var ib = newModel.Value as ImageBrush;

            if (ib != null)
            {
                var bi = ib.ImageSource as BitmapImage;

                if (bi != null &&
                    bi.UriSource != null &&
                    !string.IsNullOrEmpty(bi.UriSource.ToString()))
                {
                    _ignoreTextChange = true;
                    this.ValueTextBox.Text = bi.UriSource.ToString();
                }
            }
        }
        #endregion

        public BrushPropertyEditor()
        {
            this.InitializeComponent();
        }

        private const int TextChangedUpdateDelay = 1000;
        private int _textChangedHandler;

        private async void ValueTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var textChangedHandler = ++_textChangedHandler;

            if (_ignoreTextChange)
            {
                _ignoreTextChange = false;
                return;
            }

            Color color;

            if (ColorExtensions.TryFromString(this.ValueTextBox.Text, out color))
            {
                var scb = this.Model.Value as SolidColorBrush;

                if (scb != null &&
                    scb.Color == color)
                {
                    return;
                }

                this.Model.Value = new SolidColorBrush(color);

                return;
            }

            await Task.Delay(TextChangedUpdateDelay);

            if (textChangedHandler != _textChangedHandler)
            {
                return;
            }

            Uri uri;

            if (Uri.TryCreate(this.ValueTextBox.Text, UriKind.RelativeOrAbsolute, out uri))
            {
                var ib = this.Model.Value as ImageBrush;

                if (ib != null)
                {
                    var bi = ib.ImageSource as BitmapImage;

                    if (bi != null &&
                        bi.UriSource != null &&
                        !string.IsNullOrEmpty(bi.UriSource.ToString()) &&
                        bi.UriSource.ToString() == uri.ToString())
                    {
                        return;
                    }
                }

                try
                {
                    var bi = new BitmapImage(uri);
                    this.Model.Value = new ImageBrush { ImageSource = bi };
                }
                catch
                {
                }
            }
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
                        this.Model.Value = new ImageBrush {ImageSource = bi};
                        _ignoreTextChange = true;
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
