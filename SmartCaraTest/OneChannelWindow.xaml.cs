using SmartCaraTest.controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


        public OneChannelWindow()
        {
            InitializeComponent();
            setItems(channel);
            DetailView.motor.ValueText11 = "123";
            Loaded += OneChannelWindow_Loaded;
            port = new SerialPort();
            port.BaudRate = 115200;
            PortList.box.ItemsSource = SerialPort.GetPortNames();
            ConnectButton.Click += ConnectButton_Click;
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
                        }
                        else
                        {
                            channel.Item1Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[10];
                        seriesList.Remove(10);
                        Chart.setLegend(index, "");
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
                        }
                        else
                        {
                            channel.Item2Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[11];
                        seriesList.Remove(11);
                        Chart.setLegend(index, "");
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
                        }
                        else
                        {
                            channel.Item3Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[12];
                        seriesList.Remove(12);
                        Chart.setLegend(index, "");
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
                        }
                        else
                        {
                            channel.Item4Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[13];
                        seriesList.Remove(13);
                        Chart.setLegend(index, "");
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
                        }
                        else
                        {
                            channel.Item5Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[14];
                        seriesList.Remove(14);
                        Chart.setLegend(index, "");
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
                        }
                        else
                        {
                            channel.Item6Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[15];
                        seriesList.Remove(15);
                        Chart.setLegend(index, "");
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
                        }
                        else
                        {
                            channel.Item7Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[16];
                        seriesList.Remove(16);
                        Chart.setLegend(index, "");
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
                        }
                        else
                        {
                            channel.Item8Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[17];
                        seriesList.Remove(17);
                        Chart.setLegend(index, "");
                    }
                    break;
            }
        }
    }
}
