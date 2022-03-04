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
        private List<SettingData> error1 = new List<SettingData>();
        private List<SettingData> error2 = new List<SettingData>();
        private List<SettingData> error3 = new List<SettingData>();
        private List<SettingData> error4 = new List<SettingData>();
        private List<SettingData> error5 = new List<SettingData>();
        private List<SettingData> mode1 = new List<SettingData>();
        private List<SettingData> mode2 = new List<SettingData>();
        private List<SettingData> mode3 = new List<SettingData>();
        private List<SettingData> mode4 = new List<SettingData>();
        private List<SettingData> mode5 = new List<SettingData>();

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

        }

        public void setError(byte[] data)
        {

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
