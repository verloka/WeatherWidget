using System.Runtime.Serialization;

namespace WeatherWidget2.Model
{
    [DataContract(Name = "System")]
    public class System
    {
        [DataMember(Name = "type")]
        public int type { get; set; }
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "message")]
        public double SystemParameter { get; set; }
        [DataMember(Name = "country")]
        public string CountryCode { get; set; }
        [DataMember(Name = "sunrise")]
        public int Sunrise { get; set; }
        [DataMember(Name = "sunset")]
        public int Sunset { get; set; }
    }
}
