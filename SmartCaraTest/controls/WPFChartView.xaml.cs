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
        private Timer timer = new Timer();
        private Random random = new Random();
        private DateTime dateX = DateTime.Now;

        public WPFChartView()
        {
            InitializeComponent();
            chart.Background = Brushes.Transparent;
            LinearAxis axis = new LinearAxis();
            DateTimeAxis axisX = new DateTimeAxis();
            Style axisStyle = new Style(typeof(DateTimeAxisLabel));
            axisStyle.Setters.Add(new Setter(DateTimeAxisLabel.MinutesIntervalStringFormatProperty, "hh:mm"));
            //axisX.AxisLabelStyle = axisStyle;
            axisX.IntervalType = DateTimeIntervalType.Minutes;
            axisX.Interval = 5;
            axisX.Orientation = AxisOrientation.X;
            axisX.Location = AxisLocation.Bottom;            
            axisX.Minimum = DateTime.Now;
            axisX.Maximum = DateTime.Now.AddSeconds(7200);
            axis.Orientation = AxisOrientation.Y;
            axis.Minimum = 0;
            axis.Maximum = 100;
            axis.Location = AxisLocation.Auto;
            axis.Interval = 10;
            series1.DependentRangeAxis = axis;
            series1.IndependentAxis = axisX;
            series2.IndependentAxis = axisX;
            series3.IndependentAxis = axisX;
            series4.IndependentAxis = axisX;
            series5.IndependentAxis = axisX;
            series6.IndependentAxis = axisX;
            setStyle(series1, Brushes.Aqua);
            setStyle(series2, Brushes.Chocolate);
            setStyle(series3, Brushes.OrangeRed);

            setStyle(series4, Brushes.Yellow);
            setStyle(series5, Brushes.Gainsboro);
            setStyle(series6, Brushes.BlueViolet);


            LinearAxis axis2 = new LinearAxis();
            axis2.Orientation = AxisOrientation.Y;
            axis2.Minimum = 400;
            axis2.Maximum = 500;
            axis2.Location = AxisLocation.Left;
            axis2.Interval = 10;
            series2.DependentRangeAxis = axis2;
            LinearAxis axis3 = new LinearAxis();
            axis3.Orientation = AxisOrientation.Y;
            axis3.Minimum = 200;
            axis3.Maximum = 300;
            axis3.Location = AxisLocation.Auto;
            axis3.Interval = 10;
            
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
            style.Setters.Add(new Setter(LineDataPoint.BackgroundProperty, color));            
            series.DataPointStyle = style;
        }

        private void WPFChartView_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => 
            {
                list1.Add(new KeyValuePair<DateTime, int>(dateX, random.Next(10, 30)));
                list2.Add(new KeyValuePair<DateTime, int>(dateX, random.Next(440, 460)));
                list3.Add(new KeyValuePair<DateTime, int>(dateX, random.Next(270, 290)));

                list4.Add(new KeyValuePair<DateTime, int>(dateX, random.Next(220, 270)));
                list5.Add(new KeyValuePair<DateTime, int>(dateX, random.Next(250, 300)));
                list6.Add(new KeyValuePair<DateTime, int>(dateX, random.Next(400, 430)));
                dateX = dateX.AddSeconds(1);
                Console.WriteLine(dateX);
            }));
        }
    }
}
