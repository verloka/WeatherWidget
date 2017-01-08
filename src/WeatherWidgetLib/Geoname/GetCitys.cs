using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace WeatherWidgetLib.Geoname
{
    public static class GetCitys
    {
        public static Task<GeonameCitys> Get(string countryCode)
        {
            return Task.Run(() =>
            {
                string url = $"http://api.geonames.org/searchJSON?username=ogyreal&country={countryCode}&maxRows=1000&style=SHORT";
                GeonameCitys citys;

                using (var webClient = new WebClient())
                {
                    var response = webClient.DownloadString(url);
                    citys = JsonConvert.DeserializeObject<GeonameCitys>(response);
                }


                return citys;
            });
        }
    }
}
