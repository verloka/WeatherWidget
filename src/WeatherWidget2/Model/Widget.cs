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
        public int Left { get; set; }
        public int Top { get; set; }
        public int Type { get; set; } //0 daily, 1 forecast

        public Widget()
        {

        }

        public void CreateWindow()
        {

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
