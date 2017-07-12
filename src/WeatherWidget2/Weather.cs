using Newtonsoft.Json;
using System;
using System.Net;
using WeatherWidget2.Model;

namespace WeatherWidget2
{
    public class Weather
    {
        string key = "b8c6dced55c3dbe25f25f8c827fa76b7";
        int city = 0;
        string measures = "metric"; //metric/imperial
        public Current Current;
        public Forecast Forecast;

        public Weather()
        {
            
        }
        
        public void SetCity(int id)
        {
            city = id;
        }
        public Current LoadCurrent()
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
        public Forecast LoadForecast()
        {
            using (var webClient = new WebClient())
            {
                string URL = $"http://api.openweathermap.org/data/2.5/forecast?id={city}&units={measures}&appid={key}";
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
        public static int GetTemperature(double temp, Measure meas)
        {
            return meas == Measure.Metric ? (int)Math.Round(temp) : (int)Math.Round((9d / 5d) * temp + 32d);
        }
        public static string GetTemperatureString(double temp, Measure meas, bool sign = false)
        {
            string s = sign && temp > 0 ? "+ " : "";
            return s + (meas == Measure.Metric ? ((int)Math.Round(temp)).ToString() + " °C" : ((int)Math.Round((9d / 5d) * temp + 32d)).ToString() + " °F");
        }
    }
}
