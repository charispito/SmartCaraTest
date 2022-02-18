using SmartCaraTest.controls;
using SmartCaraTest.data;
using System;
using System.Collections.Generic;
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
    /// MultiChannelWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MultiChannelWindow : Window
    {
        public List<ChannelItem> Channels { get; set; }
        public MultiChannelWindow()
        {
            InitializeComponent();
            for(int i = 0; i < 8; i++)
            {
                addView(i, i);
            }
            AllGrapButton.Click += (s, e) => {
                new AllGraphWindow().ShowDialog();
            };
        }

        private GridIndex getIndex(int index)
        {
            GridIndex gridIndex = new GridIndex();
            int row = index / 2 + 2;
            int col = index % 2 + 1;
            gridIndex.Column = col;
            gridIndex.Row = row;
            return gridIndex;
        }

        private void addView(int channel, int index)
        {
            ChannelItem item = new ChannelItem();
            item.ChannelView.cont.Content = channel + 1;
            item.ChannelView.cont.FontWeight = FontWeights.Black;
            item.Item1.Title = "히터 온도";
            item.Item2.Title = "배기 온도";
            item.Item3.Title = "열풍히터온도";
            item.Item4.Title = "메인모터운전";
            item.Item5.Title = "배기팬 풍량";
            item.Item6.Title = "운전 시간";

            item.Item11.Title = "히터오프타임";
            item.Item12.Title = "배기온도평균";
            item.Item13.Title = "열풍히터Duty";
            item.Item14.Title = "메인모터전류";
            item.Item15.Title = "열풍팬 풍량";
            item.Item16.Title = "만수 감지";

            item.Item21.Title = "Version";
            item.Item22.Title = "Compile";
            item.Item23.Title = "상태";
            item.Item24.Title = "남은 시간";
            item.Item25.Title = "Error";
            item.Item26.Title = "MODE";

            var info = getIndex(index);
            Grid.SetColumn(item, info.Column);
            Grid.SetRow(item, info.Row);
            MainGrid.Children.Add(item);
            
        }
    }
}
