namespace WeatherWidgetLib.Geoname
{
    public class GeonameCity
    {
        public string lng { get; set; }
        public int geonameId { get; set; }
        public string countryCode { get; set; }
        public string name { get; set; }
        public string toponymName { get; set; }
        public string lat { get; set; }
        public string fcl { get; set; }
        public string fcode { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
