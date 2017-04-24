using System;
using Verloka.HelperLib.Settings;
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
                if (Type == 0)
                    return current != null;
                else
                    return false;
            }
        }

        Windows.WidgetCurrent current;

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
        }
        public Widget(string value)
        {
            SetValue(value);
        }

        public void SetEditMode(bool mode)
        {
            if (Type == 0)
            {
                current.Edit(mode);
            }
            else
            {

            }
        }
        public void CopyPosition()
        {
            if (Type == 0)
            {
                Left = (int)current.Left;
                Top = (int)current.Top;
            }
            else
            {

            }
        }
        public void CreateWindow()
        {
            if (Type == 0)
            {
                current = new WeatherWidget2.Windows.WidgetCurrent(CityID, WidgetMeasure, Size, Theme);
                current.UpdateInfo();
                current.UpdateLook();
                current.Top = Top;
                current.Left = Left;
                current.Show();
            }
            else
            {
                //TODO
            }
        }
        public void UpdateData()
        {
            if (Type == 0)
            {
                current.UpdateWeatherData(CityID, WidgetMeasure);
                current.UpdateInfo();
                current.UpdateLook();
            }
            else
            {

            }
        }
        public void Destroy()
        {
            if (Type == 0)
            {
                if (current != null)
                {
                    current.Close();
                    current = null;
                }
            }
            else
            {

            }
        }
        public void UpdateLook()
        {
            if (Type == 0)
            {
                if (current == null)
                    return;

                current.icons.UpdateData(Size, Theme);
                current.UpdateTextColor(TextColor);
            }
            else
            {

            }
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
