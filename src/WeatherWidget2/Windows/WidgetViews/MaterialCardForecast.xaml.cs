using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WeatherWidget2.Model;
using WeatherWidget2ResourceLib;
using static WeatherWidget2.Win32;

namespace WeatherWidget2.Windows.WidgetViews
{
    public partial class MaterialCardForecast : Window, IWidgetView
    {
        public Icons icons;
        List<ForecastOneDay> Days;
        string sign = "";
        string windSign = "";
        public string[] Labels { get; set; }

        public MaterialCardForecast()
        {
            InitializeComponent();
            DataContext = this;
            icons = new Icons(IconSize.Medium, IconTheme.Standart);
        }
        public MaterialCardForecast(IconTheme t)
        {
            InitializeComponent();
            DataContext = this;
            icons = new Icons(IconSize.Medium, t);
        }

        public void SetupDay(int day = 0)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = day == 0 ? icons.GetIcon(Days.LastOrDefault().GetCurrentIcon()) : icons.GetIcon(Days[day].GetDayIcon());
            bitmap.EndInit();
            imgIcon.Source = bitmap;

            tbPress.Text = day == 0 ? $"{App.Lang["Pressure"]} {Days[day].GetCurrentPressure()} {App.Lang["mmHG"]}" : $"{App.Lang["Pressure"]} {Days[day].GetDayPressure()} {App.Lang["mmHG"]}";
            tbHumi.Text = day == 0 ? $"{App.Lang["Humidity"]} {Days[day].GetCurrentHumidity()} %" : $"{App.Lang["Humidity"]} {Days[day].GetDayHumidity()} %";
            tbCondi.Text = day == 0 ? Days.LastOrDefault().GetCurrentCondition() : Days[day].GetDayCondition();
            tbThemp.Text = day == 0 ? $"{Days.LastOrDefault().GetCurrentValue()} {sign}" : $"{Days[day].GetDayValue()} {sign}";
            tbWind.Text = day == 0 ? $"{App.Lang["Wind"]} {Days[day].GetCurrentWindSpeed()} {windSign}, {ForecastOneDay.GetSideCode(Days[day].GetCurrentWindDeg())}" :
                                     $"{App.Lang["Wind"]} {Days[day].GetDayWindSpeed()} {windSign}, {ForecastOneDay.GetSideCode(Days[day].GetDayWindDeg())}";
        }
        public void SetupButtons(int day)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = day == 0 ? icons.GetIcon(Days.LastOrDefault().GetCurrentIcon()) : icons.GetIcon(Days[day].GetDayIcon());
            bitmap.EndInit();

            string themp = $"{Days[day].Values.ToArray().Max()}/{Days[day].Values.ToArray().Min()} {sign}";

            switch (day)
            {
                case 0:
                    day1.Icon = bitmap;
                    day1.TextThemperature = themp;
                    day1.TextDay = Days[day].DayString;
                    break;
                case 1:
                    day2.Icon = bitmap;
                    day2.TextThemperature = themp;
                    day2.TextDay = Days[day].DayString;
                    break;
                case 2:
                    day3.Icon = bitmap;
                    day3.TextThemperature = themp;
                    day3.TextDay = Days[day].DayString;
                    break;
                case 3:
                    day4.Icon = bitmap;
                    day4.TextThemperature = themp;
                    day4.TextDay = Days[day].DayString;
                    break;
                case 4:
                    day5.Icon = bitmap;
                    day5.TextThemperature = themp;
                    day5.TextDay = Days[day].DayString;
                    break;
                default:
                    break;
            }
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
            Dispatcher.Invoke(DispatcherPriority.Background, new
             Action(() =>
             {
                 Days = (List<ForecastOneDay>)param["Days"];
                 tbLocation.Text = param["Location"].ToString();
                 sign = param["Sign"].ToString();
                 windSign = param["Wind"].ToString();

                 ForecastOneDay fod = new ForecastOneDay();
                 fod.Day = 5;
                 fod.Values.Add((int)param["ThemperatureF"]);
                 fod.Condi.Add(param["WeatherParamF"].ToString());
                 fod.Icons.Add(param["IconF"].ToString());

                 Days.Add(fod);
             }));
        }
        public void UpdateLook(Dictionary<string, object> param)
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new
             Action(() =>
             {
                 icons.UpdateData(IconSize.Medium, (IconTheme)param["Theme"]);

                 SetupButtons(0);
                 SetupButtons(1);
                 SetupButtons(2);
                 SetupButtons(3);
                 SetupButtons(4);

                 SetupDay();
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

            day1.SetVisibleBack(true);
        }
        private void day1Click()
        {
            day2.SetVisibleBack(false);
            day3.SetVisibleBack(false);
            day4.SetVisibleBack(false);
            day5.SetVisibleBack(false);

            day1.SetVisibleBack(true);

            SetupDay(0);
        }
        private void day2Click()
        {
            day1.SetVisibleBack(false);
            day3.SetVisibleBack(false);
            day4.SetVisibleBack(false);
            day5.SetVisibleBack(false);

            day2.SetVisibleBack(true);

            SetupDay(1);
        }
        private void day3Click()
        {
            day2.SetVisibleBack(false);
            day1.SetVisibleBack(false);
            day4.SetVisibleBack(false);
            day5.SetVisibleBack(false);

            day3.SetVisibleBack(true);

            SetupDay(2);
        }
        private void day4Click()
        {
            day2.SetVisibleBack(false);
            day3.SetVisibleBack(false);
            day1.SetVisibleBack(false);
            day5.SetVisibleBack(false);

            day4.SetVisibleBack(true);

            SetupDay(3);
        }
        private void day5Click()
        {
            day2.SetVisibleBack(false);
            day3.SetVisibleBack(false);
            day4.SetVisibleBack(false);
            day1.SetVisibleBack(false);

            day5.SetVisibleBack(true);

            SetupDay(4);
        }
    }
}
