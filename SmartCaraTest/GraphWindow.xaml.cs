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
        Chart chart = new Chart();
        Legend legend1 = new Legend();
        ChartArea chartArea1 = new ChartArea();
        Series series1 = new Series();
        Series series2 = new Series();
        Series series3 = new Series();
        Timer timer1 = new Timer();
        DateTime dateX = DateTime.Now.Date;
        public GraphWindow()
        {
            InitializeComponent();
            graphInit();
            
            timer1.Interval = 1000;
            timer1.Elapsed += Timer1_Elapsed;
            Loaded += GraphWindow_Loaded1;
            Closed += GraphWindow_Closed;
        }

        private void GraphWindow_Closed(object sender, EventArgs e)
        {
            timer1.Stop();
            timer1.Dispose();
        }

        private void Timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => {
                chart.Series["Series1"].Points.AddXY(dateX, random.Next(5, 100));
                chart.Series["Series2"].Points.AddXY(dateX, random.Next(8000, 8200));
                chart.Series["Series3"].Points.AddXY(dateX, random.Next(1000, 3000));
                dateX = dateX.AddDays(1);
            }));

        }

        private void GraphWindow_Loaded1(object sender, RoutedEventArgs e)
        {
            timer1.Start();
        }

       
        private void graphInit() 
        {
            chartArea1.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisX.LabelStyle.Format = "MM/dd";
            chartArea1.AxisX.LabelStyle.Interval = 0;
            chartArea1.AxisX.LineColor = Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisX.MajorGrid.LineColor = Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisX.MajorTickMark.Size = 2F;
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.IsStartedFromZero = false;
            chartArea1.AxisY.LabelStyle.Font = new Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisY.LabelStyle.Format = "N0";
            chartArea1.AxisY.LineColor = Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.MajorGrid.LineColor = Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.BackColor = Color.OldLace;
            chartArea1.BackGradientStyle = GradientStyle.TopBottom;
            chartArea1.BackSecondaryColor = Color.White;
            chartArea1.BorderColor = Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.Name = "Default";
            chartArea1.ShadowColor = Color.Transparent;
            chart.ChartAreas.Add(chartArea1);
            legend1.BackColor = Color.Transparent;
            legend1.Enabled = false;
            legend1.Font = new Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            legend1.IsTextAutoFit = false;
            legend1.Name = "Default";
            chart.Legends.Add(legend1);
            chart.Name = "Chart";

            series1.BorderColor = Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            series1.BorderWidth = 2;
            series1.ChartArea = chartArea1.Name;
            series1.ChartType = SeriesChartType.Line;
            series1.Legend = "Default";
            series1.Name = "Series1";
            series1.ShadowColor = Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            series1.ShadowOffset = 2;
            series2.BorderColor = Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            series2.BorderWidth = 2;
            series2.ChartArea = chartArea1.Name;
            series2.ChartType = SeriesChartType.Line;
            series2.Legend = "Default";
            series2.Name = "Series2";
            series2.ShadowColor = Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            series2.ShadowOffset = 2;
            series3.BorderColor = Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            series3.BorderWidth = 2;
            series3.ChartArea = chartArea1.Name;
            series3.ChartType = SeriesChartType.Line;
            series3.Legend = "Default";
            series3.Name = "Series3";
            series3.ShadowColor = Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            series3.ShadowOffset = 2;
            chart.Series.Add(series1);
            chart.Series.Add(series2);
            chart.Series.Add(series3);
            host.Child = chart;

            //chart.ChartAreas["Default"].Position = new ElementPosition(25, 10, 68, 85);
            //chart.ChartAreas["Default"].InnerPlotPosition = new ElementPosition(10, 0, 90, 90);

            //// Create extra Y axis for second and third series
            //CreateYAxis(chart, chart.ChartAreas["Default"], chart.Series["Series2"], 13, 8);
            //CreateYAxis(chart, chart.ChartAreas["Default"], chart.Series["Series3"], 22, 8);
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

            // Disable drid lines & tickmarks
            areaAxis.AxisX.LineWidth = 0;
            areaAxis.AxisX.MajorGrid.Enabled = false;
            areaAxis.AxisX.MajorTickMark.Enabled = false;
            areaAxis.AxisX.LabelStyle.Enabled = false;
            areaAxis.AxisY.MajorGrid.Enabled = false;
            areaAxis.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;
            areaAxis.AxisY.LabelStyle.Font = area.AxisY.LabelStyle.Font;

            // Adjust area position
            areaAxis.Position.X -= axisOffset;
            areaAxis.InnerPlotPosition.X += labelsSize;
        }
    }
}
