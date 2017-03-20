using System.Runtime.Serialization;

namespace WeatherWidget2.Model
{
    [DataContract(Name = "Wind")]
    public class Wind
    {
        [DataMember(Name = "speed")]
        public double Speed { get; set; }
        [DataMember(Name = "deg")]
        public double Direction { get; set; }
        [DataMember(Name = "gust")]
        public double Gust { get; set; }
    }
}
