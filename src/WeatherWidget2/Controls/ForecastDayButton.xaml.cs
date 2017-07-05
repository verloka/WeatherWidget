using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WeatherWidget2.Controls
{
    public partial class ForecastDayButton : UserControl
    {
        public event Action Click;
        bool pressed = false;

        public ImageSource Icon
        {
            get { return GetValue(IconProperty) as ImageSource; }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(ForecastDayButton), null);
        public SolidColorBrush BorderColor
        {
            get { return GetValue(BorderColorProperty) as SolidColorBrush; }
            set { SetValue(BorderColorProperty, value); }
        }
        public static readonly DependencyProperty BorderColorProperty = DependencyProperty.Register("BorderColor", typeof(SolidColorBrush), typeof(ForecastDayButton), null);
        public SolidColorBrush BorderFill
        {
            get { return GetValue(BorderFillProperty) as SolidColorBrush; }
            set { SetValue(BorderFillProperty, value); }
        }
        public static readonly DependencyProperty BorderFillProperty = DependencyProperty.Register("BorderFill", typeof(SolidColorBrush), typeof(ForecastDayButton), null);
        public string TextDay
        {
            get { return GetValue(TextDayProperty) as string; }
            set { SetValue(TextDayProperty, value); }
        }
        public static readonly DependencyProperty TextDayProperty = DependencyProperty.Register("TextDay", typeof(string), typeof(ForecastDayButton), null);
        public string TextThemperature
        {
            get { return GetValue(TextThemperatureProperty) as string; }
            set { SetValue(TextThemperatureProperty, value); }
        }
        public static readonly DependencyProperty TextThemperatureProperty = DependencyProperty.Register("TextThemperature", typeof(string), typeof(ForecastDayButton), null);
        public int IconWidth
        {
            get { return (int)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }
        public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register("IconWidth", typeof(int), typeof(ForecastDayButton), null);
        public int IconHeight
        {
            get { return (int)GetValue(IconHeightProperty); }
            set { SetValue(IconHeightProperty, value); }
        }
        public static readonly DependencyProperty IconHeightProperty = DependencyProperty.Register("IconHeight", typeof(int), typeof(ForecastDayButton), null);
        public Thickness ButtonBorder
        {
            get { return (Thickness)GetValue(ButtonBorderProperty); }
            set { SetValue(ButtonBorderProperty, value); }
        }
        public static readonly DependencyProperty ButtonBorderProperty = DependencyProperty.Register("ButtonBorder", typeof(Thickness), typeof(ForecastDayButton), null);

        public ForecastDayButton()
        {
            InitializeComponent();
            DataContext = this;
            SetVisibleBack(false);
        }

        public void SetVisibleBack(bool visible)
        {
            bBorder.Opacity = visible ? 1 : 0;
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
