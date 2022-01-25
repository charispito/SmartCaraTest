using SmartCaraTest.data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Timers;
using System.Windows;
using System.Windows.Forms.DataVisualization.Charting;

namespace SmartCaraTest
{
    /// <summary>
    /// GraphWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GraphWindow : Window
    {
        Random random = new Random();
        
        public GraphWindow()
        {
            InitializeComponent();
            List<ChartData> list = new List<ChartData>();
            for(int i = 0; i < 7200; i++)
            {
                list.Add(new ChartData() { Id = i, Value = 1 });
            }
            graphInit(list);

        }

        private void GraphWindow_Closed(object sender, EventArgs e)
        {
        }

        private void GraphWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            
        }

        private void graphInit(List<ChartData> subMcuList) 
        { 
            Chart chartView = new Chart(); 
            chartView.BackColor = Color.FromArgb(255, 255, 255, 255);
            var view = GetChartArea(Color.Red, "(ºC)", 0, 100);
            chartView.ChartAreas.Add(view);
            //chartView.ChartAreas.Add(GetChartArea(Color.Blue, "(ºF)", 0, 300));
            Series series = new Series(); 
            series.ChartArea = "Default";
            series.Name = "series1";
            series.ChartType = SeriesChartType.Line;
            series.XValueType = ChartValueType.Int32;
            series.BackSecondaryColor = Color.Aquamarine;
            series.LabelForeColor = Color.White;
            series.Color = Color.SteelBlue;
            series.IsValueShownAsLabel = true;
            chartView.Series.Add(series);

            Series series2 = new Series();
            series2.ChartArea = "Default";
            series2.Name = "series2";
            series2.XValueType = ChartValueType.Int32;
            series2.BackSecondaryColor = Color.Orange;
            series2.LabelForeColor = Color.White;
            series2.Color = Color.Red;
            series2.ChartType = SeriesChartType.Line;
            series2.IsValueShownAsLabel = true;
            chartView.Series.Add(series2);

            Legend legend = new Legend();
            legend.ForeColor = Color.Red;
            legend.Title = "ss";
            legend.Enabled = false;
            chartView.Legends.Add(legend);
            host.Child = chartView;
            chartView.Series[0].Points.AddXY(1, 0);
            chartView.Series[0].Points.AddXY(0, 0);
            CreateYAxis(chartView, view, series2, 13, 8);
        }

        private ChartArea GetChartArea(Color color, string label, int min, int max)
        {
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "Default";
            chartArea.AxisX.IntervalAutoMode = IntervalAutoMode.FixedCount;
            chartArea.AxisX.Interval = 5;
            chartArea.AxisX.TitleForeColor = Color.Black;
            chartArea.AxisX.LineColor = Color.Black;
            chartArea.AxisX.MajorGrid.LineColor = Color.Black;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.LabelStyle.ForeColor = Color.Black;
            chartArea.AxisX.MajorTickMark.LineColor = Color.Black;
            chartArea.AxisY.LineColor = Color.Black;
            chartArea.AxisY.TitleForeColor = Color.Black;
            chartArea.AxisY.Maximum = max;
            chartArea.AxisX.Minimum = 0;
            chartArea.AxisX.Maximum = 120;
            chartArea.AxisY.Title = label;
            chartArea.AxisY.TitleForeColor = color;
            chartArea.AxisY.TitleAlignment = StringAlignment.Near;
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(255, 24, 24, 24);
            chartArea.AxisY.LabelStyle.ForeColor = color;
            chartArea.AxisY.MajorTickMark.LineColor = Color.Black;
            int second = 0;
            chartArea.AxisX.LabelStyle.Format = String.Format("{0:D1}분", second);
            return chartArea;
        }

        public void CreateYAxis(Chart chart, ChartArea area, Series series, float axisOffset, float labelsSize)
        {
            ChartArea areaSeries = chart.ChartAreas.Add("ChartArea_" + series.Name);
            areaSeries.BackColor = Color.Transparent;
            areaSeries.BorderColor = Color.Transparent;
            areaSeries.Position.FromRectangleF(area.Position.ToRectangleF());
            areaSeries.InnerPlotPosition.FromRectangleF(area.InnerPlotPosition.ToRectangleF());
            areaSeries.AxisX.MajorGrid.Enabled = false;
            areaSeries.AxisX.MajorTickMark.Enabled = false;
            areaSeries.AxisX.LabelStyle.Enabled = false;
            areaSeries.AxisY.MajorGrid.Enabled = false;
            areaSeries.AxisY.MajorTickMark.Enabled = false;
            areaSeries.AxisY.LabelStyle.Enabled = false;
            areaSeries.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;

            series.ChartArea = areaSeries.Name;

            // Create new chart area for axis
            ChartArea areaAxis = chart.ChartAreas.Add("AxisY_" + series.ChartArea);
            areaAxis.BackColor = Color.Transparent;
            areaAxis.BorderColor = Color.Transparent;
            areaAxis.Position.FromRectangleF(chart.ChartAreas[series.ChartArea].Position.ToRectangleF());
            areaAxis.InnerPlotPosition.FromRectangleF(chart.ChartAreas[series.ChartArea].InnerPlotPosition.ToRectangleF());

            // Create a copy of specified series
            Series seriesCopy = chart.Series.Add(series.Name + "_Copy");
            seriesCopy.ChartType = series.ChartType;
            foreach (DataPoint point in series.Points)
            {
                seriesCopy.Points.AddXY(point.XValue, point.YValues[0]);
            }

            // Hide copied series
            seriesCopy.IsVisibleInLegend = false;
            seriesCopy.Color = Color.Transparent;
            seriesCopy.BorderColor = Color.Transparent;
            seriesCopy.ChartArea = areaAxis.Name;

            // Disable grid lines & tickmarks
            areaAxis.AxisX.LineWidth = 0;
            areaAxis.AxisX.MajorGrid.Enabled = false;
            areaAxis.AxisX.MajorTickMark.Enabled = false;
            areaAxis.AxisX.LabelStyle.Enabled = false;
            areaAxis.AxisY.MajorGrid.Enabled = false;
            areaAxis.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;

            // Adjust area position
            areaAxis.Position.X -= axisOffset;
            areaAxis.InnerPlotPosition.X += labelsSize;
        }
    }
}
