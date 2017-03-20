using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WeatherWidget2.Model
{
    [DataContract(Name = "Forecast")]
    public class Forecast
    {
        [DataMember(Name = "cod")]
        public string cod { get; set; }
        [DataMember(Name = "message")]
        public double message { get; set; }
        [DataMember(Name = "cnt")]
        public int cnt { get; set; }
        [DataMember(Name = "list")]
        public List<WeatherBodyList> WeatherBodyList { get; set; }
        [DataMember(Name = "city")]
        public City City { get; set; }
    }
}
