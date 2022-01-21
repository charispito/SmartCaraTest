using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SmartCaraTest
{
    /// <summary>
    /// GraphWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GraphWindow : Window
    {
        private Timer timer = new Timer();
        private Random random = new Random();
        ObservableCollection<KeyValuePair<int, float>> list1 = new ObservableCollection<KeyValuePair<int, float>>();
        ObservableCollection<KeyValuePair<int, float>> list2 = new ObservableCollection<KeyValuePair<int, float>>();
        ObservableCollection<KeyValuePair<int, float>> list3 = new ObservableCollection<KeyValuePair<int, float>>();
        ObservableCollection<KeyValuePair<int, float>> list4 = new ObservableCollection<KeyValuePair<int, float>>();
        ObservableCollection<KeyValuePair<int, float>> list5 = new ObservableCollection<KeyValuePair<int, float>>();
        ObservableCollection<KeyValuePair<int, float>> list6 = new ObservableCollection<KeyValuePair<int, float>>();
        ObservableCollection<KeyValuePair<int, float>> list7 = new ObservableCollection<KeyValuePair<int, float>>();
        ObservableCollection<KeyValuePair<int, float>> list8 = new ObservableCollection<KeyValuePair<int, float>>();
        ObservableCollection<KeyValuePair<int, float>> list9 = new ObservableCollection<KeyValuePair<int, float>>();
        ObservableCollection<KeyValuePair<int, float>> list10 = new ObservableCollection<KeyValuePair<int, float>>();
        ObservableCollection<KeyValuePair<int, float>> list11 = new ObservableCollection<KeyValuePair<int, float>>();
        ObservableCollection<KeyValuePair<int, float>> list12 = new ObservableCollection<KeyValuePair<int, float>>();
        public GraphWindow()
        {
            InitializeComponent();
            series1.DataContext = list1;
            series2.DataContext = list2;

            series3.DataContext = list3;
            series4.DataContext = list4;

            series5.DataContext = list5;
            series6.DataContext = list6;

            series7.DataContext = list7;
            series8.DataContext = list8;

            series9.DataContext = list9;
            series10.DataContext = list10;

            series11.DataContext = list11;
            series12.DataContext = list12;

            Loaded += GraphWindow_Loaded;
            Closed += GraphWindow_Closed;
        }

        private void GraphWindow_Closed(object sender, EventArgs e)
        {
            timer.Stop();
            timer.Dispose();
        }

        private void GraphWindow_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                float data = random.Next(1, 200);
                list1.Add(new KeyValuePair<int, float>(list1.Count, data));
                float data2 = random.Next(1, 200);
                list2.Add(new KeyValuePair<int, float>(list2.Count, data2));

                float data3 = random.Next(1, 200);
                list3.Add(new KeyValuePair<int, float>(list3.Count, data3));
                float data4 = random.Next(1, 200);
                list4.Add(new KeyValuePair<int, float>(list4.Count, data4));

                float data5 = random.Next(1, 200);
                list5.Add(new KeyValuePair<int, float>(list5.Count, data5));
                float data6 = random.Next(1, 200);
                list6.Add(new KeyValuePair<int, float>(list6.Count, data6));

                float data7 = random.Next(1, 200);
                list7.Add(new KeyValuePair<int, float>(list7.Count, data7));
                float data8 = random.Next(1, 200);
                list8.Add(new KeyValuePair<int, float>(list8.Count, data8));

                float data9 = random.Next(1, 200);
                list9.Add(new KeyValuePair<int, float>(list9.Count, data9));
                float data10 = random.Next(1, 200);
                list10.Add(new KeyValuePair<int, float>(list10.Count, data10));

                float data11 = random.Next(1, 200);
                list11.Add(new KeyValuePair<int, float>(list11.Count, data11));
                float data12 = random.Next(1, 200);
                list12.Add(new KeyValuePair<int, float>(list12.Count, data12));
            }));
        }
    }
}
