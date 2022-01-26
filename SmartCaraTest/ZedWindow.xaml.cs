using System;
using System.Drawing;
using System.Timers;
using System.Windows;
using ZedGraph;

namespace SmartCaraTest
{
    /// <summary>
    /// ZedWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ZedWindow : Window
    {
        PointPairList data1 = new PointPairList();
        PointPairList data2 = new PointPairList();
        PointPairList data3 = new PointPairList();
        PointPairList data4 = new PointPairList();
        double dateX = DateTime.MinValue.ToOADate();
        Timer timer = new Timer();
        Random random = new Random();
        LineItem[] items = new LineItem[4];

        public ZedWindow()
        {
            InitializeComponent();
            items[0] = chart.GraphPane.AddCurve("series1", data1, Color.Red);
            items[1] = chart.GraphPane.AddCurve("series2", data1, Color.Green);
            items[2] = chart.GraphPane.AddCurve("series3", data1, Color.Blue);
            items[3] = chart.GraphPane.AddCurve("series4", data1, Color.Bisque);
            chart.GraphPane.XAxis.Type = AxisType.Date;
            YAxis yAxis = new YAxis();
            items[0].YAxisIndex = 0;
            items[1].YAxisIndex = 1;
            yAxis.Color = Color.Blue;
            yAxis.AxisGap = 10;
            yAxis.IsAxisSegmentVisible = false;
            yAxis.Title.IsVisible = false;
            yAxis.MajorGrid.IsVisible = false;
            yAxis.MinorGrid.IsVisible = false; 
            chart.GraphPane.YAxisList[0].Title.IsVisible = false;
            chart.GraphPane.YAxisList[0].MajorGrid.IsVisible = false;
            chart.GraphPane.YAxisList.Add(yAxis);
            Loaded += ZedWindow_Loaded;
            Closed += (s, e) => { timer.Stop(); timer.Dispose(); };

        }

        private void ZedWindow_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => {
                data1.Add(new PointPair(dateX, random.Next(0, 100)));
                data2.Add(new PointPair(dateX, random.Next(10, 500)));
                data3.Add(new PointPair(dateX, random.Next(800, 1000)));
                data4.Add(new PointPair(dateX, random.Next(1000, 6000)));
                dateX = DateTime.FromOADate(dateX).AddSeconds(1).ToOADate();
            }));
        }
    }
}
