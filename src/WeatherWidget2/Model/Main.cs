using System.Runtime.Serialization;

namespace WeatherWidget2.Model
{
    [DataContract(Name = "Main")]
    public class Main
    {
        [DataMember(Name = "temp")]
        public double Temperature { get; set; }
        [DataMember(Name = "pressure")]
        public double Pressure { get; set; }
        [DataMember(Name = "humidity")]
        public double Humidity { get; set; }
        [DataMember(Name = "temp_min")]
        public double MinimumTemperature { get; set; }
        [DataMember(Name = "temp_max")]
        public double MaximumTemperature { get; set; }
        [DataMember(Name = "sea_level")]
        public double PressureSeaLevel { get; set; }
        [DataMember(Name = "grnd_level")]
        public double PressureGroundLevel { get; set; }
    }
}
