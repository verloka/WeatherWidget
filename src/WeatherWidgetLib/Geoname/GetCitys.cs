using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace WeatherWidgetLib.Geoname
{
    public static class GetCitys
    {
        public static event Action<string> WrongUser;

        public static Task<GeonameCitys> Get(string countryCode, string login)
        {
            return Task.Run(() =>
            {
                string url = $"http://api.geonames.org/searchJSON?username={login}&country={countryCode}&maxRows=1000&style=SHORT";
                GeonameCitys citys;

                using (var webClient = new WebClient())
                {
                    var response = webClient.DownloadString(url);
                    citys = JsonConvert.DeserializeObject<GeonameCitys>(response);

                    if(citys?.geonames == null)
                    {
                        citys = new GeonameCitys() { geonames = new System.Collections.Generic.List<GeonameCity>(), totalResultsCount = 0 };
                        WrongUser?.Invoke(login);
                    }
                }


                return citys;
            });
        }
    }
}
