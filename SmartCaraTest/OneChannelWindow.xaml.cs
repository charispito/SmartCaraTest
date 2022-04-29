using SmartCaraTest.controls;
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
    public partial class OneChannelWindow : Window
    {
        private Dictionary<int, int> seriesList = new Dictionary<int, int>();
        private SerialPort port;
        public ObservableCollection<KeyValuePair<DateTime, int>> list1 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public ObservableCollection<KeyValuePair<DateTime, int>> list2 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public ObservableCollection<KeyValuePair<DateTime, int>> list3 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public ObservableCollection<KeyValuePair<DateTime, int>> list4 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public ObservableCollection<KeyValuePair<DateTime, int>> list5 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public ObservableCollection<KeyValuePair<DateTime, int>> list6 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public ObservableCollection<KeyValuePair<DateTime, int>> list7 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public ObservableCollection<KeyValuePair<DateTime, int>> list8 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        private List<byte> receivedData = new List<byte>();
        private Timer timer;

        public OneChannelWindow()
        {
            InitializeComponent();
            setItems(channel);
            Loaded += OneChannelWindow_Loaded;
            port = new SerialPort();
            port.BaudRate = 9600;
            port.DataReceived += Port_DataReceived;
            PortList.box.ItemsSource = SerialPort.GetPortNames();
            ConnectButton.Click += ConnectButton_Click;
            channel.port = port;
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            channel.chartView = Chart;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (port.IsOpen)
            {
                if (!channel.ParameterMode)
                {
                    if (channel.IsNewVersion)
                    {
                        //byte[] command = Protocol.GetNewCommand(1);
                        byte[] command = Protocol.GetNewCommand(1);
                        port.Write(command, 0, command.Length);
                        PrintCommand(command);
                    }
                    else
                    {
                        byte[] command = Protocol.GetCommand(1);
                        port.Write(command, 0, command.Length);
                        Console.WriteLine("command");
                    }
                    
                }
            }
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int Length = port.BytesToRead;
            byte[] data = new byte[Length];
            port.Read(data, 0, Length);
            receivedData.AddRange(data);
            PrintCommand(data);
            if (channel.IsNewVersion)
            {
                if (Length > 0)
                {
                    if (data.Length == 1)
                    {
                        if (data[0] == 0x12)
                            return;
                        else if (data[0] == 0x34)
                        {
                            PrintCommand(receivedData.ToArray());
                            CheckNewDataValid(receivedData.ToArray());
                            receivedData.Clear();
                            return;
                        }
                    }
                    else
                    {
                        if (data[0] == 0x12 && data[data.Length - 1] == 0x34)
                        {
                            PrintCommand(receivedData.ToArray());
                            CheckNewDataValid(receivedData.ToArray());
                            receivedData.Clear();
                        }
                        else if (data[data.Length - 1] == 0x34)
                        {
                            PrintCommand(receivedData.ToArray());
                            CheckNewDataValid(receivedData.ToArray());
                            receivedData.Clear();
                        }
                    }
                }
            }
            else
            {
                if (Length > 0)
                {
                    if (data.Length == 1)
                    {
                        if (data[0] == 0xCC)
                            return;
                        else if (data[0] == 0xEF)
                        {
                            CheckDataValid(receivedData.ToArray());
                            receivedData.Clear();
                            return;
                        }
                    }
                    else
                    {
                        if (data[0] == 0xCC && data[data.Length - 1] == 0xEF)
                        {
                            CheckDataValid(receivedData.ToArray());
                            receivedData.Clear();
                        }
                        else if (data[data.Length - 1] == 0xEF)
                        {
                            CheckDataValid(receivedData.ToArray());
                            receivedData.Clear();
                        }
                    }
                }
            }         
        }

        private void CheckNewDataValid(byte[] array)
        {
            int stx_cnt = 0;
            int etx_cnt = 0;
            List<int> STXIndex = new List<int>();
            List<int> ETXIndex = new List<int>();
            if (array.Length < 4)
            {
                return;
            }
            if (array[3] == array.Length)
            {
                CheckCommand(array);
            }
            else
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] == 0x12)
                    {
                        STXIndex.Add(i);
                        stx_cnt++;
                    }
                    else if (array[i] == 0x34)
                    {
                        ETXIndex.Add(i);
                        etx_cnt++;
                    }
                }
                if (stx_cnt > 1)
                {
                    if (stx_cnt == etx_cnt)
                    {
                        for (int i = 0; i < STXIndex.Count; i++)
                        {
                            ArrayView<byte> command = new ArrayView<byte>(array, STXIndex[i], ETXIndex[i] - STXIndex[i] + 1);
                            if (command[3] == command.Length)
                            {
                                CheckCommand(command.ToArray());
                            }
                            else
                            {
                                Log(string.Format("Data Invalid. Data Length : {0}, Expected Length : {1}", command.Length, command[1]));
                                Log(CommandToString(array));
                            }
                            PrintCommand(command.ToArray());
                        }
                    }
                    else
                    {
                        Console.WriteLine("STX ETX Count Different STX : {0} ETX : {1}", stx_cnt, etx_cnt);
                        Log(string.Format("STX ETX Count Different STX : {0} ETX : {1}", stx_cnt, etx_cnt));
                        Log(CommandToString(array));
                        if (etx_cnt > stx_cnt)
                        {
                            for (int i = 0; i < STXIndex.Count; i++)
                            {
                                ArrayView<byte> command = new ArrayView<byte>(array, STXIndex[i], ETXIndex[i + 1] - STXIndex[i] + 1);
                                PrintCommand(command.ToArray());
                                CheckCommand(command.ToArray());
                            }
                        }
                        else
                        {
                            for (int i = 0; i < ETXIndex.Count; i++)
                            {
                                ArrayView<byte> command = new ArrayView<byte>(array, STXIndex[i], ETXIndex[i] - STXIndex[i] + 1);
                                PrintCommand(command.ToArray());
                                CheckCommand(command.ToArray());
                            }
                        }
                    }
                }
            }
        }

        private void CheckDataValid(byte[] array)
        {
            int stx_cnt = 0;
            int etx_cnt = 0;
            List<int> STXIndex = new List<int>();
            List<int> ETXIndex = new List<int>();
            if(array.Length < 3)
            {
                return;
            }
            if(array[3] == array.Length)
            {
                CheckCommand(array);
            }
            else
            {
                for(int i = 0; i < array.Length; i++)
                {
                    if(array[i] == 0xCC)
                    {
                        STXIndex.Add(i);
                        stx_cnt++;
                    }else if(array[i] == 0xEF)
                    {
                        ETXIndex.Add(i);
                        etx_cnt++;
                    }
                }
                if(stx_cnt > 1)
                {
                    if(stx_cnt == etx_cnt)
                    {
                        for(int i = 0; i < STXIndex.Count; i++)
                        {
                            ArrayView<byte> command = new ArrayView<byte>(array, STXIndex[i], ETXIndex[i] - STXIndex[i] + 1);
                            if(command[3] == command.Length)
                            {
                                CheckCommand(command.ToArray());
                            }
                            else
                            {
                                Log(string.Format("Data Invalid. Data Length : {0}, Expected Length : {1}", command.Length, command[1]));
                                Log(CommandToString(array));
                            }
                            PrintCommand(command.ToArray());
                        }
                    }
                    else
                    {
                        Console.WriteLine("STX ETX Count Different STX : {0} ETX : {1}", stx_cnt, etx_cnt);
                        Log(string.Format("STX ETX Count Different STX : {0} ETX : {1}", stx_cnt, etx_cnt));
                        Log(CommandToString(array));
                        if (etx_cnt > stx_cnt)
                        {
                            for (int i = 0; i < STXIndex.Count; i++)
                            {
                                ArrayView<byte> command = new ArrayView<byte>(array, STXIndex[i], ETXIndex[i + 1] - STXIndex[i] + 1);
                                PrintCommand(command.ToArray());
                                CheckCommand(command.ToArray());
                            }
                        }
                        else
                        {
                            for (int i = 0; i < ETXIndex.Count; i++)
                            {
                                ArrayView<byte> command = new ArrayView<byte>(array, STXIndex[i], ETXIndex[i] - STXIndex[i] + 1);
                                PrintCommand(command.ToArray());
                                CheckCommand(command.ToArray());
                            }
                        }
                    }
                }
            }
        }
        
        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (port.IsOpen)
            {
                port.Close();
                channel.ConnectState = 0;
                ConnectButton.Content = "연결";
                return;
            }
            if(PortList.box.SelectedIndex == -1)
            {
                MessageBox.Show("포트가 선택 되지 않았습니다.");
            }
            else
            {
                port.PortName = PortList.box.SelectedItem.ToString();
                port.Open();
                channel.ConnectState = 1;
                ConnectButton.Content = "해제";
            }
        }

        private void OneChannelWindow_Loaded(object sender, RoutedEventArgs e)
        {
            channel.setHandler(Item1Check_Click);
            channel.ConnectState = 0;
            channel.ChannelView.cont.FontWeight = FontWeights.Black;
        }

        private void setItems(OneChannelValueDetail item)
        {
            item.Item1.label.Content = "히터 온도";
            item.Item2.label.Content = "배기 온도";
            item.Item3.label.Content = "열풍히터온도";
            item.Item4.label.Content = "메인모터운전";
            item.Item5.label.Content = "배기팬 풍량";
            item.Item6.label.Content = "운전 시간";

            item.Item11.label.Content = "히터오프타임";
            item.Item12.label.Content = "배기온도평균";
            item.Item13.label.Content = "열풍히터Duty";
            item.Item14.label.Content = "메인모터전류";
            item.Item15.label.Content = "열풍팬 풍량";
            item.Item16.label.Content = "만수 감지";

            item.Item21.label.Content = "Version";
            item.Item22.label.Content = "Compile";
            item.Item23.label.Content = "상태";
            item.Item24.label.Content = "운전시간";
            item.Item25.label.Content = "Error";
            item.Item26.label.Content = "MODE";

        }

        private int getIndex()
        {
            for (int i = 0; i < 8; i++)
            {
                if (!seriesList.ContainsValue(i))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Item1Check_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Control).Name)
            {
                case "Item1Check":
                    if (channel.Item1Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[10] = index;
                            Chart.setLegend(index, "히터 온도");
                            Chart.seriesList[index].ItemsSource = channel.list1;
                            try
                            {
                                Chart.setAxis(Chart.seriesList[index], 0);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                        }
                        else
                        {
                            channel.Item1Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        channel.list1.Clear();
                        int index = seriesList[10];
                        seriesList.Remove(10);
                        Chart.setLegend(index, "");
                        Chart.seriesList[index].ItemsSource = null;
                    }
                    break;
                case "Item2Check":
                    if (channel.Item2Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[11] = index;
                            Chart.setLegend(index, "배기 온도");
                            Chart.seriesList[index].ItemsSource = channel.list2;
                            Chart.setAxis(Chart.seriesList[index], 1);
                        }
                        else
                        {
                            channel.Item2Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        channel.list2.Clear();
                        int index = seriesList[11];
                        seriesList.Remove(11);
                        Chart.setLegend(index, "");
                        Chart.seriesList[index].ItemsSource = null;
                    }
                    break;
                case "Item3Check":
                    if (channel.Item3Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[12] = index;
                            Chart.setLegend(index, "열풍히터온도");
                            Chart.seriesList[index].ItemsSource = channel.list3;
                            Chart.setAxis(Chart.seriesList[index], 0);
                        }
                        else
                        {
                            channel.Item3Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        channel.list3.Clear();
                        int index = seriesList[12];
                        seriesList.Remove(12);
                        Chart.setLegend(index, "");
                        Chart.seriesList[index].ItemsSource = null;
                    }
                    break;
                case "Item4Check":
                    if (channel.Item4Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[13] = index;
                            Chart.setLegend(index, "메인모터운전");
                            Chart.seriesList[index].ItemsSource = channel.list4;
                            Chart.setAxis(Chart.seriesList[index], 1);
                        }
                        else
                        {
                            channel.Item4Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        channel.list4.Clear();
                        int index = seriesList[13];
                        seriesList.Remove(13);
                        Chart.setLegend(index, "");
                        Chart.seriesList[index].ItemsSource = null;
                    }
                    break;
                case "Item5Check":
                    if (channel.Item5Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[14] = index;
                            Chart.setLegend(index, "히터오프타임");
                            Chart.seriesList[index].ItemsSource = channel.list5;
                            Chart.setAxis(Chart.seriesList[index], 2);
                        }
                        else
                        {
                            channel.Item5Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        channel.list5.Clear();
                        int index = seriesList[14];
                        seriesList.Remove(14);
                        Chart.setLegend(index, "");
                        Chart.seriesList[index].ItemsSource = null;
                    }
                    break;
                case "Item6Check":
                    if (channel.Item6Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[15] = index;
                            Chart.setLegend(index, "배기온도평균");
                            Chart.seriesList[index].ItemsSource = channel.list6;
                            Chart.setAxis(Chart.seriesList[index], 1);
                        }
                        else
                        {
                            channel.Item6Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        channel.list6.Clear();
                        int index = seriesList[15];
                        seriesList.Remove(15);
                        Chart.setLegend(index, "");
                        Chart.seriesList[index].ItemsSource = null;
                    }
                    break;
                case "Item7Check":
                    if (channel.Item7Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[16] = index;
                            Chart.setLegend(index, "열풍히터Duty");
                            Chart.seriesList[index].ItemsSource = channel.list7;
                            Chart.setAxis(Chart.seriesList[index], 1);
                        }
                        else
                        {
                            channel.Item7Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        channel.list7.Clear();
                        int index = seriesList[16];
                        seriesList.Remove(16);
                        Chart.setLegend(index, "");
                        Chart.seriesList[index].ItemsSource = null;
                    }
                    break;
                case "Item8Check":
                    if (channel.Item8Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[17] = index;
                            Chart.setLegend(index, "메인모터전류");
                            Chart.seriesList[index].ItemsSource = channel.list8;
                            Chart.setAxis(Chart.seriesList[index], 2);
                        }
                        else
                        {
                            channel.Item8Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        channel.list8.Clear();
                        int index = seriesList[17];
                        seriesList.Remove(17);
                        Chart.setLegend(index, "");
                        Chart.seriesList[index].ItemsSource = null;
                    }
                    break;
            }
        }        
    }
}
