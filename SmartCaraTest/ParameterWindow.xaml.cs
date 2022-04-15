using SmartCaraTest.controls;
using SmartCaraTest.data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace SmartCaraTest
{
    /// <summary>
    /// ParameterWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ParameterWindow : Window
    {
        private ChannelItem channelItem = null;
        private List<SettingData> mode1 = new List<SettingData>();
        private List<SettingData> mode2 = new List<SettingData>();
        private List<SettingData> mode3 = new List<SettingData>();
        private List<SettingData> mode4 = new List<SettingData>();
        private List<SettingData> mode5 = new List<SettingData>();
        private List<SettingData> motor = new List<SettingData>();

        private List<SettingData> mode11 = new List<SettingData>();
        private List<SettingData> mode12 = new List<SettingData>();
        private List<SettingData> mode13 = new List<SettingData>();
        private List<SettingData> mode14 = new List<SettingData>();
        private List<SettingData> mode15 = new List<SettingData>();
        private List<SettingData> motor1 = new List<SettingData>();

        public ParameterWindow()
        {
            InitializeComponent();
            Initialize();
        }

        public ParameterWindow(ChannelItem channelItem)
        {
            InitializeComponent();
            this.channelItem = channelItem;
            Initialize();
        }

        private void Initialize()
        {
            ObservableCollection<SettingData> list = new ObservableCollection<SettingData>();
            list.Add(new SettingData() { Name = "에러 내용" });
            list.Add(new SettingData() { Name = "운전 모드" });
            list.Add(new SettingData() { Name = "히터 온도" });
            list.Add(new SettingData() { Name = "히터 오프 타임" });
            list.Add(new SettingData() { Name = "배기 온도" });
            list.Add(new SettingData() { Name = "열풍 온도" });
            list.Add(new SettingData() { Name = "열풍 On Time" });
            list.Add(new SettingData() { Name = "운전 횟수" });
            ErrorGrid.ItemsSource = list;
            MicomGrid.ItemsSource = getModeData();
            MicomGrid2.ItemsSource = getModeData();
            MicomGrid3.ItemsSource = getModeData();
            MicomGrid4.ItemsSource = getModeData();
            MicomGrid5.ItemsSource = getModeData();
            MotorGrid.ItemsSource = getMotorData();
            SetGrid1.ItemsSource = getModeData();
            SetGrid2.ItemsSource = getModeData();
            SetGrid3.ItemsSource = getModeData();
            SetGrid4.ItemsSource = getModeData();
            SetGrid5.ItemsSource = getModeData();
            SetGrid6.ItemsSource = getMotorData();
            HeaterGrid.ItemsSource = getHeaterData();
            FanGrid.ItemsSource = getFanData();
            HeaterGrid2.ItemsSource = getHeaterData2();
            HeaterGridSetting.ItemsSource = getHeaterData();
            FanGrid2.ItemsSource = getFanData();
            HeaterGridSetting2.ItemsSource = getHeaterData2();
            ComplieGrid.ItemsSource = getCompileData(2022, 1, 1, 0);
            Loaded += ParameterWindow_Loaded;
            Closed += ParameterWindow_Closed;
            RightButton.Click += RightButton_Click;
            ReadParamButton.Click += ReadParamButton_Click;
        }

        private void ReadParamButton_Click(object sender, RoutedEventArgs e)
        {
            byte[] command = Protocol.GetParameter(false);
            channelItem.client.GetStream().Write(command, 0, command.Length);
            channelItem.client.GetStream().Flush();

        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            mode11.Clear();
            mode12.Clear();
            mode13.Clear();
            mode14.Clear();
            mode15.Clear();
            motor1.Clear();
        }

        private void ParameterWindow_Closed(object sender, EventArgs e)
        {
            channelItem.ParameterMode = false;
        }

        private void ParameterWindow_Loaded(object sender, RoutedEventArgs e)
        {
            channelItem.ParameterMode = true;
            byte[] command = Protocol.GetParameter(false);
            channelItem.client.GetStream().Write(command, 0, command.Length);
        }

        public void setParameter(byte[] data)
        {
            if(data.Length != 70)
            {
                return;
            }
            int onTimeCW1 = data[4];
            int offTimeCW1 = data[5];
            int onTimeCCW1 = data[6];
            int offTimeCCW1 = data[7];
            int HeaterTemp1 = data[8];
            int HeaterOffTime1 = data[9];
            int VentileTemp1 = data[10];
            int OperateTime1 = data[11];
            int ExhaustFanWaitMode = data[12];

            mode1.Add(new SettingData() { Name = "ON TIME CW", Value = onTimeCW1 });
            mode1.Add(new SettingData() { Name = "OFF TIME CW", Value = offTimeCW1 });
            mode1.Add(new SettingData() { Name = "ON TIME CCW", Value = onTimeCCW1 });
            mode1.Add(new SettingData() { Name = "OFF TIME CCW", Value = offTimeCCW1 });
            mode1.Add(new SettingData() { Name = "HEATER TEMP", Value = HeaterTemp1 });
            mode1.Add(new SettingData() { Name = "HEATER OFF TIME", Value = HeaterOffTime1 });
            mode1.Add(new SettingData() { Name = "VENTILE TEMP", Value = VentileTemp1 });
            mode1.Add(new SettingData() { Name = "OPERATE TIME", Value = OperateTime1 });

            int onTimeCW2 = data[14];
            int offTimeCW2 = data[15];
            int onTimeCCW2 = data[16];
            int offTimeCCW2 = data[17];
            int HeaterTemp2 = data[18];
            int HeaterOffTime2 = data[19];
            int VentileTemp2 = data[20];
            int OperateTime2 = data[21];
            int ExhaustFanOperateMode = data[22];

            mode2.Add(new SettingData() { Name = "ON TIME CW", Value = onTimeCW2 });
            mode2.Add(new SettingData() { Name = "OFF TIME CW", Value = offTimeCW2 });
            mode2.Add(new SettingData() { Name = "ON TIME CCW", Value = onTimeCCW2 });
            mode2.Add(new SettingData() { Name = "OFF TIME CCW", Value = offTimeCCW2 });
            mode2.Add(new SettingData() { Name = "HEATER TEMP", Value = HeaterTemp2 });
            mode2.Add(new SettingData() { Name = "HEATER OFF TIME", Value = HeaterOffTime2 });
            mode2.Add(new SettingData() { Name = "VENTILE TEMP", Value = VentileTemp2 });
            mode2.Add(new SettingData() { Name = "OPERATE TIME", Value = OperateTime2 });

            int onTimeCW3 = data[24];
            int offTimeCW3 = data[25];
            int onTimeCCW3 = data[26];
            int offTimeCCW3 = data[27];
            int HeaterTemp3 = data[28];
            int HeaterOffTime3 = data[29];
            int VentileTemp3 = data[30];
            int OperateTime3 = data[31];

            mode3.Add(new SettingData() { Name = "ON TIME CW", Value = onTimeCW3 });
            mode3.Add(new SettingData() { Name = "OFF TIME CW", Value = offTimeCW3 });
            mode3.Add(new SettingData() { Name = "ON TIME CCW", Value = onTimeCCW3 });
            mode3.Add(new SettingData() { Name = "OFF TIME CCW", Value = offTimeCCW3 });
            mode3.Add(new SettingData() { Name = "HEATER TEMP", Value = HeaterTemp3 });
            mode3.Add(new SettingData() { Name = "HEATER OFF TIME", Value = HeaterOffTime3 });
            mode3.Add(new SettingData() { Name = "VENTILE TEMP", Value = VentileTemp3 });
            mode3.Add(new SettingData() { Name = "OPERATE TIME", Value = OperateTime3 });

            int onTimeCW4 = data[34];
            int offTimeCW4 = data[35];
            int onTimeCCW4 = data[36];
            int offTimeCCW4 = data[37];
            int HeaterTemp4 = data[38];
            int HeaterOffTime4 = data[39];
            int VentileTemp4 = data[40];
            int OperateTime4 = data[41];

            mode4.Add(new SettingData() { Name = "ON TIME CW", Value = onTimeCW4 });
            mode4.Add(new SettingData() { Name = "OFF TIME CW", Value = offTimeCW4 });
            mode4.Add(new SettingData() { Name = "ON TIME CCW", Value = onTimeCCW4 });
            mode4.Add(new SettingData() { Name = "OFF TIME CCW", Value = offTimeCCW4 });
            mode4.Add(new SettingData() { Name = "HEATER TEMP", Value = HeaterTemp4 });
            mode4.Add(new SettingData() { Name = "HEATER OFF TIME", Value = HeaterOffTime4 });
            mode4.Add(new SettingData() { Name = "VENTILE TEMP", Value = VentileTemp4 });
            mode4.Add(new SettingData() { Name = "OPERATE TIME", Value = OperateTime4 });

            int onTimeCW5 = data[44];
            int offTimeCW5 = data[45];
            int onTimeCCW5 = data[46];
            int offTimeCCW5 = data[47];
            int HeaterTemp5 = data[48];
            int HeaterOffTime5 = data[49];
            int VentileTemp5 = data[50];
            int OperateTime5 = data[51];

            mode5.Add(new SettingData() { Name = "ON TIME CW", Value = onTimeCW5 });
            mode5.Add(new SettingData() { Name = "OFF TIME CW", Value = offTimeCW5 });
            mode5.Add(new SettingData() { Name = "ON TIME CCW", Value = onTimeCCW5 });
            mode5.Add(new SettingData() { Name = "OFF TIME CCW", Value = offTimeCCW5 });
            mode5.Add(new SettingData() { Name = "HEATER TEMP", Value = HeaterTemp5 });
            mode5.Add(new SettingData() { Name = "HEATER OFF TIME", Value = HeaterOffTime5 });
            mode5.Add(new SettingData() { Name = "VENTILE TEMP", Value = VentileTemp5 });
            mode5.Add(new SettingData() { Name = "OPERATE TIME", Value = OperateTime5 });

            int motor1 = data[54];
            int motor2 = data[55];
            int motor3 = data[56];
            int motor4 = data[57];
            int motor5 = data[58];
            motor.Add(new SettingData() { Name = "이물질 감지 시간", Value = motor1 });
            motor.Add(new SettingData() { Name = "이물질 감지 전류", Value = motor2 });
            motor.Add(new SettingData() { Name = "이물질 감지 횟수", Value = motor3 });
            motor.Add(new SettingData() { Name = "과부하 감지 전류", Value = motor4 });
            motor.Add(new SettingData() { Name = "과부하 감지 횟수", Value = motor5 });
            Dispatcher.BeginInvoke(new Action(() => 
            {
                MicomGrid.ItemsSource = mode1;
                MicomGrid2.ItemsSource = mode2;
                MicomGrid3.ItemsSource = mode3;
                MicomGrid4.ItemsSource = mode4;
                MicomGrid5.ItemsSource = mode5;
                MotorGrid.ItemsSource = motor;
            }));
        }

        public void setError(byte[] data)
        {
            //int motor01 = data[4];
            //int motor02 = data[5];
            int runmode0 = data[6];
            int heaterTemp0 = data[7];
            int heaterofftime0 = data[8];
            int exhaustTemp0 = data[9];
            int hotWindTemp0 = data[10];
            int hotWindOnTime0 = data[11];

            int runmode1 = data[6];
            int heaterTemp1 = data[7];
            int heaterofftime1 = data[8];
            int exhaustTemp1 = data[9];
            int hotWindTemp1 = data[10];
            int hotWindOnTime1 = data[11];

            int runmode2 = data[6];
            int heaterTemp2 = data[7];
            int heaterofftime2 = data[8];
            int exhaustTemp2 = data[9];
            int hotWindTemp2 = data[10];
            int hotWindOnTime2 = data[11];

            int runmode3 = data[6];
            int heaterTemp3 = data[7];
            int heaterofftime3 = data[8];
            int exhaustTemp3 = data[9];
            int hotWindTemp3 = data[10];
            int hotWindOnTime3 = data[11];

            int runmode4 = data[6];
            int heaterTemp4 = data[7];
            int heaterofftime4 = data[8];
            int exhaustTemp4 = data[9];
            int hotWindTemp4 = data[10];
            int hotWindOnTime4 = data[11];

            ObservableCollection<SettingData> list = new ObservableCollection<SettingData>();
            list.Add(new SettingData() { Name = "에러 내용"});
            list.Add(new SettingData() { Name = "운전 모드", Value = runmode0, Value2 = runmode1, Value3 = runmode2, Value4 = runmode3, Value5 = runmode4 });
            list.Add(new SettingData() { Name = "히터 온도", Value = heaterTemp0, Value2 = heaterTemp1, Value3 = heaterTemp2, Value4 = heaterTemp3, Value5 = heaterTemp4 });
            list.Add(new SettingData() { Name = "히터 오프 타임", Value = heaterofftime0, Value2 = heaterofftime1, Value3 = heaterofftime2, Value4 = heaterofftime3, Value5 = heaterofftime4 });
            list.Add(new SettingData() { Name = "배기 온도", Value = exhaustTemp0, Value2 = exhaustTemp1, Value3 = exhaustTemp2, Value4 = exhaustTemp3, Value5 = exhaustTemp4 });
            list.Add(new SettingData() { Name = "열풍 온도", Value = hotWindTemp0, Value2 = hotWindTemp1, Value3 = hotWindTemp2, Value4 = hotWindTemp3, Value5 = hotWindTemp4 });
            list.Add(new SettingData() { Name = "열풍 On Time", Value = hotWindOnTime0, Value2 = hotWindOnTime1, Value3 = hotWindOnTime2, Value4 = hotWindOnTime3, Value5 = hotWindOnTime4 });
            list.Add(new SettingData() { Name = "운전 횟수" });
            Dispatcher.BeginInvoke(new Action(() => 
            {
                ErrorGrid.ItemsSource = list;
            }));
        }

        public ParameterWindow(int channel)
        {
            InitializeComponent();
            Initialize();
        }
        private ObservableCollection<SettingData> getModeData()
        {
            ObservableCollection<SettingData> list = new ObservableCollection<SettingData>();
            list.Add(new SettingData() { Name = "ON TIME CW" });
            list.Add(new SettingData() { Name = "OFF TIME CW" });
            list.Add(new SettingData() { Name = "ON TIME CCW" });
            list.Add(new SettingData() { Name = "OFF TIME CCW" });
            list.Add(new SettingData() { Name = "HEATER TEMP" });
            list.Add(new SettingData() { Name = "HEATER OFF TIME" });
            list.Add(new SettingData() { Name = "VENTILE TEMP" });
            list.Add(new SettingData() { Name = "OPERATE TIME" });
            return list;
        }

        private ObservableCollection<SettingData> getMotorData()
        {
            ObservableCollection<SettingData> list = new ObservableCollection<SettingData>();
            list.Add(new SettingData() { Name = "이물질 감지 시간" });
            list.Add(new SettingData() { Name = "이물질 감지 전류" });
            list.Add(new SettingData() { Name = "이물질 감지 횟수" });
            list.Add(new SettingData() { Name = "과부화 감지 전류" });
            list.Add(new SettingData() { Name = "과부하 감지 횟수" });
            return list;
        }

        private ObservableCollection<SettingData> getHeaterData()
        {
            ObservableCollection<SettingData> list = new ObservableCollection<SettingData>();
            list.Add(new SettingData() { Name = "열풍 온도 Step1" });
            list.Add(new SettingData() { Name = "열풍 온도 Step2" });
            list.Add(new SettingData() { Name = "열풍 온도 Step3" });
            list.Add(new SettingData() { Name = "열풍 온도 Step4" });
            list.Add(new SettingData() { Name = "열풍 온도 Step5" });
            list.Add(new SettingData() { Name = "열풍 온도 Step6" });
            return list;
        }

        private ObservableCollection<SettingData> getFanData()
        {
            ObservableCollection<SettingData> list = new ObservableCollection<SettingData>();
            list.Add(new SettingData() { Name = "배기 FAN 대기 온도" });
            list.Add(new SettingData() { Name = "배기 FAN 운전 온도" });
            return list;
        }

        private ObservableCollection<SettingData> getHeaterData2()
        {
            ObservableCollection<SettingData> list = new ObservableCollection<SettingData>();
            list.Add(new SettingData() { Name = "열풍 FAN SPEED" });
            list.Add(new SettingData() { Name = "열풍 히터 온도" });
            return list;
        }

        private ObservableCollection<CompileData> getCompileData(int year, int month, int day, int ver)
        {
            ObservableCollection<CompileData> list = new ObservableCollection<CompileData>();
            list.Add(new CompileData { Year = year, Month = month, Day = day, Ver = ver });
            return list;
        }
    }
}
