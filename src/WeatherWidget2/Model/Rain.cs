using System.Runtime.Serialization;

namespace WeatherWidget2.Model
{
    [DataContract(Name = "Rain")]
    public class Rain
    {
        [DataMember(Name = "3h")]
        public double LastThreeHours { get; set; }
    }
}
