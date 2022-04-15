using SmartCaraTest.data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

namespace SmartCaraTest.controls
{
    public partial class ChannelItem : UserControl
    {
        public bool ParameterMode = false;
        public bool Response = false;
        public int NonResponse { get; set; } = 0;
        public ParameterWindow ParameterWindow { get; set; }
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
            
        }

        private void ParameterButton_Click(object sender, RoutedEventArgs e)
        {
            int length = client.Client.ReceiveBufferSize;
            byte[] buffer = new byte[length];
            client.Client.Receive(buffer);
            ParameterMode = true;
            ParameterWindow = new ParameterWindow(this);
            ParameterWindow.Show();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            run = false;
            chartView.ItemRun[ItemIndex] = false;
            byte[] command = Protocol.GetCommand(3);
            client.GetStream().Write(command, 0, command.Length);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            
            byte[] command = Protocol.GetCommand(2);
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
    }
}
