using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WeatherWidget2.Model
{
    [DataContract(Name = "Country")]
    public class Country
    {
        [DataMember(Name = "countryCode")]
        public string Code { get; set; }
        [DataMember(Name = "countryName")]
        public string Name { get; set; }
        [DataMember(Name = "countryFilePath")]
        public string Path { get; set; }
        [DataMember(Name = "countryCityList")]
        public List<City> Сities { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
