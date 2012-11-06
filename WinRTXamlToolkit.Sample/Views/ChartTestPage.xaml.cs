using System.Collections.Generic;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Sample.Views
{
    public sealed partial class ChartTestPage : WinRTXamlToolkit.Controls.AlternativePage
    {
        public ChartTestPage()
        {
            this.InitializeComponent();

            List<NameValueItem> items = new List<NameValueItem>();
            items.Add(new NameValueItem { Name = "Test1", Value = 40 });
            items.Add(new NameValueItem { Name = "Test2", Value = 50 });
            items.Add(new NameValueItem { Name = "Test3", Value = 20 });
            items.Add(new NameValueItem { Name = "Test4", Value = 10 });
            items.Add(new NameValueItem { Name = "Test5", Value = 100 });

            ((ColumnSeries)Chart.Series[0]).ItemsSource = items;
            ((BarSeries)BarChart.Series[0]).ItemsSource = items;
            ((LineSeries)LineChart.Series[0]).ItemsSource = items;
            ((LineSeries)LineChart2.Series[0]).ItemsSource = items;
            ((ColumnSeries)MixedChart.Series[0]).ItemsSource = items;
            ((LineSeries)MixedChart.Series[1]).ItemsSource = items;
            ((AreaSeries)AreaChart.Series[0]).ItemsSource = items;
            ((BubbleSeries)BubbleChart.Series[0]).ItemsSource = items;
            ((ScatterSeries)ScatterChart.Series[0]).ItemsSource = items;
            ((StackedBarSeries)StackedBar.Series[0]).SeriesDefinitions[0].ItemsSource = items;
            ((StackedBarSeries)StackedBar.Series[0]).SeriesDefinitions[1].ItemsSource = items;
            ((StackedBarSeries)StackedBar.Series[0]).SeriesDefinitions[2].ItemsSource = items;
            ((Stacked100BarSeries)StackedBar100.Series[0]).SeriesDefinitions[0].ItemsSource = items;
            ((Stacked100BarSeries)StackedBar100.Series[0]).SeriesDefinitions[1].ItemsSource = items;
            ((Stacked100BarSeries)StackedBar100.Series[0]).SeriesDefinitions[2].ItemsSource = items;

            ((StackedColumnSeries)StackedColumn.Series[0]).SeriesDefinitions[0].ItemsSource = items;
            ((StackedColumnSeries)StackedColumn.Series[0]).SeriesDefinitions[1].ItemsSource = items;
            ((StackedColumnSeries)StackedColumn.Series[0]).SeriesDefinitions[2].ItemsSource = items;

            ((Stacked100ColumnSeries)StackedColumn100.Series[0]).SeriesDefinitions[0].ItemsSource = items;
            ((Stacked100ColumnSeries)StackedColumn100.Series[0]).SeriesDefinitions[1].ItemsSource = items;
            ((Stacked100ColumnSeries)StackedColumn100.Series[0]).SeriesDefinitions[2].ItemsSource = items;

            ((PieSeries)PieChart.Series[0]).ItemsSource = items;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        public class NameValueItem
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }
    }
}
