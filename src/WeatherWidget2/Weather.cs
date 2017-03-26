using Newtonsoft.Json;
using System.Net;

namespace WeatherWidget2
{
    public class Weather
    {
        string key = "b8c6dced55c3dbe25f25f8c827fa76b7";
        string city = "Kharkiv";
        string measures = "metric"; //metric/imperial
        Model.Current Current;
        Model.Forecast Forecast;

        public Weather()
        {
            
        }

        public void LoadCity()
        {

        }
        public Model.Current LoadCurrent()
        {
            using (var webClient = new WebClient())
            {
                string URL = $"http://api.openweathermap.org/data/2.5/weather?q={city}&units={measures}&appid={key}";
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
    }
}
