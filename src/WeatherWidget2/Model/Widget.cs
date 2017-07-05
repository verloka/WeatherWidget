using System;
using System.Collections.Generic;
using Verloka.HelperLib.Settings;
using WeatherWidget2.Windows.WidgetViews;
using WeatherWidget2ResourceLib;

namespace WeatherWidget2.Model
{
    public class Widget : ISettingStruct
    {
        public string guid { get; private set; }
        public string Name { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Type { get; set; } //0 daily, 1 forecast
        public int ViewCode { get; set; }
        public int CityID { get; set; }
        public Measure WidgetMeasure { get; set; }
        public IconSize Size { get; set; }
        public bool Visible { get; set; }
        public IconTheme Theme { get; set; }
        public string TextColor { get; set; }
        public bool IsCreated
        {
            get
            {
                return view != null;
            }
        }

        Weather weather;
        IWidgetView view;

        public Widget()
        {
            guid = Guid.NewGuid().ToString();
            Name = "NaN";
            Left = 100;
            Top = 100;
            Type = 0;
            CityID = 2172797;
            Size = IconSize.Medium;
            Theme = IconTheme.Standart;
            WidgetMeasure = Measure.Metric;
            TextColor = "White";
            Visible = true;

            weather = new Weather();
            weather.SetCity(CityID);
        }
        public Widget(string value)
        {
            SetValue(value);

            weather = new Weather();
            weather.SetCity(CityID);
        }
        public Widget(Widget copy)
        {
            SetValue(copy.GetValue());

            weather = new Weather();
            weather.SetCity(CityID);
        }

        public void NewGUID()
        {
            guid = Guid.NewGuid().ToString();
        }
        public void SetEditMode(bool mode)
        {
            view.Edit(mode);
        }
        public void CopyPosition()
        {
            Left = view.GetLeft();
            Top = view.GetTop();
        }
        public void CreateWindow()
        {
            switch (ViewCode)
            {
                case 0:
                default:
                    view = new OldCurrent(Size, Theme);
                    break;
                case 100:
                    view = new OldForecast(Theme);
                    break;
                case 101:
                    view = new MaterialCardForecast(Theme);
                    break;
                case 102:
                    view = new MaterialChartCardForecast(Theme);
                    break;
            }

            UpdateData();
            UpdateLook();
            view.SetLeft(Left);
            view.SetTop(Top);
            view.ShowWidget();
        }
        public void UpdateData(bool updateCity = false, bool updateMeasure = false)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();


            if (updateCity)
                weather.SetCity(CityID);

            if (view.Type == 0)
            {
                weather.LoadCurrent();
                dic.Add("Icon", weather.Current.WeatherList[0].Icon);
                dic.Add("Themperature", Weather.GetTemperatureString(weather.Current.Main.Temperature, WidgetMeasure));
                dic.Add("WeatherParam", $"{weather.Current.WeatherList[0].WeatherParameters}");
                dic.Add("Location", $"{weather.Current.Name}, {weather.Current.system.CountryCode}");
            }
            else
            {
                weather.LoadForecast();

                Dictionary<int, ForecastOneDay> forecastDic = new Dictionary<int, ForecastOneDay>();
                List<ForecastOneDay> days = new List<ForecastOneDay>();

                for (int i = 0; i < 5; i++)
                {
                    int day = weather.Forecast.WeatherBodyList[0].GetDate().AddDays(i).Day;
                    forecastDic.Add(day, new ForecastOneDay() { Day = day });
                }

                foreach (var item in weather.Forecast.WeatherBodyList)
                    if (forecastDic.ContainsKey(item.GetDate().Day))
                        forecastDic[item.GetDate().Day].SetData(Weather.GetTemperature(item.Main.Temperature, WidgetMeasure),
                                                              item.GetDate().ToShortTimeString().ToString(),
                                                              item.WeatherList[0].Icon,
                                                              item.Main.Pressure,
                                                              item.Main.Humidity,
                                                              item.WeatherList[0].WeatherParameters,
                                                              item.Wind,
                                                              GetDayByDayOfWeek(item.GetDate().DayOfWeek));


                foreach (var item in forecastDic)
                    days.Add(item.Value);

                string tempSign = WidgetMeasure == Measure.Metric ? "°C" : "°F";
                string windSign = WidgetMeasure == Measure.Metric ? "meter/sec" : "miles/hour";

                dic.Add("Sign", tempSign);
                dic.Add("Wind", windSign);
                dic.Add("Days", days);
                dic.Add("Location", $"{weather.Forecast.City.Name}, {weather.Forecast.City.Country}");
            }

            view.UpdateInfo(dic);
        }
        public void UpdateLook()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("Theme", Theme);
            dic.Add("TextColor", TextColor);

            if (view.Type == 0)
                dic.Add("Size", Size);

            view.UpdateLook(dic);
        }
        public void Destroy()
        {
            if (view == null) return;

            view.DestroyView();
            view = null;
        }

        public string GetValue()
        {
            return $"{Name}|{Left}|{Top}|{Type}|{WidgetMeasure.GetHashCode()}|{Size.GetHashCode()}|{Visible}|{CityID}|{guid}|{Theme.GetHashCode()}|{TextColor}|{ViewCode}";
        }
        public void SetValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            string[] strs = value.Split('|');
            int num = 0;
            bool vis = true;

            //Name
            Name = strs[0];
            //Left
            int.TryParse(strs[1], out num);
            Left = num;
            //Top
            int.TryParse(strs[2], out num);
            Top = num;
            //Type
            int.TryParse(strs[3], out num);
            Type = num;
            //Measure
            int.TryParse(strs[4], out num);
            WidgetMeasure = (Measure)num;
            //Size
            int.TryParse(strs[5], out num);
            Size = (IconSize)num;
            //Visible
            bool.TryParse(strs[6], out vis);
            Visible = vis;
            //CityID
            int.TryParse(strs[7], out num);
            CityID = num;
            //guid
            guid = strs[8];
            //Theme
            int.TryParse(strs[9], out num);
            Theme = (IconTheme)num;
            //Text color
            TextColor = strs[10];

            try
            {
                int.TryParse(strs[11], out num);
                ViewCode = num;
            }
            catch
            {
                ViewCode = 0;
            }
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        public static string GetDayByDayOfWeek(DayOfWeek d)
        {
            switch (d)
            {
                case DayOfWeek.Sunday:
                    return "Sun";
                case DayOfWeek.Monday:
                    return "Mon";
                case DayOfWeek.Tuesday:
                    return "Tue";
                case DayOfWeek.Wednesday:
                    return "Wed";
                case DayOfWeek.Thursday:
                    return "Thu";
                case DayOfWeek.Friday:
                    return "Fri";
                case DayOfWeek.Saturday:
                    return "Sat";
                default:
                    return "";
            }
        }
    }
}
