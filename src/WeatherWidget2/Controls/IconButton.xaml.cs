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
    public partial class IconButton : UserControl
    {
        public event Action Click;
        bool pressed = false;

        public ImageSource Icon
        {
            get { return GetValue(IconProperty) as ImageSource; }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(IconButton), null);
        public Thickness IconThickness
        {
            get { return (Thickness)GetValue(IconThicknessProperty); }
            set { SetValue(IconThicknessProperty, value); }
        }
        public static readonly DependencyProperty IconThicknessProperty = DependencyProperty.Register("IconThickness", typeof(Thickness), typeof(IconButton), null);

        public IconButton()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void gridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pressed = true;
        }
        private void gridMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (pressed)
            {
                Click?.Invoke();
                pressed = false;
            }
        }
        private void gridMouseLeave(object sender, MouseEventArgs e)
        {
            pressed = false;
        }
    }
}
