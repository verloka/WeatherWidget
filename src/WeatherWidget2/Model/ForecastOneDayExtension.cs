﻿namespace WeatherWidget2.Model
{
    public static class ForecastOneDayExtension
    {
        public static void SetData(this ForecastOneDay f, int vaule, string label, string icon, double press, double humi, string condi)
        {
            f.Values.Add(vaule);
            f.Labels.Add(label);
            f.Icons.Add(icon);
            f.Press.Add(press);
            f.Humi.Add(humi);
            f.Condi.Add(condi);
        }
    }
}
