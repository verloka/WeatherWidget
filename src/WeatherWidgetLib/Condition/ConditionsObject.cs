using System.Collections.Generic;

namespace WeatherWidgetLib.Condition
{
    public class ConditionsObject
    {
        public int code { get; set; }
        public string day { get; set; }
        public string night { get; set; }
        public int icon { get; set; }
        public List<Language.Language> languages { get; set; }
    }
}
