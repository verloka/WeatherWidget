using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using WeatherWidget2ResourceLib;
using static WeatherWidget2.Win32;

namespace WeatherWidget2.Windows
{
    public partial class WidgetCurrent : Window
    {
        public Icons icons;

        Weather weather;
        int id;
        Model.Measure ms;

        public WidgetCurrent(int id, Model.Measure ms, IconSize iconSize, IconTheme iconTheme)
        {
            InitializeComponent();
            this.id = id;
            this.ms = ms;

            icons = new Icons(iconSize, iconTheme);
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
        public void UpdateLook()
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = icons.GetIcon(weather.Current.WeatherList[0].Icon);
            bitmap.EndInit();
            imgIcon.Source = bitmap;

            switch (icons.GetSize())
            {
                case 64:
                default:
                    imgIcon.Width = 64;
                    imgIcon.Height = 64;

                    tbThemperature.FontSize = 36;
                    tbCondition.FontSize = 24;
                    tbLocation.FontSize = 16;
                    break;
                case 32:
                    imgIcon.Width = 32;
                    imgIcon.Height = 32;

                    tbThemperature.FontSize = 30;
                    tbCondition.FontSize = 20;
                    tbLocation.FontSize = 14;
                    break;
                case 128:
                    imgIcon.Width = 128;
                    imgIcon.Height = 128;

                    tbThemperature.FontSize = 56;
                    tbCondition.FontSize = 36;
                    tbLocation.FontSize = 24;
                    break;
            }
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
