using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verloka.HelperLib.Settings;

namespace WeatherWidget2.Model
{
    public class Widget: ISettingStruct
    {
        public string Name { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Type { get; set; } //0 daily, 1 forecast
        public int CityID { get; set; }
        public Measure WidgetMeasure { get; set; }
        public string IconSize { get; set; }

        WeatherWidget2.Widget.TempWidget daily;

        public Widget()
        {
            Left = 100;
            Top = 100;
            Type = 0;
        }

        public void CreateWindow()
        {
            if(Type == 0)
            {
                daily = new WeatherWidget2.Widget.TempWidget(CityID, WidgetMeasure);
                daily.UpdateInfo();
                daily.SetIcon();
                daily.Show();
                daily.Edit(true);
            }
            else
            {
                //TODO
            }
        }
        public void UpdateData()
        {
            if(Type == 0)
            {
                daily.UpdateWeatherData(CityID, WidgetMeasure);
                daily.UpdateInfo();
                daily.SetIcon();
            }
            else
            {

            }
        }
        public void Destroy()
        {
            if (Type == 0)
            {
                daily.Close();
                daily = null;
            }
            else
            {

            }
        }

        public string GetValue()
        {
            return "";
        }
        public void SetValue(string value)
        {
        }
    }
}
