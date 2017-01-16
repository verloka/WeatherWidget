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
        public event Action<Error.Error> ErrorLoadData;

        public WeatherObject Current { get; private set; }
        public List<ConditionsObject> conditions { get; private set; }
        public Geonames geonames { get; private set; }
        public string City { get; set; }
        string apiKey;

        public Weather(string apiKey, string appPath)
        {
            this.apiKey = apiKey;

            conditions = JsonConvert.DeserializeObject<List<ConditionsObject>>(File.ReadAllText($"{appPath}\\Condition\\Conditions.json"));
            geonames = JsonConvert.DeserializeObject<Geonames>(File.ReadAllText($"{appPath}\\Geoname\\Geonames.json"));
        }

        public Task<bool> LoadData()
        {
            return Task.Factory.StartNew(() =>
            {
                bool result = true;
                string url = $"http://api.apixu.com/v1/current.json?key={apiKey}&q={City}";

                using (var webClient = new WebClient())
                {
                    string resp = "";

                    try
                    {
                        resp = webClient.DownloadString(url);
                        Current = JsonConvert.DeserializeObject<WeatherObject>(resp);

                        if (Current?.current == null || Current?.location == null)
                        {
                            Error.Error error = JsonConvert.DeserializeObject<Error.Error>(resp);
                            ErrorLoadData?.Invoke(error);
                            result = false;
                        }
                    }
                    catch (WebException e)
                    {
                        if (e.Status == WebExceptionStatus.ProtocolError)
                        {
                            var response = e.Response as HttpWebResponse;
                            if (response != null && (int)response.StatusCode == 401)
                                ErrorLoadData?.Invoke(new Error.Error() { code = 2006, message = "API key provided is invalid" });
                            else if(response != null && (int)response.StatusCode == 403)
                                ErrorLoadData?.Invoke(new Error.Error() { code = 2007, message = "API key has been disabled. API key has exceeded calls per month quota." });

                        }
                        result = false;
                    }

                    return result;
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
