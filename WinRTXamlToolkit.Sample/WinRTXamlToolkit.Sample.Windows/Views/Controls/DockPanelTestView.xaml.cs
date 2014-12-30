using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using WinRTXamlToolkit.AwaitableUI;
using WinRTXamlToolkit.Controls;
using WinRTXamlToolkit.Imaging;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class DockPanelTestView : UserControl
    {
        private readonly Random random = new Random();

        public DockPanelTestView()
        {
            this.InitializeComponent();
#pragma warning disable 4014
            this.RandomizeChildrenAsync();
#pragma warning restore 4014
        }

        private async void RandomizeChildrenButton_OnClick(object sender, RoutedEventArgs e)
        {
            await this.RandomizeChildrenAsync();
        }

        private async Task RandomizeChildrenAsync()
        {
            this.dockPanel.Children.Clear();
            await this.dockPanel.WaitForNonZeroSizeAsync();
            var availableDocks = new List<Dock>(
                Enum.GetValues(typeof (Dock)).Cast<Dock>());

            int i = 0;
            var namedColors = ColorExtensions.GetNamedColors();

            while (availableDocks.Count >= 0)
            {
                i++;
                var ci = random.Next(namedColors.Count);
                var color = namedColors[ci];
                namedColors.RemoveAt(ci);

                var accentColor = 
                    ((int)color.R + color.G + color.B > 256 * 1.5)
                    ? Colors.Black
                    : Colors.White;


                var border = new Border
                {
                    BorderThickness = new Thickness(2),
                    BorderBrush = null,
                    Background = new SolidColorBrush(color)
                };

                if (availableDocks.Count > 0)
                {
                    var c = random.Next(availableDocks.Count);
                    var dock = availableDocks[c];
                    DockPanel.SetDock(border, dock);
                    availableDocks.RemoveAt(c);

                    border.Child = new TextBlock
                    {
                        Foreground = new SolidColorBrush(accentColor),
                        FontWeight = FontWeights.SemiLight,
                        FontSize = 24,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Text = i + ": " + dock
                    };

                    if (dock == Dock.Left || dock == Dock.Right)
                    {
                        border.Width = this.dockPanel.ActualWidth / 5;
                        border.VerticalAlignment = VerticalAlignment.Stretch;
                    }

                    if (dock == Dock.Top || dock == Dock.Bottom)
                    {
                        border.Height = this.dockPanel.ActualHeight / 5;
                        border.HorizontalAlignment = HorizontalAlignment.Stretch;
                    }

                    this.dockPanel.Children.Add(border);
                }
                else
                {
                    border.Child = new TextBlock
                    {
                        Foreground = new SolidColorBrush(accentColor),
                        FontWeight = FontWeights.SemiLight,
                        FontSize = 24,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Text = i + ": Fill (last)"
                    };

                    border.VerticalAlignment = VerticalAlignment.Stretch;
                    border.HorizontalAlignment = HorizontalAlignment.Stretch;

                    this.dockPanel.Children.Add(border);

                    return;
                }
            }
        }
    }
}
