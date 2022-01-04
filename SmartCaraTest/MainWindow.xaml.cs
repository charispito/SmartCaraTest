using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls.DataVisualization.Charting;

namespace SmartCaraTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Style getLineStyle(SolidColorBrush color)
        {
            Style style = new Style(typeof(LineDataPoint));
            style.Setters.Add(new Setter(LineDataPoint.TemplateProperty, null));
            style.Setters.Add(new Setter(LineDataPoint.BackgroundProperty, color));
            return style;
        }      
    }
}
