using Newtonsoft.Json;
using System;
using System.Net;
using System.Windows;

namespace WeatherWidget2
{
    public class Weather
    {
        string key = "b8c6dced55c3dbe25f25f8c827fa76b7";
        int city = 0;
        string measures = "metric"; //metric/imperial
        public Model.Current Current;
        public Model.Forecast Forecast;

        public Weather()
        {
            
        }
        
        public void SetMeasure(Model.Measure ms)
        {
            measures = ms == 0 ? "metric" : "imperial";
        }
        public void SetCity(int id)
        {
            city = id;
        }
        public Model.Current LoadCurrent()
        {
            using (var webClient = new WebClient())
            {
                string URL = $"http://api.openweathermap.org/data/2.5/weather?id={city}&units={measures}&appid={key}";
                string resp = "";

                try
                {
                    resp = webClient.DownloadString(URL);
                    Current = JsonConvert.DeserializeObject<Model.Current>(resp);
                }
                catch { }

                return Current;
            }
        }
        public Model.Forecast LoadForecast()
        {
            using (var webClient = new WebClient())
            {
                string URL = $"http://api.openweathermap.org/data/2.5/forecast?q={city}&units={measures}&appid={key}";
                string resp = "";

                try
                {
                    resp = webClient.DownloadString(URL);
                    Forecast = JsonConvert.DeserializeObject<Model.Forecast>(resp);
                }
                catch { }

                return Forecast;
            }
        }
        public string GetTemperature(double temp)
        {
            string s = (temp > 0) ? "+" : "";
            string e = (measures == "metric") ? "°C" : "°F";
            return $"{s} {temp} {e}";
        }
    }
}
