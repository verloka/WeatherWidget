using LiveCharts;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WeatherWidget2.Model;
using WeatherWidget2ResourceLib;
using static WeatherWidget2.Win32;

namespace WeatherWidget2.Windows.WidgetViews
{
    public partial class OldForecast : Window, IWidgetView
    {
        public Icons icons;
        ChartValues<int> Values;
        List<ForecastOneDay> Days;
        string sign = "";
        public string[] Labels { get; set; }

        public OldForecast()
        {
            InitializeComponent();
            DataContext = this;
            icons = new Icons(IconSize.Medium, IconTheme.Standart);
        }
        public OldForecast(IconTheme t)
        {
            InitializeComponent();
            DataContext = this;
            icons = new Icons(IconSize.Medium, t);
        }

        public void SetupDay(int day = 0)
        {
            if (chartLine.Values == null)
                chartLine.Values = new ChartValues<int>();

            if (chartXAxis.Labels == null)
                chartXAxis.Labels = new List<string>();

            chartLine.Values.Clear();
            chartXAxis.Labels.Clear();

            foreach (var item in Days[day].Values)
                chartLine.Values.Add(item);

            foreach (var item in Days[day].Labels)
                chartXAxis.Labels.Add(item);

            if(day == 0 && Days[0].Values.Count != Days[1].Values.Count)
                for (int i = 0; i < Days[1].Values.Count - Days[0].Values.Count; i++)
                {
                    chartLine.Values.Add(Days[1].Values[i]);
                    chartXAxis.Labels.Add(Days[1].Labels[i]);
                }

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = day == 0 ? icons.GetIcon(Days[day].GetCurrentIcon()) : icons.GetIcon(Days[day].GetDayIcon());
            bitmap.EndInit();
            imgIcon.Source = bitmap;

            tbPress.Text = day == 0 ? $"Pressure {Days[day].GetCurrentPressure()} hPa" : $"Pressure {Days[day].GetDayPressure()} hPa";
            tbHumi.Text = day == 0 ? $"Humidity {Days[day].GetCurrentHumidity()} %" : $"Humidity {Days[day].GetDayHumidity()} %";
            tbCondy.Text = day == 0 ? Days[day].GetCurrentCondition() : Days[day].GetDayCondition();

            tbThemperature.Text = day == 0 ? $"{Days[day].GetCurrentValue()} {sign}" : $"{Days[day].GetDayValue()} {sign}";
        }

        #region IWidgetView
        public int Type => 1;

        public void DestroyView()
        {
            Close();
        }
        public void Edit(bool mode)
        {
            gridHeader.Visibility = mode ? Visibility.Visible : Visibility.Collapsed;
        }
        public int GetLeft()
        {
            return (int)Left;
        }
        public int GetTop()
        {
            return (int)Top;
        }
        public void SetLeft(int left)
        {
            Left = left;
        }
        public void SetTop(int top)
        {
            Top = top;
        }
        public void ShowWidget()
        {
            Show();
        }
        public void UpdateInfo(Dictionary<string, object> param)
        {
            Days = (List<ForecastOneDay>)param["Days"];
            chartLine.Title = param["Location"].ToString();
            sign = param["Sign"].ToString();

            SetupDay();
        }
        public void UpdateLook(Dictionary<string, object> param)
        {
            icons.UpdateData(IconSize.Medium, (IconTheme)param["Theme"]);
            chartLine.DataLabels = true;

            Dispatcher.Invoke(DispatcherPriority.Background, new
             Action(() =>
             {
                 SolidColorBrush scb = new SolidColorBrush(ColorParser.FromName(param["TextColor"].ToString()));
                 tbThemperature.Foreground = scb;
                 ccLegend.Foreground = scb;
                 chartXAxis.Foreground = scb;
                 chartLine.Foreground = scb;
             }));
        }
        #endregion

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

        private void img1Click(object sender, MouseButtonEventArgs e)
        {
            SetupDay(0);
        }
        private void img2Click(object sender, MouseButtonEventArgs e)
        {
            SetupDay(1);
        }
        private void img3Click(object sender, MouseButtonEventArgs e)
        {
            SetupDay(2);
        }
        private void img4Click(object sender, MouseButtonEventArgs e)
        {
            SetupDay(3);
        }
        private void img5Click(object sender, MouseButtonEventArgs e)
        {
            SetupDay(4);
        }
    }
}
