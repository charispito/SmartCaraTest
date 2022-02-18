using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;

namespace SmartCaraTest.controls
{
    /// <summary>
    /// ChannelItem.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChannelItem : UserControl
    {
        public ObservableCollection<KeyValuePair<DateTime, int>> list1 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public ObservableCollection<KeyValuePair<DateTime, int>> list2 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public ObservableCollection<KeyValuePair<DateTime, int>> list3 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public ObservableCollection<KeyValuePair<DateTime, int>> list4 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public ObservableCollection<KeyValuePair<DateTime, int>> list5 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public ObservableCollection<KeyValuePair<DateTime, int>> list6 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public ObservableCollection<KeyValuePair<DateTime, int>> list7 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public ObservableCollection<KeyValuePair<DateTime, int>> list8 = new ObservableCollection<KeyValuePair<DateTime, int>>();
        public TcpClient client { get; set; }
        private int _Channel;

        public int Channel {
            get { return _Channel; }
            set
            {
                _Channel = value;
                ChannelView.cont.Content = value;
            }
        }
        private int _Column;
        private int _Row;
        public int Column { 
            get { return _Column; }
            set { 
                _Column = value;
                Grid.SetColumn(this, value);
            }
        }
        public int Row {
            get { return _Row; }
            set
            {
                _Row = value;
                Grid.SetRow(this, value);
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
