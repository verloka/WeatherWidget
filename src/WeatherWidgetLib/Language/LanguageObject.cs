using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWidgetLib.Language
{
    public class LanguageObject
    {
        public string LanguageIso { get; set; }
        public string LanguageName { get; set; }

        public override string ToString()
        {
            return LanguageName;
        }
    }
}
