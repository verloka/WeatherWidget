using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WeatherWidgetLib.Condition;
using WeatherWidgetLib.Geoname;

namespace WeatherWidgetLib
{
    public class Weather
    {
        public WeatherObject Current { get; private set; }
        public List<ConditionsObject> conditions { get; private set; }
        public Geonames geonames { get; private set; }
        public string City { get; set; } = "Kharkiv";
        string apiKey = "40cb9b0803474ba6982135724170701";

        public Weather()
        {
            conditions = JsonConvert.DeserializeObject<List<ConditionsObject>>(File.ReadAllText(@"Condition\Conditions.json"));
            geonames = JsonConvert.DeserializeObject<Geonames>(File.ReadAllText(@"Geoname\Geonames.json"));
        }

        public Task LoadData()
        {
            return Task.Factory.StartNew(() =>
            {
                string url = $"http://api.apixu.com/v1/current.json?key={apiKey}&q={City}";

                using (var webClient = new WebClient())
                {
                    var response = webClient.DownloadString(url);
                    Current = JsonConvert.DeserializeObject<WeatherObject>(response);
                }
            });
        }

        public string GetLocation()
        {
            return $"{Current.location.country}, {Current.location.name}";
        }
        public int GetConditionCode()
        {
            return Current.current.condition.code;
        }
        public string GetThemperature(bool Celsium)
        {
            if (Celsium)
                return (Current.current.temp_c >= 0 ? $"+{Current.current.temp_c} °C" : $"{Current.current.temp_c} °C").Replace(',', '.');
            else
                return (Current.current.temp_f >= 0 ? $"+{Current.current.temp_f} °F" : $"{Current.current.temp_f} °F").Replace(',', '.');
        }
        public string GetCondition(int code, string locale = "en")
        {
            string result = "";

            if (locale != "en")
                foreach (var itemCode in conditions)
                {
                    if (itemCode.code == code)
                        foreach (var itemLang in itemCode.languages)
                        {
                            if (itemLang.lang_iso == locale)
                                if (Current.current.is_day == 1)
                                    result = itemLang.day_text;
                                else
                                    result = itemLang.night_text;
                        }
                }
            else
                foreach (var itemCode in conditions)
                {
                    if (itemCode.code == code)
                        if (Current.current.is_day == 1)
                            result = itemCode.day;
                        else
                            result = itemCode.night;
                }

            return result;
        }
        public string GetConditionIconURL(int code)
        {
            return $"http:{Current.current.condition.icon}";
        }
    }
}
