using SmartCaraTest.data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SmartCaraTest.controls
{
    public partial class ChannelItem : UserControl
    {
        public StreamWriter streamWriter;
        public bool ParameterMode = false;
        string now = DateTime.Now.ToString("yyyy-MM-dd");
        public bool Response = false;
        public int number = 1;
        public bool IsNewVersion = false;
        public int off_sum = 0;
        public int air_sum = 0;
        public int NonResponse { get; set; } = 0;
        public MultiParameterWindow ParameterWindow { get; set; }
        private bool _run = false;
        public bool run {
            get
            {
                return _run;
            }
            set
            {
                _run = value;
                if (value)
                {
                    Item23.cont.Content = "운전중";
                }
                else
                {
                    Item23.cont.Content = "정지";
                }
            }
        }
        public int ItemIndex = 0;
        public int ParameterCount = 0;
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
            streamWriter.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}", data.date, data.mode, data.remain_time, data.heater_temp, data.heater_off_time,
                data.air_temp, data.fan_speed, data.hot_air_temp, data.hot_air_ontime, data.motor, data.motor_current, number, off_sum, (double)(off_sum / (double)number), air_sum, (double)(air_sum / (double)number));
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
            streamWriter.Close();
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
            if (run)
            {
                MessageBox.Show("이미 시작했습니다.");
                return;
            }
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\ChannelData";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string file = DateTime.Now.ToString("CH" + _Channel + "_yyyy년MM월dd일_HH시mm분ss초") + ".csv";
            streamWriter = new StreamWriter(new FileStream(Path.Combine(path, file), FileMode.CreateNew), System.Text.Encoding.Default);
            initFile();
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

        private void PrintCommand(byte[] command)
        {
            string hex = "";
            foreach (byte b in command)
            {
                hex += " " + b.ToString("X2");
            }
            Console.WriteLine("Length:{0}, Data:{1}", command.Length, hex);
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
            PrintCommand(data);
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
            if (mode == 7)
            {
                if (run)
                {
                    run = false;
                }
            }
            else
            {
                if (!run)
                {
                    run = true;
                }
            }
            int t_hour = data[18];
            int t_min = data[19];
            int t_sec = data[20];
            int runTime = data[21];
            int fan_duty = data[29];
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
            int hot_air_fan_duty = data[30];
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
            for (int i = 0; i < 8; i++)
            {
                if (binary0[i] == 0)
                {
                    errors0[i] = true;
                }
                else
                {
                    errors0[i] = false;
                }
                if (binary1[i] == 0)
                {
                    errors1[i] = true;
                }
                else
                {
                    errors1[i] = false;
                }
            }
            if (errors1[6])
            {
                Item16.cont.Content = "정상";
            }
            else
            {
                Item16.cont.Content = "감지";
            }
            if (run)
            {
                ReadData read = new ReadData()
                {
                    date = now,
                    mode = mode + 1,
                    remain_time = time,
                    heater_temp = heatertemp,
                    heater_off_time = heateroff,
                    air_temp = airtemp,
                    fan_speed = fan_duty,
                    hot_air_temp = airheatertemp,
                    hot_air_ontime = heaterduty,
                    motor = getMotorState(motorRun),
                    motor_current = currnetDouble
                };
                WriteFile(read);
            }
            

            Item21.cont.Content = getModelName(data[48]);
            Item22.cont.Content = micom;
            Item24.cont.Content = time;
            Item1.cont.Content = heatertemp + "ºC";
            Item2.cont.Content = airtemp + "ºC";
            Item3.cont.Content = airheatertemp + "ºC";
            Item5.cont.Content = fan_duty + "%";
            Item15.cont.Content = hot_air_fan_duty + "%";
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
