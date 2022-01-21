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
    /// SingleChannelWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SingleChannelWindow : Window
    {
        public SingleChannelWindow()
        {
            InitializeComponent();
            init();
            initCheckbox();
        }

        private void ParameterButton_Click(object sender, RoutedEventArgs e)
        {
            ParameterWindow window = new ParameterWindow();
            window.ShowDialog();
        }
    }
}
