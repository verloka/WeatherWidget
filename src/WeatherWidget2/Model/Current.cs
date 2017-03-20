using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WeatherWidget2.Model
{
    [DataContract(Name = "Current")]
    public class Current
    {
        [DataMember(Name = "coord")]
        public Coordinates Coordinates { get; set; }
        [DataMember(Name = "weather")]
        public List<WeatherInfo> WeatherList { get; set; }
        [DataMember(Name = "base")]
        public string Base { get; set; }
        [DataMember(Name = "main")]
        public Main Main { get; set; }
        [DataMember(Name = "visibility")]
        public int Visibility { get; set; }
        [DataMember(Name = "wind")]
        public Wind Wind { get; set; }
        [DataMember(Name = "clouds")]
        public Clouds Clouds { get; set; }
        [DataMember(Name = "dt")]
        public int Data { get; set; }
        [DataMember(Name = "sys")]
        public System system { get; set; }
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "cod")]
        public int cod { get; set; }

        public DateTime GetDate
        {
            get
            {
                DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dt = dt.AddSeconds(Data).ToLocalTime();
                return dt;
            }
        }
    }
}
