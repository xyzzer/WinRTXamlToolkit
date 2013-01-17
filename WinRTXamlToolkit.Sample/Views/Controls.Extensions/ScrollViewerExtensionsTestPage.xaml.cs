using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using WinRTXamlToolkit.Controls.Extensions;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ScrollViewerExtensionsTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        private Random r = new Random();

        public ScrollViewerExtensionsTestPage()
        {
            this.InitializeComponent();
            var items = new List<dynamic>();

            // For anonymous type binding info check this article:
            // http://timheuer.com/blog/archive/2012/04/10/anonymous-type-binding-metro-style-app.aspx
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                {
                    items.Add(new
                              {
                                  Row = j,
                                  Column = i,
                                  Brush = new SolidColorBrush(Color.FromArgb(255, (byte)(i * 63), 255, (byte)(j * 63))),
                                  Text = (i * 5 + j).ToString()
                              });
                }

            foreach (var item in items)
            {
                var itemPresenter =
                    (FrameworkElement)((DataTemplate)this.Resources["TestItemTemplate"]).LoadContent();
                itemPresenter.DataContext = item;
                Grid.SetColumn(itemPresenter, item.Column);
                Grid.SetRow(itemPresenter, item.Row);
                scrolledGrid.Children.Add(itemPresenter);
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void OnAnimatedScrollTestButtonClick(object sender, RoutedEventArgs e)
        {
            scrollViewer.ScrollToHorizontalOffsetWithAnimation(r.NextDouble() * (scrollViewer.ExtentWidth - scrollViewer.ViewportWidth));
            scrollViewer.ScrollToVerticalOffsetWithAnimation(r.NextDouble() * (scrollViewer.ExtentHeight - scrollViewer.ViewportHeight));
            //scrollViewer.ScrollToHorizontalOffsetWithAnimation(500 - scrollViewer.HorizontalOffset);
            //scrollViewer.ScrollToVerticalOffsetWithAnimation(500 - scrollViewer.VerticalOffset);
        }

        private void OnAnimatedZoomTestButtonClick(object sender, RoutedEventArgs e)
        {
            scrollViewer.ZoomToFactorWithAnimation((float)
                (r.NextDouble() * (scrollViewer.MaxZoomFactor - scrollViewer.MinZoomFactor) + scrollViewer.MinZoomFactor));
        }
    }
}
