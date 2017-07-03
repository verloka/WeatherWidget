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
            if (Type == 0)
                view = new OldCurrent(Size, Theme);
            else
                view = new OldForecast();

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

                ForecastOneDay d1 = new ForecastOneDay();
                d1.Day = weather.Forecast.WeatherBodyList[0].GetDate().Day;

                ForecastOneDay d2 = new ForecastOneDay();
                d2.Day = weather.Forecast.WeatherBodyList[0].GetDate().Day + 1;

                ForecastOneDay d3 = new ForecastOneDay();
                d3.Day = weather.Forecast.WeatherBodyList[0].GetDate().Day + 2;

                ForecastOneDay d4 = new ForecastOneDay();
                d4.Day = weather.Forecast.WeatherBodyList[0].GetDate().Day + 2;

                ForecastOneDay d5 = new ForecastOneDay();
                d5.Day = weather.Forecast.WeatherBodyList[0].GetDate().Day + 2;
                
                foreach (var item in weather.Forecast.WeatherBodyList)
                {
                    if (d1.Day == item.GetDate().Day)
                    {
                        d1.Values.Add(Weather.GetTemperature(item.Main.Temperature, WidgetMeasure));
                        d1.Labels.Add(item.GetDate().ToShortTimeString().ToString());
                    }
                    else if (d2.Day == item.GetDate().Day)
                    {
                        d2.Values.Add(Weather.GetTemperature(item.Main.Temperature, WidgetMeasure));
                        d2.Labels.Add(item.GetDate().ToShortTimeString().ToString());
                    }
                    else if (d3.Day == item.GetDate().Day)
                        ;
                    else if (d4.Day == item.GetDate().Day)
                        ;
                    else if (d5.Day == item.GetDate().Day)
                        ;
                }

                List<ForecastOneDay> days = new List<ForecastOneDay>() { d1, d2, d3, d4, d5 };
                dic.Add("Days", days);
            }

            view.UpdateInfo(dic);
        }
        public void UpdateLook()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            if (view.Type == 0)
            {
                dic.Add("Size", Size);
                dic.Add("Theme", Theme);
                dic.Add("TextColor", TextColor);
            }
            else
            {

            }

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
            return $"{Name}|{Left}|{Top}|{Type}|{WidgetMeasure.GetHashCode()}|{Size.GetHashCode()}|{Visible}|{CityID}|{guid}|{Theme.GetHashCode()}|{TextColor}";
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
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
