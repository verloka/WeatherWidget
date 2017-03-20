using System.Runtime.Serialization;

namespace WeatherWidget2.Model
{
    [DataContract(Name = "Snow")]
    public class Snow
    {
        [DataMember(Name = "3h")]
        public double LastThreeHours { get; set; }
    }
}
