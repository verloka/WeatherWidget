using System.Runtime.Serialization;

namespace WeatherWidget2.Model
{
    [DataContract(Name = "Language File")]
    public class Localization
    {
        [DataMember(Name = "GeneralTabHome")]
        public string GeneralTabHome { get; set; } = "!home";
        [DataMember(Name = "GeneralTabWidgets")]
        public string GeneralTabWidgets { get; set; } = "!widgets";
        [DataMember(Name = "GeneralTabOptions")]
        public string GeneralTabOptions { get; set; } = "!options";
        [DataMember(Name = "GeneralTabInformation")]
        public string GeneralTabInformation { get; set; } = "!information";

        [DataMember(Name = "TabHomeWhatNew")]
        public string TabHomeWhatNew { get; set; } = "!what new?";
    }
}
