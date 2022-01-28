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
    /// AllGraphWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AllGraphWindow : Window
    {
        List<ChartView> charts = new List<ChartView>();
        public AllGraphWindow()
        {
            InitializeComponent();
            for (int i = 0; i < 8; i++)
            {
                addView(i, i);
            }

            Loaded += AllGraphWindow_Loaded;
            Closing += AllGraphWindow_Closing;
        }

        private void AllGraphWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (ChartView chart in charts)
            {
                chart.timer.Stop();
                chart.timer.Dispose();
            }
        }

        private void AllGraphWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(ChartView chart in charts)
            {
                chart.timer.Start();
            }
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
            ChartView item = new ChartView(channel + 1);
            var info = getIndex(index);
            Grid.SetColumn(item, info.Column);
            Grid.SetRow(item, info.Row);
            charts.Add(item);
            MainGrid.Children.Add(item);
        }
    }
}
