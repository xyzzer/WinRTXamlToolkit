using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Tools;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ChartTestView : UserControl
    {
        private bool isInitialized;

        public ChartTestView()
        {
            this.InitializeComponent();
            this.isInitialized = true;
            //UpdateCharts();
        }

        private Random _random = new Random();
        private bool axisLabelsHidden = false;

        private EventThrottler _updateThrottler = new EventThrottler();

        ObservableCollection<NameValueItem> items1 = new ObservableCollection<NameValueItem>();
        ObservableCollection<NameValueItem> items2 = new ObservableCollection<NameValueItem>();
        ObservableCollection<NameValueItem> items3 = new ObservableCollection<NameValueItem>();

        private void RunIfSelected(UIElement element, Action action)
        {
            if (ChartsList.SelectedItem == element)
            {
                action.Invoke();
            }
        }

        private void UpdateCharts()
        {
            if (!this.isInitialized)
            {
                return;
            }

            this.items1 = new ObservableCollection<NameValueItem>();
            this.items2 = new ObservableCollection<NameValueItem>();
            this.items3 = new ObservableCollection<NameValueItem>();

            for (int i = 0; i < NumberOfIitemsNumericUpDown.Value; i++)
            {
                this.items1.Add(new NameValueItem { Name = "Test" + i, Value = _random.Next(10, 100) });
                this.items2.Add(new NameValueItem { Name = "Test" + i, Value = _random.Next(10, 100) });
                this.items3.Add(new NameValueItem { Name = "Test" + i, Value = _random.Next(10, 100) });
            }

            this.RunIfSelected(this.ColumnChart, () => ((ColumnSeries)this.ColumnChart.Series[0]).ItemsSource = items1);
            this.RunIfSelected(this.BarChart, () => ((BarSeries)this.BarChart.Series[0]).ItemsSource = items1);
            this.RunIfSelected(this.LineChart, () => ((LineSeries)this.LineChart.Series[0]).ItemsSource = items1);
            this.RunIfSelected(this.LineChart, () => ((LineSeries)this.LineChart.Series[1]).ItemsSource = items2);
            this.RunIfSelected(this.LineChart, () => ((LineSeries)this.LineChart.Series[2]).ItemsSource = items3);
            this.RunIfSelected(this.LineChart, () => ((LineSeries)this.LineChart2.Series[0]).ItemsSource = items1);
            this.RunIfSelected(this.MixedChart, () => ((ColumnSeries)this.MixedChart.Series[0]).ItemsSource = items1);
            this.RunIfSelected(this.MixedChart, () => ((LineSeries)this.MixedChart.Series[1]).ItemsSource = items1);
            this.RunIfSelected(this.AreaChart, () => ((AreaSeries)this.AreaChart.Series[0]).ItemsSource = items1);
            this.RunIfSelected(this.AreaChartWithNoLabels,
                () =>
                {
                    var series = (AreaSeries)this.AreaChartWithNoLabels.Series[0];
                    series.ItemsSource = items1;

                    if (!this.axisLabelsHidden)
                    {
                        series.DependentRangeAxis =
                            new LinearAxis
                            {
                                Minimum = 0,
                                Maximum = 100,
                                Orientation = AxisOrientation.Y,
                                Interval = 20,
                                ShowGridLines = false,
                                Width = 0
                            };
                        series.IndependentAxis =
                            new CategoryAxis
                            {
                                Orientation = AxisOrientation.X,
                                Height = 0
                            };
                        this.axisLabelsHidden = true;
                    }
                });

            this.RunIfSelected(this.BubbleChart, () => ((BubbleSeries)this.BubbleChart.Series[0]).ItemsSource = items1);
            this.RunIfSelected(this.ScatterChart, () => ((ScatterSeries)this.ScatterChart.Series[0]).ItemsSource = items1);
            this.RunIfSelected(this.StackedBar, () => ((StackedBarSeries)this.StackedBar.Series[0]).SeriesDefinitions[0].ItemsSource = items1);
            this.RunIfSelected(this.StackedBar, () => ((StackedBarSeries)this.StackedBar.Series[0]).SeriesDefinitions[1].ItemsSource = items2);
            this.RunIfSelected(this.StackedBar, () => ((StackedBarSeries)this.StackedBar.Series[0]).SeriesDefinitions[2].ItemsSource = items3);
            this.RunIfSelected(this.StackedBar100, () => ((Stacked100BarSeries)this.StackedBar100.Series[0]).SeriesDefinitions[0].ItemsSource = items1);
            this.RunIfSelected(this.StackedBar100, () => ((Stacked100BarSeries)this.StackedBar100.Series[0]).SeriesDefinitions[1].ItemsSource = items2);
            this.RunIfSelected(this.StackedBar100, () => ((Stacked100BarSeries)this.StackedBar100.Series[0]).SeriesDefinitions[2].ItemsSource = items3);

            this.RunIfSelected(this.StackedColumn, () => ((StackedColumnSeries)this.StackedColumn.Series[0]).SeriesDefinitions[0].ItemsSource = items1);
            this.RunIfSelected(this.StackedColumn, () => ((StackedColumnSeries)this.StackedColumn.Series[0]).SeriesDefinitions[1].ItemsSource = items2);
            this.RunIfSelected(this.StackedColumn, () => ((StackedColumnSeries)this.StackedColumn.Series[0]).SeriesDefinitions[2].ItemsSource = items3);

            this.RunIfSelected(this.StackedColumn100, () => ((Stacked100ColumnSeries)this.StackedColumn100.Series[0]).SeriesDefinitions[0].ItemsSource = items1);
            this.RunIfSelected(this.StackedColumn100, () => ((Stacked100ColumnSeries)this.StackedColumn100.Series[0]).SeriesDefinitions[1].ItemsSource = items2);
            this.RunIfSelected(this.StackedColumn100, () => ((Stacked100ColumnSeries)this.StackedColumn100.Series[0]).SeriesDefinitions[2].ItemsSource = items3);

            this.RunIfSelected(this.PieChart, () => ((PieSeries)this.PieChart.Series[0]).ItemsSource = items1);
            this.RunIfSelected(this.PieChartWithCustomDesign, () => ((PieSeries)this.PieChartWithCustomDesign.Series[0]).ItemsSource = items1);
            this.RunIfSelected(this.LineChartWithAxes,
                () =>
                {
                    ((LineSeries)LineChartWithAxes.Series[0]).ItemsSource = items1;
                    ((LineSeries)LineChartWithAxes.Series[0]).DependentRangeAxis =
                        new LinearAxis
                        {
                            Minimum = 0,
                            Maximum = 100,
                            Orientation = AxisOrientation.Y,
                            Interval = 20,
                            ShowGridLines = true
                        };
                });
        }

        public class NameValueItem : INotifyPropertyChanged
        {
            private int _value;

            public string Name { get; set; }
            public int Value
            {
                get
                {
                    return _value;
                }
                set
                {
                    if (_value != value)
                    {
                        _value = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        private void OnUpdateButtonClick(object sender, RoutedEventArgs e)
        {
            this.ThrottledUpdate(
                () =>
                {
                    for (int i = 0; i < items1.Count; ++i)
                    {
                        items1[i].Value = _random.Next(10, 100);
                        items2[i].Value = _random.Next(10, 100);
                        items3[i].Value = _random.Next(10, 100);
                    }
                });
        }

        private void ThrottledUpdate(Action updateAction)
        {
            _updateThrottler.Run(
                async () =>
                {
                    var sw = new Stopwatch();
                    sw.Start();
                    updateAction();
                    sw.Stop();
                    await Task.Delay(sw.Elapsed);
                });
        }

        private void NumberOfIitemsNumericUpDown_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            this.ThrottledUpdate(() =>
            {
                int diff = (int)(e.NewValue - items1.Count);
                if (diff < 0)
                {
                    while (items1.Count > e.NewValue && items1.Count > 0)
                    {
                        items1.RemoveAt(items1.Count - 1);
                        items2.RemoveAt(items2.Count - 1);
                        items3.RemoveAt(items3.Count - 1);
                    }
                }
                else
                {
                    for (int i = 0; i < diff; ++i)
                    {
                        this.items1.Add(new NameValueItem { Name = "Test" + items1.Count, Value = _random.Next(10, 100) });
                        this.items2.Add(new NameValueItem { Name = "Test" + items2.Count, Value = _random.Next(10, 100) });
                        this.items3.Add(new NameValueItem { Name = "Test" + items3.Count, Value = _random.Next(10, 100) });
                    }
                }
            });
        }

        private void ChartsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ThrottledUpdate(UpdateCharts);
        }
    }
}
