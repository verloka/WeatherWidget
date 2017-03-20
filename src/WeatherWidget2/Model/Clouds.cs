using System.Runtime.Serialization;

namespace WeatherWidget2.Model
{
    [DataContract(Name = "Clouds")]
    public class Clouds
    {
        [DataMember(Name = "all")]
        public int Cloudiness { get; set; }
    }
}
