namespace WeatherWidgetLib.Geoname
{
    public class Geoname
    {
        public string continent { get; set; }
        public string capital { get; set; }
        public string languages { get; set; }
        public int geonameId { get; set; }
        public double south { get; set; }
        public string isoAlpha3 { get; set; }
        public double north { get; set; }
        public string fipsCode { get; set; }
        public string population { get; set; }
        public double east { get; set; }
        public string isoNumeric { get; set; }
        public string areaInSqKm { get; set; }
        public string countryCode { get; set; }
        public double west { get; set; }
        public string countryName { get; set; }
        public string continentName { get; set; }
        public string currencyCode { get; set; }

        public override string ToString()
        {
            return countryName;
        }
    }
}
