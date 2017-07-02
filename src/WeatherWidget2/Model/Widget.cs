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
            weather.LoadCurrent();

            view = new OldCurrent(Size, Theme);

            UpdateData();
            UpdateLook();
            view.SetLeft(Left);
            view.SetTop(Top);
            view.ShowWidget();
        }
        public void UpdateData(bool updateCity = false, bool updateMeasure = false)
        {
            if (updateCity)
                weather.SetCity(CityID);

            if(view.Type == 0)
                weather.LoadCurrent();
            else
                weather.LoadForecast();


            view.UpdateInfo(new List<object>
            {
                weather.Current.WeatherList[0].Icon,
                weather.GetTemperatureString(weather.Current.Main.Temperature, WidgetMeasure),
                $"{weather.Current.WeatherList[0].WeatherParameters}",
                $"{weather.Current.Name}, {weather.Current.system.CountryCode}"
            });
        }
        public void UpdateLook()
        {
            //TODO Generic full but use only a part
            view.UpdateLook(new List<object>
            {
                Size,
                Theme,
                TextColor
            });


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
