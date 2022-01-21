using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartCaraTest.controls
{
    /// <summary>
    /// DoubleLabel.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DoubleLabel : UserControl
    {
        public DoubleLabel()
        {
            InitializeComponent();
        }

        [Category("Text"), Description("Set Text")]
        public string Title {
            get
            {
                return this.label.Content.ToString();
            }
            set
            {
                this.label.Content = value;
            }
        }

        [Category("Text"), Description("Set Text")]
        public string Text
        {
            get
            {
                return this.cont.Content.ToString();
            }
            set
            {
                this.cont.Content = value;
            }
        }

        [Category("TitleColor"), Description("Set Title Color")]
        public string TitleColor
        {
            get
            {
                return this.label.Foreground.ToString();
            }
            set
            {
                this.label.Foreground = new BrushConverter().ConvertFromString(value) as SolidColorBrush;
            }
        }

        [Category("ContentColor"), Description("Set Title Color")]
        public string ContentColor
        {
            get
            {
                return this.cont.Foreground.ToString();
            }
            set
            {
                this.cont.Foreground = new BrushConverter().ConvertFromString(value) as SolidColorBrush;
            }
        }
        [Category("TitleBackground"), Description("Set Title Background")]
        public string TitleBackground
        {
            get
            {
                return this.label.Background.ToString();
            }
            set
            {
                this.label.Background = new BrushConverter().ConvertFromString(value) as SolidColorBrush;
            }
        }
    }
}
