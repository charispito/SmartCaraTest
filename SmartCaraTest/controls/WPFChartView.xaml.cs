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
        private ObservableCollection<KeyValuePair<DateTime, int>> list1 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        private ObservableCollection<KeyValuePair<DateTime, int>> list2 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        private ObservableCollection<KeyValuePair<DateTime, int>> list3 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        private ObservableCollection<KeyValuePair<DateTime, int>> list4 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        private ObservableCollection<KeyValuePair<DateTime, int>> list5 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        private ObservableCollection<KeyValuePair<DateTime, int>> list6 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        private ObservableCollection<KeyValuePair<DateTime, int>> list7 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        private ObservableCollection<KeyValuePair<DateTime, int>> list8 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public Timer timer = new Timer();
        private Random random = new Random();
        private DateTime dateX = DateTime.Now;
        private int yInterval = 20;
        private int xInterval = 10;

        public WPFChartView(int channel)
        {
            InitializeComponent();
            Channel.Content = "Ch " + (channel + 1);
            chart.Background = Brushes.Transparent;

            LinearAxis axis = new LinearAxis();
            Style verticalStyle = new Style(typeof(NumericAxisLabel));
            verticalStyle.Setters.Add(new Setter(NumericAxisLabel.FontSizeProperty, 12.5));
            Style axisStyle = new Style(typeof(DateTimeAxisLabel));
            axisStyle.Setters.Add(new Setter(DateTimeAxisLabel.FontSizeProperty, 10.0));
            axisStyle.Setters.Add(new Setter(DateTimeAxisLabel.StringFormatProperty, "{0: HH:mm}"));
            //axisX.AxisLabelStyle = axisStyle;
            DateTimeAxis Time = new DateTimeAxis();
            Time.Interval = xInterval;
            Time.Orientation = AxisOrientation.X;
            Time.IntervalType = DateTimeIntervalType.Minutes;
            Time.Location = AxisLocation.Bottom;
            Time.Minimum = DateTime.Now;
            Time.Maximum = DateTime.Now.AddSeconds(14400);
            Time.AxisLabelStyle = axisStyle;
            axis.Orientation = AxisOrientation.Y;
            axis.Foreground = Brushes.Red;
            axis.Minimum = 0;
            axis.Maximum = 200;
            axis.Location = AxisLocation.Auto;
            axis.Interval = 20;
            series1.DependentRangeAxis = axis;
            series1.IndependentAxis = Time;
            series2.IndependentAxis = Time;
            series3.IndependentAxis = Time;
            series4.IndependentAxis = Time;
            series5.IndependentAxis = Time;
            series6.IndependentAxis = Time;
            setStyle(series1, Brushes.Aqua);
            setStyle(series2, Brushes.Chocolate);
            setStyle(series3, Brushes.OrangeRed);

            setStyle(series4, Brushes.Yellow);
            setStyle(series5, Brushes.Gainsboro);
            setStyle(series6, Brushes.BlueViolet);


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
            series3.DependentRangeAxis = axis3;
            series4.DependentRangeAxis = axis3;
            series5.DependentRangeAxis = axis3;
            series6.DependentRangeAxis = axis2;
            series1.ItemsSource = list1;
            series2.ItemsSource = list2;
            series3.ItemsSource = list3;
            series4.ItemsSource = list4;
            series5.ItemsSource = list5;
            series6.ItemsSource = list6;
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            Loaded += WPFChartView_Loaded;
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
        public void setChartData(ObservableCollection<KeyValuePair<DateTime, int>> list, int index)
        {
            switch (index)
            {
                case 0:
                    list1.Clear();
                    list1 = list;
                    break;
                case 1:
                    list2.Clear();
                    list2 = list;
                    break;
                case 2:
                    list3.Clear();
                    list3 = list;
                    break;
                case 3:
                    list4.Clear();
                    list4 = list;
                    break;
                case 4:
                    list5.Clear();
                    list5 = list;
                    break;
                case 5:
                    list6.Clear();
                    list6 = list;
                    break;
                case 6:
                    list7.Clear();
                    list7 = list;
                    break;
                case 7:
                    list8.Clear();
                    list8 = list;
                    break;
            }
        }

        public WPFChartView()
        {
            InitializeComponent();
            chart.Background = Brushes.Transparent;
            LinearAxis axis = new LinearAxis();
            Style axisStyle = new Style(typeof(DateTimeAxisLabel));
            axisStyle.Setters.Add(new Setter(DateTimeAxisLabel.StringFormatProperty, "{0: HH:mm}"));


            DateTimeAxis Time = new DateTimeAxis();
            Time.AxisLabelStyle = axisStyle;
            Time.Interval = xInterval;
            Time.Orientation = AxisOrientation.X;
            Time.IntervalType = DateTimeIntervalType.Minutes;
            Time.Location = AxisLocation.Bottom;
            Time.Minimum = DateTime.Now;
            Time.Maximum = DateTime.Now.AddSeconds(14400);
            axis.Orientation = AxisOrientation.Y;
            axis.Foreground = Brushes.Red;
            axis.Minimum = 0;
            axis.Maximum = 100;
            axis.Location = AxisLocation.Auto;
            axis.Interval = yInterval;
            series1.DependentRangeAxis = axis;
            series1.IndependentAxis = Time;
            series2.IndependentAxis = Time;
            series3.IndependentAxis = Time;
            series4.IndependentAxis = Time;
            series5.IndependentAxis = Time;
            series6.IndependentAxis = Time;
            setStyle(series1, Brushes.Aqua);
            setStyle(series2, Brushes.Chocolate);
            setStyle(series3, Brushes.OrangeRed);

            setStyle(series4, Brushes.Yellow);
            setStyle(series5, Brushes.Gainsboro);
            setStyle(series6, Brushes.BlueViolet);


            LinearAxis axis2 = new LinearAxis();
            axis2.ShowGridLines = true;
            axis2.Orientation = AxisOrientation.Y;
            axis2.Minimum = 400;
            axis2.Foreground = Brushes.Blue;
            axis2.Maximum = 500;
            axis2.Location = AxisLocation.Left;
            axis2.Interval = yInterval;
            series2.DependentRangeAxis = axis2;
            LinearAxis axis3 = new LinearAxis();
            axis3.Orientation = AxisOrientation.Y;
            axis3.Minimum = 200;
            axis3.Maximum = 300;
            axis3.Foreground = Brushes.Green;
            axis3.Location = AxisLocation.Left;
            axis3.Interval = yInterval;

            series3.DependentRangeAxis = axis3;
            series4.DependentRangeAxis = axis3;
            series5.DependentRangeAxis = axis3;
            series6.DependentRangeAxis = axis2;
            series1.ItemsSource = list1;
            series2.ItemsSource = list2;
            series3.ItemsSource = list3;
            series4.ItemsSource = list4;
            series5.ItemsSource = list5;
            series6.ItemsSource = list6;
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            Loaded += WPFChartView_Loaded;
        }

        private void setStyle(LineSeries series, SolidColorBrush color)
        {
            Style style = new Style(typeof(LineDataPoint));
            style.Setters.Add(new Setter(LineDataPoint.TemplateProperty, null));
            //style.Setters.Add(new Setter(LineDataPoint.BorderBrushProperty, null));
            style.Setters.Add(new Setter(LineDataPoint.BackgroundProperty, color));
            series.DataPointStyle = style;
        }

        private void WPFChartView_Loaded(object sender, RoutedEventArgs e)
        {
            //timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                //list1.Add(new KeyValuePair<DateTime, int>(dateX, random.Next(10, 30)));
                //list2.Add(new KeyValuePair<DateTime, int>(dateX, random.Next(440, 460)));
                //list3.Add(new KeyValuePair<DateTime, int>(dateX, random.Next(270, 290)));

                //list4.Add(new KeyValuePair<DateTime, int>(dateX, random.Next(220, 270)));
                //list5.Add(new KeyValuePair<DateTime, int>(dateX, random.Next(250, 300)));
                //list6.Add(new KeyValuePair<DateTime, int>(dateX, random.Next(400, 430)));
                //dateX = dateX.AddSeconds(30);
                //Console.WriteLine(dateX);
            }));
        }
    }
}
