using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WeatherWidget2.Model
{
    [DataContract(Name = "WeatherBodyList")]
    public class WeatherBodyList
    {
        [DataMember(Name = "dt")]
        public int Date { get; set; }
        [DataMember(Name = "main")]
        public Main Main { get; set; }
        [DataMember(Name = "weather")]
        public List<WeatherInfo> WeatherList { get; set; }
        [DataMember(Name = "clouds")]
        public Clouds Clouds { get; set; }
        [DataMember(Name = "wind")]
        public Wind Wind { get; set; }
        [DataMember(Name = "sys")]
        public System system { get; set; }
        [DataMember(Name = "dt_txt")]
        public string Date2 { get; set; }
        [DataMember(Name = "rain")]
        public Rain Rain { get; set; }
        [DataMember(Name = "snow")]
        public Snow Snow { get; set; }
    }
}
