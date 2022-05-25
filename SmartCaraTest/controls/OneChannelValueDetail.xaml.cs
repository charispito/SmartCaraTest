using SmartCaraTest.data;
using SmartCaraTest.util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace SmartCaraTest.controls
{
    /// <summary>
    /// OneChannelValueDetail.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OneChannelValueDetail : UserControl
    {
        private int _ConnectState = 0;
        public bool ParameterMode = false;
        public ParameterWindow parameterWindow;
        public delegate void OnStart();
        public event OnStart OnTestStart;
        public bool _run = false;
        public bool run {
            get { return _run; }
            set { 
                _run = value;
                if (value)
                {
                    Item23.cont.Content = "운전중";
                }
                else
                {
                    Item23.cont.Content = "대기중";
                }
            }
        }
        public int number = 1;
        public bool IsNewVersion = false;
        public float off_sum = 0;
        public int air_sum = 0;
        public SerialPort port;
        public WPFChartView chartView;
        public StreamWriter streamWriter;
        public ObservableCollection<KeyValuePair<double, int>> list1 = new ObservableCollection<KeyValuePair<double, int>>();
        public ObservableCollection<KeyValuePair<double, int>> list2 = new ObservableCollection<KeyValuePair<double, int>>();
        public ObservableCollection<KeyValuePair<double, int>> list3 = new ObservableCollection<KeyValuePair<double, int>>();
        public ObservableCollection<KeyValuePair<double, int>> list4 = new ObservableCollection<KeyValuePair<double, int>>();
        public ObservableCollection<KeyValuePair<double, int>> list5 = new ObservableCollection<KeyValuePair<double, int>>();
        public ObservableCollection<KeyValuePair<double, int>> list6 = new ObservableCollection<KeyValuePair<double, int>>();
        public ObservableCollection<KeyValuePair<double, int>> list7 = new ObservableCollection<KeyValuePair<double, int>>();
        public ObservableCollection<KeyValuePair<double, double>> list8 = new ObservableCollection<KeyValuePair<double, double>>();
        public int ConnectState
        {
            get { return _ConnectState; }
            set { 
                _ConnectState = value;
                if(value == 0)
                {
                    ChannelView.Text = "연결안됨";
                    ChannelView.cont.Foreground = Brushes.Red;
                }
                else
                {
                    ChannelView.Text = "연결됨";
                    ChannelView.cont.Foreground = Brushes.Blue;
                }
            }
        }
        public OneChannelValueDetail()
        {
            InitializeComponent();
            StartButton.Click += StartButton_Click;
            StopButton.Click += StopButton_Click;
            ParameterButton.Click += ParameterButton_Click;
            ApplyNewVersion.Click += ApplyNewVersion_Click;
            Item1.MouseLeftButtonDown += (s, e) => {
                typeof(System.Windows.Controls.Primitives.ButtonBase).GetMethod("OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(Item1Check, new object[0]);
            };
            Item2.MouseLeftButtonDown += (s, e) => {
                typeof(System.Windows.Controls.Primitives.ButtonBase).GetMethod("OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(Item2Check, new object[0]);
            };
            Item3.MouseLeftButtonDown += (s, e) => {
                typeof(System.Windows.Controls.Primitives.ButtonBase).GetMethod("OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(Item3Check, new object[0]);
            };
            Item4.MouseLeftButtonDown += (s, e) => {
                typeof(System.Windows.Controls.Primitives.ButtonBase).GetMethod("OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(Item4Check, new object[0]);
            };
            Item11.MouseLeftButtonDown += (s, e) => {
                typeof(System.Windows.Controls.Primitives.ButtonBase).GetMethod("OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(Item5Check, new object[0]);
            };
            Item12.MouseLeftButtonDown += (s, e) => {
                typeof(System.Windows.Controls.Primitives.ButtonBase).GetMethod("OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(Item6Check, new object[0]);
            };
            Item13.MouseLeftButtonDown += (s, e) => {
                typeof(System.Windows.Controls.Primitives.ButtonBase).GetMethod("OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(Item7Check, new object[0]);
            };
            Item14.MouseLeftButtonDown += (s, e) => {
                typeof(System.Windows.Controls.Primitives.ButtonBase).GetMethod("OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(Item8Check, new object[0]);
            };
            Loaded += OneChannelValueDetail_Loaded;
        }

        private void OneChannelValueDetail_Loaded(object sender, RoutedEventArgs e)
        {
            //1 2 6 3 8 5 7 4
            typeof(System.Windows.Controls.Primitives.ButtonBase).GetMethod("OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(Item1Check, new object[0]);
            typeof(System.Windows.Controls.Primitives.ButtonBase).GetMethod("OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(Item2Check, new object[0]);
            typeof(System.Windows.Controls.Primitives.ButtonBase).GetMethod("OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(Item6Check, new object[0]);
            typeof(System.Windows.Controls.Primitives.ButtonBase).GetMethod("OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(Item3Check, new object[0]);
            typeof(System.Windows.Controls.Primitives.ButtonBase).GetMethod("OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(Item8Check, new object[0]);
            typeof(System.Windows.Controls.Primitives.ButtonBase).GetMethod("OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(Item5Check, new object[0]);
            typeof(System.Windows.Controls.Primitives.ButtonBase).GetMethod("OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(Item7Check, new object[0]);
            typeof(System.Windows.Controls.Primitives.ButtonBase).GetMethod("OnClick", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(Item4Check, new object[0]);
        }

        private void ApplyNewVersion_Click(object sender, RoutedEventArgs e)
        {
            IsNewVersion = (sender as CheckBox).IsChecked.Value;
        }

        private void ParameterButton_Click(object sender, RoutedEventArgs e)
        {
            if(port != null && port.IsOpen && !run)
            {
                parameterWindow = new ParameterWindow(port, this);
                parameterWindow.Show();
            }
            
        }

        private void initFile()
        {
            air_sum = 0;
            off_sum = 0;
            number = 0;
            streamWriter.WriteLine("날짜,모드,남은 시간,히터 온도,히터 오프타임,배기온도,FAN Speed," +
                "열풍온도,열풍온타임,MOTOR,모터 전류,번호,오프타임합,오프평균,배기 합,배기 평균");
        }

        public void WriteFile(ReadData data)
        {
            number++;
            air_sum += data.air_temp;
            off_sum += data.heater_off_time;
            if(streamWriter != null)
            {
                streamWriter.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}", data.date, data.mode, data.remain_time, data.heater_temp, data.heater_off_time,
                data.air_temp, data.fan_speed, data.hot_air_temp, data.hot_air_ontime, data.motor, data.motor_current, number, off_sum, (double)(off_sum / (double)number), air_sum, (double)(air_sum / (double)number));
            }
            else
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ChannelData";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string file = DateTime.Now.ToString("싱글yyyy년MM월dd일_HH시mm분ss초") + ".csv";
                streamWriter = new StreamWriter(new FileStream(Path.Combine(path, file), FileMode.CreateNew), System.Text.Encoding.Default);
                initFile();
            }

        }

        public void setHandler(RoutedEventHandler handler)
        {
            Item1Check.Click += handler;
            Item2Check.Click += handler;
            Item3Check.Click += handler;
            Item4Check.Click += handler;
            Item5Check.Click += handler;
            Item6Check.Click += handler;
            Item7Check.Click += handler;
            Item8Check.Click += handler;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!port.IsOpen)
            {
                MessageBox.Show("연결 되지 않았습니다.");
                return;
            }
            if (run)
            {
                MessageBox.Show("이미 운전중입니다.");
                return;
            }
            OnTestStart();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ChannelData";
            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string file = DateTime.Now.ToString("싱글yyyy년MM월dd일_HH시mm분ss초") + ".csv";
            streamWriter = new StreamWriter(new FileStream(Path.Combine(path, file), FileMode.CreateNew), System.Text.Encoding.Default);
            initFile();
            if (IsNewVersion)
            {
                byte[] command = Protocol.GetNewCommand(2);
                port.Write(command, 0, command.Length);
            }
            else
            {
                byte[] command = Protocol.GetCommand(2);
                port.Write(command, 0, command.Length);
            }

            list1.Clear();
            list2.Clear();
            list3.Clear();
            list4.Clear();
            list5.Clear();
            list6.Clear();
            list7.Clear();
            list8.Clear();

            if (chartView != null)
            {
                chartView.initXAxis();
                chartView.ItemRun[0] = true;
            }
        }
        //SMARTCARA-IPTIME1
        //SMARTCARA-IPTIME2
        //SMARTCARA-IPTIME3

        

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (!port.IsOpen)
            {
                MessageBox.Show("연결 되지 않았습니다.");
                return;
            }
            if (streamWriter != null)
            streamWriter.Close();
            streamWriter = null;
            //chartView.ItemRun[0] = false;
            if (IsNewVersion)
            {
                byte[] command = Protocol.GetNewCommand(3);
                port.Write(command, 0, command.Length);
            }
            else
            {
                byte[] command = Protocol.GetCommand(3);
                port.Write(command, 0, command.Length);
            }
            //client.GetStream().Write(command, 0, command.Length);
        }
    }
}
