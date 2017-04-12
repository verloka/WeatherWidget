using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using static WeatherWidget2.Win32;

namespace WeatherWidget2.Widget
{
    public partial class TempWidget : Window
    {
        Weather weather;
        int id;
        Model.Measure ms;

        public TempWidget(int id, Model.Measure ms)
        {
            InitializeComponent();
            this.id = id;
            this.ms = ms;

            weather = new Weather();
        }

        public void UpdateWeatherData(int id, Model.Measure ms)
        {
            this.id = id;
            this.ms = ms;
        }
        public void UpdateInfo()
        {
            weather.SetCity(id);
            weather.SetMeasure(ms);
            weather.LoadCurrent();

            tbThemperature.Text = weather.GetTemperature(weather.Current.Main.Temperature);
            tbCondition.Text = weather.Current.WeatherList[0].WeatherParameters;
            tbLocation.Text = weather.Current.Name;
        }
        public void SetIcon()
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri($"{AppDomain.CurrentDomain.BaseDirectory}\\WeatherIcons\\Standart\\64\\{weather.Current.WeatherList[0].Icon}.png", UriKind.RelativeOrAbsolute);
            bitmap.EndInit();
            imgIcon.Source = bitmap;
        }
        public void Edit(bool edit)
        {
            gridHeader.Visibility = edit ? Visibility.Visible : Visibility.Collapsed;
        }

        private void gridHeaderMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }
        private void windowLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowInteropHelper wndHelper = new WindowInteropHelper(this);

                int exStyle = (int)GetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE);

                exStyle |= (int)ExtendedWindowStyles.WS_EX_TOOLWINDOW;
                SetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);
            }
            catch { }
        }
    }
}
