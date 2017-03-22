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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeatherWidget2.Controls
{
    public partial class WindowHeaderButton : UserControl
    {
        public ImageSource Icon
        {
            get { return GetValue(IconProperty) as ImageSource; }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(WindowHeaderButton), null);

        public WindowHeaderButton()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
