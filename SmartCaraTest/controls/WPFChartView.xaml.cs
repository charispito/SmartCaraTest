using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartCaraTest.controls
{
    /// <summary>
    /// WPFChartView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WPFChartView : UserControl
    {
        public Timer timer = new Timer();
        private Random random = new Random();
        private DateTime dateX = DateTime.Now;
        private int yInterval = 20;
        private int xInterval = 10;
        public Dictionary<int, bool> ItemRun = new Dictionary<int, bool>();
        public List<LineSeries> seriesList = new List<LineSeries>();
        public List<LinearAxis> Yaxis = new List<LinearAxis>();
        private DateTimeAxis Time = new DateTimeAxis();

        public WPFChartView(int channel)
        {
            InitializeComponent();
            Channel.Content = "Ch " + (channel + 1);
            chart.Background = Brushes.Transparent;

            LinearAxis axis = new LinearAxis();
            Style verticalStyle = new Style(typeof(NumericAxisLabel));
            verticalStyle.Setters.Add(new Setter(NumericAxisLabel.FontSizeProperty, 12.5));
            for(int i = 1; i < 5; i++)
            {
                ItemRun[i] = false;
            }
            axis.Orientation = AxisOrientation.Y;
            axis.Foreground = Brushes.Red;
            axis.Minimum = 0;
            axis.Maximum = 200;
            axis.Location = AxisLocation.Auto;
            axis.Interval = 20;
            series1.DependentRangeAxis = axis;

            setStyle(series1, Brushes.Green);
            setStyle(series2, Brushes.Red);
            setStyle(series3, Brushes.Blue);
            setStyle(series4, Brushes.Magenta);
            setStyle(series5, Brushes.Brown);
            setStyle(series6, Brushes.DarkOrchid);
            setStyle(series7, Brushes.SpringGreen);
            setStyle(series8, Brushes.SteelBlue);

            LinearAxis axis2 = new LinearAxis();
            axis2.ShowGridLines = false;
            axis2.Orientation = AxisOrientation.Y;
            axis2.Minimum = 0;
            axis2.Foreground = Brushes.Blue;
            axis2.Maximum = 100;
            axis2.Location = AxisLocation.Left;
            axis2.Interval = 10;
            series2.DependentRangeAxis = axis2;
            LinearAxis axis3 = new LinearAxis();
            axis3.Orientation = AxisOrientation.Y;
            axis3.Minimum = 0.0;
            axis3.Maximum = 2.0;
            axis3.Foreground = Brushes.Magenta;
            axis3.Location = AxisLocation.Left;
            axis3.Interval = 0.2;
            
            axis.AxisLabelStyle = verticalStyle;
            axis2.AxisLabelStyle = verticalStyle;
            axis3.AxisLabelStyle = verticalStyle;
            Yaxis.Add(axis);
            Yaxis.Add(axis2);
            Yaxis.Add(axis3);
        }

        public void setAxis(LineSeries series, int index)
        {
            series.DependentRangeAxis = Yaxis[index];
        }

        public void setLegend(int index, string label)
        {
            switch (index)
            {
                case 0:
                    series1Legend.Content = label;
                    break;
                case 1:
                    series2Legend.Content = label;
                    break;
                case 2:
                    series3Legend.Content = label;
                    break;
                case 3:
                    series4Legend.Content = label;
                    break;
                case 4:
                    series5Legend.Content = label;
                    break;
                case 5:
                    series6Legend.Content = label;
                    break;
                case 6:
                    series7Legend.Content = label;
                    break;
                case 7:
                    series8Legend.Content = label;
                    break;
            }
        }

        public void initXAxis()
        {
            bool run = false;
            for(int i = 1; i < 5; i++)
            {
                if(ItemRun[i])
                    run = true;
            }
            if (run)
                return;
            Time = new DateTimeAxis();
            Style axisStyle = new Style(typeof(DateTimeAxisLabel));
            axisStyle.Setters.Add(new Setter(DateTimeAxisLabel.FontSizeProperty, 10.0));
            axisStyle.Setters.Add(new Setter(DateTimeAxisLabel.StringFormatProperty, "{0: HH:mm}"));
            Time.Interval = xInterval;
            Time.Orientation = AxisOrientation.X;
            Time.IntervalType = DateTimeIntervalType.Minutes;
            Time.Location = AxisLocation.Bottom;
            Time.Minimum = DateTime.Now;
            Time.Maximum = DateTime.Now.AddSeconds(14400);
            Time.AxisLabelStyle = axisStyle;
            series1.IndependentAxis = Time;
            series2.IndependentAxis = Time;
            series3.IndependentAxis = Time;
            series4.IndependentAxis = Time;
            series5.IndependentAxis = Time;
            series6.IndependentAxis = Time;
            series7.IndependentAxis = Time;
            series8.IndependentAxis = Time;
        }
     
        public WPFChartView()
        {
            InitializeComponent();
            chart.Background = Brushes.Transparent;

            LinearAxis axis = new LinearAxis();
            Style verticalStyle = new Style(typeof(NumericAxisLabel));
            verticalStyle.Setters.Add(new Setter(NumericAxisLabel.FontSizeProperty, 12.5));
            for (int i = 1; i < 5; i++)
            {
                ItemRun[i] = false;
            }
            axis.Orientation = AxisOrientation.Y;
            axis.Foreground = Brushes.Red;
            axis.Minimum = 0;
            axis.Maximum = 200;
            axis.Location = AxisLocation.Auto;
            axis.Interval = 20;
            series1.DependentRangeAxis = axis;
            setStyle(series1, Brushes.Green);
            setStyle(series2, Brushes.Red);
            setStyle(series3, Brushes.Blue);
            setStyle(series4, Brushes.Magenta);
            setStyle(series5, Brushes.Brown);
            setStyle(series6, Brushes.DarkOrchid);
            setStyle(series7, Brushes.SpringGreen);
            setStyle(series8, Brushes.SteelBlue);


            LinearAxis axis2 = new LinearAxis();
            axis2.ShowGridLines = false;
            axis2.Orientation = AxisOrientation.Y;
            axis2.Minimum = 0;
            axis2.Foreground = Brushes.Blue;
            axis2.Maximum = 100;
            axis2.Location = AxisLocation.Left;
            axis2.Interval = 10;
            series2.DependentRangeAxis = axis2;
            LinearAxis axis3 = new LinearAxis();
            axis3.Orientation = AxisOrientation.Y;
            axis3.Minimum = 0.0;
            axis3.Maximum = 2.0;
            axis3.Foreground = Brushes.Green;
            axis3.Location = AxisLocation.Left;
            axis3.Interval = 0.2;

            axis.AxisLabelStyle = verticalStyle;
            axis2.AxisLabelStyle = verticalStyle;
            axis3.AxisLabelStyle = verticalStyle;
            Yaxis.Add(axis);
            Yaxis.Add(axis2);
            Yaxis.Add(axis3);
            seriesList.Add(series1);
            seriesList.Add(series2);
            seriesList.Add(series3);
            seriesList.Add(series4);
            seriesList.Add(series5);
            seriesList.Add(series6);
            seriesList.Add(series7);
            seriesList.Add(series8);
        }

        private void setStyle(LineSeries series, SolidColorBrush color)
        {
            Style style = new Style(typeof(LineDataPoint));
            style.Setters.Add(new Setter(LineDataPoint.TemplateProperty, null));
            style.Setters.Add(new Setter(LineDataPoint.BackgroundProperty, color));
            series.DataPointStyle = style;
        }
    }
}
