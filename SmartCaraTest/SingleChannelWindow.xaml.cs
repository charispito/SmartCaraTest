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

            //Loaded += SingleChannelWindow_Loaded;
            //check1.Checked += CheckBox_Checked;
            //check2.Checked += CheckBox_Checked;
            //check3.Checked += CheckBox_Checked;
            //check4.Checked += CheckBox_Checked;
            //check5.Checked += CheckBox_Checked;
            //check6.Checked += CheckBox_Checked;
            //check7.Checked += CheckBox_Checked;
            //check8.Checked += CheckBox_Checked;
            //check1.Unchecked += CheckBox_Unchecked;
            //check2.Unchecked += CheckBox_Unchecked;
            //check3.Unchecked += CheckBox_Unchecked;
            //check4.Unchecked += CheckBox_Unchecked;
            //check5.Unchecked += CheckBox_Unchecked;
            //check6.Unchecked += CheckBox_Unchecked;
            //check7.Unchecked += CheckBox_Unchecked;
            //check8.Unchecked += CheckBox_Unchecked;
        }

        //private void CheckBox_Checked(object sender, RoutedEventArgs e)
        //{
        //    switch((sender as Control).Name)
        //    {
        //        case "check1":
        //            ChartView.seriesList[0].Enabled = true;
        //            break;
        //        case "check2":
        //            ChartView.seriesList[1].Enabled = true;
        //            break;
        //        case "check3":
        //            ChartView.seriesList[2].Enabled = true;
        //            break;
        //        case "check4":
        //            ChartView.seriesList[3].Enabled = true;
        //            break;
        //        case "check5":
        //            ChartView.seriesList[4].Enabled = true;
        //            break;
        //        case "check6":
        //            ChartView.seriesList[5].Enabled = true;
        //            break;
        //        case "check7":
        //            ChartView.seriesList[6].Enabled = true;
        //            break;
        //        case "check8":
        //            ChartView.seriesList[7].Enabled = true;
        //            break;
        //    }
        //}

        //private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    switch ((sender as Control).Name)
        //    {
        //        case "check1":
        //            ChartView.seriesList[0].Enabled = false;
        //            break;
        //        case "check2":
        //            ChartView.seriesList[1].Enabled = false;
        //            break;
        //        case "check3":
        //            ChartView.seriesList[2].Enabled = false;
        //            break;
        //        case "check4":
        //            ChartView.seriesList[3].Enabled = false;
        //            break;
        //        case "check5":
        //            ChartView.seriesList[4].Enabled = false;
        //            break;
        //        case "check6":
        //            ChartView.seriesList[5].Enabled = false;
        //            break;
        //        case "check7":
        //            ChartView.seriesList[6].Enabled = false;
        //            break;
        //        case "check8":
        //            ChartView.seriesList[7].Enabled = false;
        //            break;
        //    }
        //}

        //private void SingleChannelWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    ChartView.timer.Start();
        //}

        private void ParameterButton_Click(object sender, RoutedEventArgs e)
        {
            ParameterWindow window = new ParameterWindow();
            window.ShowDialog();
        }
    }
}
