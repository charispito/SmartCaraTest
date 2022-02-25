using SmartCaraTest.controls;
using SmartCaraTest.data;
using SmartCaraTest.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SmartCaraTest
{
    /// <summary>
    /// MultiWindow1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MultiWindow1 : Window
    {
        private MultiWindow2 multiWindow2;
        private int count = 0;
        private Dictionary<int, int> seriesList = new Dictionary<int, int>();
        private AsyncServer server;
        private Dictionary<int, int> channelList = new Dictionary<int, int>();

        public MultiWindow1()
        {
            InitializeComponent();
            Loaded += MultiWindow1_Loaded;
            setItems(Item1);
            setItems(Item2);
            setItems(Item3);
            setItems(Item4);
            server = new AsyncServer(this);
            StartButton.Click += (s, e) =>
            {
                if (server.run)
                {
                    MessageBox.Show("이미 작동 중 입니다.");
                    return;
                }
                server.Start();
            };
            StopButton.Click += (s, e) =>
            {
                if (ClientManager.clientDic.Count > 0)
                {
                    Task.Run(new Action(() =>
                    {
                        foreach (var item in ClientManager.clientDic)
                        {
                            if (item.Value.client != null && item.Value.client.Connected)
                            {
                                byte[] cmd = Protocol.GetCommand(3);
                                item.Value.client.GetStream().Write(cmd, 0, cmd.Length);
                                item.Value.client.Close();
                            }
                        }
                    }));
                }
                if (server.listener != null)
                {
                    server.listener.Stop();
                }
                server.run = false;
            };
        }

        public void OnDataReceived(ClientData data)
        {
            byte[] packet = data.readCompleteData.ToArray();
            data.ResponseCount++;
            data.channel.NonResponse = 0;
            data.readCompleteData.Clear();
            Dispatcher.BeginInvoke(new Action(() =>
            {

            }));
        }

        public void SetView(byte[] data, ChannelItem channel)
        {
            if (data.Length != 57)
                return;
            if (data[0] != 0xCC)
                return;
            if (data.Last() != 0xEF)
                return;
            int heateroff = data[7]; //히터 오프타임
            int heatertemp = data[6]; //히터 온도
            int airtemp = data[8]; //배기 온도
            int airaverage = data[9];
            int airheatertemp = data[10]; //열풍 온도
            int heaterduty = data[11];

            int minute = data[16];
            int second = data[17];

            int t_hour = data[18];
            int t_min = data[19];
            int t_sec = data[20];
            string time = string.Format("{0:D2}:{1:D2}", minute, second);
            string t_time = string.Format("{0:D2}:{1:D2}:{2:D2}",t_hour, t_min, t_sec);
            byte[] current = { 0, 0, data[13], data[14] };
            Array.Reverse(current);
            int currentInt = BitConverter.ToInt32(current, 0) * 10;
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
            bool[] errors0 = new bool[8];
            bool[] errors1 = new bool[8];
            channel.Item21.cont.Content = micom;
            channel.Item24.cont.Content = time;
            channel.Item1.cont.Content = heatertemp;
            channel.Item2.cont.Content = airtemp;
            channel.Item3.cont.Content = airheatertemp;
            channel.Item11.cont.Content = heateroff;
            channel.Item12.cont.Content = airaverage;
            channel.Item13.cont.Content = heaterduty;
            channel.Item14.cont.Content = currentInt;
            channel.Item6.cont.Content = t_time;
            channel.list1.Add(new KeyValuePair<DateTime, int>(DateTime.Now, heatertemp));
            channel.list2.Add(new KeyValuePair<DateTime, int>(DateTime.Now, airtemp));
            channel.list3.Add(new KeyValuePair<DateTime, int>(DateTime.Now, airheatertemp));
            //channel.list3.Add(new KeyValuePair<DateTime, int>(DateTime.Now, airheatertemp));
            channel.list4.Add(new KeyValuePair<DateTime, int>(DateTime.Now, heateroff));
            channel.list5.Add(new KeyValuePair<DateTime, int>(DateTime.Now, airaverage));
            channel.list6.Add(new KeyValuePair<DateTime, int>(DateTime.Now, heaterduty));
            channel.list7.Add(new KeyValuePair<DateTime, int>(DateTime.Now, currentInt));
        }

        private void setItems(ChannelItem item)
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
            item.Item24.label.Content = "남은시간";
            item.Item25.label.Content = "Error";
            item.Item26.label.Content = "MODE";
        }

        private void MultiWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            Item1.setHandler(Item1Check_Click);
            Item2.setHandler(Item2Check_Click);
            Item3.setHandler(Item3Check_Click);
            Item4.setHandler(Item4Check_Click);
            //multiWindow2 = new MultiWindow2();
            //multiWindow2.Owner = this;
            //multiWindow2.Show();
            Task.Delay(100).ContinueWith(task =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    WindowState = WindowState.Maximized;
                }));
            });
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

        public int GetChannelIndex()
        {
            for (int i = 0; i < 8; i++)
            {
                if (!channelList.ContainsValue(i))
                {
                    return i;
                }
            }
            return -1;
        }

        public ChannelItem GetChannelItem(int index)
        {
            switch (index)
            {
                case 0:
                    return Item1;
                case 1:
                    return Item2;
                case 2:
                    return Item3;
                case 3:
                    return Item4;
                case 4:
                    return multiWindow2.Item1;
                case 5:
                    return multiWindow2.Item2;
                case 6:
                    return multiWindow2.Item3;
                case 7:
                    return multiWindow2.Item4;
            }
            return null;
        }

        public void Item1Check_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Control).Name)
            {
                case "Item1Check":
                    if (Item1.Item1Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[10] = index;
                            Chart.setLegend(index, "히터 온도");
                            Chart.seriesList[index].ItemsSource = Item1.list1;
                            try
                            {
                                Chart.seriesList[index].DependentRangeAxis = Chart.Yaxis[0];
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                        }
                        else
                        {
                            Item1.Item1Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[10];
                        seriesList.Remove(10);
                        Chart.setLegend(index, "");
                        Chart.seriesList[index].ItemsSource = null;
                    }
                    break;
                case "Item2Check":
                    if (Item1.Item2Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[11] = index;
                            Chart.setLegend(index, "배기 온도");
                            Chart.seriesList[index].ItemsSource = Item1.list2;
                            Chart.seriesList[index].DependentRangeAxis = Chart.Yaxis[0];
                        }
                        else
                        {
                            Item1.Item2Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[11];
                        seriesList.Remove(11);
                        Chart.setLegend(index, "");
                        Chart.seriesList[index].ItemsSource = null;
                    }
                    break;
                case "Item3Check":
                    if (Item1.Item3Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[12] = index;
                            Chart.setLegend(index, "열풍히터온도");
                            Chart.seriesList[index].ItemsSource = Item1.list3;
                            Chart.seriesList[index].DependentRangeAxis = Chart.Yaxis[0];
                        }
                        else
                        {
                            Item1.Item3Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[12];
                        seriesList.Remove(12);
                        Chart.setLegend(index, "");
                        Chart.seriesList[index].ItemsSource = null;
                    }
                    break;
                case "Item4Check":
                    if (Item1.Item4Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[13] = index;
                            Chart.setLegend(index, "메인모터운전");
                            Chart.seriesList[index].ItemsSource = Item1.list4;
                        }
                        else
                        {
                            Item1.Item4Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[13];
                        seriesList.Remove(13);
                        Chart.setLegend(index, "");
                        Chart.seriesList[index].ItemsSource = null;
                    }
                    break;
                case "Item5Check":
                    if (Item1.Item5Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[14] = index;
                            Chart.setLegend(index, "히터오프타임");
                            Chart.seriesList[index].ItemsSource = Item1.list5;
                            Chart.seriesList[index].DependentRangeAxis = Chart.Yaxis[1];
                        }
                        else
                        {
                            Item1.Item5Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[14];
                        seriesList.Remove(14);
                        Chart.setLegend(index, "");
                        Chart.seriesList[index].ItemsSource = null;
                    }
                    break;
                case "Item6Check":
                    if (Item1.Item6Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[15] = index;
                            Chart.setLegend(index, "배기온도평균");
                            Chart.seriesList[index].DependentRangeAxis = Chart.Yaxis[1];
                        }
                        else
                        {
                            Item1.Item6Check.IsChecked = false;
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
                    if (Item1.Item7Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[16] = index;
                            Chart.setLegend(index, "열풍히터Duty");
                            Chart.seriesList[index].ItemsSource = Item1.list7;
                            Chart.seriesList[index].DependentRangeAxis = Chart.Yaxis[1];
                        }
                        else
                        {
                            Item1.Item7Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[16];
                        seriesList.Remove(16);
                        Chart.setLegend(index, "");
                        Chart.seriesList[index].ItemsSource = null;
                    }
                    break;
                case "Item8Check":
                    if (Item1.Item8Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[17] = index;
                            Chart.setLegend(index, "메인모터전류");
                            Chart.seriesList[index].ItemsSource = Item1.list8;
                            Chart.seriesList[index].DependentRangeAxis = Chart.Yaxis[0];
                        }
                        else
                        {
                            Item1.Item8Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[17];
                        seriesList.Remove(17);
                        Chart.setLegend(index, "");
                        Chart.seriesList[index].ItemsSource = null;
                    }
                    break;
            }
        }
        public void Item2Check_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Control).Name)
            {
                case "Item1Check":
                    if (Item2.Item1Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[20] = index;
                            Chart.setLegend(index, "히터 온도");
                        }
                        else
                        {
                            Item2.Item1Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[20];
                        seriesList.Remove(20);
                        Chart.setLegend(index, "");
                    }
                    break;
                case "Item2Check":
                    if (Item2.Item2Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[21] = index;
                            Chart.setLegend(index, "배기 온도");
                        }
                        else
                        {
                            Item2.Item2Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[21];
                        seriesList.Remove(21);
                        Chart.setLegend(index, "");
                    }
                    break;
                case "Item3Check":
                    if (Item2.Item3Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[22] = index;
                            Chart.setLegend(index, "열풍히터온도");
                        }
                        else
                        {
                            Item2.Item3Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[22];
                        seriesList.Remove(22);
                        Chart.setLegend(index, "");
                    }
                    break;
                case "Item4Check":
                    if (Item2.Item4Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[23] = index;
                            Chart.setLegend(index, "메인모터운전");
                        }
                        else
                        {
                            Item2.Item4Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[23];
                        seriesList.Remove(23);
                        Chart.setLegend(index, "");
                    }
                    break;
                case "Item5Check":
                    if (Item2.Item5Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[24] = index;
                            Chart.setLegend(index, "히터오프타임");
                        }
                        else
                        {
                            Item2.Item5Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[24];
                        seriesList.Remove(24);
                        Chart.setLegend(index, "");
                    }
                    break;
                case "Item6Check":
                    if (Item2.Item6Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[25] = index;
                            Chart.setLegend(index, "배기온도평균");
                        }
                        else
                        {
                            Item2.Item6Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[25];
                        seriesList.Remove(25);
                        Chart.setLegend(index, "");
                    }
                    break;
                case "Item7Check":
                    if (Item2.Item7Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[26] = index;
                            Chart.setLegend(index, "열풍히터Duty");
                        }
                        else
                        {
                            Item2.Item7Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[26];
                        seriesList.Remove(26);
                        Chart.setLegend(index, "");
                    }
                    break;
                case "Item8Check":
                    if (Item2.Item8Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[27] = index;
                            Chart.setLegend(index, "메인모터전류");
                        }
                        else
                        {
                            Item2.Item8Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[27];
                        seriesList.Remove(27);
                        Chart.setLegend(index, "");
                    }
                    break;
            }
        }
        public void Item3Check_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Control).Name)
            {
                case "Item1Check":
                    if (Item3.Item1Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[30] = index;
                            Chart.setLegend(index, "히터 온도");
                        }
                        else
                        {
                            Item3.Item1Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[30];
                        seriesList.Remove(30);
                        Chart.setLegend(index, "");
                    }
                    break;
                case "Item2Check":
                    if (Item3.Item2Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[31] = index;
                            Chart.setLegend(index, "배기 온도");
                        }
                        else
                        {
                            Item3.Item2Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[31];
                        seriesList.Remove(31);
                        Chart.setLegend(index, "");
                    }
                    break;
                case "Item3Check":
                    if (Item3.Item3Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[32] = index;
                            Chart.setLegend(index, "열풍히터온도");
                        }
                        else
                        {
                            Item3.Item3Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[32];
                        seriesList.Remove(32);
                        Chart.setLegend(index, "");
                    }
                    break;
                case "Item4Check":
                    if (Item3.Item4Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[33] = index;
                            Chart.setLegend(index, "메인모터운전");
                        }
                        else
                        {
                            Item3.Item4Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[33];
                        seriesList.Remove(33);
                        Chart.setLegend(index, "");
                    }
                    break;
                case "Item5Check":
                    if (Item3.Item5Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[34] = index;
                            Chart.setLegend(index, "히터오프타임");
                        }
                        else
                        {
                            Item3.Item5Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[34];
                        seriesList.Remove(34);
                        Chart.setLegend(index, "");
                    }
                    break;
                case "Item6Check":
                    if (Item3.Item6Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[35] = index;
                            Chart.setLegend(index, "배기온도평균");
                        }
                        else
                        {
                            Item3.Item6Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[35];
                        seriesList.Remove(35);
                        Chart.setLegend(index, "");
                    }
                    break;
                case "Item7Check":
                    if (Item3.Item7Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[36] = index;
                            Chart.setLegend(index, "열풍히터Duty");
                        }
                        else
                        {
                            Item3.Item7Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[36];
                        seriesList.Remove(36);
                        Chart.setLegend(index, "");
                    }
                    break;
                case "Item8Check":
                    if (Item3.Item8Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[37] = index;
                            Chart.setLegend(index, "메인모터전류");
                        }
                        else
                        {
                            Item3.Item8Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[37];
                        seriesList.Remove(37);
                        Chart.setLegend(index, "");
                    }
                    break;
            }
        }
        public void Item4Check_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Control).Name)
            {
                case "Item1Check":
                    if (Item4.Item1Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[40] = index;
                            Chart.setLegend(index, "히터 온도");
                        }
                        else
                        {
                            Item4.Item1Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[40];
                        seriesList.Remove(40);
                        Chart.setLegend(index, "");
                    }
                    break;
                case "Item2Check":
                    if (Item4.Item2Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[41] = index;
                            Chart.setLegend(index, "배기 온도");
                        }
                        else
                        {
                            Item4.Item2Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[41];
                        seriesList.Remove(41);
                        Chart.setLegend(index, "");
                    }
                    break;
                case "Item3Check":
                    if (Item4.Item3Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[42] = index;
                            Chart.setLegend(index, "열풍히터온도");
                        }
                        else
                        {
                            Item4.Item3Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[42];
                        seriesList.Remove(42);
                        Chart.setLegend(index, "");
                    }
                    break;
                case "Item4Check":
                    if (Item4.Item4Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[43] = index;
                            Chart.setLegend(index, "메인모터운전");
                        }
                        else
                        {
                            Item4.Item4Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[43];
                        seriesList.Remove(43);
                        Chart.setLegend(index, "");
                    }
                    break;
                case "Item5Check":
                    if (Item4.Item5Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[44] = index;
                            Chart.setLegend(index, "히터오프타임");
                        }
                        else
                        {
                            Item4.Item5Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[44];
                        seriesList.Remove(44);
                        Chart.setLegend(index, "");
                    }
                    break;
                case "Item6Check":
                    if (Item4.Item6Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[45] = index;
                            Chart.setLegend(index, "배기온도평균");
                        }
                        else
                        {
                            Item4.Item6Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[45];
                        seriesList.Remove(45);
                        Chart.setLegend(index, "");
                    }
                    break;
                case "Item7Check":
                    if (Item4.Item7Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[46] = index;
                            Chart.setLegend(index, "열풍히터Duty");
                        }
                        else
                        {
                            Item4.Item7Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[46];
                        seriesList.Remove(46);
                        Chart.setLegend(index, "");
                    }
                    break;
                case "Item8Check":
                    if (Item4.Item8Check.IsChecked.Value)
                    {
                        if (seriesList.Count < 8)
                        {
                            int index = getIndex();
                            seriesList[47] = index;
                            Chart.setLegend(index, "메인모터전류");
                        }
                        else
                        {
                            Item4.Item8Check.IsChecked = false;
                            MessageBox.Show("최대 8개 선택 가능합니다.");
                        }
                    }
                    else
                    {
                        int index = seriesList[47];
                        seriesList.Remove(47);
                        Chart.setLegend(index, "");
                    }
                    break;
            }
        }

        private void DataReceived(IAsyncResult ar)
        {
            ClientData callbackClient = ar.AsyncState as ClientData;
            int bytesRead = callbackClient.client.GetStream().EndRead(ar);
            string readString = Encoding.Default.GetString(callbackClient.readByteData, 0, bytesRead);
            Console.WriteLine("{0}번 사용자 : {1}", callbackClient.clientNumber, readString); callbackClient.client.GetStream().BeginRead(callbackClient.readByteData, 0, callbackClient.readByteData.Length, new AsyncCallback(DataReceived), callbackClient);
        }
    }
}
