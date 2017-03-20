using System.Runtime.Serialization;

namespace WeatherWidget2.Model
{
    [DataContract(Name = "Coordinates")]
    public class Coordinates
    {
        [DataMember(Name = "lon")]
        public double Longitude { get; set; }
        [DataMember(Name = "lat")]
        public double Latitude { get; set; }
    }
}
