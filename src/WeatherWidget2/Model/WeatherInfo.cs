using System.Runtime.Serialization;

namespace WeatherWidget2.Model
{
    [DataContract(Name = "WeatherInfo")]
    public class WeatherInfo
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "main")]
        public string WeatherParameters { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "icon")]
        public string Icon { get; set; }
    }
}
