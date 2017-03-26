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

        [DataMember(Name = "WidgetFactoryTitle")]
        public string WidgetFactoryTitle { get; set; } = "!create widgt";
        [DataMember(Name = "WidgetFactoryType")]
        public string WidgetFactoryType { get; set; } = "!type of widget";
        [DataMember(Name = "WidgetFactoryTypeDaily")]
        public string WidgetFactoryTypeDaily { get; set; } = "!daily (one day)";
        [DataMember(Name = "WidgetFactoryTypeForecast")]
        public string WidgetFactoryTypeForecast { get; set; } = "!forecast (5 days)";
        [DataMember(Name = "WidgetFactoryTypeMeasures")]
        public string WidgetFactoryTypeMeasures { get; set; } = "!system of measures";
        [DataMember(Name = "WidgetFactoryTypeMetrical")]
        public string WidgetFactoryTypeMetrical { get; set; } = "!metrical";
        [DataMember(Name = "WidgetFactoryTypeImperial")]
        public string WidgetFactoryTypeImperial { get; set; } = "!imperial";
        [DataMember(Name = "WidgetFactoryTypeSize")]
        public string WidgetFactoryTypeSize { get; set; } = "!size";
        [DataMember(Name = "WidgetFactoryTypeSizeBig")]
        public string WidgetFactoryTypeSizeBig { get; set; } = "!big";
        [DataMember(Name = "WidgetFactoryTypeSizeMedium")]
        public string WidgetFactoryTypeSizeMedium { get; set; } = "!medium";
        [DataMember(Name = "WidgetFactoryTypeSizeSmall")]
        public string WidgetFactoryTypeSizeSmall { get; set; } = "!small";
        [DataMember(Name = "WidgetFactoryTypeTheme")]
        public string WidgetFactoryTypeTheme { get; set; } = "!theme of widget";
        [DataMember(Name = "WidgetFactoryTypeThemeDark")]
        public string WidgetFactoryTypeThemeDark { get; set; } = "!dark";
        [DataMember(Name = "WidgetFactoryTypeThemeWhite")]
        public string WidgetFactoryTypeThemeWhite { get; set; } = "!white";
        [DataMember(Name = "WidgetFactoryTypeIconTheme")]
        public string WidgetFactoryTypeIconTheme { get; set; } = "!theme of icons";
        [DataMember(Name = "WidgetFactoryContry")]
        public string WidgetFactoryContry { get; set; } = "!contry";
        [DataMember(Name = "WidgetFactoryCity")]
        public string WidgetFactoryCity { get; set; } = "!city";
        [DataMember(Name = "WidgetFactoryWidgetName")]
        public string WidgetFactoryWidgetName { get; set; } = "!name of widget";
    }
}
