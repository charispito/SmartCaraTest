using SmartCaraTest.data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SmartCaraTest.controls
{
    public partial class ChannelItem : UserControl
    {
        public bool ParameterMode = false;
        public bool Response = false;
        public bool IsNewVersion = false;
        public int NonResponse { get; set; } = 0;
        public MultiParameterWindow ParameterWindow { get; set; }
        public bool run = false;
        public int ItemIndex = 0;
        public WPFChartView chartView { get; set; }
        public ObservableCollection<KeyValuePair<DateTime, int>> list1 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public ObservableCollection<KeyValuePair<DateTime, int>> list2 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public ObservableCollection<KeyValuePair<DateTime, int>> list3 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public ObservableCollection<KeyValuePair<DateTime, int>> list4 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public ObservableCollection<KeyValuePair<DateTime, int>> list5 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public ObservableCollection<KeyValuePair<DateTime, int>> list6 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public ObservableCollection<KeyValuePair<DateTime, int>> list7 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public ObservableCollection<KeyValuePair<DateTime, double>> list8 = new ObservableCollection<KeyValuePair<DateTime, double>>();
        public TcpClient client { get; set; }
        public long TimeMills { get; set; }
        private int _Channel;

        public void clearData()
        {
            list1.Clear();
            list2.Clear();
            list3.Clear();
            list4.Clear();
            list5.Clear();
            list6.Clear();
            list7.Clear();
            list8.Clear();
            NonResponse = 0;
            Item1.cont.Content = "";
            Item2.cont.Content = "";
            Item3.cont.Content = "";
            Item4.cont.Content = "";
            Item5.cont.Content = "";
            Item6.cont.Content = "";
            Item11.cont.Content = "";
            Item12.cont.Content = "";
            Item13.cont.Content = "";
            Item14.cont.Content = "";
            Item15.cont.Content = "";
            Item16.cont.Content = "";
            Item21.cont.Content = "";
            Item22.cont.Content = "";
            Item23.cont.Content = "";
            Item24.cont.Content = "";
            Item25.cont.Content = "";
            Item26.cont.Content = "";
            ChannelView.cont.Content = "";
        }

        public int Channel {
            get { return _Channel; }
            set
            {
                _Channel = value;
                Dispatcher.Invoke(new Action(() => { ChannelView.cont.Content = value; }));
            }
        }
        
        public ChannelItem()
        {
            InitializeComponent();
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
            StartButton.Click += StartButton_Click;
            StopButton.Click += StopButton_Click;
            ParameterButton.Click += ParameterButton_Click;
            ApplyNewVersion.Click += ApplyNewVersion_Click;            
        }

        private void ApplyNewVersion_Click(object sender, RoutedEventArgs e)
        {
            IsNewVersion = (sender as CheckBox).IsChecked.Value;
        }

        private void ParameterButton_Click(object sender, RoutedEventArgs e)
        {
            ParameterMode = true;
            if (client.GetStream().DataAvailable)
            {
                byte[] buffer = new byte[57];
                client.GetStream().Read(buffer, 0, buffer.Length);
            }
            Task.Delay(200).ContinueWith(_ => 
            {
                Dispatcher.BeginInvoke(new Action(() => {
                    ParameterWindow = new MultiParameterWindow(this);
                    ParameterWindow.Show();
                }));
            });
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            run = false;
            chartView.ItemRun[ItemIndex] = false;
            byte[] command = null;
            if (IsNewVersion)
            {
                command = Protocol.GetNewCommand(3);
            }
            else
            {
                command = Protocol.GetCommand(3);
            }
            client.GetStream().Write(command, 0, command.Length);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

            byte[] command = null;
            if (IsNewVersion)
            {
                command = Protocol.GetNewCommand(2);
            }
            else
            {
                command = Protocol.GetCommand(2);
            }
            client.GetStream().Write(command, 0, command.Length);
            list1.Clear();
            list2.Clear();
            list3.Clear();
            list4.Clear();
            list5.Clear();
            list6.Clear();
            list7.Clear();
            list8.Clear();
            run = true;
            if (chartView != null)
            {
                chartView.initXAxis();
                chartView.ItemRun[ItemIndex] = true;
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

        public void SetView(byte[] data)
        {
            Console.WriteLine("set");
            if (data.Length < 57)
                return;
            if (data[3] != 57)
                return;
            if (IsNewVersion)
            {
                if (data[0] != 0x12)
                    return;
                if (data.Last() != 0x34)
                    return;
            }
            else
            {
                if (data[0] != 0xCC)
                    return;
                if (data.Last() != 0xEF)
                    return;
            }
            Console.WriteLine("view");
            int motorRun = data[5];
            int heateroff = data[7]; //히터 오프타임
            int heatertemp = data[6]; //히터 온도
            int airtemp = data[8]; //배기 온도
            int airaverage = data[9];
            int airheatertemp = data[10]; //열풍 온도
            int heaterduty = data[11];
            int mode = data[15];
            int minute = data[16];
            int second = data[17];

            int t_hour = data[18];
            int t_min = data[19];
            int t_sec = data[20];
            int runTime = data[21];
            string time = string.Format("{0:D2}:{1:D2}", minute, second);
            string t_time = string.Format("{0:D2}:{1:D2}:{2:D2}", t_hour, t_min, t_sec);
            byte[] current = { 0, 0, data[13], data[14] };
            Array.Reverse(current);
            int currentInt = BitConverter.ToInt32(current, 0) * 10;
            double currnetDouble = (double)currentInt / 1000.0;
            int year = data[51];
            int month = data[52];
            int day = data[53];
            int version = data[54];
            string micom = $"{year}.{month}.{day}. ver {version}";
            byte error0 = data[49];
            byte error1 = data[50];
            int[] binary0 = Enumerable.Range(1, 8).Select(i => error0 / (1 << (8 - i)) % 2).ToArray();
            int[] binary1 = Enumerable.Range(1, 8).Select(i => error1 / (1 << (8 - i)) % 2).ToArray();
            Array.Reverse(binary0);
            Array.Reverse(binary1);
            //5 = 0101 RUN CCW
            //9 = 1001 RUN STOP
            //3 = 0011 RUN CW
            //8 = 1000 STOP 
            //2 = 0010 CW
            //4 = 0100 CCW
            bool[] errors0 = new bool[8];
            bool[] errors1 = new bool[8];
            int motorRunTime = data[12];
            Item21.cont.Content = getModelName(data[48]);
            Item22.cont.Content = micom;
            Item24.cont.Content = time;
            Item1.cont.Content = heatertemp + "ºC";
            Item2.cont.Content = airtemp + "ºC";
            Item3.cont.Content = airheatertemp + "ºC";
            Item11.cont.Content = heateroff;
            Item12.cont.Content = airaverage + "ºC";
            Item13.cont.Content = heaterduty;
            Item14.cont.Content = currnetDouble + "A";
            Item6.cont.Content = t_time;
            Item4.Title = getMotorState(motorRun);
            Item4.cont.Content = motorRunTime.ToString() + "s";
            Item26.cont.Content = (mode + 1).ToString();
            if (run)
            {
                if (Item1Check.IsChecked.Value)
                {
                    list1.Add(new KeyValuePair<DateTime, int>(DateTime.Now, heatertemp));
                }
                if (Item2Check.IsChecked.Value)
                {
                    list2.Add(new KeyValuePair<DateTime, int>(DateTime.Now, airtemp));
                }
                if (Item3Check.IsChecked.Value)
                {
                    list3.Add(new KeyValuePair<DateTime, int>(DateTime.Now, airheatertemp));
                }
                if (Item4Check.IsChecked.Value)
                {
                    list4.Add(new KeyValuePair<DateTime, int>(DateTime.Now, getMotorValue(motorRun)));
                }
                if (Item5Check.IsChecked.Value)
                {
                    list5.Add(new KeyValuePair<DateTime, int>(DateTime.Now, heateroff));
                }
                if (Item6Check.IsChecked.Value)
                {
                    list6.Add(new KeyValuePair<DateTime, int>(DateTime.Now, airaverage));
                }
                if (Item7Check.IsChecked.Value)
                {
                    list7.Add(new KeyValuePair<DateTime, int>(DateTime.Now, heaterduty));
                }
                if (Item8Check.IsChecked.Value)
                {
                    list8.Add(new KeyValuePair<DateTime, double>(DateTime.Now, currnetDouble));
                }
            }
        }

        private string getModelName(int model)
        {
            switch (model)
            {
                case 1:
                    return "PCS 500";
                case 2:
                    return "PCS 350";
                default:
                    return "";
            }
        }

        private int getMotorValue(int run)
        {
            switch (run)
            {
                case 2:
                    return 75;
                case 3:
                    return 75;
                case 5:
                    return 25;
                case 8:
                    return 50;
                case 9:
                    return 50;
                default:
                    return 0;
            }
        }

        private string getMotorState(int run)
        {
            switch (run)
            {
                case 2:
                    return "CW";
                case 3:
                    return "CW";
                case 4:
                    return "CCW";
                case 5:
                    return "CCW";
                case 8:
                    return "STOP";
                case 9:
                    return "STOP";
                default:
                    return "";
            }
        }
    }
}
