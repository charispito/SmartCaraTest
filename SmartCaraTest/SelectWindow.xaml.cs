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
    /// SelectWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SelectWindow : Window
    {
        public SelectWindow()
        {
            InitializeComponent();
            OneChannelButton.Click += OneChannelButton_Click;
            MultiChannelButton.Click += MultiChannelButton_Click;
        }

        private void MultiChannelButton_Click(object sender, RoutedEventArgs e)
        {
            new MultiWindow1().Show();
            this.Close();
        }

        private void OneChannelButton_Click(object sender, RoutedEventArgs e)
        {
            new OneChannelWindow().Show();            
            this.Close();
        }
    }
}
