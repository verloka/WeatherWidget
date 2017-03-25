using System.Runtime.Serialization;

namespace WeatherWidget2.Model
{
    [DataContract(Name = "Language File")]
    public class Localization
    {
        [DataMember(Name = "LanguageName")]
        public string LanguageName { get; set; } = "!default";

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

        [DataMember(Name = "TabOptionsAutorun")]
        public string TabOptionsAutorun { get; set; } = "!startup with windows";
        [DataMember(Name = "TabOptionsMsgInet")]
        public string TabOptionsMsgInet { get; set; } = "!show message when no internet";
        [DataMember(Name = "TabOptionsCheckUpdate")]
        public string TabOptionsCheckUpdate { get; set; } = "!check update with startup";
        [DataMember(Name = "TabOptionsLanguage")]
        public string TabOptionsLanguage { get; set; } = "!lang";
        [DataMember(Name = "TabOptionsTheme")]
        public string TabOptionsTheme { get; set; } = "!theme";

        [DataMember(Name = "TabInfoDeveloped")]
        public string TabInfoDeveloped { get; set; } = "!developed in 2017 by Verloka Vadim";
    }
}
