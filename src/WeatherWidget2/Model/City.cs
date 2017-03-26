using System.Runtime.Serialization;

namespace WeatherWidget2.Model
{
    [DataContract(Name = "City")]
    public class City
    {
        [DataMember(Name = "_id")]
        public int ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "country")]
        public string Country { get; set; }
        [DataMember(Name = "coord")]
        public Coordinates Coordinates { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
