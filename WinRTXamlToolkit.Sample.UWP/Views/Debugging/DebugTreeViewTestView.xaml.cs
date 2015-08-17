using System;
using System.Collections.Generic;
using System.Diagnostics;
using WinRTXamlToolkit.AwaitableUI;
using WinRTXamlToolkit.Debugging;
using WinRTXamlToolkit.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class DebugTreeViewTestView : UserControl
    {
        public DebugTreeViewTestView()
        {
            this.InitializeComponent();
            this.CollapseReleaseBuildWarning();
        }

        private List<Grid> _grids = new List<Grid>();

        [Conditional("DEBUG")]
        private void CollapseReleaseBuildWarning()
        {
            releaseBuildWarning.Visibility = Visibility.Collapsed;
            testPanel.Visibility = Visibility.Visible;
            InitializeTest();
        }

        [Conditional("DEBUG")]
        private async void InitializeTest()
        {
            _grids.Add(testPanel);
            await this.WaitForLoadedAsync();
            var random = new Random();
            var colors = ColorExtensions.GetNamedColors();

            for (int i = 0; i < 20; i++)
            {
                var panel = _grids[random.Next(0, _grids.Count)];

                if (panel.ActualWidth - 10 <= 10 ||
                    panel.ActualHeight - 10 <= 10)
                {
                    i--;
                    continue;
                }

                var border = new Border();
                border.HorizontalAlignment = HorizontalAlignment.Left;
                border.VerticalAlignment = VerticalAlignment.Top;
                border.Width = random.Next((int)(panel.ActualWidth / 2), (int)(panel.ActualWidth - 10));
                border.Height = random.Next((int)(panel.ActualHeight / 2), (int)(panel.ActualHeight - 10));
                border.Margin =
                    new Thickness(
                        random.Next(0, (int)(panel.ActualWidth - border.Width) + 1),
                        random.Next(0, (int)(panel.ActualHeight - border.Height) + 1),
                        0,
                        0);
                border.Background = new SolidColorBrush(colors[random.Next(colors.Count)]);
                var childGrid = new Grid {VerticalAlignment = VerticalAlignment.Stretch, HorizontalAlignment = HorizontalAlignment.Stretch};
                _grids.Add(childGrid);
                border.Child = childGrid;
                panel.Children.Add(border);
                var tb =
                    new TextBlock
                    {
                        Text = string.Format("Panel {0}", i + 1),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };
                childGrid.Children.Add(tb);
                await childGrid.WaitForNonZeroSizeAsync();
            }

            DC.ShowVisualTree(_grids[random.Next(_grids.Count)]);
            DC.Expand();
        }
    }
}
