using System;
using System.IO;

namespace WeatherWidget2ResourceLib
{
    public class Geonames
    {
        public static string GetCoutryJson(string name)
        {
            return File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\GeoData\\{name}");
        }
        public static string GetCoutrysJson()
        {
            return File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\GeoData\\Countrys.json");
        }
    }
}
