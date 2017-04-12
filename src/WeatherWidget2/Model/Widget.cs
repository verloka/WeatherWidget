using Verloka.HelperLib.Settings;

namespace WeatherWidget2.Model
{
    public class Widget : ISettingStruct
    {
        public string Name { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Type { get; set; } //0 daily, 1 forecast
        public int CityID { get; set; }
        public Measure WidgetMeasure { get; set; }
        public IconSize Size { get; set; }
        public bool Visible { get; set; }

        WeatherWidget2.Widget.TempWidget daily;

        public Widget()
        {
            Name = "NaN";
            Left = 100;
            Top = 100;
            Type = 0;
            CityID = 2172797;
            Size = IconSize.Medium;
            WidgetMeasure = Measure.Metric;
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
                daily.Edit(mode);
            }
            else
            {

            }
        }
        public void CopyPosition()
        {
            if (Type == 0)
            {
                Left = (int)daily.Left;
                Top = (int)daily.Top;
            }
            else
            {

            }
        }
        public void CreateWindow()
        {
            if (Type == 0)
            {
                daily = new WeatherWidget2.Widget.TempWidget(CityID, WidgetMeasure);
                daily.UpdateInfo();
                daily.SetIcon();
                daily.Top = Top;
                daily.Left = Left;
                daily.Show();
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
                if (daily != null)
                {
                    daily.Close();
                    daily = null;
                }
            }
            else
            {

            }
        }

        public string GetValue()
        {
            return $"{Name}|{Left}|{Top}|{Type}|{WidgetMeasure}|{Size}|{Visible}|{CityID}";
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
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
