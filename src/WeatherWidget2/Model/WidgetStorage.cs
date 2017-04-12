using System;
using System.Collections.Generic;
using Verloka.HelperLib.Settings;

namespace WeatherWidget2.Model
{
    public class WidgetStorage: ISettingStruct
    {
        public event Action ListChangded;
        public List<Widget> Widgets { get; private set; }

        public WidgetStorage()
        {
            Widgets = new List<Widget>();
        }

        public void Add(Widget widg)
        {
            Widgets.Add(widg);
            ListChangded?.Invoke();
        }
        public void Remove()
        {

        }

        public string GetValue()
        {
            string res = "";

            foreach (var item in Widgets)
            {
                if (res != "")
                    res += $"%{item.GetValue()}";
                else
                    res += item.GetValue();
            }

            return res;
        }
        public void SetValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            if (Widgets == null)
                Widgets = new List<Widget>();

            string[] strs = value.Split('%');
            foreach (var item in strs)
                Widgets.Add(new Widget(item));
        }
    }
}
