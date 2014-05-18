using System.Threading.Tasks;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class SquareGridTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        private int i;
        private bool isUp;
        private bool isAuto;
        private bool isAlive = true;

        private SolidColorBrush[] brushes = new[]
                    {
                        new SolidColorBrush(Colors.Red),
                        new SolidColorBrush(Colors.White),
                        new SolidColorBrush(Colors.Yellow),
                        new SolidColorBrush(Colors.GreenYellow),
                        new SolidColorBrush(Colors.DeepSkyBlue),
                        new SolidColorBrush(Colors.MediumPurple),
                        new SolidColorBrush(Colors.Orange),
                        new SolidColorBrush(Colors.DeepPink),
                        new SolidColorBrush(Colors.ForestGreen),
                        //new SolidColorBrush(Colors.DodgerBlue),
                        //new SolidColorBrush(Colors.Gray),
                        //new SolidColorBrush(Colors.IndianRed),
                    };

        public SquareGridTestPage()
        {
            this.InitializeComponent();
            for (int i = 0; i < 9; i++)
            {
                Add();
            }

            this.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                async () =>
                {
                    while (isAlive)
                    {
                        await Task.Delay(2);

                        if (!isAuto)
                            continue;

                        if (isUp)
                        {
                            Add();
                        }
                        else
                        {
                            Remove();
                        }
                    }
                })
            ;
            CoreWindow.GetForCurrentThread().KeyDown += OnKeyDown;
            //this.Loaded += (s, e) => DC.ShowVisualTree(this);
        }

        private void OnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Up)
            {
                isAuto = false;
                Add();
            }
            if (args.VirtualKey == VirtualKey.Down)
            {
                isAuto = false;
                Remove();
            }
        }

        private void Remove()
        {
            if (this.sp.Children.Count > 0)
            {
                this.sp.Children.RemoveAt(this.sp.Children.Count - 1);
                i--;
            }
        }

        private void Add()
        {
            this.sp.Children.Add(new Border
            {
                Background = brushes[++i % brushes.Length],
                Child =
                    new Viewbox
                    {
                        Child =
                            new TextBlock
                            {
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                Text = i.ToString(),
                                Foreground = new SolidColorBrush(Colors.Black),
                                FontWeight = FontWeights.ExtraLight
                            },
                            Margin = new Thickness(3)
                    }
            });
        }

        private void OnAutoAddButtonClick(object sender, RoutedEventArgs e)
        {
            isUp = true;
            isAuto = true;
        }

        private void OnAutoRemoveButtonClick(object sender, RoutedEventArgs e)
        {
            isUp = false;
            isAuto = true;
        }

        private void OnRemoveButtonClick(object sender, RoutedEventArgs e)
        {
            isAuto = false;
            Remove();
        }

        private void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            isAuto = false;
            Add();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            isAlive = false;
            CoreWindow.GetForCurrentThread().KeyDown -= OnKeyDown;
            this.Frame.GoBack();
        }
    }
}
